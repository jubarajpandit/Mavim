using Mavim.Manager.Api.Connect.Read.Validators;
using System;
using System.Collections.Generic;

namespace Mavim.Manager.Api.Connect.Read.Versions.V1.DTO
{
    /// <summary>
    /// Ids DTO
    /// </summary>
    public record UserIdsDTO(int ModelVersion, int AggregateId, IEnumerable<Guid> Ids) : MetaDTO(ModelVersion, AggregateId)
    {
        /// <summary>
        /// UserIds
        /// </summary>
        [RequiredGuid]
        public IEnumerable<Guid> Ids { get; init; } = Ids;
    };
}