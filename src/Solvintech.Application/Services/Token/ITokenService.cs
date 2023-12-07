using System.Security.Claims;

namespace Solvintech.Application.Services.Token;

public interface ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims);

    public string GenerateRefreshToken();

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}