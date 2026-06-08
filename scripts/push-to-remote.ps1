<#
Simple push-to-remote.ps1
Usage: powershell -ExecutionPolicy Bypass -File .\scripts\push-to-remote.ps1 -RepoUrl "https://github.com/USER/REPO.git"
#>
param(
    [Parameter(Mandatory=$true)]
    [string]$RepoUrl,
    [string]$Branch = 'main',
    [switch]$Force
)

if (-not (Get-Command git -ErrorAction SilentlyContinue)) {
    Write-Error "Git not found on PATH"
    exit 2
}

$root = Get-Location
Write-Host "Working dir: $root"

# ensure .gitignore
$gitignore = Join-Path $root '.gitignore'
if (-not (Test-Path $gitignore)) {
    @(
        ".vs/",
        "bin/",
        "obj/",
        ".env",
        "docker-compose.override.yml",
        "**/publish/"
    ) | Out-File -FilePath $gitignore -Encoding utf8
    Write-Host "Created .gitignore"
}

if (-not (Test-Path (Join-Path $root '.git'))) {
    git init | Out-Null
    Write-Host "Initialized git repository"
}

git add -A
try { git commit -m "Initial commit from workspace" --no-verify | Out-Null } catch { Write-Host "No changes to commit" }

# set remote
$existing = $null
try { $existing = git remote get-url origin 2>$null } catch { }
if ($existing) {
    if ($existing -ne $RepoUrl) {
        $ans = Read-Host "Remote origin exists ($existing). Replace with $RepoUrl? Type YES to replace"
        if ($ans -eq 'YES') { git remote remove origin; git remote add origin $RepoUrl }
        else { Write-Host "Aborted"; exit 0 }
    }
} else { git remote add origin $RepoUrl }

if ($Force) { git push -u origin $Branch --force }
else {
    git push -u origin $Branch 2>&1
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Push failed; try run with -Force or resolve remote conflicts manually."
        exit 3
    }
}

Write-Host "Push complete."
