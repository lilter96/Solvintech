using Solvintech.Application.DTO.Token;
using Solvintech.Application.DTO.User;
using Solvintech.Shared.Utils;

namespace Solvintech.Application.Services.Users;

public interface IUserService
{
    public Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto);

    public Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerUserDto);

    public Task<Result<RefreshTokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
}