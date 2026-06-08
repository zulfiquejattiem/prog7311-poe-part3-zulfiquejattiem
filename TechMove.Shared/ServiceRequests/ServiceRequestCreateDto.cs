namespace TechMove.Shared.ServiceRequests
{
    public class ServiceRequestCreateDto
    {
        public int ContractId { get; set; }
        public string Description { get; set; } = "";
        public decimal CostUSD { get; set; }
    }
}