using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Mavim.Manager.Api.Topic.Extensions
{
    /// <summary>
    /// Adds default responses
    /// </summary>
    public class ProduceResponseTypeModelProvider : IApplicationModelProvider
    {
        /// <summary>
        /// 
        /// </summary>
        public int Order => 3;

        /// <summary>
        /// OnProvidersExecuted
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuted(ApplicationModelProviderContext context) { }

        /// <summary>
        /// Add default responses to all the actions
        /// </summary>
        /// <param name="context"></param>
        public void OnProvidersExecuting(ApplicationModelProviderContext context)
        {
            foreach (ControllerModel controller in context.Result.Controllers)
            {
                foreach (ActionModel action in controller.Actions)
                {
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status401Unauthorized));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status400BadRequest));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status404NotFound));
                    action.Filters.Add(new ProducesResponseTypeAttribute(typeof(void), StatusCodes.Status500InternalServerError));
                }
            }
        }
    }
}
