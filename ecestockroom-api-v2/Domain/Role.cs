namespace ecestockroom_api_v2.Domain
{
    public partial class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool DefaultFlag { get; set; }
        public Guid[] PermissionIds { get; set; } = null!;
    }
}
