using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1.Fields;
using Mavim.Manager.Api.Topic.Services.v1.Models.Fields.Abstract;

namespace Mavim.Manager.Api.Topic.Services.v1.Models.Fields
{
    public class MultiRelationshipField : MultiField<IRelationshipElement>, IMultiRelationshipField { }
}