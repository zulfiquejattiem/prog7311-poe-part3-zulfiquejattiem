namespace TechMove.Shared.Contracts
{
    public class ContractCreateDto
    {
        public int ClientId { get; set; }
        public string ServiceLevel { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}