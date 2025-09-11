namespace Project.Application.Features.Analytics.Dtos
{
    public class TopUrlDto
    {
        public string ShortUrl { get; set; } = null!;
        public string OriginalUrl { get; set; } = null!;
        public long Clicks { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
