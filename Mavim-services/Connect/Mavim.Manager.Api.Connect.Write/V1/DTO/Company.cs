using Mavim.Manager.Api.Connect.Write.Validators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Api.Connect.Write.V1.DTO
{
    /// <summary>
    /// Add Company Dto
    /// </summary>
    public record AddCompanyDto
    {
        /// <summary>
        /// Company Name
        /// </summary>
        [Required]
        public string Name { get; init; }

        /// <summary>
        /// Company Domain
        /// </summary>
        [Required]
        public string Domain { get; init; }

        /// <summary>
        /// Company TenantId
        /// </summary>
        [RequiredGuid]
        public Guid TenantId { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="tenantId"></param>
        public AddCompanyDto([Required] string name, string domain, Guid tenantId) => (Name, Domain, TenantId) = (name, domain, tenantId);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="name"></param>
        /// <param name="domain"></param>
        /// <param name="tenantId"></param>
        public void Deconstruct(out string name, out string domain, out Guid tenantId)
        {
            name = Name;
            domain = Domain;
            tenantId = TenantId;
        }

    };

    /// <summary>
    /// Undo EventSourcing Dto
    /// </summary>
    public record UndoEventSourcingDto
    {
        /// <summary>
        /// Start Date to undo all events in the database
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startDate"></param>
        public UndoEventSourcingDto([Required] DateTime startDate) => (StartDate) = (startDate);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="startDate"></param>
        public void Deconstruct(out DateTime startDate)
        {
            startDate = StartDate;
        }

    }

    /// <summary>
    /// Resend EventSourcing Dto
    /// </summary>
    public record ResendEventSourcingDto
    {
        /// <summary>
        /// Start AggregateId from the entity to send events
        /// </summary>
        public int AggregateId { get; init; }

        /// <summary>
        /// Start AggregateId from the entity to send events
        /// </summary>
        public Guid EntityId { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="entityId"></param>
        public ResendEventSourcingDto([Required] int aggregateId, [Required] Guid entityId) => (AggregateId, EntityId) = (aggregateId, entityId);

        /// <summary>
        /// Deconstruct
        /// </summary>
        /// <param name="aggregateId"></param>
        /// <param name="entityId"></param>
        public void Deconstruct(out int aggregateId, out Guid entityId)
        {
            aggregateId = AggregateId;
            entityId = EntityId;
        }

    }
}