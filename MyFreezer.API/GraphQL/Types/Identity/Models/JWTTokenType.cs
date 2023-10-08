using System;

namespace MyFreezer.API.GraphQL.Types.Identity;

public class JWTTokenType
{
    public string token { get; set; }
    public DateTime issuedAt { get; set; }
    public DateTime expiredAt { get; set; }
}