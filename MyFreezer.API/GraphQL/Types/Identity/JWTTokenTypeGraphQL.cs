using System;
using GraphQL.Types;

namespace MyFreezer.API.GraphQL.Types.Identity;

public class JWTTokenTypeGraphQL : ObjectGraphType<JWTTokenType>
{
    public JWTTokenTypeGraphQL()
    {
        Field(i => i.token);
        Field(i => i.expiredAt);
        Field(i => i.issuedAt);
    }
}