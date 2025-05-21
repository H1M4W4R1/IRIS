namespace IRIS.Utility.Awaitable
{
    /// <summary>
    ///     Represents a wait operation that waits until a collection contains a specific length.
    /// </summary>
    /// <typeparam name="TObject">The type of objects in the collection.</typeparam>
    public readonly struct WaitUntilCollectionExceeds<TObject>(IEnumerable<TObject> collection,
        int length, CancellationToken cancellationToken = default)
    {
        public CollectionLengthAwaiter<TObject> GetAwaiter() => new(collection, length);
    }
}