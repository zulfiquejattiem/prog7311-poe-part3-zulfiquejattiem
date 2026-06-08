namespace TechMove.Api.Services.Interfaces
{
    public interface ICurrencyExchangeService
    {
        Task<decimal> GetUsdToZarAsync();
    }
}