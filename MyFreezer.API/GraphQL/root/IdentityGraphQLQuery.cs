using GraphQL;
using GraphQL.Types;
using MyFreezer.API.GraphQL.Types.Identity;
using MyFreezer.API.Models;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.GraphQL.Queries;

public class IdentityGraphQLQuery : ObjectGraphType
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationManager _authorizationManager;
    private readonly IAuthorizationRepository _authorizationRepository;

    public IdentityGraphQLQuery(
        IUserRepository userRepository,
        IAuthorizationManager authorizationManager,
        IAuthorizationRepository authorizationRepository)
    {
        _userRepository = userRepository;
        _authorizationManager = authorizationManager;
        _authorizationRepository = authorizationRepository;

        Field<IdentityOutputTypeGraphQL>("login")
            .Argument<NonNullGraphType<LoginInputTypeGraphQL>>("loginInfo")
            .Resolve(context =>
            {
                var loginInfo = context.GetArgument<InputLogin>("loginInfo");

                var user = _userRepository.GetUserByCredentials(loginInfo.Login, loginInfo.Password);

                if (user == null)
                {
                    context.Errors.Add(new ExecutionError("User not found"));
                    return null;
                }

                var accessToken = _authorizationManager.GetAccessToken((int)user.Id);
                var refreshToken = _authorizationManager.GetRefreshToken((int)user.Id);
                _authorizationRepository.CreateRefreshToken(refreshToken, (int)user.Id);

                var response = new IdentityOutputType
                {
                    accessToken = accessToken,
                    refreshToken = refreshToken,
                    userId = (int)user.Id,
                };
                return response;
            });
    }
}