using System;
using System.ComponentModel.DataAnnotations;

namespace Mavim.Manager.Connect.Write.Database.Models
{
    public record EventSourcingModel
    {
        [Required] public EventType EventType { get; set; }
        [Required] public int AggregateId { get; set; } // Ordering of entity change
        [Required] public Guid EntityId { get; set; } // Entity ID 12345
        [Required] public EntityType EntityType { get; set; } // Identiity, Groups, Company
        [Required] public int EntityModelVersion { get; set; } // 1, 2, 3
        [Required] public string Payload { get; set; } // { "name": "Marco" }
        [Required] public DateTime TimeStamp { get; set; }
        [Required] public Guid CompanyId { get; set; }

        public EventSourcingModel(
                EventType eventType,
                int aggregateId,
                Guid entityId,
                EntityType entityType,
                int entityModelVersion,
                string payload,
                DateTime timeStamp,
                Guid companyId
            ) => (
                EventType,
                AggregateId,
                EntityId,
                EntityType,
                EntityModelVersion,
                Payload,
                TimeStamp,
                CompanyId
            ) = (
                eventType,
                aggregateId,
                entityId,
                entityType,
                entityModelVersion,
                payload,
                timeStamp,
                companyId
            );

        public void Deconstruct(
            out EventType EventType,
            out int AggregateId,
            out Guid EntityId,
            out EntityType EntityType,
            out int EntityModelVersion,
            out string Payload,
            out DateTime TimeStamp,
            out Guid CompanyId)
        {
            EventType = this.EventType;
            AggregateId = this.AggregateId;
            EntityId = this.EntityId;
            EntityType = this.EntityType;
            EntityModelVersion = this.EntityModelVersion;
            Payload = this.Payload;
            TimeStamp = this.TimeStamp;
            CompanyId = this.CompanyId;
        }
    }

    //public class EventSourcingModel
    //{
    //    [Required]
    //    // Event
    //    public EventType EventType { get; set; }

    //    [Required] public int AggregateId { get; set; } // Ordering of entity change

    //    // Entity
    //    [Required] public Guid EntityId { get; set; } // Entity ID 12345

    //    [Required] public EntityType EntityType { get; set; } // Identiity, Groups, Company

    //    [Required] public int EntityModelVersion { get; set; } // 1, 2, 3

    //    [Required] public string Payload { get; set; } // { "name": "Marco" }

    //    // Meta
    //    [Required] public DateTime TimeStamp { get; set; }

    //    [Required] public Guid CompanyId { get; set; }

    //    internal void Deconstruct(
    //        out EventType EventType,
    //        out int AggregateId,
    //        out Guid EntityId,
    //        out EntityType EntityType,
    //        out int EntityModelVersion,
    //        out string Payload,
    //        out DateTime TimeStamp,
    //        out Guid CompanyId)
    //    {
    //        EventType = this.EventType;
    //        AggregateId = this.AggregateId;
    //        EntityId = this.EntityId;
    //        EntityType = this.EntityType;
    //        EntityModelVersion = this.EntityModelVersion;
    //        Payload = this.Payload;
    //        TimeStamp = this.TimeStamp;
    //        CompanyId = this.CompanyId;
    //    }
    //}
}