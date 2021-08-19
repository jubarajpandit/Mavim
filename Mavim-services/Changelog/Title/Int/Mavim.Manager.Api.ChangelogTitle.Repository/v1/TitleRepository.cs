using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Enum;
using Mavim.Manager.Api.ChangelogTitle.Repository.Interfaces.v1.Interface;
using Mavim.Manager.Api.ChangelogTitle.Repository.v1.Model;
using Mavim.Manager.ChangelogTitle.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbModel = Mavim.Manager.ChangelogTitle.DbModel;

namespace Mavim.Manager.Api.ChangelogTitle.Repository.v1
{
    public class TitleRepository : ITitleRepository
    {
        #region Private Members
        private readonly TitleDbContext _dbContext;
        private readonly ILogger<TitleRepository> _logger;
        private readonly DbModel.Enums.DataLanguageType _dataLanguage;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleRepository"/> class.
        /// </summary>
        /// <param name="changelogDbContext">The changelog database context.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// changelogDbContext
        /// or
        /// logger
        /// </exception>
        public TitleRepository(TitleDbContext changelogDbContext, ILogger<TitleRepository> logger, Libraries.Middlewares.Language.Interfaces.IDataLanguage dataLanguage)
        {
            _dbContext = changelogDbContext ?? throw new ArgumentNullException(nameof(changelogDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dataLanguage = dataLanguage != null ? Map(dataLanguage.Type) : throw new ArgumentNullException(nameof(dataLanguage));
        }

        /// <summary>
        /// Gets the titles.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ITitle>> GetTitles(Guid tenantId, Guid dbid, string dcvid)
        {
            _logger.LogTrace($"Retrieving titles for tenant: {tenantId} and db: {dbid} and dcvid: {dcvid}.");
            CheckInputParameters(tenantId, dbid, dcvid);
            IEnumerable<DbModel.Title> result = await _dbContext.Titles.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbid &&
                    x.DataLanguage == _dataLanguage &&
                    x.TopicDcv == dcvid)
                .ToListAsync();

            return result.Select(Map);
        }

        /// <summary>
        /// Gets the pending title.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ITitle> GetPendingTitle(Guid tenantId, Guid dbid, string dcvid)
        {
            _logger.LogTrace($"Retrieving pending titles for tenant: {tenantId} and db: {dbid} and dcvid: {dcvid}.");
            CheckInputParameters(tenantId, dbid, dcvid);

            DbModel.Title result = await _dbContext.Titles.AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.TenantId == tenantId &&
                x.DatabaseId == dbid &&
                x.DataLanguage == _dataLanguage &&
                x.TopicDcv == dcvid &&
                x.Status == DbModel.Enums.ChangeStatus.Pending
            );

            return Map(result);
        }

        /// <summary>
        /// Gets all pending titles.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="dbid">The dbid.</param>
        /// <returns></returns>
        public async Task<IEnumerable<ITitle>> GetAllPendingTitles(Guid tenantId, Guid dbid)
        {
            _logger.LogTrace($"Retrieving pending titles for tenant: {tenantId} and db: {dbid}.");
            CheckInputParameters(tenantId, dbid);
            IEnumerable<DbModel.Title> result = await _dbContext.Titles.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbid &&
                    x.DataLanguage == _dataLanguage &&
                    x.Status == DbModel.Enums.ChangeStatus.Pending)
                .ToListAsync();

            return result.Select(Map);
        }

        /// <summary>
        /// Gets the title status.
        /// </summary>
        /// <param name="tenantId">The tenant identifier.</param>
        /// <param name="dbid">The dbid.</param>
        /// <param name="dcvid">The dcvid.</param>
        /// <returns></returns>
        public async Task<ChangeStatus> GetTitleStatus(Guid tenantId, Guid dbid, string dcvid)
        {
            _logger.LogTrace($"Get current title status for tenant: {tenantId}, db: {dbid}, dcvid: {dcvid}.");
            CheckInputParameters(tenantId, dbid, dcvid);
            DbModel.Title result = await _dbContext.Titles.AsNoTracking()
                .Where(x =>
                    x.TenantId == tenantId &&
                    x.DatabaseId == dbid &&
                    x.TopicDcv == dcvid &&
                    x.DataLanguage == _dataLanguage)
                .OrderByDescending(x => x.TimestampChanged)
                .FirstOrDefaultAsync();

            return Map(result?.Status ?? DbModel.Enums.ChangeStatus.Approved);
        }

        /// <summary>
        /// Saves the title.
        /// </summary>
        /// <param name="title">The title.</param>
        public async Task SaveTitle(ITitle title)
        {
            CheckInputParameters(title);
            _logger.LogTrace($"Saving title: {title.ToTitleValue} for changelog id: {title.ChangelogId}.");
            await _dbContext.Titles.AddAsync(Map(title));
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the state of the title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        public async Task<ITitle> UpdateTitleState(ITitle title)
        {
            CheckInputParameters(title);
            _logger.LogTrace($"Updating status title of changelog id: {title.ChangelogId}.");
            _dbContext.Titles.Update(Map(title));
            await _dbContext.SaveChangesAsync();

            return title;
        }

        private void CheckInputParameters(Guid tenantId, Guid dbid, params string[] values)
        {
            if (values.Any(string.IsNullOrEmpty))
            {
                _logger.LogError("string argument is null or empty");
                throw new ArgumentNullException(nameof(values));
            }

            if (dbid == Guid.Empty)
            {
                _logger.LogError("dbid argument empty");
                throw new ArgumentNullException(nameof(dbid));
            }

            if (tenantId == Guid.Empty)
            {
                _logger.LogError("tenantId argument empty");
                throw new ArgumentNullException(nameof(tenantId));
            }
        }

        private ITitle Map(DbModel.Title title) =>
            title == null ? null : new Title
            {
                ChangelogId = title.ChangelogId,
                TenantId = title.TenantId,
                DatabaseId = title.DatabaseId,
                DataLanguage = Map(title.DataLanguage),
                InitiatorUserEmail = title.InitiatorUserEmail,
                TimestampChanged = title.TimestampChanged,
                ReviewerUserEmail = title.ReviewerUserEmail,
                TimestampApproved = title.TimestampApproved,
                TopicDcv = title.TopicDcv,
                Status = Map(title.Status),
                FromTitleValue = title.FromTitleValue,
                ToTitleValue = title.ToTitleValue
            };

        private DbModel.Title Map(ITitle title) =>
            title == null ? null : new DbModel.Title
            {
                ChangelogId = title.ChangelogId,
                TenantId = title.TenantId,
                DatabaseId = title.DatabaseId,
                DataLanguage = _dataLanguage,
                InitiatorUserEmail = title.InitiatorUserEmail,
                TimestampChanged = title.TimestampChanged,
                ReviewerUserEmail = title.ReviewerUserEmail,
                TimestampApproved = title.TimestampApproved,
                TopicDcv = title.TopicDcv,
                Status = Map(title.Status),
                FromTitleValue = title.FromTitleValue,
                ToTitleValue = title.ToTitleValue
            };

        private static DbModel.Enums.ChangeStatus Map(ChangeStatus changeStatus)
        {
            return changeStatus switch
            {
                ChangeStatus.Pending => DbModel.Enums.ChangeStatus.Pending,
                ChangeStatus.Approved => DbModel.Enums.ChangeStatus.Approved,
                ChangeStatus.Rejected => DbModel.Enums.ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(changeStatus), changeStatus,
                    "ChangeStatus not defined.")
            };
        }

        private static DbModel.Enums.DataLanguageType Map(Libraries.Middlewares.Language.Enums.DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                Libraries.Middlewares.Language.Enums.DataLanguageType.Dutch => DbModel.Enums.DataLanguageType.Dutch,
                Libraries.Middlewares.Language.Enums.DataLanguageType.English => DbModel.Enums.DataLanguageType.English,
                _ => throw new ArgumentException(String.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };

        private static DataLanguageType Map(DbModel.Enums.DataLanguageType dataLanguageType) =>
            dataLanguageType switch
            {
                DbModel.Enums.DataLanguageType.Dutch => DataLanguageType.Dutch,
                DbModel.Enums.DataLanguageType.English => DataLanguageType.English,
                _ => throw new ArgumentOutOfRangeException("DataLanguage not supported")
            };

        private static ChangeStatus Map(DbModel.Enums.ChangeStatus changeStatus)
        {
            return changeStatus switch
            {
                DbModel.Enums.ChangeStatus.Pending => ChangeStatus.Pending,
                DbModel.Enums.ChangeStatus.Approved => ChangeStatus.Approved,
                DbModel.Enums.ChangeStatus.Rejected => ChangeStatus.Rejected,
                _ => throw new ArgumentOutOfRangeException(nameof(changeStatus), changeStatus,
                    "ChangeStatus not defined.")
            };
        }

        private static void CheckInputParameters(ITitle title)
        {
            if (title is null)
                throw new ArgumentNullException(nameof(title));
        }
    }
}