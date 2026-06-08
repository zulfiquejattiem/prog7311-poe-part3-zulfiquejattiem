using TechMove.Api.Models;

namespace TechMove.Api.Services.Interfaces
{
    public interface IContractService
    {
        Task<List<Contract>> GetAllAsync();
        Task<Contract> CreateAsync(Contract contract);
        Task<Contract?> UpdateStatusAsync(int id, int status);
    }
}