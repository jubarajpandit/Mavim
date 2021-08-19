# Cheat Sheet 🤡

**_Kubernetes commands_**
**namespaces**: _mavim-private, mavim-public_

- kubectl get pods -n {namespaces}
- kubectl get pods --all-namespaces
- kubectl get svc -n {namespaces}
- kubectl get ingress -n {namespaces}
- kubectl get azureidentity
- kubectl get azureidentitybinding

with every resource you can use kubectl describe {resource}
if you want to debug pods you can execute kubectl logs -f pod {name} -n {namespaces}

# Management Identity

When you experience this error:

    Microsoft.Data.SqlClient.SqlException (0x80131904): Login failed for user '<token-identified principal>'.

It could be multiple things:

1.  Connection string
2.  TenantId
3.  Application ID
4.  Management Identity user corrupt (Database permissions)

### To solve number 4:

To check what is currently available please execute the following code on the database.

    SELECT members.name as 'members_name', roles.name as 'roles_name',roles.type_desc as 'roles_desc',members.type_desc as 'members_desc'
    FROM sys.database_role_members rolemem
    INNER JOIN sys.database_principals roles
    ON rolemem.role_principal_id = roles.principal_id
    INNER JOIN sys.database_principals members
    ON rolemem.member_principal_id = members.principal_id
    ORDER BY members.name

This query returns you the managed identity with their attached roles.
The following roles are mandatory for the managed identity to function:

1.  **db_ddladmin**
2.  **db_datareader**
3.  **db_datawriter**

Incase you like to remove this user + roles because somehow this user is corrupted. please execute the following code:

_(In this code example we do it for the management identity 'mav_test_azmi_databaseinfo')_

    EXEC sp_droprolemember db_datareader, 'mav_test_azmi_databaseinfo';
    EXEC sp_droprolemember db_datawriter, 'mav_test_azmi_databaseinfo';
    EXEC sp_droprolemember db_ddladmin, 'mav_test_azmi_databaseinfo';
    DROP USER [mav_test_azmi_databaseinfo]

Please release the Resources pipeline to grand new permissions for your management identity.
