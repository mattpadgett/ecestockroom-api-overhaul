namespace ecestockroom_api_v2.Domain
{
    public partial class Status
    {
        public Status()
        {
            Classifications = new HashSet<Classification>();
            Majors = new HashSet<Major>();
            Users = new HashSet<User>();
        }

        public Guid StatusId { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Classification> Classifications { get; set; }
        public virtual ICollection<Major> Majors { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
