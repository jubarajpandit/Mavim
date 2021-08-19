FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

COPY ["Mavim-services/Admin", "Mavim-services/Admin/"]
COPY ["Mavim-services/Shared", "Mavim-services/Shared/"]
COPY ["SolutionItems/SharedAppSettings", "SolutionItems/SharedAppSettings/"]
COPY ./Mavim-services/nuget.config ./

RUN dotnet restore "Mavim-services/Admin/Mavim.Admin.Api.Import.Authorization/Mavim.Admin.Api.Import.Authorization.csproj" --configfile nuget.config -nowarn:msb3202,nu1503

COPY . .

WORKDIR "/src/Mavim-services/Admin/Mavim.Admin.Api.Import.Authorization"
RUN dotnet build "Mavim.Admin.Api.Import.Authorization.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mavim.Admin.Api.Import.Authorization.csproj" -c Release -o /app

FROM base AS final

ENV ASPNETCORE_URLS=http://+:80; \
	DOTNET_RUNNING_IN_CONTAINERS=true \
	DOTNET_SKIP_FIRST_TIME_EXPERIENCE="true"

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mavim.Admin.Api.Import.Authorization.dll", "--server.urls", "http://*:5008"]