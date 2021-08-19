using Mavim.Manager.Api.ChangelogField.Services.Interfaces.v1;

namespace Mavim.Manager.Api.ChangelogField.Services.v1.Model
{
    public class RelationshipElement : IRelationshipElement
    {
        public string Dcv { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
    }
}
