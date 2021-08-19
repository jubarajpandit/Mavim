using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Authorization.Controllers.v1
{
    public class EmailPatchBody
    {
        [Required]
        public Role Role { get; set; }
    }
}
