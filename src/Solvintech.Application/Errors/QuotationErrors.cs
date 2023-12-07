using Solvintech.Shared.Utils;

namespace Solvintech.Application.Errors;

public class QuotationErrors
{
    public static readonly Error ExternalApiFailure = new(
        "Quotation.ExternalApiFailure", "Something went wrong with CBR API client.");
}