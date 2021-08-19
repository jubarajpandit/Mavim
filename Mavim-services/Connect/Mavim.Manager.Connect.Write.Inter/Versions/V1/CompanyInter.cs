using Mavim.Manager.Connect.Write.Database;
using Mavim.Manager.Connect.Write.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mavim.Manager.Connect.Write.Inter.Versions.V1
{
    public class CompanyInter
    {
        [Fact]
        [Trait("Category", "Connect AddCompanyCommand Write")]
        public async Task AddCompany_ValidArguments_ValidModelStateAsync()
        {
            // Arrange
            var context = GetMockContext();
            var tenantId = Guid.NewGuid();
            // Act
            var companyId = await HelperEventSourcing.CreateCompany(context, "Test", "test.nl", tenantId);

            // Assert
            var record = await context.EventSourcings.OrderByDescending(e => e.AggregateId).FirstOrDefaultAsync(e => e.EntityId == companyId);
            string payload = "{\"Name\":\"Test\",\"Domain\":\"test.nl\",\"TenantId\":\"" + tenantId + "\",\"Id\":\"" + companyId + "\",\"IsActive\":true}";
            var expectRecord = new EventSourcingModel(EventType.Create, 0, record.EntityId, EntityType.Company, 1, payload, record.TimeStamp, record.CompanyId);


            Assert.Equal(expectRecord, record);
        }

        private static ConnectDbContext GetMockContext()
        {
            var options = new DbContextOptionsBuilder<ConnectDbContext>()
                              .UseInMemoryDatabase(Guid.NewGuid().ToString())
                              .Options;

            var context = new ConnectDbContext(options);

            return context;
        }
    }
}
