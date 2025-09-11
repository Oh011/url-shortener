using MediatR;

namespace Project.Application.Features.Urls.Queries.ResolveShortUrl
{
    public class ResolveShortUrlQuery : IRequest<string>
    {
        public string ShortUrl { get; }
        public string IpAddress { get; }
        public string UserAgent { get; }
        public string? Referrer { get; }


        public ResolveShortUrlQuery(
            string shortUrl,
            string ipAddress,
            string userAgent,
            string? referrer = null)
        {
            ShortUrl = shortUrl;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            Referrer = referrer;

        }
    }

}
