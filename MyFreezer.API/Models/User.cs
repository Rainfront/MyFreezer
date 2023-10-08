using System;

namespace MyFreezer.API.Models;

public class User
{
    public int? Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public DateTime? RegistrationDate { get; set; }

    public User()
    {
        
    }

    public User(int id, string login, string password)
    {
        Id = id;
        Login = login;
        Password = password;
    }
}