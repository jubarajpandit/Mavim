using System.Text.Json;

namespace Mavim.Manager.Connect.Read.Queries
{
    public abstract class BaseHandler
    {
        protected static T Map<T>(string value) => value is null ? default : JsonSerializer.Deserialize<T>(value);
    }
}
