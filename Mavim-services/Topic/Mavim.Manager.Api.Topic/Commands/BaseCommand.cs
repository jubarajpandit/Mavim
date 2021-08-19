using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Libraries.Middlewares.Language.Enums;
using Mavim.Libraries.Middlewares.Language.Interfaces;
using Mavim.Manager.Model;
using Mavim.Manager.Server;
using System;

namespace Mavim.Manager.Api.Topic.Commands
{
    /// <summary>
    /// BaseCommand
    /// </summary>
    public abstract class BaseCommand
    {
        /// <summary>
        /// _model
        /// </summary>
        protected readonly IMavimDatabaseModel _model;

        /// <summary>
        /// BaseCommand
        /// </summary>
        /// <param name="dataAccess"></param>
        /// <param name="dataLanguage"></param>
        public BaseCommand(IMavimDbDataAccess dataAccess, IDataLanguage dataLanguage)
        {
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
            _model.DataLanguage = new Language(Map(dataLanguage.Type));
        }

        private static LanguageSupport.MvmSRV_Lang Map(DataLanguageType dataLanguage) =>
            dataLanguage switch
            {
                DataLanguageType.Dutch => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_NL,
                DataLanguageType.English => LanguageSupport.MvmSRV_Lang.MvmSRV_Lang_EN,
                _ => throw new ArgumentException(string.Format($"unsupported DataLanguage: {dataLanguage}"))
            };
    }
}
