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
    public static class GetTopicByCodeQuery 
    {
        // Query
        public record Query(string TopicCode) : IRequest<IReadOnlyList<ITopic>>;

        //Handler
        public class Handler : BaseHandler, IRequestHandler<GetTopicByCodeQuery.Query, IReadOnlyList<ITopic>>
        {
            public Handler(IBusiness.ITopicBusiness topicBusiness) : base(topicBusiness) { }

            public async Task<IReadOnlyList<ITopic>> Handle(GetTopicByCodeQuery.Query request, CancellationToken cancellationToken)
            {
                var topics = await _business.GetTopicsByCode(request.TopicCode);
                return topics.Select(t => TopicMapper.Map(t)).ToList();
            }
        }
    }
}