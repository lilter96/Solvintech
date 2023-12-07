using System.Xml.Serialization;

namespace Solvintech.Infrastructure.ExternalApiClients.CbrApiClient.Models;

public class Rate
{
    [XmlAttribute("ID")]
    public string ID { get; set; }

    public int NumCode { get; set; }
    
    public string CharCode { get; set; }
    
    public int Nominal { get; set; }
    
    public string Name { get; set; }

    public string Value { get; set; }
    
    public string VunitRate { get; set; }
}