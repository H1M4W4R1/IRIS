namespace IRIS.Communication.Protocols.Addressing
{
    /// <summary>
    /// Represents abstraction of device address - eg. COM Port Name
    /// </summary>
    public abstract class DeviceAddress<T> : DeviceAddress where T : notnull
    {
#pragma warning disable CS8601 // Possible null reference assignment. (T cannot be null, IntelliSense is dumb)
        protected T _value = default;
#pragma warning restore CS8601 // Possible null reference assignment.

        public T GetAddress() => _value;

        public override string ToString()
        {
            return _value?.ToString() ?? "";
        }
    }

    public abstract class DeviceAddress
    {

    }
}
