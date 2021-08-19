using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Mavim.Manager.Api.Topic.Services.Interfaces.v1;
using Mavim.Manager.Api.Topic.Services.v1.Mappers;
using IBusiness = Mavim.Manager.Api.Topic.Business.Interfaces.v1;

namespace Mavim.Manager.Topic.Queries
{    
    public static class GetTopicCustomIconByCustomIconIdQuery
    {
        // Query
        public record Query(string TopicCustoIconId) : IRequest<byte[]>;

        //Handler
        public class Handler : BaseHandler, IRequestHandler<GetTopicCustomIconByCustomIconIdQuery.Query, byte[]>
        {
            public Handler(IBusiness.ITopicBusiness topicBusiness) : base(topicBusiness) { }

            public async Task<byte[]> Handle(GetTopicCustomIconByCustomIconIdQuery.Query request, CancellationToken cancellationToken)
            {
                return await _business.GetTopicCustomIconByCustomIconId(request.TopicCustoIconId);
            }
        }
    }
}
