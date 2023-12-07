using System.Xml.Serialization;

namespace Solvintech.Infrastructure.ExternalApiClients.CbrApiClient.Models;

[XmlRoot("ValCurs")]
public class ExchangeRates
{
    [XmlElement("Valute")]
    public List<Rate> Valutes { get; set; }
}