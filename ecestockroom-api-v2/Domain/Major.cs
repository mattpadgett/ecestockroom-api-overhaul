namespace ecestockroom_api_v2.Domain
{
    public partial class Major
    {
        public Major()
        {
            Users = new HashSet<User>();
        }

        public Guid MajorId { get; set; }
        public string Name { get; set; } = null!;
        public Guid StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
