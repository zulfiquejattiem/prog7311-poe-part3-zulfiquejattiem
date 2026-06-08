using TechMove.Api.Models;
using TechMove.Shared.ServiceRequests;

namespace TechMove.Api.Extensions
{
    public static class ServiceRequestExtensions
    {
        public static ServiceRequestDto ToDto(this ServiceRequest request)
        {
            return new ServiceRequestDto
            {
                Id = request.Id,
                ContractId = request.ContractId,
                Description = request.Description,
                CostUSD = request.CostUSD,
                CostZAR = request.CostZAR,
                Status = request.Status
            };
        }
    }
}
