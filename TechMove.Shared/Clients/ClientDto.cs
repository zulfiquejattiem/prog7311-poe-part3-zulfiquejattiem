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
