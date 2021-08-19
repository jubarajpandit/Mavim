using Mavim.Manager.Api.Topic.Commands;
using Mavim.Manager.Api.Topic.Commands.Interfaces;
using Mavim.Manager.Api.Topic.Queries;
using Mavim.Manager.Api.Topic.Queries.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// CommandsExtension
    /// </summary>
    public static class CommandsExtension
    {
        /// <summary>
        /// AddCommands
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            services.AddTransient<IMoveToTopCommand, MoveToTopCommand>();
            services.AddTransient<IMoveToBottomCommand, MoveToBottomCommand>();
            services.AddTransient<IMoveUpCommand, MoveUpCommand>();
            services.AddTransient<IMoveDownCommand, MoveDownCommand>();
            services.AddTransient<IMoveLevelUpCommand, MoveLevelUpCommand>();
            services.AddTransient<IMoveLevelDownCommand, MoveLevelDownCommand>();
            services.AddTransient<IQueryRetrieveTopicTypesCommand, QueryRetrieveTopicTypesCommand>();
            services.AddTransient<IQueryRetrieveTopicIconsCommand, QueryRetrieveTopicIconsCommand>();
            services.AddTransient<ICreateTopicAfterCommand, CreateTopicAfterCommand>();
            services.AddTransient<ICreateChildTopicCommand, CreateChildTopicCommand>();
            services.AddTransient<IDeleteTopicCommand, DeleteTopicCommand>();
            services.AddTransient<IChangeParentCommand, ChangeParentCommand>();

            return services;
        }
    }
}
