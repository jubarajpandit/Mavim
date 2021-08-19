using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Write.V1.DTO
{
    /// <summary>
    /// Add User DTO
    /// </summary>
    public record AddUserDto
    {
        /// <summary>
        /// User Email Address
        /// </summary>
        [Required]
        public string Email { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email"></param>
        public AddUserDto([Required] string email) => (Email) = (email);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="email"></param>
        public void Deconstruct(out string email)
        {
            email = Email;
        }

    }
}