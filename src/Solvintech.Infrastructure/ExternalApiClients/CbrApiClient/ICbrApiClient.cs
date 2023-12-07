using Solvintech.Infrastructure.ExternalApiClients.CbrApiClient.Models;

namespace Solvintech.Infrastructure.ExternalApiClients.CbrApiClient;

public interface ICbrApiClient
{
    public Task<ExchangeRates> GetCurrencyRatesAsync(DateOnly date);
}