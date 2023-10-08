using GraphQL.Types;
using MyFreezer.API.GraphQL.Types.UserTypes;

namespace MyFreezer.API.GraphQL.Queries;

public class GraphQLMutation : ObjectGraphType
{
    public GraphQLMutation()
    {
        Field<UserMutationGraphQL>("user")
            .Resolve(context => new { });
    }
}