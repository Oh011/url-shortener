namespace Project.Application.Features.Urls.Dtos
{
    public class ShortUrlDto
    {
        public string ShortUrl { get; set; }
        public string LongUrl { get; set; }           // the original URL
        public string? CustomAlias { get; set; }      // if the user provided one

        public DateTime CreatedAt { get; set; }       // creation timestamp
        public DateTime? ExpirationDate { get; set; } // optional
    }

}
