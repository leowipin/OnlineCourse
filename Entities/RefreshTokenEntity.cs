namespace OnlineCourse.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public required string Token { get; set; }
        public required string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime CreationDate { get; set; }
        // Foreign keys
        public Guid UserId { get; set; }
        // Nav properties
        public User User { get; set; } = null!;
    }
}
