using Mavim.Manager.Api.WopiFileLock.Repository.Interfaces;
using Mavim.Manager.Api.WopiFileLock.Repository.Interfaces.v1;
using Mavim.Manager.WopiFileLock.DbContext;
using Mavim.Manager.WopiFileLock.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiFileLock.Repository.Model.v1
{
    public class WopiFileLockRepository : IWopiFileLockRepository
    {
        private WopiFileLockStateDbContext DbContext { get; }
        private readonly ILogger<WopiFileLockRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiFileLockRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The generic repository.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// genericRepository
        /// or
        /// logger
        /// </exception>
        public WopiFileLockRepository(WopiFileLockStateDbContext dbContext, ILogger<WopiFileLockRepository> logger)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets the file lock status.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="xWopiOldLockValue">The x wopi old lock value.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockResult> GetFileLockStatus(Guid dbId, string id, string xWopiOldLockValue)
        {
            _logger.LogTrace($"Retrieving file lock status for db id: {dbId} and dcv id: {id}.");
            WopiFileLockState wopiFileLockState = await DbContext.WopiFileLockStates.AsNoTracking().FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);

            if (wopiFileLockState == null) return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };

            if (!string.IsNullOrWhiteSpace(wopiFileLockState.Value) && !wopiFileLockState.Value.Equals(xWopiOldLockValue)) return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileAlreadyLocked, LockValue = wopiFileLockState.Value };

            if (wopiFileLockState.Expires < DateTime.Now)
            {
                DbContext.WopiFileLockStates.Remove(wopiFileLockState);
                await DbContext.SaveChangesAsync();
            }

            return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };
        }

        /// <summary>
        /// Adds the file lock.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The user object identifier.</param>
        /// <param name="xWopiLockValue">The x wopi lock value.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockResult> AddFileLock(Guid dbId, string id, string userId, string xWopiLockValue, string version)
        {
            _logger.LogDebug($"Add file lock for db id: {dbId}, user object id: {userId} and dcv id: {id}.");

            WopiFileLockState lockState = new WopiFileLockState()
            {
                DbId = dbId,
                DcvId = id,
                UserId = userId,
                Value = xWopiLockValue,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Version = version
            };

            WopiFileLockState wopiFileLockState =
                await DbContext.WopiFileLockStates.AsNoTracking().FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);

            if (wopiFileLockState == null || string.IsNullOrWhiteSpace(wopiFileLockState.Value))
            {
                await DbContext.WopiFileLockStates.AddAsync(lockState);
                await DbContext.SaveChangesAsync();
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockSuccessful };
            }

            if (wopiFileLockState.Value != xWopiLockValue)
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockMismatch, LockValue = wopiFileLockState.Value };

            // Lock and Refresh the expiration
            wopiFileLockState.Expires = DateTime.UtcNow.AddMinutes(30);
            wopiFileLockState.Value = xWopiLockValue;
            DbContext.WopiFileLockStates.Update(wopiFileLockState);
            await DbContext.SaveChangesAsync();
            return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockSuccessful };
        }

        /// <summary>Unlocks the file.</summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="xWopiLockValue">The x wopi lock value.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockResult> UnlockFile(Guid dbId, string id, string xWopiLockValue)
        {
            _logger.LogDebug($"Unlock file for db id: {dbId} and dcv id: {id}.");
            WopiFileLockState wopiFileLockState =
                await DbContext.WopiFileLockStates.AsNoTracking().FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);

            if (wopiFileLockState == null || string.IsNullOrWhiteSpace(wopiFileLockState.Value))
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockMismatch, LockValue = string.Empty };

            if (wopiFileLockState.Expires < DateTime.UtcNow)
            {
                DbContext.WopiFileLockStates.Remove(wopiFileLockState);
                await DbContext.SaveChangesAsync();
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };
            }

            if (wopiFileLockState.Value != xWopiLockValue)
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockMismatch, LockValue = wopiFileLockState.Value };

            DbContext.WopiFileLockStates.Remove(wopiFileLockState);
            await DbContext.SaveChangesAsync();
            return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileUnLockSuccessful };
        }

        /// <summary>Unlocks and re-locks the file.</summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="xWopiLockValue">The x wopi lock value.</param>
        /// <param name="xWopiOldLockValue">The x wopi old lock value.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockResult> UnlockAndRelock(Guid dbId, string id, string xWopiLockValue, string xWopiOldLockValue)
        {
            _logger.LogDebug($"Unlock and relock file for db id: {dbId} and dcv id: {id}.");

            WopiFileLockState wopiFileLockState =
                await DbContext.WopiFileLockStates.AsNoTracking().FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);

            if (wopiFileLockState == null || string.IsNullOrWhiteSpace(wopiFileLockState.Value))
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };

            if (wopiFileLockState.Expires < DateTime.UtcNow)
            {
                DbContext.WopiFileLockStates.Remove(wopiFileLockState);
                await DbContext.SaveChangesAsync();
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };
            }

            if (wopiFileLockState.Value != xWopiOldLockValue)
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockMismatch, LockValue = wopiFileLockState.Value };

            wopiFileLockState.Value = xWopiLockValue;
            wopiFileLockState.Expires = DateTime.UtcNow.AddMinutes(30);
            DbContext.WopiFileLockStates.Update(wopiFileLockState);
            await DbContext.SaveChangesAsync();

            return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileUnLockNReLockSuccessful };
        }

        /// <summary>
        /// Refreshes the lock file.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="xWopiLockValue">The x wopi lock value.</param>
        /// <param name="xWopiOldLockValue">The x wopi old lock value.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockResult> RefreshLockFile(Guid dbId, string id, string xWopiLockValue, string xWopiOldLockValue)
        {
            _logger.LogDebug($"Refresh lock file for db id: {dbId} and dcv id: {id}.");

            WopiFileLockState wopiFileLockState =
                await DbContext.WopiFileLockStates.AsNoTracking()
                .FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);

            if (wopiFileLockState == null || string.IsNullOrWhiteSpace(wopiFileLockState.Value))
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileNotLocked };

            if (wopiFileLockState.Value != xWopiLockValue)
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockMismatch, LockValue = wopiFileLockState.Value };

            if (wopiFileLockState.Expires < DateTime.UtcNow)
            {
                wopiFileLockState.Value = xWopiLockValue;
                wopiFileLockState.Expires = DateTime.UtcNow.AddMinutes(30);
                DbContext.WopiFileLockStates.Update(wopiFileLockState);
                await DbContext.SaveChangesAsync();
                return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileLockExtendedSuccessful };
            }

            wopiFileLockState.Value = xWopiLockValue;
            wopiFileLockState.Expires = DateTime.UtcNow.AddMinutes(30);
            DbContext.WopiFileLockStates.Update(wopiFileLockState);
            await DbContext.SaveChangesAsync();

            return new WopiFileLockResult { FileLockStatus = WopiFileLockStatus.FileUnLockNReLockSuccessful };
        }

        /// <summary>
        /// Gets the file lock.
        /// </summary>
        /// <param name="dbId">The database identifier.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<IWopiFileLockState> GetFileLock(Guid dbId, string id)
        {
            _logger.LogDebug($"Get file lock for db id: {dbId} and dcv id: {id}.");

            return await DbContext.WopiFileLockStates.AsNoTracking().FirstOrDefaultAsync(u => u.DcvId == id && u.DbId == dbId);
        }
    }
}