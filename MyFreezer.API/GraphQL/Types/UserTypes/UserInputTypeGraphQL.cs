using GraphQL.Types;
using MyFreezer.API.Models;

namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserInputTypeGraphQL : InputObjectGraphType<User>
{
    public UserInputTypeGraphQL()
    {
        Field(i => i.Login);
        Field(i => i.Password);
    }
}