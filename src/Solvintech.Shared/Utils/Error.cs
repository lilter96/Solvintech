namespace Solvintech.Shared.Utils;

public sealed class Error
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public string Code { get; init; }
    
    public string Description { get; init; }
    
    public List<string> Details { get; set; }

    public void AddDetails(List<string> details)
    {
        Details = details;
    }
}