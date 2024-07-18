namespace IRIS.Utility
{
    /// <summary>
    /// Useful for marking structs as unmanaged and constraining them, so it would
    /// reduce issues while working with some IRIS data processing methods.
    /// </summary>
    /// <typeparam name="TSelf">Struct this interface is being implemented on</typeparam>
    public interface IUnmanaged<TSelf> where TSelf : unmanaged
    {
        
    }
}