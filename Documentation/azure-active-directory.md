#Azure Active Directory
Azure Active Directory (AAD) is used as the indentity provider.

##Groups
For every customer we create a group. This group is given access to the Azure SQL database, where the data is stored.
In this group we add the employees/users of the specific customer.

##App registration
We use OAuth 2.0 [implicit grant flow](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-implicit-grant-flow)
and the [On-Behalf-Of (OBO) grant flow](<(https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-on-behalf-of-flow)>) for the Mavim Web Manager.
To get the implicit flow working in AAD we need to set the "oauth2AllowIdTokenImplicitFlow" to "true" in the Manifest of the app registration.

For the OBO grant flow we need to generate a client secret.

To gain access to the Azure SQL database, we need add the API permission "Azure SQL Database user_impersonation".

##Claims
Authenticated users will get a custom claim containing the encrypted connection string that contains the database server url; database name and the schema.
