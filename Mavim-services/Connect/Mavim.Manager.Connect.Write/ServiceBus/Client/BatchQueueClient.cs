using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using Microsoft.Azure.ServiceBus;

namespace Mavim.Manager.Connect.Write.ServiceBus.Client
{
    public class BatchQueueClient : QueueClient, IBatchQueueClient
    {
        public BatchQueueClient(ServiceBusConnectionStringBuilder connectionStringBuilder, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null)
            : base(connectionStringBuilder, receiveMode, retryPolicy) { }

        public BatchQueueClient(string connectionString, string entityPath, ReceiveMode receiveMode = ReceiveMode.PeekLock, RetryPolicy retryPolicy = null)
            : base(connectionString, entityPath, receiveMode, retryPolicy) { }
    }
}
