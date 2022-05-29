namespace ecestockroom_api_v2.Domain
{
    public partial class Permission
    {
        public Guid PermissionId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Key { get; set; } = null!;
    }
}
