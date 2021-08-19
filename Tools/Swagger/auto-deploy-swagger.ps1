param([string]$link, [string]$sas, [string]$tenantName, [string]$version)

Start-Sleep -s 15

$contentType = "application/vnd.swagger.link+json"

$apiId = $sas.substring($sas.indexOf(" "), $sas.length);
$apiId = $apiId.split("&")[0];

if($apiId) {
  Write-Host "($apiId) is id used in uri"
} else {
  Write-Host "error: no apiId found" -ForegroundColor Red
}


$apiUri = $apiUri = "https://$tenantName.management.azure-api.net/apis/{0}?api-version=$version&import=true" -f $apiId

$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Authorization", "$sas")
$headers.Add("If-Match", '*')

$payload = @{ link = $link }
$json = $payload | ConvertTo-Json

Write-Host "Deploying Swagger ($link) to Management API ($apiUri)"
$response = Invoke-RestMethod -Uri $apiUri -Method Put -ContentType $contentType -Body $json -Headers $headers
