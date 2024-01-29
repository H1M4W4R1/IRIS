namespace IRIS.Exceptions
{
    public class CommunicationException : Exception
    {
        public CommunicationException(string msg) : base(msg)
        { }

        public CommunicationException(string msg, Exception inner) : base(msg, inner)
        { }
    }
}
