
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using NET6.BlazorWebAssemblyApp.Dtos;

namespace MyFreezer.API.DataServices;

public class UserDataService : IUserDataService
{
    private readonly HttpClient _httpClient;

    public UserDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        var options = new JsonSerializerOptions()
        {
            ReferenceHandler = ReferenceHandler.Preserve,
            PropertyNameCaseInsensitive = true
        };
        var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("User/GetUsers", options);
        //Thread.Sleep(5000);
        return users;
    }

    public async Task<UserDto> GetUser(int id)
    {
        var user = await _httpClient.GetFromJsonAsync<UserDto>($"User/GetUser/{id}");
        return user;
    }

    public async Task AddUser(UserDto userDto)
    {
        var jsonContent = JsonContent.Create(userDto);
        await _httpClient.PostAsync("User/CreateUser", jsonContent);
    }

    public async Task UpdateUser(UserDto userDto)
    {
        var jsonContent = JsonContent.Create(userDto);
        await _httpClient.PutAsync($"User/UpsertUser/{userDto.Id}", jsonContent);
    }
    public async Task DeleteUser(int userId)
    {
        await _httpClient.DeleteAsync($"User/DeleteUser/{userId}");
    }
}