using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mavim.Libraries.Middlewares.WopiValidator.WopiProofKeyValidator
{
    public interface IProofKeyValidator
    {
        Task ValidateWopiProofKey(HttpContext context);
    }
}
