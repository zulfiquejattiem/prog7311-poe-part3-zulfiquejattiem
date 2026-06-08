<#
cleanup-keep-image.ps1

Stops and removes all containers, deletes all images except the specified image ID,
removes all volumes, and then brings the compose stack back up.

WARNING: This is destructive. Back up any volumes (especially DB) you need before running.
#>

param(
    [string]$KeepImageId = '39c0f2a4b4346b784d6c0c9a8f7512fec1ab6e082b8be313e1c32bad111abb91'
)

Write-Host "This script will:"
Write-Host " - Stop & remove ALL containers"
Write-Host " - Remove ALL images except: $KeepImageId"
Write-Host " - Remove ALL volumes"
Write-Host " - Recreate the compose stack (docker compose up --build -d)"

$confirm = Read-Host "Type YES to continue"
if ($confirm -ne 'YES') { Write-Host 'Aborted.'; exit 1 }

# Verify the keep image exists
$found = docker images --no-trunc --format '{{.ID}}' 2>$null | Where-Object { $_ -eq $KeepImageId }
if (-not $found) {
    Write-Host "Image $KeepImageId not found locally. Aborting to avoid accidental deletion."
    Write-Host "Available images:"; docker images --no-trunc
    exit 1
}

Write-Host 'Stopping and removing all containers...'
$all = docker ps -aq
if ($all) { docker rm -f $all | Out-Null }

Write-Host 'Removing images (keeping specified image)...'
$allImageIds = docker images --no-trunc --format '{{.ID}}' | Select-Object -Unique
$toRemove = $allImageIds | Where-Object { $_ -ne $KeepImageId }
if ($toRemove) {
    foreach ($id in $toRemove) {
        Write-Host "Removing image $id"
        docker rmi -f $id 2>$null
    }
} else { Write-Host 'No other images to remove.' }

Write-Host 'Removing all volumes...'
$vols = docker volume ls -q
if ($vols) {
    foreach ($v in $vols) {
        Write-Host "Removing volume $v"
        docker volume rm -f $v 2>$null
    }
} else { Write-Host 'No volumes to remove.' }

Write-Host 'Pruning dangling networks and images (safe cleanup)...'
docker network prune -f 2>$null
docker image prune -f 2>$null

Write-Host 'Rebuilding and starting compose stack...'
# Use compose in the current directory
docker compose up --build -d

Write-Host 'Final containers:'
docker ps -a

Write-Host 'Done.'
