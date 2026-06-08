using TechMove.Shared.Contracts;

namespace TechMove.Api.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ContractStatus Status { get; set; }
        public string ServiceLevel { get; set; } = "";

        public string? AgreementPath { get; set; }

        public List<ServiceRequest> ServiceRequests { get; set; } = new();
    }
}