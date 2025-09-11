namespace Project.Infrastructure.Identity.Entities
{
    public class RefreshToken
    {

        public int Id { get; set; }

        public string Token { get; set; } = null!;

        public string DeviceId { get; set; }  // New field to store DeviceId

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public bool IsRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;



    }
}
