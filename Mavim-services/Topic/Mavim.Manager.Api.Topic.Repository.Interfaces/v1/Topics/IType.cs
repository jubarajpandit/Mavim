namespace Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics
{
    public interface IType
    {
        bool HasSystemName { get; set; }
        bool IsImportedVersionsRoot { get; set; }
        bool IsImportedVersion { get; set; }
        bool IsCreatedVersionsRoot { get; set; }
        bool IsCreatedVersion { get; set; }
        bool IsRecycleBin { get; set; }
        bool IsRelationshipsCategoriesRoot { get; set; }
        bool IsExternalReferencesRoot { get; set; }
        bool IsObjectsRoot { get; set; }
    }
}
