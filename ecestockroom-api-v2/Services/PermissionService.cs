using ecestockroom_api_v2.Database;
using ecestockroom_api_v2.Domain;
using Microsoft.EntityFrameworkCore;

namespace ecestockroom_api_v2.Services;

public class PermissionService
{
    private readonly StockroomDbContext _stockroomDbContext;

    public PermissionService(StockroomDbContext stockroomDbContext)
    {
        _stockroomDbContext = stockroomDbContext;
    }

    public async Task<List<Permission>> Get()
    {
        return await _stockroomDbContext.Permissions.ToListAsync();
    }

    public async Task<List<Permission>> Get(List<Guid> permissionIds)
    {
        List<Permission> permissions = new List<Permission>();
        
        foreach (var permissionId in permissionIds)
        {
            var permission = await Get(permissionId);

            if (permission == null)
            {
                throw new NullReferenceException("Somebody directly modified the database without knowing what they were doing. What an Akhil move!");
            }
            
            permissions.Add(permission);
        }

        return permissions;
    }

    public async Task<Permission?> Get(Guid permissionId)
    {
        return await _stockroomDbContext.Permissions.FirstOrDefaultAsync(x => x.PermissionId == permissionId);
    }

    public async Task<Permission?> Get(string key)
    {
        return await _stockroomDbContext.Permissions.FirstOrDefaultAsync(x => EF.Functions.ILike(x.Key, key));
    }
}