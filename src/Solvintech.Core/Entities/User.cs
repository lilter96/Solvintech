using Microsoft.AspNetCore.Identity;

namespace Solvintech.Core.Entities;

public class User : IdentityUser
{
    public string ApiToken { get; set; }
}