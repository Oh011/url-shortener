namespace Project.Application.Features.Urls.Dtos
{
    public class UrlDetailsDto
    {
        public string ShortUrl { get; set; } = null!;
        public string LongUrl { get; set; } = null!;
        public long TotalClicks { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }

}
