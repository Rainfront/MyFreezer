using GraphQL.Types;

namespace MyFreezer.API.GraphQL.Types.Identity;

public class IdentityOutputTypeGraphQL: ObjectGraphType<IdentityOutputType>
{
    public IdentityOutputTypeGraphQL()
    {
        Field(i => i.userId);
        Field(i => i.accessToken, nullable: true, type: typeof(JWTTokenTypeGraphQL));
        Field(i => i.refreshToken, nullable: true, type: typeof(JWTTokenTypeGraphQL));
    }
}