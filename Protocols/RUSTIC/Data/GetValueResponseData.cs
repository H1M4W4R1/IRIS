using IRIS.Utility;

namespace IRIS.Protocols.RUSTIC.Data
{
    public struct GetValueResponseData()
    {
        /// <summary>
        /// Name of the property to get
        /// </summary>
        public UnmanagedString128 name = new();
        
        /// <summary>
        /// Received value
        /// </summary>
        public UnmanagedString128 value = new();

    }
}