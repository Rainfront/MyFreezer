namespace MyFreezer.API.GraphQL.Types.Identity;

public interface IAuthorizationManager
{
    public const int AccessTokenExperationSeconds = 120;
    public const int RefreshTokenExperationSeconds = 31536000;
    public JWTTokenType GetAccessToken(int id);
    public JWTTokenType GetRefreshToken(int id);
}