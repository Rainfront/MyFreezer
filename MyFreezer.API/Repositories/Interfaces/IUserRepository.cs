using System.Collections.Generic;
using MyFreezer.API.Controllers.ViewModels;
using MyFreezer.API.Models;

namespace MyFreezer.API.Repositories.Interfaces;

public interface IUserRepository
{
    public IEnumerable<User> GetUsers();
    public User GetUser(int id);
    public User GetUserByCredentials(string login, string password);
    public void CreateUser(User user);
    public void ChangePasswordByCredentials(string login, string oldPass, string newPass);
    public void UpsertUser(User user);
    public void DeleteUser(int id);

    
    public Permissions GetPermissions(int userId);
    public void CreatePermissions(Permissions permissions);
    public void UpdatePermissions(Permissions permissions);
    public void DeletePermissions(int userId);
}