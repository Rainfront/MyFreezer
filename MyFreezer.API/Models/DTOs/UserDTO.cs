using System;

namespace MyFreezer.API.Models.DTOs;

public class UserDTO
{
    public int? userId { get; set; }
    public string login { get; set; }
    public string password { get; set; }
    public DateTime? registrationDate { get; set; }
}