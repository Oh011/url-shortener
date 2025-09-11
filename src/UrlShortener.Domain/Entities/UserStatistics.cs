namespace Project.Domain.Entities
{

    //created First time the user creates a short URL
    public class UserStatistics
    {



        public string UserId { get; set; } = null!;  // PK, matches AspNetUsers.Id

        public int TotalUrls { get; set; } = 0;
        public long TotalClicks { get; set; } = 0;
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow;
    }

}
