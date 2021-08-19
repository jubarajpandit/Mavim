#Customer database

Every customer is supplied with their own database to store all their Mavim databases that ensures that there is an isolated environment for each of them.
Access to the database is arranged by the (customer) AAD group, which grants access to the data.

Schema's are used to create multiple Mavim databases in the Azure SQL database.

> To connect to a customer database we need connection string that contains the database server url; database name and schema.
> This connection string is provided in a [Claim](Fazure-active-directory.md) of the authenticated user.
