using IRIS.Utility;

namespace IRIS.Protocols.RUSTIC.Data
{
    public struct SetValueResponseData()
    {
        /// <summary>
        /// Name of the property to set
        /// </summary>
        public UnmanagedString128 name = new();
        
        /// <summary>
        /// Value of property set or OK
        /// </summary>
        public UnmanagedString128 value = new();
        
        /// <summary>
        /// True if the value was set successfully
        /// </summary>
        public bool IsSuccess => value.ToString() == "OK";

        /// <summary>
        /// True if any error occurred while setting the value
        /// </summary>
        public bool IsError => value.ToString().StartsWith("ERR");
        
        /// <summary>
        /// Gets the error message if any error occurred while setting the value.
        /// If no error occurred, returns an empty string.
        /// </summary>
        public string ErrorMessage => IsError ? value.ToString() : string.Empty;
    }
}