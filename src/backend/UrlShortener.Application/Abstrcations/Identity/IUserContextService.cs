namespace UrlShortener.Application.Abstrcations.Identity
{
    public interface IUserContextService
    {
        string? GetCurrentUserId();
    }

}
