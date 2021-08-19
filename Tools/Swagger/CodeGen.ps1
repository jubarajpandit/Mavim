function CleanOutputDirectory{
[CmdletBinding()]
        param(
            $outputdir
        )

    if((Test-Path $outputdir) -eq $true) {
        Write-Host "Cleaning location $outputdir ..." -ForegroundColor Yellow
        # Cleanup the docker generator output location if exists before performing anything
        Remove-Item -Path $outputdir -Recurse -Force
    } else {
        Write-Host "Docker output location is clean. No Action required." -ForegroundColor Green
    }
    
    if(!$?) { 
        Write-Host "Error: $_" -ForegroundColor Red
        exit 
    }
	Write-Host "Done." -ForegroundColor Green
}

function Docker-Generate-WebApi-Code {
        [CmdletBinding()]
        param(
            $codegenName, #Example: aspnetcore or aspnetcoreioc
            $dockermapfolder,
            $projectName,
            $output,
            $swaggerjsoninput,
            $swaggerconfig,
			$compile = $false
        )

        Write-Host "Generating $projectName ..." -ForegroundColor Green        
        docker run --rm -v ${PWD}:/$dockermapfolder mavim/openapi-generator-cli generate -i /$dockermapfolder/$swaggerjsoninput -g $codegenName -o /$dockermapfolder/$output -c /$dockermapfolder/$swaggerconfig
        Write-Host "" 

        # Check if there were any errors during the previous execution
        if($?){ 
            Write-Host "Successfully generated $projectName."-ForegroundColor Green 
        } else { 
            Write-Host "Error while generating $projectName." -ForegroundColor Red
            exit
        }

		Write-Host ""

		if($compile) {
			Write-Host "Compiling $projectName ..." -ForegroundColor Yellow
			dotnet build .\$output\src\$projectName\$projectName.csproj
		} else {
			Write-Host "Skipping compilation of $projectName ..." -ForegroundColor Yellow
		}
        Write-Host ""

        # Check if there were any errors during the previous execution
        if($?) { 
            Write-Host "Successfully compiled $projectName." -ForegroundColor Green 
        } else { 
            Write-Host "Error while compiling $projectName." -ForegroundColor Red
            exit
        }

        Write-Host "Copying $projectName files ..." -ForegroundColor Yellow

        Write-Host "Copy-Item -Path .\$output\src\$projectName -Destination ..\..\ -Force"

        Copy-Item -Path .\$output\src\$projectName -Destination ..\..\ -Force -Recurse

        # Check if there were any errors during the previous execution
        if($?) { 
            Write-Host "Successfully copied $projectName." -ForegroundColor Green 
        } else { 
            Write-Host "Error while copying $projectName." -ForegroundColor Red
            exit
        }
}

function Docker-Generate-Angular-Code {
        [CmdletBinding()]
        param(
            $codegenName, #Example: aspnetcore or aspnetcoreioc
            $dockermapfolder,
            $projectName,
            $output,
            $swaggerjsoninput,
            $swaggerconfig,
			$compile = $false
        )

        Write-Host "Generating $projectName ..." -ForegroundColor Green        
        docker run --rm -v ${PWD}:/$dockermapfolder mavim/openapi-generator-cli generate -i /$dockermapfolder/$swaggerjsoninput -g $codegenName -o /$dockermapfolder/$output -c /$dockermapfolder/$swaggerconfig
        Write-Host "" 

        # Check if there were any errors during the previous execution
        if($?){ 
            Write-Host "Successfully generated $projectName."-ForegroundColor Green 
        } else { 
            Write-Host "Error while generating $projectName." -ForegroundColor Red
            exit
        }

		Write-Host "Setting current location to "
        Set-Location $output

		if($compile) {
			Write-Host "Running npm install on $projectName ..." -ForegroundColor Yellow
			npm install
			Write-Host ""

			Write-Host "Running npm audit fix on $projectName ..." -ForegroundColor Yellow
			npm audit fix --force
			Write-Host ""

			Write-Host "Running npm build fix on $projectName ..." -ForegroundColor Yellow
			npm run build
			Write-Host ""

			Remove-Item -Path "node_modules" -Force -Recurse
			Remove-Item -Path "dist" -Force -Recurse
		} else {
			Write-Host "Skipping compilation of $projectName ..." -ForegroundColor Yellow
		}

        # Check if there were any errors during the previous execution
        if($?) { 
            Write-Host "Successfully compiled $projectName." -ForegroundColor Green 
        } else { 
            Write-Host "Error while compiling $projectName." -ForegroundColor Red
            exit
        }

        CleanOutputDirectory -outputdir "..\..\..\$projectName"

        Write-Host "Copying $projectName files ..." -ForegroundColor Yellow        

        Copy-Item -Path . -Destination ..\..\..\$projectName -Force -Recurse

        Set-Location ..\

        # Check if there were any errors during the previous execution
        if($?) { 
            Write-Host "Successfully copied $projectName." -ForegroundColor Green 
        } else { 
            Write-Host "Error while copying $projectName." -ForegroundColor Red
            exit
        }
}