FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src

COPY ["Mavim-services/Connect", "Mavim-services/Connect/"]
COPY ["Mavim-services/Shared", "Mavim-services/Shared/"]
COPY ["SolutionItems/SharedAppSettings", "SolutionItems/SharedAppSettings/"]
COPY ./Mavim-services/nuget.config ./

RUN dotnet restore "Mavim-services/Connect/Mavim.Manager.Api.Connect.Read/Mavim.Manager.Api.Connect.Read.csproj" --configfile nuget.config -nowarn:msb3202,nu1503

COPY . .

WORKDIR "/src/Mavim-services/Connect/Mavim.Manager.Api.Connect.Read"
RUN dotnet build "Mavim.Manager.Api.Connect.Read.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mavim.Manager.Api.Connect.Read.csproj" -c Release -o /app

FROM base AS final

ENV ASPNETCORE_URLS=http://+:8080; \
	DOTNET_RUNNING_IN_CONTAINERS=true \
	DOTNET_SKIP_FIRST_TIME_EXPERIENCE="true"

RUN adduser --disabled-password -u 1001 serviceuser
RUN chown serviceuser -R /app

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mavim.Manager.Api.Connect.Read.dll", "--server.urls", "http://*:5028"]
USER 1001