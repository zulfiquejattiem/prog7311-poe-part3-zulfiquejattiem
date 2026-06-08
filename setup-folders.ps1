# Script to create the project folders and files listed by the user.
# For each folder: if it exists it will be removed and recreated.
# For each file: it will be created (empty) or overwritten. ClientDto.cs will be populated.

$root = (Get-Location).Path

$dirs = @(
    "TechMove.Shared\Clients",
    "TechMove.Shared\Contracts",
    "TechMove.Shared\ServiceRequests",
    "TechMove.Shared\Auth",

    "TechMove.Api\Models",
    "TechMove.Api\Data",
    "TechMove.Api\Controllers",
    "TechMove.Api\Services\Interfaces",
    "TechMove.Api\Services\Implementations",

    "TechMove.Web\ViewModels",
    "TechMove.Web\Views\Clients",
    "TechMove.Web\Views\Contracts",
    "TechMove.Web\Views\ServiceRequests"
)

foreach ($d in $dirs) {
    $full = Join-Path $root $d
    if (Test-Path $full) {
        Remove-Item $full -Recurse -Force -ErrorAction SilentlyContinue
    }
    New-Item -Path $full -ItemType Directory -Force | Out-Null
}

# Files to create (empty). Will overwrite if exists.
$files = @(
    "TechMove.Shared\Clients\ClientDto.cs",
    "TechMove.Shared\Contracts\ContractStatus.cs",
    "TechMove.Shared\Contracts\ContractDto.cs",
    "TechMove.Shared\Contracts\ContractCreateDto.cs",
    "TechMove.Shared\Contracts\ContractStatusUpdateDto.cs",
    "TechMove.Shared\ServiceRequests\ServiceRequestStatus.cs",
    "TechMove.Shared\ServiceRequests\ServiceRequestDto.cs",
    "TechMove.Shared\ServiceRequests\ServiceRequestCreateDto.cs",
    "TechMove.Shared\Auth\LoginRequestDto.cs",
    "TechMove.Shared\Auth\LoginResponseDto.cs",

    "TechMove.Api\Models\Client.cs",
    "TechMove.Api\Models\Contract.cs",
    "TechMove.Api\Models\ServiceRequest.cs",
    "TechMove.Api\Models\FileDownloadResult.cs",
    "TechMove.Api\Data\ApplicationDbContext.cs",
    "TechMove.Api\Controllers\AuthController.cs",
    "TechMove.Api\Controllers\ClientsController.cs",
    "TechMove.Api\Controllers\ContractsController.cs",
    "TechMove.Api\Controllers\ServiceRequestsController.cs",
    "TechMove.Api\Services\Interfaces\IClientService.cs",
    "TechMove.Api\Services\Interfaces\IContractService.cs",
    "TechMove.Api\Services\Interfaces\IServiceRequestService.cs",
    "TechMove.Api\Services\Interfaces\ICurrencyExchangeService.cs",
    "TechMove.Api\Services\Interfaces\IFileService.cs",
    "TechMove.Api\Services\Implementations\ClientService.cs",
    "TechMove.Api\Services\Implementations\ContractService.cs",
    "TechMove.Api\Services\Implementations\ServiceRequestService.cs",
    "TechMove.Api\Services\Implementations\CurrencyExchangeService.cs",
    "TechMove.Api\Services\Implementations\FileService.cs",

    "TechMove.Web\Controllers\ClientsController.cs",
    "TechMove.Web\Controllers\ContractsController.cs",
    "TechMove.Web\Controllers\ServiceRequestsController.cs",
    "TechMove.Web\ViewModels\ContractSearchViewModel.cs",
    "TechMove.Web\ViewModels\CostCalculationRequest.cs",
    "TechMove.Web\Views\Clients\Index.cshtml",
    "TechMove.Web\Views\Clients\Create.cshtml",
    "TechMove.Web\Views\Clients\Edit.cshtml",
    "TechMove.Web\Views\Clients\Delete.cshtml",
    "TechMove.Web\Views\Clients\Details.cshtml",
    "TechMove.Web\Views\Contracts\Index.cshtml",
    "TechMove.Web\Views\Contracts\Create.cshtml",
    "TechMove.Web\Views\Contracts\Edit.cshtml",
    "TechMove.Web\Views\Contracts\Delete.cshtml",
    "TechMove.Web\Views\Contracts\Details.cshtml",
    "TechMove.Web\Views\ServiceRequests\Index.cshtml",
    "TechMove.Web\Views\ServiceRequests\Create.cshtml",
    "TechMove.Web\Views\ServiceRequests\Edit.cshtml",
    "TechMove.Web\Views\ServiceRequests\Delete.cshtml",
    "TechMove.Web\Views\ServiceRequests\Details.cshtml"
)

foreach ($f in $files) {
    $full = Join-Path $root $f
    $dir = Split-Path $full -Parent
    if (-not (Test-Path $dir)) { New-Item -Path $dir -ItemType Directory -Force | Out-Null }

    # If this is the ClientDto file, write the provided content. Otherwise create an empty file.
    if ($f -ieq "TechMove.Shared\\Clients\\ClientDto.cs") {
        @"
using System.ComponentModel.DataAnnotations;

namespace TechMove.Shared.Clients
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ContactDetails { get; set; } = "";
        public string Region { get; set; } = "";
    }

    public class ClientCreateDto
    {
        public string Name { get; set; } = "";
        public string ContactDetails { get; set; } = "";
        public string Region { get; set; } = "";
    }
}
"@ | Out-File -FilePath $full -Encoding UTF8 -Force
    }
    else {
        # Create or truncate the file
        Out-File -FilePath $full -Encoding UTF8 -Force
    }
}

Write-Host "Folders and files created/overwritten as listed." -ForegroundColor Green
