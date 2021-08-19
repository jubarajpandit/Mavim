using Mavim.Libraries.Authorization.Interfaces;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Models;
using Mavim.Manager.Model;
using System;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class GenericRelationshipFieldMapper<TModel, TRepo> : GenericFieldMapper<TModel, TRepo> where TModel : ISimpleField where TRepo : IRepo.IField
    {
        private readonly IMavimDatabaseModel _model;

        public GenericRelationshipFieldMapper(IMavimDbDataAccess dataAccess)
        {
            _model = dataAccess?.DatabaseModel ?? throw new ArgumentNullException(nameof(dataAccess));
        }

        protected IRelationshipElement GetRelationFieldValue(IElement topic)
        {
            if (topic == null)
                return null;

            return new RelationshipElement
            {
                Dcv = topic.DcvID?.ToString(),
                Icon = topic.Visual?.IconResourceID.ToString("G"),
                Name = topic.Name
            };

        }

        /// <summary>
        /// Gets the element by DCV identifier.
        /// </summary>
        /// <param name="dcvId">The DCV identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">dcvId</exception>
        protected IElement GetElementByDcvId(string dcvId)
        {
            if (string.IsNullOrWhiteSpace(dcvId)) throw new ArgumentNullException(nameof(dcvId));

            IDcvId iDcvId = DcvId.FromDcvKey(dcvId);
            return _model.ElementRepository.GetElement(iDcvId);
        }
    }
}
