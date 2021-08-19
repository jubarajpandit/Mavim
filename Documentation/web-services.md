#Containarized web services

To host our webservices we use docker containers in our kubernetes cluster.
This allows use to automatically scale more easily and to become cloud native.

The web services are using the .NET Core framework.
We need at the least version 3.0 to use token based authentication to the SQL Server.
Since we use server.dll that is dependent on .NET Framework 3.7.2, we do not have any direct dependency.
However, we still use the 3.0 version to have the option for future functionalities.
