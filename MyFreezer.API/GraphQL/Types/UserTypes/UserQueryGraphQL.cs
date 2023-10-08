using GraphQL;
using GraphQL.Types;
using MyFreezer.API.Models;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserQueryGraphQL : ObjectGraphType
{
    private readonly IUserRepository _userRepository;

    public UserQueryGraphQL(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        Field<ListGraphType<UserTypeGraphQL>>("users")
            .Resolve(context => _userRepository.GetUsers())
            .AuthorizeWithPolicy("CRUDUsers");
        Field<UserTypeGraphQL>("user")
            .Argument<NonNullGraphType<IntGraphType>>("id")
            .Resolve(context =>
            {
                int id = context.GetArgument<int>("id");
                var user = _userRepository.GetUser(id);
                return user;
            });
        
    }
}