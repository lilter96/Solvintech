using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solvintech.Application.DTO.Quotation;
using Solvintech.Application.Services.Quotations;
using Solvintech.Shared.Utils;

namespace Solvintech.Presentation.Controllers;

[ApiController]
[Route("api/quotation")]
[Authorize]
public class QuotationController : ControllerBase
{
    private readonly IQuotationService _quotationService;

    public QuotationController(IQuotationService quotationService)
    {
        _quotationService = quotationService ?? throw new ArgumentNullException(nameof(quotationService));
    }
    
    [HttpGet]
    [Route("{date}")]
    public async Task<IActionResult> GetQuotation(DateOnly date)
    {
        var result = await _quotationService.GetQuotationsAsync(date);
        
        return result.Match<IActionResult, List<QuotationDto>>(
            onSuccess: quotations => quotations.Any() ? Ok(quotations) : NotFound(),
            onFailure: error => StatusCode(StatusCodes.Status500InternalServerError, error));;
    }
}