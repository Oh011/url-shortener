namespace UrlShortener.Application.Exceptions
{
    public class NotFoundException : Exception
    {


        public NotFoundException(string msg) : base(msg) { }
    }
}
