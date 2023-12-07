using Microsoft.AspNetCore.Identity;

namespace Solvintech.Infrastructure.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public string ApiToken { get; set; }
    
    public string RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }
}