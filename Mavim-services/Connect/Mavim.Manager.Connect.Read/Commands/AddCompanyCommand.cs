using Mavim.Libraries.Middlewares.ExceptionHandler.Exceptions;
using Mavim.Manager.Connect.Read.Constants;
using Mavim.Manager.Connect.Read.Databases;
using Mavim.Manager.Connect.Read.Databases.Models;
using Mavim.Manager.Connect.Read.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Commands
{
    public static class AddCompanyCommand
    {
        // Command
        public record Command(Guid CompanyId, string Name, string Domain, Guid TenantId, int ModelVersion, int AggregateId) : IRequest;

        // Handler
        public class Handler : BaseHandler, IRequestHandler<Command, Unit>
        {
            private readonly ConnectDatabaseContext _dbContext;

            public Handler(ConnectDatabaseContext dbContext)
            {
                _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var (companyId, _, _, _, _, aggregateId) = request ?? throw new ArgumentNullException(nameof(request));

                if (aggregateId != 0)
                    throw new UnprocessableEntityException(
                        string.Format(Logging.INCORRECT_AGGREGATEID, aggregateId, 0),
                        aggregateId > 0 ? (int)ErrorCode.AggregateIdHigher : (int)ErrorCode.AggregateIdLower
                        );

                var company = await _dbContext.Companies.FirstOrDefaultAsync(x => x.Id == companyId, cancellationToken);
                if (company is not null) throw new UnprocessableEntityException(string.Format(Logging.COMPANY_ALREADY_EXISTS, request.CompanyId));

                _dbContext.Companies.Add(Map(request));
                await _dbContext.SaveChangesAsync(cancellationToken);

                return await Task.FromResult(Unit.Value);
            }

            private static CompanyTable Map(Command command) => new(command.CompanyId, Map(MapToCompanyValue(command)), command.ModelVersion, command.AggregateId, command.CompanyId, false, DateTime.Now);
            private static CompanyValue MapToCompanyValue(Command command) => new(command.CompanyId, command.Name, command.Domain, command.TenantId);
        }
    }
}
