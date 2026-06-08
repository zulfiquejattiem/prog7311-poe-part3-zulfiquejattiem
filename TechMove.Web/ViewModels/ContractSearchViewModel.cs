using TechMove.Shared.Contracts;

namespace TechMove.Web.ViewModels
{
    public class ContractSearchViewModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public ContractStatus? Status { get; set; }

        public List<ContractDto> Results { get; set; } = new();
    }
}