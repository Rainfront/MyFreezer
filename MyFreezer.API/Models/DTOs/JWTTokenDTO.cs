using System;

namespace MyFreezer.API.Models.DTOs;

public class JWTTokenDTO
{
    public int userId { get; set; }
    public DateTime issuedAt { get; set; }
    public DateTime expiredAt { get; set; }
    public string refreshToken { get; set; }
}