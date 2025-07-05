param([string]$environment = "Development")

$env:ASPNETCORE_ENVIRONMENT = $environment
dotnet ef database update --project Plainly.Api --msbuildprojectextensionspath ../build/obj/Plainly.Api