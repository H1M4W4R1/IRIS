namespace IRIS.DataEncoders.RUSTIC.Data
{
    public readonly struct GetValueResponseData(string name, string value)
    {
        /// <summary>
        /// Name of the property to get
        /// </summary>
        public readonly string name = name;

        /// <summary>
        /// Received value
        /// </summary>
        public readonly string value = value;

    }
}