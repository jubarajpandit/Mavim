using Mavim.Manager.Api.Authorization.Services.Interfaces.v1;
using Mavim.Manager.Api.Utils.CustomDataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Authorization.Controllers.v1
{
    public class User
    {
        [Required]
        [EmailValidator(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required]
        public Role Role { get; set; }
    }
}
