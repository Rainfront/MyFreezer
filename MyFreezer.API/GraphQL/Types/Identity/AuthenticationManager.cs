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

    public AuthorizationManager(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
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
    
}