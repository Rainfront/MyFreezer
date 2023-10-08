namespace MyFreezer.API.Controllers.ViewModels;

public class CreateUserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}
public class UserResponse
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
}
public class UpsertUserRequest
{
    public string Login { get; set; }
    public string Password { get; set; }
}