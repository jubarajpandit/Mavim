using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips;
using Mavim.Manager.Api.Topic.Repository.v1.Fields.Base;

namespace Mavim.Manager.Api.Topic.Repository.v1.Fields
{
    public class MultiRelationshipField : MultiField<IRelationshipElement>, IMultiRelationshipField { }
}