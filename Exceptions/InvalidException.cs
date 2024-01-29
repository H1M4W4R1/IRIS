namespace IRIS.Exceptions
{
    public class InvalidException : Exception
    {
        public InvalidException(string msg) : base(msg)
        { }

        public InvalidException(string msg, Exception inner) : base(msg, inner)
        { }
    }
}
