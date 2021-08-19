#Authentication

For our Single Page Application (SPA) we use [OAuth implicit grant flow](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-implicit-grant-flow).
This allows us to securly obtain a client access token to communicate to our [web services](web-services.md).
The idea is to propagate the delegated user identity and permissions through the request chain. For the middle-tier service to make authenticated requests to the downstream service,
it needs to secure an access token from Azure Active Directory (Azure AD), on behalf of the user.

![Authentication flow](https://mavim.visualstudio.com/c3cd027e-117f-4d38-90b7-66606c65dae2/_apis/git/repositories/8c5518e6-1372-435d-a0bf-ff6eef5f794d/Items?path=%2FDocumentation%2Fauthentication-flow.jpg&versionDescriptor%5BversionOptions%5D=0&versionDescriptor%5BversionType%5D=0&versionDescriptor%5Bversion%5D=feature%2F14003-documentation-architecture&download=false&resolveLfs=true&%24format=octetStream&api-version=5.0-preview.1 "Authentication flow")

## Implicit grant flow

The OAuth 2.0 implicit grant flow is explaned on the [Microsoft documentation site](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-implicit-grant-flow).
The specific authentication configuration for the Mavim web manager is described below.

> https://login.microsoftonline.com/ca8108ce-8f2c-40ea-b285-f9f7049e55cc/oauth2/authorize?response_type=token&client_id=b3e9323a-7104-4522-84b7-a19b5b8e12f4&redirect_uri=https%3A%2F%2Flocalhost%3A4200%2F&scope=openid%20profile%20email&resource=b3e9323a-7104-4522-84b7-a19b5b8e12f4

| URL part                                       | Documented part                         |
| ---------------------------------------------- | --------------------------------------- |
| https://login.microsoftonline.com/             | [Identity provider](#identity-provider) |
| ca8108ce-8f2c-40ea-b285-f9f7049e55cc           | [Tenant id](#tenant-id)                 |
| client_id=b3e9323a-7104-4522-84b7-a19b5b8e12f4 | [Client id](#client-id)                 |
| resource=b3e9323a-7104-4522-84b7-a19b5b8e12f4  | [Resoruce id](#resource-id)             |
| redirect_uri=https%3A%2F%2Flocalhost%3A4200%2F | [Redirect url](#redirect-url)           |

###Identity provider
For now we allow Azure Active Directory (AAD) to be our only Identity provider. When we are going to support i.e. [OKTA](https://www.okta.com/) we (at first) only support this through AAD.
In a later iteration we are going to look if we can support multiple identity providers and calling these services directly.

###Client id
The Client id is the registered application id in AAD, witch also accommodates a client secret for the On-Behalf-Of grant flow.

This setting can be found on the azure portal under "[Azure Active Directory](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/Overview) > [App registrations](https://portal.azure.com/#blade/Microsoft_AAD_IAM/ActiveDirectoryMenuBlade/RegisteredAppsPreview)".

On this page there should be a new app been registerd.

###Resource id
For the resource id we need to set the id to witch resources we need to access. Because we do not want to expose that we are using a Azure SQL database to the world, we use the client id to "hide" this configuration.

###Redirect url
This is the URL back to our Angular application.

###Access token
You can validate the token returned on the website https://jwt.io or https://jwt.ms

- It's important that the audience (aud) is the _cliend id_, because this has access to the resource (https://databases.windows.net).
- Also the appid should be the _client id_

## On-Behalf-Of grant flow

> POST ht&#8203;tps://login.microsoftonline.com/**ca8108ce-8f2c-40ea-b285-f9f7049e55cc**/oauth2/v2.0/token
>
> > **BODY**
> >
> > grant_type=urn:ietf:params:oauth:grant-type:jwt-bearer
> >
> > &client_id=**b3e9323a-7104-4522-84b7-a19b5b8e12f4**
> >
> > &client_secret=**CDk?oCIlvbw.j1@}Lv5=TK4YRn.41KR7r^^e!4)Q2!%KpPtwz*x2H*M%**
> >
> > \$resource=ht&#8203;tps://database.windows.net/
> >
> > &assertion=**eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6Ii1zeE1KTUxDSURXTVRQdlp5SjZ0eC1DRHh3MCIsImtpZCI6Ii1zeE1KTUxDSURXTVRQdlp5SjZ0eC1DRHh3MCJ9.eyJhdWQiOiJiM2U5MzIzYS03MTA0LTQ1MjItODRiNy1hMTliNWI4ZTEyZjQiLCJpc3MiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9jYTgxMDhjZS04ZjJjLTQwZWEtYjI4NS1mOWY3MDQ5ZTU1Y2MvIiwiaWF0IjoxNTUxMjYzNTg3LCJuYmYiOjE1NTEyNjM1ODcsImV4cCI6MTU1MTI2NzQ4NywiYWNyIjoiMSIsImFpbyI6IkFTUUEyLzhLQUFBQWtVTmk3YUhTbmRCdlcybXVWOFhITVNxcTVPdGQyUVBqeC9FZzF4WTk5dFk9IiwiYW1yIjpbInB3ZCJdLCJhcHBpZCI6ImIzZTkzMjNhLTcxMDQtNDUyMi04NGI3LWExOWI1YjhlMTJmNCIsImFwcGlkYWNyIjoiMCIsImlwYWRkciI6IjIxMi4yMzguMTY4LjYwIiwibmFtZSI6IkFkbWluaXN0cmF0b3IiLCJvaWQiOiI1NDE4YTAwMy1mOWFkLTQ2ZmUtYWEzMS0xMzU3YWY5N2M2NDgiLCJzY3AiOiJlbWFpbCBvcGVuaWQgcHJvZmlsZSBVc2VyLlJlYWQiLCJzdWIiOiJEOXJGNmxWVENoYlhXUndGZkVkQ2lmMVBCNHBvdEFHOHNkSWlHLXRCcklrIiwidGlkIjoiY2E4MTA4Y2UtOGYyYy00MGVhLWIyODUtZjlmNzA0OWU1NWNjIiwidW5pcXVlX25hbWUiOiJhZG1pbkByYWxwaG5vb3JkYW51cy5vbm1pY3Jvc29mdC5jb20iLCJ1cG4iOiJhZG1pbkByYWxwaG5vb3JkYW51cy5vbm1pY3Jvc29mdC5jb20iLCJ1dGkiOiJycURtYmFlSXRFLTExckxhbkI4ZkFBIiwidmVyIjoiMS4wIn0.qzu-6WYe-v-Y0v1UtKwQmXrsHKQgRWQiZZaIPqxwgPDVlKyPmmSIk23YnJbZn_9n0i0duTq4XoduESVv9NYrzVeMf1SzC_UpWP5ZQykEJeVLW9_sNp8--30NSk92-C5X-u5Ac5DKK2alUhv4Ey0OZoiJm75I8wDhDlb9KZpzdlZB6O-FP29dkv9-hUlzd4eGrz7uCIL0sXwhGyamK0f5qeMwEvAF81ZHuqEfcWJn-6NAA2nM6YGaYGecFKvJuJVnc6rF6mShJ7mtGfsUumBTy_jvlQ1MWBa6Boqi_ikfQ3LqTfrsVthXdI1zyfsUDVD1NTjASjNA4eF5z6VNEN_gUg**
> >
> > &requested_token_use=on_behalf_of
> >
> > &redirect_uri=**https%3A%2F%2Flocalhost%3A4200%2F**

> NOTE: It is very important to have a slash at the end of the resouce ht​&#8203;tps://database.windows.net​&#8203;**/**
