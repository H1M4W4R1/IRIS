namespace IRIS.Communication.Protocols.Addressing
{
    public class SerialPortAddress : DeviceAddress<string>
    {
        public SerialPortAddress(string address)
        {
            _value = address;
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
