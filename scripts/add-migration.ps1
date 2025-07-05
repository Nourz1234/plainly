param([string]$name, [string]$environment = "Development")

$env:ASPNETCORE_ENVIRONMENT = $environment
dotnet ef migrations add $name --project Plainly.Api --output-dir Database/Migrations --msbuildprojectextensionspath ../build/obj/Plainly.Api