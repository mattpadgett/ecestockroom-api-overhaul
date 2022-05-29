namespace ecestockroom_api_v2.Domain
{
    public partial class TokenFamily
    {
        public Guid TokenFamilyId { get; set; }
        public Guid UserId { get; set; }
        public string AuthorizationToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public bool ValidFlag { get; set; }
        public string CreationReason { get; set; } = null!;
        public DateTime CreationUtc { get; set; }
    }
}
