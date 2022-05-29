using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Services;

public class UserService
{
    private readonly StockroomDbContext _stockroomDbContext;

    public UserService(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public async Task<User?> GetUser(Guid userId)
    {
        return await _stockroomDbContext.Users.FirstOrDefaultAsync(x => x.UserId == userId);
    }

    public async Task<User?> GetUser(string emailAddress)
    {
        return await _stockroomDbContext.Users.FirstOrDefaultAsync(x => EF.Functions.ILike(x.EmailAddress, emailAddress));
    }
}