using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Api.Utils;
using Mavim.Manager.Api.WopiHost.Repository.Interfaces.v1;
using Mavim.Manager.Api.WopiHost.Repository.Models;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.WopiHost.Repository.v1
{
    public class ChartRepository : IChartRepository
    {
        private readonly IMavimDatabaseModel _model;

        public ChartRepository(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage)
        {
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
            if (DcvId.FromDcvKey(dcvId) == null)
                throw new ArgumentException("DcvId not in right format");

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(dcvId));

            if (topic == null)
                throw new ArgumentException("DcvId has no attached element");

            if (!topic.Type.IsChart)
                throw new Exception($"The element with dcvid { topic.DcvID.ToString() } is not of type chart.");

            IChartElement chartElement = (IChartElement)topic;

            ICheckFileInfo chartInfo = await GetChartInfo(chartElement, dcvId);

            return chartInfo;
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
        public Task<Stream> GetFileContent(string dcvId)
        {
            if (DcvId.FromDcvKey(dcvId) == null)
                throw new ArgumentException("DcvId not in right format");

            IElement topic = GetElementByDcvId(DcvId.FromDcvKey(dcvId));

            if (topic == null)
                throw new ArgumentException("DcvId has no attached element");

            if (!topic.Type.IsChart)
                throw new Exception($"The element with dcvid { topic.DcvID.ToString() } is not of type chart.");

            IChartElement chartElement = (IChartElement)topic;

            return Task.FromResult(chartElement.ChartData);
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
        /// <param name="chartElement">The chart element.</param>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private async Task<ICheckFileInfo> GetChartInfo(IChartElement chartElement, string dcvId)
        {
            byte[] chartBytes = GetChartBytes(chartElement);

            ICheckFileInfo checkFileInfo = new CheckFileInfo
            {
                ComputedFileHash = HashUtils.ComputeSHA256(new MemoryStream(chartBytes)),
                FileSize = chartBytes.Length
            };

            return await Task.FromResult(checkFileInfo);
        }

        /// <summary>
        /// Gets the chart bytes.
        /// </summary>
        /// <param name="chartElement">The chart element.</param>
        /// <returns></returns>
        private byte[] GetChartBytes(IChartElement chartElement)
        {
            using Stream chartStream = chartElement.ChartData;
            using var memoryStream = new MemoryStream();

            chartStream.CopyTo(memoryStream);
            byte[] chartbytes = memoryStream.ToArray();

            return chartbytes;
        }

        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(String.Format("unsupported DataLanguage: {0}", dataLanguage.ToString()))
            };
        #endregion
    }
}
