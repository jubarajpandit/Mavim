using Microsoft.Extensions.Logging;
using System;
using IModel = Mavim.Manager.Model;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericHyperlinkFieldMapper<TModel, TRepo> : GenericFieldMapper<TModel, TRepo> where TModel : IModel.ISimpleField where TRepo : IRepo.IField
    {
        private readonly ILogger _logger;

        public GenericHyperlinkFieldMapper(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger(GetType().Name) ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        protected Uri Map(object fieldValue)
        {
            if (Uri.TryCreate(fieldValue?.ToString(), UriKind.Absolute, out Uri result))
                return result;

            _logger.LogWarning($"Unable to map fieldvalue to URL.");
            return null;
        }
    }
}
