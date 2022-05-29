using ecestockroom_api_v2.Domain;

namespace ecestockroom_api_v2.Helpers.Security;

public class PermissionHelpers
{
    private readonly PermissionService _permissionService;
    private readonly RoleService _roleService;
    private readonly UserService _userService;

    public PermissionHelpers(PermissionService permissionService, RoleService roleService, UserService userService)
    {
        _permissionService = permissionService;
        _roleService = roleService;
        _userService = userService;
    }

    public async Task<List<Permission>> GetUserPermissions(User user)
    {
        var allRoles = await _roleService.Get(); // Preload roles
        var allPerms = await _permissionService.Get(); // Preload permissions

        List<Permission> finalPermissions = new();

        foreach (var permissionId in user.PermissionIds)
        {
            var permission = allPerms.FirstOrDefault(x => x.PermissionId == permissionId);

            if (permission is null)
            {
                throw new NullReferenceException(
                    "A permission was deleted improperly and a user references a nonexistent permission");
            }
            
            finalPermissions.Add(permission);
        }

        foreach (var roleId in user.RoleIds)
        {
            var role = allRoles.FirstOrDefault(x => x.RoleId == roleId);

            if (role is null)
            {
                throw new NullReferenceException("A role was deleted improperly and user references a nonexistent role");
            }
            
            // Finished loading the permissions of each role and don't forget to make the final list distinct before sending it on.
            // Distinct should work because they are all referencing the exact same permission object from the preloaded permissions.
        }
        
        throw new NotImplementedException();
    }
}