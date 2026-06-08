<#
delete-remote-then-push.ps1

Usage (from repo root):
  powershell -ExecutionPolicy Bypass -File .\scripts\delete-remote-then-push.ps1 -RepoUrl "https://github.com/USER/REPO.git" -Branch main [-Force]

This script will:
- init git if needed
- stage & commit current changes
- ensure origin remote points to the supplied RepoUrl (prompts before replacing)
- if the remote branch exists it will ask to delete it (destructive)
- push the local branch to origin (force if -Force provided)

WARNING: deleting a remote branch is destructive. Back up remote history if needed.
#>

param(
	[Parameter(Mandatory=$true)]
	[string]$RepoUrl,
	[string]$Branch = 'main',
	[switch]$Force
)

function ExitWith($msg, $code=1) { Write-Host $msg -ForegroundColor Yellow; exit $code }

if (-not (Get-Command git -ErrorAction SilentlyContinue)) { ExitWith "Git not found on PATH" 2 }

$root = Get-Location
Write-Host "Working directory: $root"

# Init repo if needed
if (-not (Test-Path (Join-Path $root '.git'))) {
	Write-Host "Initializing git repository..."
	git init | Out-Null
}

# Stage & commit
git add -A
try { git commit -m "Prepare push from local workspace" --no-verify | Out-Null } catch { Write-Host "No changes to commit or commit skipped" }

# Manage origin remote
$existingRemote = $null
try { $existingRemote = git remote get-url origin 2>$null } catch { $existingRemote = $null }
if ($existingRemote) {
	if ($existingRemote -ne $RepoUrl) {
		Write-Host "A remote named 'origin' already exists and points to: $existingRemote"
		$answer = Read-Host "Replace origin remote with $RepoUrl? Type YES to replace"
		if ($answer -eq 'YES') {
			git remote remove origin
			git remote add origin $RepoUrl
			Write-Host "Replaced origin remote."
		} else {
			ExitWith "Aborted by user. Remote not changed." 0
		}
	} else {
		Write-Host "Remote 'origin' already set to provided URL."
	}
} else {
	git remote add origin $RepoUrl
	Write-Host "Added remote origin -> $RepoUrl"
}

# Check if remote branch exists
Write-Host "Checking if remote branch '$Branch' exists..."
$exists = git ls-remote --heads origin $Branch 2>$null
if ($exists) {
	Write-Host "Remote branch 'origin/$Branch' exists."
	$del = Read-Host "Type YES to DELETE the remote branch 'origin/$Branch' and continue"
	if ($del -eq 'YES') {
		Write-Host "Deleting remote branch origin/$Branch..."
		git push origin --delete $Branch
	} else {
		Write-Host "Skipping delete. Existing remote branch will remain; push may fail without --Force."
	}
} else {
	Write-Host "Remote branch does not exist. Proceeding to push."
}

# Push
if ($Force) {
	Write-Host "Force pushing branch '$Branch' to origin..."
	git push -u origin $Branch --force
} else {
	Write-Host "Pushing branch '$Branch' to origin..."
	git push -u origin $Branch
}

if ($LASTEXITCODE -ne 0) { ExitWith "Push failed. Check credentials and remote repository." 3 }

Write-Host "Push completed. Verify on GitHub." -ForegroundColor Green
