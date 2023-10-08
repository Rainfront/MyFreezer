using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyFreezer.API.Controllers.ViewModels;
using MyFreezer.API.Repositories;
using MyFreezer.API.Models;
using MyFreezer.API.Repositories.Interfaces;

namespace MyFreezer.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository { get; set; }

    public UserController( IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    [HttpGet]
    [Route("GetUsers")]
    public IActionResult GetUsers()
    {
        var users = _userRepository.GetUsers();
        return Ok(users);
    }
    [HttpGet]
    [Route("GetUser/{id}")]
    public IActionResult GetUser([FromRoute] int id)
    {
        User user = _userRepository.GetUser(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    [Route("CreateUser")]
    public IActionResult CreateUser([FromBody] CreateUserRequest createUserRequest)
    {
        var user = new User
        {
            Login = createUserRequest.Login,
            Password = createUserRequest.Password,
        };
        _userRepository.CreateUser(user);
        
//        return CreatedAtAction(nameof(GetUser), new {id=user.Id}, user);
        return CreatedAtRoute(nameof(GetUser), new {id=user.Id}, user);
        return CreatedAtRoute(nameof(GetUser), user);
    }

    [HttpPut]
    [Route("UpsertUser/{id}")]
    public IActionResult UpsertUser([FromRoute] int id, [FromBody] UpsertUserRequest upsertUserRequest)
    {
        var user = new User
        {
            Id = id,
            Login = upsertUserRequest.Login,
            Password = upsertUserRequest.Password
        };
        _userRepository.UpsertUser(user);
        return Ok();
    }

    [HttpDelete]
    [Route("DeleteUser/{id}")]
    public IActionResult DeleteUser([FromRoute] int id)
    {
        _userRepository.DeleteUser(id);
        return Ok();
    }
}