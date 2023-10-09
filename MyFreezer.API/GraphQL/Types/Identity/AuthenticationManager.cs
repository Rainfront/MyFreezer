using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.GraphQL.Types.Identity;

public class AuthorizationManager : IAuthorizationManager
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IAuthorizationRepository _authorizationRepository;

    public AuthorizationManager(IUserRepository userRepository, IConfiguration configuration,
        IAuthorizationRepository authorizationRepository)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _authorizationRepository = authorizationRepository;
    }

    public JWTTokenType GetAccessToken(int userId)
    {
        var permissions = _userRepository.GetPermissions(userId);

        var issuedAt = DateTime.UtcNow;
        var expiredAt = DateTime.UtcNow.Add(
            TimeSpan.FromMinutes(IAuthorizationManager.AccessTokenExperationSeconds)
        );
        var newAccessToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Author"],
            audience: _configuration["JWT:Audience"],
            claims: new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)issuedAt).ToUnixTimeSeconds().ToString()),
                new Claim("CRUDUsers", permissions.CRUDUsers.ToString())
            },
            expires: expiredAt,
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256
            )
        );
        var jwtToken = new JWTTokenType
        {
            token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            expiredAt = expiredAt,
            issuedAt = issuedAt
        };
        return jwtToken;
    }

    public JWTTokenType GetRefreshToken(int userId)
    {
        var issuedAt = DateTime.UtcNow;
        var expiredAt = DateTime.UtcNow.Add(
            TimeSpan.FromMinutes(IAuthorizationManager.RefreshTokenExperationSeconds)
        );

        var newRefreshToken = new JwtSecurityToken(
            issuer: _configuration["JWT:Author"],
            audience: _configuration["JWT:Audience"],
            claims: new[]
            {
                new Claim("userId", userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)issuedAt).ToUnixTimeSeconds().ToString()),
                new Claim("isRefreshToken", true.ToString())
            },
            expires: expiredAt,
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256
            )
        );
        var jwtToken = new JWTTokenType
        {
            token = new JwtSecurityTokenHandler().WriteToken(newRefreshToken),
            expiredAt = expiredAt,
            issuedAt = issuedAt
        };
        return jwtToken;
    }

    public JwtSecurityToken ReadJwtToken(string token) => new JwtSecurityTokenHandler().ReadJwtToken(token);

    public string GetValueFromClaims(JwtSecurityToken token, string claimName) =>
        token.Claims.First(c => c.Type == claimName).Value;


    public bool IsValidToken(string token)
    {
        try
        {
            var tokenValidate = new JwtSecurityTokenHandler();

            tokenValidate.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _configuration["JWT:Author"],
                ValidateAudience = true,
                ValidAudience = _configuration["JWT:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken securityToken);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public bool IsValidRefreshToken(string token)
    {
        if (!IsValidToken(token))
            return false;

        JwtSecurityToken refreshToken = ReadJwtToken(token);
        var isRefreshToken = bool.Parse(GetValueFromClaims(refreshToken, "isRefreshToken"));
        if (!isRefreshToken)
            throw new Exception("It is not refresh token!");

        var savedTokenType = _authorizationRepository.GetRefreshToken(token);
        var savedToken = ReadJwtToken(savedTokenType!.token);

        return refreshToken.RawSignature == savedToken.RawSignature;
    }
}