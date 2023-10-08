using GraphQL;
using GraphQL.Types;
using MyFreezer.API.Models;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserMutationGraphQL : ObjectGraphType
{
    private readonly IUserRepository _userRepository;

    public UserMutationGraphQL(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        Field<StringGraphType>("createUser")
            .Argument<NonNullGraphType<UserInputTypeGraphQL>>("inputUser")
            .Resolve(context =>
            {
                var inputUser = context.GetArgument<User>("inputUser");
                var user = new User { Login = inputUser.Login, Password = inputUser.Password };
                _userRepository.CreateUser(user);
                return "User created successfully";
            });
        Field<StringGraphType>("updateUserPassword")
            .Argument<NonNullGraphType<UserInputPassTypeGraphQL>>("inputUser")
            .Resolve(context =>
            {
                var inputUser = context.GetArgument<UserInputPassType>("inputUser");
                _userRepository.ChangePasswordByCredentials(inputUser.login, inputUser.oldPassword, inputUser.newPassword);
                return "Password changed successfully";

            });
        Field<StringGraphType>("deleteUser")
            .Argument<NonNullGraphType<IdGraphType>>("id")
            .Resolve(context =>
            {
                var id = context.GetArgument<int>("id");
                _userRepository.DeleteUser(id);
                return "User deleted successfully";
            });
    }

}