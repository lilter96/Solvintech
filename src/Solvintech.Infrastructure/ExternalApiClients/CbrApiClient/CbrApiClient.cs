using System.Text;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Solvintech.Infrastructure.ExternalApiClients.CbrApiClient.Models;

namespace Solvintech.Infrastructure.ExternalApiClients.CbrApiClient;

public class CbrApiClient : ICbrApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CbrApiClient> _logger;

    public CbrApiClient(HttpClient httpClient, ILogger<CbrApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRates> GetCurrencyRatesAsync(DateOnly date)
    {
        var queryString = $"date_req={date:dd/MM/yyyy}";

        var requestUri = "XML_daily.asp?" + queryString;
        
        try
        {
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            
            var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream, Encoding.GetEncoding("windows-1251"));
            var content = await reader.ReadToEndAsync();

            using var stringReader = new StringReader(content);
            var serializer = new XmlSerializer(typeof(ExchangeRates));

            return (serializer.Deserialize(stringReader) as ExchangeRates)!;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to {RequestUri} failed", requestUri);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the HTTP request");
            throw;
        }
    }
}