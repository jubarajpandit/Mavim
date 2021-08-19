using System.Text.Json;

namespace Mavim.Manager.Connect.Read.Commands
{
    public abstract class BaseHandler
    {
        protected static T Map<T>(string value) => value is null ? default : JsonSerializer.Deserialize<T>(value);

        protected static string Map(object value) => value is null ? null : JsonSerializer.Serialize(value);
    }
}
