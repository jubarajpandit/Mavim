using Mavim.Manager.Authorization.Read.Databases.Interfaces;
using Mavim.Manager.Authorization.Read.Databases.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Authorization.Read.Databases
{
    public class Repository : IRepository
    {
        private readonly IConnection _connection;

        public Repository(IConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<IReadOnlyList<Role>> GetRoles(Guid companyId)
        {
            var query = @"SELECT * FROM [dbo].[Roles] WHERE [CompanyId] = @COMPANYID";
            using var command = new SqlCommand(query);
            command.Parameters.Add("@COMPANYID", SqlDbType.UniqueIdentifier).Value = companyId;
            var result = await _connection.ExecuteQueryAsync(command);

            var roles = result.Rows?.Cast<DataRow>()?.Select(MapToRolesTable).ToList();

            return roles ?? default;
        }

        private static Role MapToRolesTable(DataRow row) => new(
                    row.Field<Guid>("Id"),
                    row.Field<int>("AggregateId"),
                    row.Field<string>("Name"),
                    row.Field<string>("Groups")?.Split(',', StringSplitOptions.None).Select(s => Guid.Parse(s)).ToArray(),
                    row.Field<string>("TopicPermissions")?.Split(',', StringSplitOptions.None).Select(s => Guid.Parse(s)).ToArray(),
                    row.Field<Guid>("CompanyId"),
                    row.Field<bool>("Disabled"),
                    row.Field<DateTime>("LastUpdated")
                );

    }
}
