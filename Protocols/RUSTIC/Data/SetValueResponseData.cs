namespace IRIS.Protocols.RUSTIC.Data
{
    public readonly struct SetValueResponseData(string name, string value)
    {
        /// <summary>
        /// Name of the property to set
        /// </summary>
        public readonly string name = name;

        /// <summary>
        /// Value of property set or OK
        /// </summary>
        public readonly string value = value;
        
        /// <summary>
        /// True if the value was set successfully
        /// </summary>
        public bool IsSuccess => value == "OK";

        /// <summary>
        /// True if any error occurred while setting the value
        /// </summary>
        public bool IsError => value.StartsWith("ERR");
        
        /// <summary>
        /// Gets the error message if any error occurred while setting the value.
        /// If no error occurred, returns an empty string.
        /// </summary>
        public string ErrorMessage => IsError ? value : string.Empty;
    }
}