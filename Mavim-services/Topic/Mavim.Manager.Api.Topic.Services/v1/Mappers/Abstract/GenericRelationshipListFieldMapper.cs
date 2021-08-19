namespace Mavim.Manager.Api.Topic.Services.v1.Mappers.Abstract
{
    internal abstract class GenericRelationshipListFieldMapper<TBusiness, TService> : GenericFieldMapper<TBusiness, TService> where TBusiness : Business.Interfaces.v1.Fields.IField where TService : Interfaces.v1.Fields.IField
    {
    }
}