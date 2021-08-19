using System;

namespace Mavim.Manager.Connect.Read.Functions.Models
{
    public class ReceivedCompany
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public Guid? TenantId { get; set; }
        public bool? IsActive { get; set; }
    }
}