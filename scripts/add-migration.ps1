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
dotnet ef migrations add $name --project Plainly.Api --context "$($db)DbContext" --output-dir $location_map[$db] --msbuildprojectextensionspath ./build/obj/Plainly.Api