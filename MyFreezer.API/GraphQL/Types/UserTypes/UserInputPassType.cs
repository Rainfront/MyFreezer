namespace MyFreezer.API.GraphQL.Types.UserTypes;

public class UserInputPassType
{
    public string login { get; set; }
    public string newPassword { get; set; }
    public string oldPassword { get; set; }
}