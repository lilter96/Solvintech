namespace Solvintech.Application.DTO.Quotation;

public class QuotationDto
{
    public string Id { get; set; }

    public int NumCode { get; set; }

    public string CharCode { get; set; }

    public int Nominal { get; set; }

    public string Name { get; set; }

    public double Value { get; set; }

    public double VunitRate { get; set; }
}