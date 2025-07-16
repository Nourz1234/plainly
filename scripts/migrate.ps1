param(
    [Parameter(Mandatory = $true)]
    [string]$db,
    [Parameter(Mandatory = $true)]
    [string]$environment = "Development"
)

$env:ASPNETCORE_ENVIRONMENT = $environment
dotnet ef database update --project Plainly.Api --context "$($db)DbContext" --msbuildprojectextensionspath ./build/obj/Plainly.Api