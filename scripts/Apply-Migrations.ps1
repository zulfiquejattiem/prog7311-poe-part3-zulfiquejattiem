<#
Apply migrations to the database for TechMove.Api.

Usage: ./scripts/Apply-Migrations.ps1

Notes:
- This will run `dotnet ef database update` for TechMove.Api project using TechMove.Api as startup project.
- Ensure your connection string is correct in appsettings or environment variables before running.
#>

param(
    [string]$SolutionRoot = (Get-Location).Path
)

$apiProject = Join-Path $SolutionRoot 'TechMove.Api'
Push-Location $apiProject
try {
    Write-Host "Applying migrations to database for TechMove.Api..."
    dotnet ef database update --project TechMove.Api.csproj --startup-project TechMove.Api.csproj
    Write-Host "Database update completed."
} catch {
    Write-Error "Database update failed: $_"
} finally {
    Pop-Location
}

