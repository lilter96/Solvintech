using System.Globalization;
using AutoMapper;
using Solvintech.Application.DTO.Quotation;
using Solvintech.Infrastructure.ExternalApiClients.CbrApiClient.Models;

namespace Solvintech.Application.Mappings;

public class QuotationProfile : Profile
{
    public QuotationProfile()
    {
        CreateMap<Rate, QuotationDto>()
            .ForMember(d => d.Id, s => s.MapFrom(x => x.ID))
            .ForMember(d => d.Value, s => s.MapFrom(x => double.Parse(x.Value,CultureInfo.GetCultureInfo("ru-RU"))))
            .ForMember(d => d.VunitRate, s => s.MapFrom(x => double.Parse(x.VunitRate,CultureInfo.GetCultureInfo("ru-RU"))));

    }
}