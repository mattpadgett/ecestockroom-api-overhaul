using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Services;

public class AuthService
{
    private readonly StockroomDbContext _stockroomDbContext;

    public AuthService(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public async Task<TokenFamily?> GetTokenFamily(Guid tokenFamilyId)
    {
        return await _stockroomDbContext.TokenFamilies.FirstOrDefaultAsync(x => x.TokenFamilyId == tokenFamilyId);
    }

    public async Task<TokenFamily> AddTokenFamily(TokenFamily tokenFamily)
    {
        await _stockroomDbContext.TokenFamilies.AddAsync(tokenFamily);

        return (await GetTokenFamily(tokenFamily.TokenFamilyId))!;
    }
}