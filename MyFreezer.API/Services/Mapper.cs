using MyFreezer.API.GraphQL.Types.Identity;
using MyFreezer.API.Models;
using MyFreezer.API.Models.DTOs;

namespace MyFreezer.API.Services;

public static class Mapper
{
    public static User? DTOToUser(UserDTO? userDTO)
    {
        if (userDTO == null)
            return null;
        var user = new User
        {
            Id = userDTO.userId,
            Login = userDTO.login,
            Password = userDTO.password,
            RegistrationDate = userDTO.registrationDate
        };
        return user;
    }

    public static UserDTO? UserToDTO(User? user)
    {
        
        if (user == null)
            return null;
        var userDTO = new UserDTO
        {
            userId = user.Id,
            login = user.Login,
            password = user.Password,
            registrationDate = user.RegistrationDate
        };
        return userDTO;
    }

    public static PermissionsDTO? PermissionsToDTO(Permissions? permissions)
    {
        if (permissions == null)
            return null;
        var permissionsDTO = new PermissionsDTO
        {
            userId = permissions.UserId,
            crudUser = permissions.CRUDUsers
        };
        return permissionsDTO;
    }

    public static Permissions? DTOToPermissions(PermissionsDTO? permissionsDTO)
    {
        if (permissionsDTO == null)
            return null;
        var permissions = new Permissions
        {
            UserId = permissionsDTO.userId,
            CRUDUsers = permissionsDTO.crudUser
        };
        return permissions;
    }

    public static JWTTokenDTO JWTTokenToDTO(JWTTokenType? jwtTokenType)
    {
        if (jwtTokenType == null)
            return null;
        var jwtDTO = new JWTTokenDTO
        {
            expiredAt = jwtTokenType.expiredAt,
            issuedAt = jwtTokenType.issuedAt,
            refreshToken = jwtTokenType.token
        };
        return jwtDTO;
    }

    public static JWTTokenType DTOToJWTToken(JWTTokenDTO? jwtTokenDTO)
    {
        if (jwtTokenDTO == null)
            return null;
        var jwt = new JWTTokenType
        {
            expiredAt = jwtTokenDTO.expiredAt,
            issuedAt = jwtTokenDTO.issuedAt,
            token = jwtTokenDTO.refreshToken
        };
        return jwt;
    }
}