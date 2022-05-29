using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using ecestockroom_api_v2.Helpers.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Endpoints.Hello;

public class HelloWorldEndpoint : EndpointWithoutRequest<Temp>
{
    private readonly StockroomDbContext _stockroomDbContext;
    
    public HelloWorldEndpoint(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public override void Configure()
    {
        Get("hello-world");
        // AllowAnonymous();
        Permissions("login");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var token = JWTBearer.CreateToken(
            signingKey: "230BC7D5CFED2287FA404CAB2475A5C3FFAF90B3834CE86B677BDB567D812D57",
            issuer: "ecestockroom.ece.ttu.edu",
            audience: "ecestockroom.ece.ttu.edu",
            expireAt: DateTime.UtcNow.AddDays(1),
            roles: new [] {"RoleA", "RoleB"},
            permissions: new [] {"PermA", "PermC"}
        );

        // var x = await _stockroomDbContext.Users.FirstAsync();
        
        Console.WriteLine(HttpContext.User.Claims.Select(x => x.Subject));
        
        await SendOkAsync(new () {Message = "c"}, ct);
    }
}

public class Temp
{
    public string Message { get; set; }
}