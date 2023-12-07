namespace Solvintech.Application.Configurations;

public record JwtOptions(
    string Issuer,
    string Audience,
    string SigningKey,
    int ExpirationSeconds,
    int RefreshTokenValidityInDays
);