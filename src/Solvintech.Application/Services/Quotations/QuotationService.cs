using AutoMapper;
using Solvintech.Application.DTO.Quotation;
using Solvintech.Application.Errors;
using Solvintech.Infrastructure.ExternalApiClients.CbrApiClient;
using Solvintech.Shared.Utils;

namespace Solvintech.Application.Services.Quotations;

public class QuotationService : IQuotationService
{
    private readonly ICbrApiClient _cbrApiClient;
    private readonly IMapper _mapper;

    public QuotationService(
        ICbrApiClient cbrApiClient,
        IMapper mapper)
    {
        _cbrApiClient = cbrApiClient ?? throw new ArgumentNullException(nameof(cbrApiClient));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<List<QuotationDto>>> GetQuotationsAsync(DateOnly date)
    {
        try
        {
            var exchangeRates = await _cbrApiClient.GetCurrencyRatesAsync(date);
            var quotations = exchangeRates != null
                ? _mapper.Map<List<QuotationDto>>(exchangeRates.Valutes)
                : new List<QuotationDto>();
            
            return Result<List<QuotationDto>>.Success(quotations);
        }
        catch (Exception ex)
        {
            return Result<List<QuotationDto>>.Failure(QuotationErrors.ExternalApiFailure, new List<string> { ex.ToString() });
        }
    } 
}