using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Fields.Base;
using IRepo = Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields
{
    public class SingleRelationshipField : SingleField<IRelationshipElement>, IRepo.ISingleRelationshipField { }
}
