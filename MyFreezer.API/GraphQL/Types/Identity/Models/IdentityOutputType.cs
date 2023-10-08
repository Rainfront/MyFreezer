namespace MyFreezer.API.GraphQL.Types.Identity;

public class IdentityOutputType
{
    public int userId { get; set; }
    public JWTTokenType? accessToken { get; set; }
    public JWTTokenType? refreshToken { get; set; }
}