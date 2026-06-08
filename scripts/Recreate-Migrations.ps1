<#
Recreate migrations for TechMove projects (PowerShell).

Usage (PowerShell):
  ./scripts/Recreate-Migrations.ps1

What it does:
- Stops any running dotnet processes that reference the solution (to avoid file locks);
- Deletes the existing Migrations folder for TechMove.Api (if present);
- Runs `dotnet ef migrations add InitialCreate` for TechMove.Api using TechMove.Api as the startup project.

Notes:
- Requires dotnet-ef tools installed: dotnet tool install --global dotnet-ef (or use dotnet-ef as a local tool)
- Run from repository root (where the solution file is located).
- This script does not attempt to connect to a database; if you want to apply migrations you may run `dotnet ef database update` afterwards.
#>

param(
    [string]$SolutionRoot = (Get-Location).Path
)

Write-Host "Running migration recreation script from: $SolutionRoot"

# Kill dotnet processes that may lock files (best-effort)
Write-Host "Stopping potential dotnet processes that reference TechMove..."
$dotnetProcs = Get-Process -Name dotnet -ErrorAction SilentlyContinue
if ($dotnetProcs) {
    foreach ($p in $dotnetProcs) {
        try {
            $cmd = (Get-CimInstance Win32_Process -Filter "ProcessId = $($p.Id)").CommandLine
            if ($cmd -and ($cmd -like '*TechMove*' -or $cmd -like '*TechMove.Web*' -or $cmd -like '*TechMove.Api*')) {
                Write-Host "Stopping process Id $($p.Id) (commandline contains TechMove)"
                Stop-Process -Id $p.Id -Force -ErrorAction SilentlyContinue
            }
        } catch {
            # ignore
        }
    }
}

# Target project(s)
$apiProject = Join-Path $SolutionRoot 'TechMove.Api'
$migrationsFolder = Join-Path $apiProject 'Migrations'

if (Test-Path $migrationsFolder) {
    Write-Host "Deleting existing migrations in $migrationsFolder"
    Get-ChildItem -Path $migrationsFolder -Recurse -Force | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue
    # Remove the folder itself if empty
    if (Test-Path $migrationsFolder) {
        try { Remove-Item $migrationsFolder -Force -Recurse -ErrorAction SilentlyContinue } catch {}
    }
} else {
    Write-Host "No existing migrations folder found at $migrationsFolder"
}

# Ensure dotnet-ef is available
if (-not (Get-Command dotnet-ef -ErrorAction SilentlyContinue)) {
    Write-Warning "dotnet-ef was not found on PATH. Install with: dotnet tool install --global dotnet-ef"
    Write-Warning "Continuing will likely fail without dotnet-ef."
}

# Create new migration for API project
Push-Location $apiProject
try {
    $migrationName = 'InitialCreate'
    Write-Host "Adding new migration '$migrationName' for project TechMove.Api"
    dotnet ef migrations add $migrationName --project TechMove.Api.csproj --startup-project TechMove.Api.csproj
    Write-Host "Migration added. You may now run: dotnet ef database update --project TechMove.Api.csproj --startup-project TechMove.Api.csproj"
} catch {
    Write-Error "Failed to add migration: $_"
} finally {
    Pop-Location
}

Write-Host "Done."
