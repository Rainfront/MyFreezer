using System.Text.Json.Serialization;

namespace NET6.BlazorWebAssemblyApp.Dtos;

public class UserDto
{
    [JsonPropertyName("Id")]
    public int? Id { get; set; }
    
    [JsonPropertyName("Login")]
    public string? Login { get; set; }
    
    [JsonPropertyName("Password")]
    public string? Password { get; set; }
    

    public UserDto()
    {
    }

    public UserDto(int id, string login, string password)
    {
        Id = id;
        Login = login;
        Password = password;
    }
}