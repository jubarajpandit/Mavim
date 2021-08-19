# Set Current working directory
$CURRENT_WORKING_DIR = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent

# Set the name of the output directory where the docker would generate the api, stub and angular services and delete them later.
$OUT_DIR = "out"

# Set the Docker folder name that can be used for mapping
$DOCKER_MAP_FOLDER = "local"

# Set Output location for the Docker generator where all the codes are generated
$OUT_DIR_LOCATION = $CURRENT_WORKING_DIR + "\" + $OUT_DIR

# list of folders to be deleted after test compilation before copying the generated code
$DIRS_TO_DELETE = @("node_modules", "dist")

<#________________________________________________________________________________________
             Set the configuration files for the docker generator as input
________________________________________________________________________________________#>

# Set Swagger json file for input to the docker generator
$SWAGGER_JSON_FILENAME = "mmw-api-swagger.json"

# Set the name for the webapi stub configuration file to be used by the OpenApi generator
$SWAGGER_WEBAPI_STUB_CONFIG = "mmw-api-stub-config.json"

# Set the name for the web api configuration file to be used by the OpenApi generator
$SWAGGER_WEBAPI_CONFIG = "mmw-api-config.json"

# Set the name for the angular service configuration file to be used by the OpenApi generator
$SWAGGER_ANGULAR_CONFIG = "mmw-angular-service-config.json"

<#________________________________________________________________________________________
             Set the project names for the docker generator as input
________________________________________________________________________________________#>

# Set the name of the web api stub project to be generated
$WEBAPI_STUB_PROJ_NAME = "Mavim.Manager.WebApi.Stub"

# Set the name of the web api project to be generated
$WEBAPI_PROJ_NAME = "Mavim.Manager.WebApi"

# Set the name of the angular services project to be generated
$ANGULAR_SERVICES_PROJ_NAME = "Mavim.Manager.Web.ManagerService"

#*********************************************************************************

<#________________________________________________________________________________________
             Set the output location for the docker generator as input
________________________________________________________________________________________#>

# Set Docker generator output location
$DOCKER_OUTPUT_LOCATION = $DOCKER_MAP_FOLDER + "/" + $OUT_DIR

. "$CURRENT_WORKING_DIR\CodeGen.ps1"

CleanOutputDirectory -outputdir $OUT_DIR_LOCATION

Docker-Generate-WebApi-Code -codegenName aspnetcore -dockermapfolder $DOCKER_MAP_FOLDER -projectName $WEBAPI_STUB_PROJ_NAME -output $OUT_DIR -swaggerjsoninput $SWAGGER_JSON_FILENAME -swaggerconfig $SWAGGER_WEBAPI_STUB_CONFIG

CleanOutputDirectory -outputdir $OUT_DIR_LOCATION

Docker-Generate-WebApi-Code -codegenName aspnetcoreioc -dockermapfolder $DOCKER_MAP_FOLDER -projectName $WEBAPI_PROJ_NAME -output $OUT_DIR -swaggerjsoninput $SWAGGER_JSON_FILENAME -swaggerconfig $SWAGGER_WEBAPI_CONFIG

CleanOutputDirectory -outputdir $OUT_DIR_LOCATION

Docker-Generate-Angular-Code -codegenName typescript-angular -dockermapfolder $DOCKER_MAP_FOLDER -projectName $ANGULAR_SERVICES_PROJ_NAME -output $OUT_DIR -swaggerjsoninput $SWAGGER_JSON_FILENAME -swaggerconfig $SWAGGER_ANGULAR_CONFIG -compile $true

CleanOutputDirectory -outputdir $OUT_DIR_LOCATION

