using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Functions.Models
{
    public class Group
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public IReadOnlyList<Guid> Ids { get; set; }
        public bool? IsActive { get; set; }
        public int AggregateId { get; set; }
        public int ModelVersion { get; set; }
    }
}