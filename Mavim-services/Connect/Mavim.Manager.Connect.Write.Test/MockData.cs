using Mavim.Manager.Connect.Write.Database.Models;
using System;
using System.Collections.Generic;

namespace Mavim.Manager.Connect.Write.Test
{
    public static class MockData
    {
        public static readonly Guid companyId = new Guid("f0fd2957-ddca-40c2-bc87-7dc9029ad2d3");
        public static readonly Guid tenantId = new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6");
        public static readonly Guid userEntityId1 = new Guid("385568ba-94b0-46f9-b1e3-1fb00d0f4493");
        public static readonly Guid userEntityId2 = new Guid("ffd3efe2-822c-4a28-a153-2b3d4ad85d84");
        public static readonly Guid userEntityId3 = new Guid("5cdeae96-98a6-41e3-b94a-6677e55d928f");
        public static readonly Guid companyEntityId1 = new Guid("e9b663bc-3a77-4075-825d-68d5f8e65575");
        public static readonly Guid companyEntityId2 = new Guid("b561fe27-4c26-4326-8460-baf361e884f6");
        public static readonly Guid companyEntityId3 = new Guid("4b6b9d4b-99d2-4e6b-b858-1f0042d83208");
        public static readonly Guid groupEntityId1 = new Guid("3a5abf41-a2b5-491e-aa7d-8487513ba7d6");
        public static readonly Guid groupEntityId2 = new Guid("d0f77ff8-aecd-458c-88ab-d4d9a780762b");
        public static readonly Guid groupEntityId3 = new Guid("919edf76-7cfd-4d4c-8973-d9e8e2f9d562");
        public static readonly EventSourcingModel EmptyMock = new EventSourcingModel(EventType.Create, 0, Guid.Empty, EntityType.User, 0, string.Empty, DateTime.MinValue, Guid.Empty);

        public static readonly List<EventSourcingModel> mockCompanyEvents = new()
        {
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = companyEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.Company,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + companyEntityId1 + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.nl\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 1,
                CompanyId = companyId,
                EntityId = companyEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.Company,
                EventType = EventType.Delete,
                Payload = "{\"IsActive\": false}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = companyEntityId2,
                EntityModelVersion = 1,
                EntityType = EntityType.Company,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + companyEntityId2 + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.nl\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 11, 15)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = companyEntityId3,
                EntityModelVersion = 1,
                EntityType = EntityType.Company,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + companyEntityId3 + "\",\"Name\":\"Mavim\",\"Domain\":\"mavim.nl\",\"TenantId\":\"" + tenantId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 34)
            },
        };

        public static readonly List<EventSourcingModel> mockUserEvents = new()
        {
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = userEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.User,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + userEntityId1 + "\",\"Email\":\"mah@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 1,
                CompanyId = companyId,
                EntityId = userEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.User,
                EventType = EventType.Delete,
                Payload = "{\"IsActive\": false}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = userEntityId2,
                EntityModelVersion = 1,
                EntityType = EntityType.User,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + userEntityId2 + "\",\"Email\":\"sna@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 11, 15)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = userEntityId3,
                EntityModelVersion = 1,
                EntityType = EntityType.User,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + userEntityId3 + "\",\"Email\":\"rno@mavim.com\",\"CompanyId\":\"" + companyId + "\",\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 34)
            },
        };

        public static readonly List<EventSourcingModel> mockGroupEvents = new()
        {
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = groupEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.Group,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + groupEntityId1 + "\",\"Name\":\"Mavim Group\",\"Description\":\"Mavim Group Description\",\"CompanyId\":\"" + companyId + "\",\"UserIds\": [\"385568ba-94b0-46f9-b1e3-1fb00d0f4493\"],\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 1,
                CompanyId = companyId,
                EntityId = groupEntityId1,
                EntityModelVersion = 1,
                EntityType = EntityType.Group,
                EventType = EventType.Delete,
                Payload = "{\"IsActive\": false}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 51)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = groupEntityId2,
                EntityModelVersion = 1,
                EntityType = EntityType.Group,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + groupEntityId2 + "\",\"Name\":\"Mavim Group 2\",\"Description\":\"Mavim Group 2 Description\",\"CompanyId\":\"" + companyId + "\",\"UserIds\": [\"ffd3efe2-822c-4a28-a153-2b3d4ad85d84\",\"5cdeae96-98a6-41e3-b94a-6677e55d928f\"],\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 11, 15)
            },
            EmptyMock with
            {
                AggregateId = 0,
                CompanyId = companyId,
                EntityId = groupEntityId3,
                EntityModelVersion = 1,
                EntityType = EntityType.Group,
                EventType = EventType.Create,
                Payload = "{\"Id\":\"" + groupEntityId3 + "\",\"Name\":\"Mavim 3 Group\",\"Description\":\"Mavim Group 3 Description\",\"CompanyId\":\"" + companyId + "\",\"UserIds\": [],\"IsActive\":true}",
                TimeStamp = new DateTime(2021, 02, 11, 14, 10, 34)
            },
        };
    }
}
