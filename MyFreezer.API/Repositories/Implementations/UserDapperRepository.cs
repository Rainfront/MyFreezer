using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using MyFreezer.API.Models;
using MyFreezer.API.Models.DTOs;
using MyFreezer.API.Repositories.Interfaces;
using MyFreezer.API.Services;

namespace MyFreezer.API.Repositories;

public class UserDapperRepository : IUserRepository
{
    private readonly DapperContext _dapperContext;

    public UserDapperRepository(DapperContext context)
    {
        _dapperContext = context;
    }

    public IEnumerable<User> GetUsers()
    {
        string query = "SELECT * FROM Users";
        using var connection = _dapperContext.CreateConnection();
        var userDTos = connection.Query<UserDTO>(query);
        var users = userDTos.Select(x => Mapper.DTOToUser(x)).ToList();
        return users;
    }

    public User GetUser(int id)
    {
        string query = "SELECT * FROM Users WHERE userId = @id";
        using var connection = _dapperContext.CreateConnection();
        var users = connection.Query<UserDTO>(query, new { id });
        if (users.Count() < 1)
        {
            return null;
        }

        var userDTO = users.First();
        var user = Mapper.DTOToUser(userDTO);
        return user;
    }

    public User GetUserByCredentials(string login, string password)
    {
        string query = "SELECT * FROM Users WHERE login = @login AND password = @password";
        using var connection = _dapperContext.CreateConnection();
        var userDTO = connection.Query<UserDTO>(query, new { login, password }).FirstOrDefault();
        var user = Mapper.DTOToUser(userDTO);
        return user;
    }

    public void CreateUser(User user)
    {
        if (IsLoginTaken(user.Login))
            return;
        var userDTO = new UserDTO
        {
            userId = user.Id,
            login = user.Login,
            password = user.Password,
            registrationDate = DateTime.Now
        };
        string query = "INSERT INTO Users (login, password, registrationDate) " +
                       "VALUES (@login, @password, @registrationDate)";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query, userDTO);
    }

    public void ChangePasswordByCredentials(string login, string oldPass, string newPass)
    {
        string query = "SELECT password FROM Users WHERE login = @login";
        using var connection = _dapperContext.CreateConnection();
        var dbPass = connection.QuerySingle<string>(query, new { login });
        if (oldPass != dbPass)
            return;
        query = "UPDATE Users SET password = @newPass WHERE login = @login";
        connection.Execute(query, new { newPass, login });
    }
    public void UpsertUser(User user)
    {
        throw new NotImplementedException();
    }

    public void DeleteUser(int id)
    {
        string query = "DELETE FROM Users WHERE userId = @id";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query, new { id });

    }

    public Permissions GetPermissions(int userId)
    {
        string query = "SELECT * FROM Permissions WHERE userID = @userId";
        using var connection = _dapperContext.CreateConnection();
        var permissionsDTO = connection.QuerySingle<PermissionsDTO>(query, new { userId });
        var permissions = Mapper.DTOToPermissions(permissionsDTO);
        return permissions;
    }

    public void CreatePermissions(Permissions permissions)
    {
        string query = "INSERT INTO Permissions (userId, crudUser) VALUES (@userId, @crudUser)";
        using var connection = _dapperContext.CreateConnection();
        var permissionsDTO = Mapper.PermissionsToDTO(permissions);
        connection.Execute(query, new { permissionsDTO.userId, permissionsDTO.crudUser });
    }

    public void UpdatePermissions(Permissions permissions)
    {
        string query = "UPDATE Permissions SET crudUser = @crudUser";
        using var connection = _dapperContext.CreateConnection();
        var permissionsDTO = Mapper.PermissionsToDTO(permissions);
        connection.Execute(query, new { permissionsDTO.crudUser });
    }

    public void DeletePermissions(int userId)
    {
        string query = "DELETE Permissions WHERE userId = @userId";
        using var connection = _dapperContext.CreateConnection();
        connection.Execute(query, new { userId });
    }

    public bool IsLoginTaken(string login)
    {
        string query = "SELECT userId FROM Users WHERE login = @login";
        using var connection = _dapperContext.CreateConnection();
        var list = connection.Query<int>(query, new { login });
        return list.Count() != 0;
    }
}