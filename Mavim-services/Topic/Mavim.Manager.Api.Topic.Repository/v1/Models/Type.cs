using Mavim.Manager.Api.Topic.Repository.Interfaces.v1.Topics;

namespace Mavim.Manager.Api.Topic.Repository.v1.Models
{
    public class Type : IType
    {
        public bool HasSystemName { get; set; }
        public bool IsImportedVersionsRoot { get; set; }
        public bool IsImportedVersion { get; set; }
        public bool IsCreatedVersionsRoot { get; set; }
        public bool IsCreatedVersion { get; set; }
        public bool IsRecycleBin { get; set; }
        public bool IsRelationshipsCategoriesRoot { get; set; }
        public bool IsExternalReferencesRoot { get; set; }
        public bool IsObjectsRoot { get; set; }
    }
}
