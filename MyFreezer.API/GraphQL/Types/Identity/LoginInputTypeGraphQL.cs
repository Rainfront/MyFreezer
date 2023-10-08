using GraphQL.Types;
using MyFreezer.API.Models;

namespace MyFreezer.API.GraphQL.Types.Identity;

public class LoginInputTypeGraphQL : InputObjectGraphType<InputLogin>
{
    public LoginInputTypeGraphQL()
    {
        Field(i => i.Login);
        Field(i => i.Password);
    }
}