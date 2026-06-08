namespace TechMove.Shared.Contracts
{
    public class ContractDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }

        public string ClientName { get; set; } = "";

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ContractStatus Status { get; set; }

        public string ServiceLevel { get; set; } = "";

        // ✅ ADD THIS
        public string? AgreementPath { get; set; }
    }
}