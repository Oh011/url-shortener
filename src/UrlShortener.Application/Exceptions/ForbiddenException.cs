namespace Project.Application.Exceptions
{
    public class ForbiddenException : Exception
    {


        public ForbiddenException(string msg = "Forbidden") : base(msg) { }
    }
}
