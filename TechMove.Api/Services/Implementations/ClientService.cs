using TechMove.Api.Data;
using TechMove.Api.Models;
using TechMove.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechMove.Api.Services.Implementations
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> GetAllAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }
    }
}
