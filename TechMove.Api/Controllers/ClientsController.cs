using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMove.Api.Data;
using TechMove.Api.Models;
using TechMove.Shared.Clients;

namespace TechMove.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<ClientDto>> Get()
        {
            return await _context.Clients
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    ContactDetails = c.ContactDetails,
                    Region = c.Region
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            return Ok(new ClientDto
            {
                Id = client.Id,
                Name = client.Name,
                ContactDetails = client.ContactDetails,
                Region = client.Region
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ClientCreateDto dto)
        {
            var client = new Client
            {
                Name = dto.Name,
                ContactDetails = dto.ContactDetails,
                Region = dto.Region
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return Ok(client);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ClientDto dto)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            client.Name = dto.Name;
            client.ContactDetails = dto.ContactDetails;
            client.Region = dto.Region;

            await _context.SaveChangesAsync();
            return Ok(client);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return NotFound();

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}