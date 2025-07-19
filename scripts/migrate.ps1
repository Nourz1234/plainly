param(
    [Parameter(Mandatory = $true)]
    [string]$DbName,
    [string]$Environment = "Development"
)

$env:ASPNETCORE_ENVIRONMENT = $environment
dotnet ef database update --project Plainly.Api --context "$($DbName)DbContext" --msbuildprojectextensionspath ./build/obj/Plainly.Api