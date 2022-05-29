namespace ecestockroom_api_v2.Domain
{
    public partial class Classification
    {
        public Classification()
        {
            Users = new HashSet<User>();
        }

        public Guid ClassificationId { get; set; }
        public string Name { get; set; } = null!;
        public Guid StatusId { get; set; }

        public virtual Status Status { get; set; } = null!;
        public virtual ICollection<User> Users { get; set; }
    }
}
