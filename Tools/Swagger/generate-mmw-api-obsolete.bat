@echo off
cls

REM Set output directory to delete before generating new server stub code.
set "OUT_DIR_NAME=.\out\"

REM Set node_moddules folder name, which will be deleted before copying the output from the compiled source code.
set "NODE_MODULES_DIR=node_modules"

REM Set dist folder name, which will be deleted before copying the output from the compiled source code.
set "DIST=dist"

REM Set folder name to map from docker to the local drive
set "DOCKER_MAP_FOLDER_NAME=/local"

REM Set config file for angular service for the swagger code generator
set "SWAGGER_ANGULAR_CONFIG=%DOCKER_MAP_FOLDER_NAME%/mmw-angular-service-config.json"

REM Set config file for web api for the swagger code generator
set "SWAGGER_API_CONFIG=%DOCKER_MAP_FOLDER_NAME%/mmw-api-config.json"

REM Set config file for web api stub for the swagger code generator
set "SWAGGER_API_STUB_CONFIG=%DOCKER_MAP_FOLDER_NAME%/mmw-api-stub-config.json"

REM Set swagger.json input file for the swagger code generator
set "SWAGGER_JSON_INPUT=mmw-api-swagger.json"

REM Set WebApi Stub output directory 
set "WEBAPISTUBDIR=Mavim.Manager.WebApi.Stub"

REM Set WebApi output directory 
set "WEBAPIDIR=Mavim.Manager.WebApi"

REM Set output folder location where the generated service stubs are placed by the docker
set "OUTPUT=%DOCKER_MAP_FOLDER_NAME%/out"

REM Set angular client api directory name
set "MANAGERSERVICEDIR=Mavim.Manager.Web.ManagerService"

echo [33m Cleaning up the output directory:%OUT_DIR_NAME% ... [0m
if exist %OUT_DIR_NAME% @RD /S /Q %OUT_DIR_NAME% || goto :error
echo [42m Done. [0m

echo [33m Generating Mavim.Manager.WebApi.Stub ... [0m
echo ====================================================================================================================================================================================================
echo [33m docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g aspnetcore -o %OUTPUT% -c %SWAGGER_API_STUB_CONFIG% [0m
echo ====================================================================================================================================================================================================
docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g aspnetcore -o %OUTPUT% -c %SWAGGER_API_STUB_CONFIG% || goto :error
echo [42m Done.[0m

echo [33m Compile generated Mavim.Manager.WebApi.Stub before Copying the files ... [0m
dotnet build .\out\src\%WEBAPISTUBDIR%\%WEBAPISTUBDIR%.csproj || goto :error
echo [42m Done.[0m

echo [33m Copying generated Mavim.Manager.WebApi.Stub files... [0m
xcopy .\out\src\%WEBAPISTUBDIR%\* ..\..\%WEBAPISTUBDIR% /exclude:Exclusions.txt /e /y || goto :error
echo [42m Done.[0m

echo [33m Cleaning output folder...
@RD /S /Q %OUT_DIR_NAME% || goto :error
echo [42m Done.[0m

echo [33m Generating Mavim.Manager.WebApi ... [0m
echo ====================================================================================================================================================================================================
echo [33m docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g aspnetcoreioc -o %OUTPUT% -c %SWAGGER_API_CONFIG% [0m
echo ====================================================================================================================================================================================================
docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g aspnetcoreioc -o %OUTPUT% -c %SWAGGER_API_CONFIG%
echo [42m Done.[0m

echo [33m Compile generated Mavim.Manager.WebApi before Copying the files ... [0m
echo dotnet build .\out\src\%WEBAPIDIR%\%WEBAPIDIR%.csproj || goto :error
echo [42m Done.[0m

echo [33m Copying generated Mavim.Manager.WebApi files... [0m
xcopy .\out\src\%WEBAPIDIR%\* ..\..\%WEBAPIDIR% /exclude:Exclusions.txt /e /y || goto :error
echo [42m Done.[0m

echo [33m Cleaning output folder... [0m
@RD /S /Q %OUT_DIR_NAME% || goto :error
echo [42m Done.[0m

echo [33m Generating Mavim.Manager.Web.ManagerService angular code... [0m
echo ====================================================================================================================================================================================================
echo [33m docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g typescript-angular -o %OUTPUT% -c %SWAGGER_ANGULAR_CONFIG% [0m
echo ====================================================================================================================================================================================================
docker run --rm -v %CD%:/%DOCKER_MAP_FOLDER_NAME% mavim-openapi-generator generate -i %DOCKER_MAP_FOLDER_NAME%/%SWAGGER_JSON_INPUT% -g typescript-angular -o %OUTPUT% -c %SWAGGER_ANGULAR_CONFIG% || goto :error
echo [42m Done.[0m

echo [33m Compiling Mavim.Manager.Web.ManagerService angular code before copying the files ... [0m
REM push the current working directory as we need to pop it back later
pushd .
REM change directory to output directory source that we can compile the generated code before copying
echo [33m Changing the working directory to the Mavim.Manager.Web.ManagerService angular code directory ... [0m
chdir %OUT_DIR_NAME%
echo [33m Running npm install command ... [0m
call npm install || goto :error
echo [42m Done.[0m

echo [33m Running npm audit fix command ... [0m
call npm audit fix --force || goto :error
echo [42m Done.[0m

echo [33m Running npm run build command ... [0m
call npm run build || goto :error
echo [42m Done.[0m

echo [33m Remove node_modules folder before copying the generated Mavim.Manager.Web.ManagerService ... [0m
@RD /S /Q .\%NODE_MODULES_DIR% || goto :error
echo [42m Done.[0m

echo [33m Remove dist folder before copying the generated Mavim.Manager.Web.ManagerService ... [0m
@RD /S /Q .\%DIST% || goto :error
echo [42m Done.[0m

echo [33m Cleaning Mavim.Manager.Web.ManagerService angular destination folder before copying the compiled code... [0m
@RD /S /Q ..\..\..\%MANAGERSERVICEDIR%\ || goto :error
echo [42m Done.[0m

echo [33m Copying the Mavim.Manager.Web.ManagerService output to the destination directory ... [0m
xcopy * ..\..\..\%MANAGERSERVICEDIR%\ /e /y || goto :error
echo [42m Done.[0m

REM pop the original path location that was pushed earlier.
popd

echo [33m Cleaning output folder... [0m
@RD /S /Q %OUT_DIR_NAME% || goto :error
echo [42m Done.[0m

echo.  

echo [32m ============================================================================== [0m
echo [32m Completed generating swagger output.  This window will close in 5 seconds ... [0m
echo [32m ============================================================================== [0m
choice /d y /t 5 > nul
exit

:error
echo [101;93m ========================================================== [0m
echo [101;93m There were one or more errors during script execution !!!  [0m
echo [101;93m ========================================================== [0m
pause

