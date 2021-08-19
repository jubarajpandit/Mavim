namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.RelationShips
{
    public interface IRelationshipElement
    {
        string Dcv { get; set; }
        string Name { get; set; }
        string Icon { get; set; }
        bool IsPublic { get; set; }
    }
}
