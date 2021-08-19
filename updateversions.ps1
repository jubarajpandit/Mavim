param(
    [Parameter(Mandatory = $true)]
    [String]$buildVersion,
    
    [Parameter(Mandatory =$true)]
    [String]$servicesRootFolder,
    
    [Parameter(Mandatory =$true)]
    [String]$appRootFolder,

    [Parameter(Mandatory = $false)]
    [boolean]$quietGitExecution = $true
)

function UpdateOrInsertVersion {   
    param(
        [parameter(Mandatory = $true)]
        [xml]$xmlDocument,
        [parameter(Mandatory = $true)]
        [string]$elementName
    )
     
    $version = $xmlDocument.GetElementsByTagName($elementName)

    if ($version.Count -gt 0) {
        Write-Host "Updating the $elementName to $buildVersion ..." -ForegroundColor Yellow
        $version.Item(0).InnerText = $buildVersion
    }
    else {
        Write-Host "No assembly version element found. Proceeding to create one..." -ForegroundColor Yellow

        $propertyGroupNode = $xmlProjectFile.Project.PropertyGroup

        $versionElement = $xmlDocument.CreateElement($elementName)
        $versionElement.InnerText = $buildVersion
            
        $propertyGroupNode.AppendChild($versionElement)
    }
}

git config --global user.email "rbr@mavim.com"
git config --global user.name "rbr"

Write-Output "=============== Branch before manually checking out ==============="
 if ($quietGitExecution) {
     git checkout --force master --quiet
 }
 else {
     git checkout --force master
 }

$gitCreated = $false

If (!(test-path .\.git)) {
    Write-Output "Creating .git directory ..."
    $gitCreated = $true
    New-Item -ItemType Directory -Force -Path ".\.git"
}


#------- Code to update the assembly version in the dotnet core project files.
Write-Output "Updating versions of the .net core projects to $buildVersion"

$csprojFiles = Get-ChildItem -Path $servicesRootFolder -Filter *.csproj -Recurse | ForEach-Object{$_.FullName}

foreach ($csprojFile in $csprojFiles) {
    # only current working directory folders with a csproj file
    if ( Test-Path $csprojFile -PathType Leaf) {
        $xmlProjectFile = [Xml](Get-Content $csprojFile)
        UpdateOrInsertVersion -xmlDocument $xmlProjectFile -elementName "AssemblyVersion"
        UpdateOrInsertVersion -xmlDocument $xmlProjectFile -elementName "FileVersion"
                
        $xmlProjectFile.Save($csprojFile);
    }
}

Write-Output "Updating versions of the angular projects to $buildVersion"
$jsonFiles = Get-ChildItem -Path $appRootFolder package.json -Recurse | Where-Object { $_.FullName -notmatch 'node_modules' } | ForEach-Object{$_.FullName}

Write-Output "=============== Updating the version file now...  ==============="
foreach ($jsonFile in $jsonFiles) {
    $packageJsonFile = Get-Content $jsonFile -raw | ConvertFrom-Json
    $packageJsonFile.version = $buildVersion
    $packageJsonFile = $packageJsonFile | ConvertTo-Json
    New-Item -Path $jsonFile -Value $packageJsonFile -Force
}
#------- version update ends here

git stage *
git commit -a -m "***NO_CI*** Updated build version to $buildVersion by build agent."


# The belwo url uses a Personal Access Token in the URL to push the updates to the GIT repository.
if ($quietGitExecution) {
    git push https://agdw4fat4ue73ltddmugmuzlfczg5b3jakbnhqbbrbkdig6tbc5q@mavim.visualstudio.com/MavimCloud/_git/mavim.cloud master --quiet
}
else {
    git push https://agdw4fat4ue73ltddmugmuzlfczg5b3jakbnhqbbrbkdig6tbc5q@mavim.visualstudio.com/MavimCloud/_git/mavim.cloud master    
}

if ($gitCreated) {
    Write-Output "Removing the .git directory ..."
    Remove-Item -Path ".\.git"
}