#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet-framework:4.7.2-runtime-windowsservercore-ltsc2016 as base

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ENV DOTNET30_SDK_DOWNLOAD_URL https://download.visualstudio.microsoft.com/download/pr/14417e56-3f88-486d-8065-ef802653699b/30cfec0c5c4af7993e7ce83670fb279b/dotnet-sdk-3.0.100-preview3-010431-win-x64.zip
ENV DOTNET30_SDK_DOWNLOAD_SHA 60EF0FEE5107243066EE68CFD9591746DF281FFCEBA197AA9ACBBBEB319DA753965B8A923947860AEAA29A58A213CCE466847C0C05829A2A6D6C335445E4B2BF

# dotnet core 3.0.0 SDK -> install
RUN Invoke-WebRequest $Env:DOTNET30_SDK_DOWNLOAD_URL -OutFile dotnet30.zip; \
    if ((Get-FileHash dotnet30.zip -Algorithm sha512).Hash -ne $Env:DOTNET30_SDK_DOWNLOAD_SHA) { \
        Write-Host 'dotnet30.zip CHECKSUM VERIFICATION FAILED!'; \
        exit 1; \
    }; \
    \
    Expand-Archive dotnet30.zip -Force -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet30.zip

# dotnet core SDK -> set path
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

FROM microsoft/dotnet-framework:4.7.2-sdk-windowsservercore-ltsc2016 as dotnet472build

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# dotnet core 3.0.0 Preview 2 download and extract the binaries
ENV DOTNET30_SDK_DOWNLOAD_URL https://download.visualstudio.microsoft.com/download/pr/14417e56-3f88-486d-8065-ef802653699b/30cfec0c5c4af7993e7ce83670fb279b/dotnet-sdk-3.0.100-preview3-010431-win-x64.zip
ENV DOTNET30_SDK_DOWNLOAD_SHA 60EF0FEE5107243066EE68CFD9591746DF281FFCEBA197AA9ACBBBEB319DA753965B8A923947860AEAA29A58A213CCE466847C0C05829A2A6D6C335445E4B2BF

# dotnet core 3.0.0 SDK -> install
RUN Invoke-WebRequest $Env:DOTNET30_SDK_DOWNLOAD_URL -OutFile dotnet30.zip; \
    if ((Get-FileHash dotnet30.zip -Algorithm sha512).Hash -ne $Env:DOTNET30_SDK_DOWNLOAD_SHA) { \
        Write-Host 'dotnet30.zip CHECKSUM VERIFICATION FAILED!'; \
        exit 1; \
    }; \
    \
    Expand-Archive dotnet30.zip -Force -DestinationPath $Env:ProgramFiles\dotnet; \
    Remove-Item -Force dotnet30.zip

# dotnet core SDK -> set path
RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '\dotnet')

FROM dotnet472build as build
WORKDIR /src
COPY ["Mavim.Manager.WebApi/Mavim.Manager.WebApi.csproj", "Mavim.Manager.WebApi/"]
COPY ["Mavim.Manager.Web.Services/Mavim.Manager.Web.Services.csproj", "Mavim.Manager.Web.Services/"]
COPY ["Mavim.Manager.Web.Interfaces/Mavim.Manager.Web.Interfaces.csproj", "Mavim.Manager.Web.Interfaces/"]
COPY ["Mavim.Manager.AutoRegisterInterface/Mavim.Manager.ARInterface.csproj", "Mavim.Manager.AutoRegisterInterface/"]
RUN dotnet restore "Mavim.Manager.WebApi/Mavim.Manager.WebApi.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "Mavim.Manager.WebApi/Mavim.Manager.WebApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Mavim.Manager.WebApi/Mavim.Manager.WebApi.csproj" -c Release -o /app

FROM base as final

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# Configure web servers to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80; \
	 # Enable detection of running in a container
	DOTNET_RUNNING_IN_CONTAINERS=true \
	# ignore first time expierence
	DOTNET_SKIP_FIRST_TIME_EXPERIENCE="true"

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Mavim.Manager.WebApi.dll", "--server.urls", "http://*:5000"]
