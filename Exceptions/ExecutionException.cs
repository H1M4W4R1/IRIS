namespace IRIS.Exceptions
{
    public class ExecutionException : Exception
    {
        public ExecutionException(string msg) : base(msg)
        { }

        public ExecutionException(string msg, Exception inner) : base(msg, inner)
        { }
    }
}
