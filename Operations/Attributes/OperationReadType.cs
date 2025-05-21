namespace IRIS.Operations.Attributes
{
    /// <summary>
    ///     Attribute used to indicate type current method will return when
    ///     operation is successful.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
#pragma warning disable CS9113 // Parameter is unread.
    public sealed class OperationReadType(Type dataType) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
    {
        
    }
}