using GraphQL.Types;
using MyFreezer.API.Models;

namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserTypeGraphQL : ObjectGraphType<User>
{
    public UserTypeGraphQL()
    {
        Field(i => i.Id, type: typeof(IdGraphType));
        Field(i => i.Login);
        Field(i => i.Password);
        Field(i => i.RegistrationDate, nullable: true);
    }
}