using NET6.BlazorWebAssemblyApp.Dtos;

namespace MyFreezer.API.DataServices;

public interface IUserDataService
{
    public Task<List<UserDto>> GetUsers();
    public Task AddUser(UserDto userDto);
    public Task<UserDto> GetUser(int id);
    public Task DeleteUser(int userId);
    public Task UpdateUser(UserDto userDto);
}