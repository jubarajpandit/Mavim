using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Read.Functions.Models
{
    public class ReceivedGroup
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? CompanyId { get; set; }
        public IReadOnlyList<Guid> UserIds { get; set; }
        public bool? IsActive { get; set; }
    }
}