using Mavim.Manager.Connect.Read.Databases.Interfaces;
using Mavim.Manager.Connect.Read.Databases.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mavim.Manager.Connect.Read.Databases
{
    public class ConnectRepository : IConnectRepository
    {
        private readonly IConnectConnection _connection;

        public ConnectRepository(IConnectConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<IDiscoveryUser> GetDiscoveryUser(string email, Guid tenantId)
        {
            var query = @"SELECT * FROM [dbo].[DiscoveryUsers] 
                            WHERE [Email] = @EMAIL
                            AND [TenantId] = @TENANTID";

            using var command = new SqlCommand(query);
            command.Parameters.Add("@EMAIL", SqlDbType.NVarChar).Value = email;
            command.Parameters.Add("@TENANTID", SqlDbType.UniqueIdentifier).Value = tenantId;
            var result = await _connection.ExecuteQueryAsync(command);

            var row = result.Rows?.Cast<DataRow>().SingleOrDefault();
            var user = MapToDiscoveryUser(row);

            return user;
        }

        public async Task<UserTable> GetUser(Guid userId)
        {
            var query = @"SELECT * FROM [dbo].[Users] 
                            WHERE [Id] = @USERID";
            using var command = new SqlCommand(query);
            command.Parameters.Add("@USERID", SqlDbType.UniqueIdentifier).Value = userId;
            var result = await _connection.ExecuteQueryAsync(command);

            var row = result.Rows?.Cast<DataRow>().SingleOrDefault();
            var userTable = MapToUserTable(row);

            return userTable;
        }

        public async Task<CompanyTable> GetCompany(Guid companyId)
        {
            var query = @"SELECT * FROM [dbo].[Companies] 
                            WHERE [Id] = @COMPANYID";

            using var command = new SqlCommand(query);
            command.Parameters.Add("@COMPANYID", SqlDbType.UniqueIdentifier).Value = companyId;
            var result = await _connection.ExecuteQueryAsync(command);

            var row = result.Rows?.Cast<DataRow>().SingleOrDefault();
            var companyTable = MapToCompanyTable(row);

            return companyTable;
        }

        public async Task<IReadOnlyList<UserTable>> GetCompanyUsers(Guid companyId)
        {
            var query = @"SELECT * FROM [dbo].[Users] 
                            WHERE [CompanyId] = @COMPANYID";

            using var command = new SqlCommand(query);
            command.Parameters.Add("@COMPANYID", SqlDbType.UniqueIdentifier).Value = companyId;
            var result = await _connection.ExecuteQueryAsync(command);

            var userTables = result.Rows?.Cast<DataRow>().Select(MapToUserTable);

            return userTables?.ToList() ?? default;
        }

        public async Task<IReadOnlyList<GroupTable>> GetCompanyGroups(Guid companyId)
        {
            var query = @"SELECT * FROM [dbo].[Groups] 
                            WHERE [CompanyId] = @COMPANYID";

            using var command = new SqlCommand(query);
            command.Parameters.Add("@COMPANYID", SqlDbType.UniqueIdentifier).Value = companyId;
            var result = await _connection.ExecuteQueryAsync(command);

            var groupTables = result.Rows?.Cast<DataRow>().Select(MapToGroupTable);

            return groupTables?.ToList() ?? default;
        }

        public async Task<GroupTable> GetGroup(Guid groupId)
        {
            var query = @"SELECT * FROM [dbo].[Groups] 
                            WHERE [Id] = @GROUPID";

            using var command = new SqlCommand(query);
            command.Parameters.Add("@GROUPID", SqlDbType.UniqueIdentifier).Value = groupId;
            var result = await _connection.ExecuteQueryAsync(command);

            var row = result.Rows?.Cast<DataRow>().SingleOrDefault();
            var groupTable = MapToGroupTable(row);

            return groupTable;
        }

        public async Task<IReadOnlyList<GroupTable>> GetGroups(IEnumerable<Guid> groupIds)
        {
            if (!groupIds.Any()) return Enumerable.Empty<GroupTable>().ToList();

            using var command = new SqlCommand();

            var groupList = groupIds.ToList();
            string inClause = "";
            for (int index = 0; index < groupList.Count; index++)
            {
                inClause += $"@GROUPID{index},";
                command.Parameters.AddWithValue($"@GROUPID{index}", groupList[index]);
            }

            // remove last comma from inClause
            inClause = inClause.Remove(inClause.Length - 1, 1);

            command.CommandText = $"SELECT * FROM [dbo].[Groups] WHERE [Id] IN ({inClause})";

            var result = await _connection.ExecuteQueryAsync(command);

            var groupTables = result.Rows?.Cast<DataRow>().Select(MapToGroupTable);

            return groupTables?.ToList() ?? default;
        }

        public async Task<IReadOnlyList<UserTable>> GetUsers(IEnumerable<Guid> userIds)
        {
            if (!userIds.Any()) return Enumerable.Empty<UserTable>().ToList();

            using var command = new SqlCommand();
            var userList = userIds.ToList();
            string inClause = "";
            for (int index = 0; index < userList.Count; index++)
            {
                inClause += $"@USERID{index},";
                command.Parameters.AddWithValue($"@USERID{index}", userList[index]);
            }

            // remove last comma from inClause
            inClause = inClause.Remove(inClause.Length - 1, 1);

            command.CommandText = $"SELECT * FROM [dbo].[Users] WHERE [Id] IN ({inClause})";

            var result = await _connection.ExecuteQueryAsync(command);

            var userTables = result.Rows?.Cast<DataRow>().Select(MapToUserTable);

            return userTables?.ToList() ?? default;
        }

        private static CompanyTable MapToCompanyTable(DataRow row) =>
            row is null
                ? null
                : new CompanyTable(
                    row.Field<Guid>("Id"),
                    row.Field<string>("Value"),
                    row.Field<int>("ModelVersion"),
                    row.Field<int>("AggregateId"),
                    row.Field<Guid>("CompanyId"),
                    row.Field<bool>("Disabled"),
                    row.Field<DateTime>("LastUpdated")
                );

        private static UserTable MapToUserTable(DataRow row) =>
            row is null
                ? null
                : new UserTable(
                    row.Field<Guid>("Id"),
                    row.Field<string>("Value"),
                    row.Field<int>("ModelVersion"),
                    row.Field<int>("AggregateId"),
                    row.Field<Guid>("CompanyId"),
                    row.Field<bool>("Disabled"),
                    row.Field<DateTime>("LastUpdated")
                );

        private static GroupTable MapToGroupTable(DataRow row) =>
            row is null
                ? null
                : new GroupTable(
                    row.Field<Guid>("Id"),
                    row.Field<string>("Value"),
                    row.Field<int>("ModelVersion"),
                    row.Field<int>("AggregateId"),
                    row.Field<Guid>("CompanyId"),
                    row.Field<bool>("Disabled"),
                    row.Field<DateTime>("LastUpdated")
                );

        private static IDiscoveryUser MapToDiscoveryUser(DataRow row) =>
            row is null
                ? null
                : new DiscoveryUser(
                    row.Field<Guid>("Id"),
                    row.Field<string>("Email"),
                    row.Field<Guid>("TenantId"),
                    row.Field<bool>("Disabled"),
                    row.Field<DateTime>("LastUpdated")
                );
    }
}
