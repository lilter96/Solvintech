using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Solvintech.Application.Configurations;
using Solvintech.Application.DTO.Token;
using Solvintech.Application.DTO.User;
using Solvintech.Application.Errors;
using Solvintech.Application.Services.Token;
using Solvintech.Infrastructure.Data.Entities;
using Solvintech.Shared.Utils;

namespace Solvintech.Application.Services.Users;

public class UserService : IUserService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwtOptions;
    public UserService(
        ITokenService tokenService,
        UserManager<ApplicationUser> userManager,
        IMapper mapper, 
        JwtOptions jwtOptions)
    {
        _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));;
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));;
        _jwtOptions = jwtOptions ?? throw new ArgumentNullException(nameof(jwtOptions));
    }

    public async Task<Result<UserDto>> LoginAsync(LoginUserDto loginUserDto)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(loginUserDto.Email);
            
            if (user is null)
            {
                return Result<UserDto>.Failure(UserErrors.NotFound, $"User with email {loginUserDto.Email} not found.");
            }

            var passwordVerificationResult = _userManager
                .PasswordHasher
                .VerifyHashedPassword(user, user.PasswordHash!, loginUserDto.Password);

            if (passwordVerificationResult != PasswordVerificationResult.Success)
            {
                return Result<UserDto>.Failure(UserErrors.UnableLogin, "Can't complete password sign in");
            }

            var claims = GetClaims(user);
            var apiToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.ApiToken = apiToken;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays);

            var updateWithTokenResult = await _userManager.UpdateAsync(user);

            if (!updateWithTokenResult.Succeeded)
            {
                return GetFailureResultFromIdentityResult<UserDto>(updateWithTokenResult, UserErrors.UnableUpdate);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(UserErrors.UnableLogin, ex.ToString());
        }
    }

    public async Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            var user = _mapper.Map<ApplicationUser>(registerUserDto);
            var claims = GetClaims(user);
            var jwtToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.ApiToken = jwtToken;
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays);
            var createResult = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (!createResult.Succeeded)
            {
                return GetFailureResultFromIdentityResult<UserDto>(createResult, UserErrors.UnableRegister);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure(UserErrors.UnableRegister, ex.ToString());
        }
    }

    public async Task<Result<RefreshTokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var accessToken = refreshTokenDto.ApiToken;
        var refreshToken = refreshTokenDto.RefreshToken;

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
        if (principal == null)
        {
            return Result<RefreshTokenDto>.Failure(UserErrors.InvalidTokens, "Invalid access token or refresh token");
        }

        var username = principal.Identity!.Name;

        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            return Result<RefreshTokenDto>.Failure(UserErrors.InvalidTokens, "Invalid access token or refresh token");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.ApiToken = newAccessToken;
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddDays(_jwtOptions.RefreshTokenValidityInDays);
        await _userManager.UpdateAsync(user);

        var tokensResult = new RefreshTokenDto(newAccessToken, newRefreshToken);
        
        return Result<RefreshTokenDto>.Success(tokensResult);
    }

    private Result<T> GetFailureResultFromIdentityResult<T>(
        IdentityResult result,
        Error error)
    {
        var details = result.Errors.Select(e => e.Description).ToList();
        return Result<T>.Failure(error, details);
    }

    private List<Claim> GetClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        return claims;
    }
}