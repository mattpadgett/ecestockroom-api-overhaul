using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Services;

public class TokenFamilyService
{
    private readonly StockroomDbContext _stockroomDbContext;

    public TokenFamilyService(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public async Task<TokenFamily?> Get(Guid tokenFamilyId)
    {
        return await _stockroomDbContext.TokenFamilies
            .Where(x => x.TokenFamilyId == tokenFamilyId)
            .FirstOrDefaultAsync();
    }

    public async Task<List<TokenFamily>> GetByUserId(Guid userId)
    {
        return await _stockroomDbContext.TokenFamilies
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<TokenFamily?> GetByAuthorizationToken(string token)
    {
        return await _stockroomDbContext.TokenFamilies
            .Where(x => x.AuthorizationToken.Equals(token))
            .FirstOrDefaultAsync();
    }
    
    public async Task<TokenFamily?> GetByRefreshToken(string token)
    {
        return await _stockroomDbContext.TokenFamilies
            .Where(x => x.RefreshToken.Equals(token))
            .FirstOrDefaultAsync();
    }

    public async Task<TokenFamily> Create(TokenFamily tokenFamily)
    {
        await _stockroomDbContext.TokenFamilies.AddAsync(tokenFamily);
        await _stockroomDbContext.SaveChangesAsync();

        var createdFamily = await Get(tokenFamily.TokenFamilyId);
        
        if (createdFamily is null)
        {
            throw new RecordNotFoundException("Token family not found");
        }

        return createdFamily;
    }

    public async Task Invalidate(Guid tokenFamilyId)
    {
        var tokenFamily = await Get(tokenFamilyId);

        if (tokenFamily is null)
        {
            throw new RecordNotFoundException("Token family not found");
        }

        tokenFamily.ValidFlag = false;
        await _stockroomDbContext.SaveChangesAsync();
    }

    public async Task InvalidateByUserId(Guid userId)
    {
        var tokenFamilies = await GetByUserId(userId);

        foreach (var tokenFamily in tokenFamilies)
        {
            tokenFamily.ValidFlag = false;
        }
        
        await _stockroomDbContext.SaveChangesAsync();
    }
}