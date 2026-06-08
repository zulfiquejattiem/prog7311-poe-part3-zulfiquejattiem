using TechMove.Api.Models;

namespace TechMove.Api.Services.Interfaces
{
    public interface IServiceRequestService
    {
        Task<ServiceRequest> CreateAsync(ServiceRequest request);
    }
}