using Dapper;
using MyFreezer.API.GraphQL.Types.Identity;
using MyFreezer.API.Models.DTOs;
using MyFreezer.API.Repositories.Interfaces;
using MyFreezer.API.Services;

namespace MyFreezer.API.Repositories;

public class AuthorizationDapperRepository : IAuthorizationRepository
{
    private readonly DapperContext _dapperContext;

    public AuthorizationDapperRepository(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    public void CreateRefreshToken(JWTTokenType refreshToken, int userId)
    {
        string query = "INSERT INTO Tokens (userId, issuedAt, expiresAt,refreshToken) " +
                       "VALUES (@userId, @issuedAt, @expiredAt, @token)";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query,
            new { userId, refreshToken.issuedAt, refreshToken.expiredAt, refreshToken.token });
    }

    public void UpdateRefreshToken(string oldRefreshToken, JWTTokenType refreshToken, int userId)
    {
        string query =
            "UPDATE Tokens SET refreshToken = @token, expiresAt = @issuedAt , issuedAt = @expiredAt " +
            "WHERE userId = @userId AND refreshToken = @oldRefreshToken";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query,
            new { userId, refreshToken.token, refreshToken.expiredAt, refreshToken.issuedAt, oldRefreshToken });
    }

    public void DeleteRefreshToken(string refreshToken)
    {
        string query = "DELETE Tokens WHERE refreshtoken = @refreshToken";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query, new { refreshToken });
    }

    public void DeleteAllRefreshTokens(int userId)
    {
        string query = "DELETE Tokens WHERE refreshtoken = @userId";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query, new { userId });
    }

    public JWTTokenType? GetRefreshToken(string refreshToken)
    {
        string query = "SELECT * FROM Tokens WHERE refreshToken = @refreshToken";
        using var connection = _dapperContext.CreateConnection();
        var jwtDTO = connection.QuerySingleOrDefault<JWTTokenDTO?>(query, new { refreshToken });
        var jwt = Mapper.DTOToJWTToken(jwtDTO);
        return jwt;
    }
}