using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Services;

public class RoleService
{
    private readonly StockroomDbContext _stockroomDbContext;

    public RoleService(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public async Task<List<Role>> Get()
    {
        return await _stockroomDbContext.Roles.ToListAsync();
    }
    
    public async Task<Role?> GetRole(Guid roleId)
    {
        return await _stockroomDbContext.Roles.FirstOrDefaultAsync(x => x.RoleId == roleId);
    }
}