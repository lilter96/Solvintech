using Solvintech.Application.DTO.Quotation;
using Solvintech.Shared.Utils;

namespace Solvintech.Application.Services.Quotations;

public interface IQuotationService
{
    public Task<Result<List<QuotationDto>>> GetQuotationsAsync(DateOnly date);
}