using TechMove.Api.Models;

namespace TechMove.Api.Services.Interfaces
{
    public interface IClientService
    {
        Task<List<Client>> GetAllAsync();
        Task<Client> CreateAsync(Client client);
    }
}