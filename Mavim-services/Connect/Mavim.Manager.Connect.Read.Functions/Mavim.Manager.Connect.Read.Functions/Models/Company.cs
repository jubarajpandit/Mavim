using System;

namespace Mavim.Manager.Connect.Read.Functions.Models
{
    public class Company
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public Guid? TenantId { get; set; }
        public bool? IsActive { get; set; }
        public int AggregateId { get; set; }
        public int ModelVersion { get; set; }
    }
}