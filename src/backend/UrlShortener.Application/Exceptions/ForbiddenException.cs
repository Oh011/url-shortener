namespace UrlShortener.Application.Exceptions
{
    public class ForbiddenException : Exception
    {


        public ForbiddenException(string msg = "Forbidden") : base(msg) { }
    }
}
