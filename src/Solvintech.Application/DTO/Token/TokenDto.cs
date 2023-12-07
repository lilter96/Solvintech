namespace Solvintech.Application.DTO.Token;

public record RefreshTokenDto(
    string ApiToken,
    string RefreshToken
);