using System.IdentityModel.Tokens.Jwt;

namespace MyFreezer.API.GraphQL.Types.Identity;

public interface IAuthorizationManager
{
    public const int AccessTokenExperationSeconds = 120;
    public const int RefreshTokenExperationSeconds = 1314000;
    public JWTTokenType GetAccessToken(int id);
    public JWTTokenType GetRefreshToken(int id);
    public bool IsValidRefreshToken(string token);
    public bool IsValidToken(string token);
    public JwtSecurityToken ReadJwtToken(string token);
    public string GetValueFromClaims(JwtSecurityToken token, string claimName);
}