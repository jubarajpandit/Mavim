using System;
using Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Topic
{
    public abstract class BaseHandler
    {
        /// <summary>
        /// _business
        /// </summary>
        protected readonly ITopicBusiness _business;

        /// <summary>
        /// BaseHandler
        /// </summary>
        /// <param name="business"></param>
        public BaseHandler(ITopicBusiness business)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
        }
    }
}
