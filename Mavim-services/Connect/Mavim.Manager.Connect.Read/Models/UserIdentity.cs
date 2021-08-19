using Mavim.Manager.Connect.Read.Models.Interfaces;
using System;

namespace Mavim.Manager.Connect.Read.Models
{
    public class UserIdentity : IUserIdentity
    {
        public Guid Id { get; set; }
    }
}
