using System;

namespace Mavim.Manager.Connect.Read.Functions.Models
{
    public class ReceivedUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid? CompanyId { get; set; }
        public bool? IsActive { get; set; }
    }
}