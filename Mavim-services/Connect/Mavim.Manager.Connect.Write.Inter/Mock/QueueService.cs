using Mavim.Manager.Connect.Write.ServiceBus.Interfaces;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Write.Inter.Mock
{
    public class BatchQueueClientMock : IBatchQueueClient
    {
        public string QueueName => throw new NotImplementedException();

        public int PrefetchCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReceiveMode ReceiveMode => throw new NotImplementedException();

        public string ClientId => throw new NotImplementedException();

        public bool IsClosedOrClosing => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public TimeSpan OperationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ServiceBusConnection ServiceBusConnection => throw new NotImplementedException();

        public bool OwnsConnection => throw new NotImplementedException();

        public IList<ServiceBusPlugin> RegisteredPlugins => throw new NotImplementedException();

        public Task AbandonAsync(string lockToken, IDictionary<string, object> propertiesToModify = null)
        {
            throw new NotImplementedException();
        }

        public Task CancelScheduledMessageAsync(long sequenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public Task CompleteAsync(string lockToken)
        {
            throw new NotImplementedException();
        }

        public Task DeadLetterAsync(string lockToken, IDictionary<string, object> propertiesToModify = null)
        {
            throw new NotImplementedException();
        }

        public Task DeadLetterAsync(string lockToken, string deadLetterReason, string deadLetterErrorDescription = null)
        {
            throw new NotImplementedException();
        }

        public void RegisterMessageHandler(Func<Message, CancellationToken, Task> handler, Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterMessageHandler(Func<Message, CancellationToken, Task> handler, MessageHandlerOptions messageHandlerOptions)
        {
            throw new NotImplementedException();
        }

        public void RegisterPlugin(ServiceBusPlugin serviceBusPlugin)
        {
            throw new NotImplementedException();
        }

        public void RegisterSessionHandler(Func<IMessageSession, Message, CancellationToken, Task> handler, Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterSessionHandler(Func<IMessageSession, Message, CancellationToken, Task> handler, SessionHandlerOptions sessionHandlerOptions)
        {
            throw new NotImplementedException();
        }

        public Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(Message message)
        {
            await Task.CompletedTask;
        }

        public async Task SendAsync(IList<Message> messageList)
        {
            await Task.CompletedTask;
        }

        public Task UnregisterMessageHandlerAsync(TimeSpan inflightMessageHandlerTasksWaitTimeout)
        {
            throw new NotImplementedException();
        }

        public void UnregisterPlugin(string serviceBusPluginName)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterSessionHandlerAsync(TimeSpan inflightSessionHandlerTasksWaitTimeout)
        {
            throw new NotImplementedException();
        }
    }

    public class QueueClientMock : IQueueClient
    {
        public QueueClientMock()
        {
        }

        public string QueueName => throw new NotImplementedException();

        public int PrefetchCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ReceiveMode ReceiveMode => throw new NotImplementedException();

        public string ClientId => throw new NotImplementedException();

        public bool IsClosedOrClosing => throw new NotImplementedException();

        public string Path => throw new NotImplementedException();

        public TimeSpan OperationTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ServiceBusConnection ServiceBusConnection => throw new NotImplementedException();

        public bool OwnsConnection => throw new NotImplementedException();

        public IList<ServiceBusPlugin> RegisteredPlugins => throw new NotImplementedException();

        public Task AbandonAsync(string lockToken, IDictionary<string, object> propertiesToModify = null)
        {
            throw new NotImplementedException();
        }

        public Task CancelScheduledMessageAsync(long sequenceNumber)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }

        public Task CompleteAsync(string lockToken)
        {
            throw new NotImplementedException();
        }

        public Task DeadLetterAsync(string lockToken, IDictionary<string, object> propertiesToModify = null)
        {
            throw new NotImplementedException();
        }

        public Task DeadLetterAsync(string lockToken, string deadLetterReason, string deadLetterErrorDescription = null)
        {
            throw new NotImplementedException();
        }

        public void RegisterMessageHandler(Func<Message, CancellationToken, Task> handler, Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterMessageHandler(Func<Message, CancellationToken, Task> handler, MessageHandlerOptions messageHandlerOptions)
        {
            throw new NotImplementedException();
        }

        public void RegisterPlugin(ServiceBusPlugin serviceBusPlugin)
        {
            throw new NotImplementedException();
        }

        public void RegisterSessionHandler(Func<IMessageSession, Message, CancellationToken, Task> handler, Func<ExceptionReceivedEventArgs, Task> exceptionReceivedHandler)
        {
            throw new NotImplementedException();
        }

        public void RegisterSessionHandler(Func<IMessageSession, Message, CancellationToken, Task> handler, SessionHandlerOptions sessionHandlerOptions)
        {
            throw new NotImplementedException();
        }

        public Task<long> ScheduleMessageAsync(Message message, DateTimeOffset scheduleEnqueueTimeUtc)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(Message message)
        {
            await Task.CompletedTask;
        }

        public async Task SendAsync(IList<Message> messageList)
        {
            await Task.CompletedTask;
        }

        public Task UnregisterMessageHandlerAsync(TimeSpan inflightMessageHandlerTasksWaitTimeout)
        {
            throw new NotImplementedException();
        }

        public void UnregisterPlugin(string serviceBusPluginName)
        {
            throw new NotImplementedException();
        }

        public Task UnregisterSessionHandlerAsync(TimeSpan inflightSessionHandlerTasksWaitTimeout)
        {
            throw new NotImplementedException();
        }
    }
}
