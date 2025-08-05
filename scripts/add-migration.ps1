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
    "App" = "Persistence/AppDatabase/Migrations"
    "Log" = "Persistence/LogDatabase/Migrations"
}
dotnet ef migrations add $Name --project Plainly.Infrastructure --context "$($DbName)DbContext" --output-dir $location_map[$DbName] --msbuildprojectextensionspath ./build/obj/Plainly.Infrastructure