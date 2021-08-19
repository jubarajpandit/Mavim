using System;

namespace Mavim.Manager.Authorization.Read.Databases.Models
{
    public class TopicPermission : Permission
    {
        public string Name { get; set; }
        public Guid Database { get; set; }
        public string TopicID { get; set; }
        public TopicPermissionsType Permission { get; set; }
    }
}
