using MediatR;
using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Api.FeatureFlag.Queries
{
    public static class GetActiveFeatureFlags
    {
        // Command
        public record Query() : IRequest<IReadOnlyList<string>>;

        // Handler
        public class Handler : IRequestHandler<Query, IReadOnlyList<string>>
        {
            private readonly IFeatureManager _featureManager;

            public Handler(IFeatureManager featureManager)
            {
                _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
            }

            public async Task<IReadOnlyList<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                IAsyncEnumerable<string> featureFlagNames = _featureManager.GetFeatureNamesAsync();
                List<string> featureFlags = await GetActiveFeatureFlags(featureFlagNames);

                return featureFlags;
            }

            private async Task<List<string>> GetActiveFeatureFlags(IAsyncEnumerable<string> featureFlagNames)
            {
                List<string> featureFlags = new();

                await foreach (string name in featureFlagNames)
                {
                    if (await _featureManager.IsEnabledAsync(name))
                        featureFlags.Add(name);
                }

                return featureFlags;
            }
        }

    }

}
