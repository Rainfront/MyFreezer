namespace DBTest;

public class User
{
    public int? Id { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }

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