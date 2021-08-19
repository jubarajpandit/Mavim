using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Model;

namespace Mavim.Manager.Api.Topic.Repository.v1.Mappers.Abstract
{
    internal abstract class FieldMapperBase
    {
        internal abstract IField GetMappedRepoField(ISimpleField field);

        internal abstract object[] ConvertToArrayObject(IField field, ISimpleField simpleField = null);
    }
}
