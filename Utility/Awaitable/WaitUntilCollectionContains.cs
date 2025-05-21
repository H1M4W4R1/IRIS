namespace IRIS.Utility.Awaitable
{
    /// <summary>
    ///     Represents a wait operation that waits until a collection contains a specified object.
    /// </summary>
    /// <typeparam name="TObject">The type of objects in the collection.</typeparam>
    public readonly struct WaitUntilCollectionContains<TObject>(IEnumerable<TObject> collection,
        TObject objectToFind,
        CancellationToken cancellationToken = default
    )
    {
        public CollectionContainsAwaiter<TObject> GetAwaiter() => new(collection, objectToFind, cancellationToken);
    }
}