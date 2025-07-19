[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string]$DbName,
    [Parameter(Mandatory = $true)]
    [string]$Name,
    [string]$Environment = "Development"
)

$env:ASPNETCORE_ENVIRONMENT = $environment
$location_map = @{
    "App" = "Data/AppDatabase/Migrations"
    "Log" = "Data/LogDatabase/Migrations"
}
dotnet ef migrations add $Name --project Plainly.Api --context "$($DbName)DbContext" --output-dir $location_map[$DbName] --msbuildprojectextensionspath ./build/obj/Plainly.Api