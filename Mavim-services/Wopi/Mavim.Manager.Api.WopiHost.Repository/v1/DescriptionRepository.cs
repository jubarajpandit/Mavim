using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Utils;
using Mavim.Manager.Api.Utils.AzAppConfiguration;
using Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Repository.Models;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Repository.v1
{
    public class DescriptionRepository : IDescriptionRepository
    {
        private readonly IMavimDatabaseModel _model;
        readonly IOptionsSnapshot<WopiSettings> _wopiSettings;

        public DescriptionRepository(IMavimDbDataAccess dataAccess, IOptionsSnapshot<WopiSettings> wopiSettings, IDataLanguage dataLanguage)
        {
            _wopiSettings = wopiSettings;
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
            _model.DataLanguage = new Language(Map(dataLanguage.Type));
        }

        /// <summary>
        /// Checks the file information.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">You need to set connection info before getting data
        /// or
        /// Error while logging in to the Mavim database: {session.LoginError.ToString()}</exception>
        /// <exception cref="System.ArgumentException">DcvId not in right format
        /// or
        /// DcvId has no attached element</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<ICheckFileInfo> CheckFileInfo(string dcvId)
        {
            if (DcvId.FromDcvKey(dcvId) == null) throw new ArgumentException("DcvId not in right format");

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(dcvId));

            if (topic == null)
                throw new ArgumentException("DcvId has no attached element");

            IDescription description = topic.Description;

            ICheckFileInfo descriptionInfo = await GetDescriptionInfo(description, dcvId);

            return descriptionInfo;
        }

        /// <summary>
        /// Gets the content of the file.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">You need to set connection info before getting data
        /// or
        /// Error while logging in to the Mavim database: {session.LoginError.ToString()}</exception>
        /// <exception cref="System.ArgumentException">DcvId not in right format
        /// or
        /// DcvId has no attached element</exception>
        public async Task<Stream> GetFileContent(string dcvId)
        {
            if (DcvId.FromDcvKey(dcvId) == null)
                throw new ArgumentException("DcvId not in right format");

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(dcvId));

            if (topic == null)
                throw new ArgumentException("DcvId has no attached element");

            IDescription description = topic.Description;

            Stream descriptionStream = await GetDescriptionStream(description);

            return descriptionStream;
        }

        /// <summary>
        /// Updates the description.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="descriptionStream">The description stream.</param>
        /// <exception cref="Exception">
        /// You need to set connection info before getting data
        /// or
        /// Error while logging in to the Mavim database: {session.LoginError.ToString()}
        /// </exception>
        /// <exception cref="ArgumentException">
        /// DcvId not in right format
        /// or
        /// DcvId has no attached element
        /// </exception>
        public async Task UpdateDescription(string dcvId, Stream descriptionStream)
        {
            if (DcvId.FromDcvKey(dcvId) == null)
                throw new ArgumentException("DcvId not in right format");

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(dcvId));

            if (topic == null)
                throw new ArgumentException("DcvId has no attached element");

            IDescription description = topic.Description;

            IDocument doc = description.Document;
            IDocxDocument rtfdoc = (IDocxDocument)doc;
            descriptionStream.Position = 0;

            rtfdoc.SetDescription(descriptionStream, descriptionStream.Length);

            await Task.FromResult(true);
        }

        #region Private Methods
        /// <summary>
        /// Gets the element by DCV identifier.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <param name="sessionMavimHandle">The session mavim handle.</param>
        /// <returns></returns>
        private IElement GetElementByDcvId(IDcvId dcvId)
        {

            return _model.ElementRepository.GetElement(dcvId);

        }
        /// <summary>
        /// Gets the description information.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private async Task<ICheckFileInfo> GetDescriptionInfo(IDescription description, string dcvId)
        {
            IDocument descriptionDoc = description.Document;
            IDocxDocument descriptionDocx = (IDocxDocument)descriptionDoc;

            using Stream stream = descriptionDocx.GetDescription();

            ICheckFileInfo checkFileInfo = new CheckFileInfo
            {
                ComputedFileHash = HashUtils.ComputeSHA256(stream),
                FileSize = stream.Length
            };

            return await Task.FromResult(checkFileInfo);
        }

        /// <summary>
        /// Gets the description stream.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns></returns>
        private async Task<Stream> GetDescriptionStream(IDescription description)
        {
            IDocument descriptionDoc = description.Document;
            IDocxDocument descriptionDocx = (IDocxDocument)descriptionDoc;

            return await Task.FromResult(descriptionDocx.GetDescription());
        }

        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(string.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
        #endregion
    }
}
