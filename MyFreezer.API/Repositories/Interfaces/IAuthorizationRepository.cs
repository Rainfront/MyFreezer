using MyFreezer.API.GraphQL.Types.Identity;

namespace MyFreezer.API.Repositories.Interfaces;

public interface IAuthorizationRepository
{
    public void CreateRefreshToken(JWTTokenType refreshToken,int userId);
    public void UpdateRefreshToken(string oldRefreshToken, JWTTokenType refreshToken, int userId);
    public void DeleteRefreshToken(string refreshToken);
    public void DeleteAllRefreshTokens(int userId);
    public JWTTokenType? GetRefreshToken(string refreshToken);
}