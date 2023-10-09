using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
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
        Field<IdentityOutputTypeGraphQL>("refreshTokens")
            .Resolve(context =>
            {
                HttpContext httpContext = context.RequestServices.GetService<IHttpContextAccessor>().HttpContext;

                var refreshTokenRaw = httpContext.Request.Headers.Authorization;
                var token = refreshTokenRaw.ToString().Replace("Bearer ", string.Empty);
                if (token == "")
                {
                    context.Errors.Add(new ExecutionError("Token is missing."));
                    return null;
                }
                var refreshToken = _authorizationManager.ReadJwtToken(token);
                try
                {
                    if (_authorizationManager.IsValidRefreshToken(token))
                    {
                        var userId = Convert.ToInt32(_authorizationManager.GetValueFromClaims(refreshToken, "userId"));
                        var newAccessToken = _authorizationManager.GetAccessToken(userId);
                        var newRefreshToken = _authorizationManager.GetRefreshToken(userId);
                        _authorizationRepository.UpdateRefreshToken(token, newRefreshToken, userId);
                        var response = new IdentityOutputType
                        {
                            accessToken = newAccessToken,
                            refreshToken = newRefreshToken,
                            userId = userId
                        };
                        return response;
                    }
                    _authorizationRepository.DeleteRefreshToken(token);
                    context.Errors.Add(new ExecutionError("Refresh token is not valid."));
                    return null;
                }
                catch (Exception exception)
                {
                    context.Errors.Add(new ExecutionError(exception.Message));
                    return null;
                }
            });
        Field<StringGraphType>("logout")
            .Resolve(context =>
            {
                HttpContext httpContext = context.RequestServices.GetService<IHttpContextAccessor>().HttpContext;
                var refreshTokenRaw = httpContext.Request.Headers.Authorization;
                var token = refreshTokenRaw.ToString().Replace("Bearer ", string.Empty);
                _authorizationRepository.DeleteRefreshToken(token);
                return "You successfully logged out";
            });
    }
}