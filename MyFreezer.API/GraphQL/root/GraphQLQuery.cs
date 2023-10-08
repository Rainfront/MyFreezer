using GraphQL.Types;
using MyFreezer.API.GraphQL.Types.UserTypes;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.GraphQL.Queries;

public class GraphQLQuery : ObjectGraphType
{
    
    public GraphQLQuery()
    {
        Field<UserQueryGraphQL>("user")
            .Resolve(context => new { });
    }
}