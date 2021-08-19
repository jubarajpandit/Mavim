namespace Mavim.Manager.Api.Topic.Business.v1.Mappers.Abstract
{
    internal abstract class GenericDateFieldMapper<TRepo, TBusiness> : GenericFieldMapper<TRepo, TBusiness> where TRepo : Repository.Interfaces.v1.Fields.IField where TBusiness : Interfaces.v1.Fields.IField
    {
    }
}