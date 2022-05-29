namespace ecestockroom_api_v2.Domain
{
    public partial class User
    {
        public Guid UserId { get; set; }
        public string TechId { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string PreferredName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Guid ClassificationId { get; set; }
        public Guid MajorId { get; set; }
        public Guid[] RoleIds { get; set; } = null!;
        public Guid[] PermissionIds { get; set; } = null!;
        public Guid StatusId { get; set; }

        public virtual Classification Classification { get; set; } = null!;
        public virtual Major Major { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
    }
}
