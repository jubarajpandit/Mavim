using Azure.Messaging.ServiceBus;
using System;

namespace Mavim.Manager.Connect.Read.Functions.Utils
{
    public static class MessageUtils
    {
        public static Guid GetGuidProperty(this ServiceBusReceivedMessage message, string key)
        {
            if (!message.ApplicationProperties.ContainsKey(key) || !Guid.TryParse(message.ApplicationProperties[key].ToString(), out Guid guidValue))
                return Guid.Empty;
            return guidValue;
        }

        public static int GetIntProperty(this ServiceBusReceivedMessage message, string key)
        {
            if (!message.ApplicationProperties.ContainsKey(key) || !int.TryParse(message.ApplicationProperties[key]?.ToString(), out int intValue))
                return -1;
            return intValue;
        }
    }
}
