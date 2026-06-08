using TechMove.Shared.Contracts;

namespace TechMove.Api.Models
{
    public static class ContractMappings
    {
        public static ContractDto ToDto(this Contract c)
        {
            return new ContractDto
            {
                Id = c.Id,
                ClientId = c.ClientId,
                ClientName = c.Client?.Name ?? string.Empty,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                Status = c.Status,
                ServiceLevel = c.ServiceLevel,
                AgreementPath = c.AgreementPath
            };
        }
    }
}
