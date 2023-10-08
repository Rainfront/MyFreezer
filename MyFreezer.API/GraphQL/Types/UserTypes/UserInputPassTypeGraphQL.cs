using GraphQL.Types;
using MyFreezer.API.Models;

namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserInputPassTypeGraphQL : InputObjectGraphType<UserInputPassType>
{
    public UserInputPassTypeGraphQL()
    {
        Field(i => i.login);
        Field(i => i.newPassword);
        Field(i => i.oldPassword);
    }
}