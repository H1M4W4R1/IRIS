using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Storage.Streams;
using IRIS.Addressing.Bluetooth;

namespace IRIS.Communication.Bluetooth
{
    /// <summary>
    /// Represents bluetooth endpoint (service) we can connect to
    /// </summary>
    public sealed class BluetoothEndpoint(GattDeviceService service, GattCharacteristic characteristic)
    {
        public delegate void CommunicationFailedHandler();

        public delegate void NotificationReceivedHandler(
            GattCharacteristic sender,
            GattValueChangedEventArgs args);

        /// <summary>
        /// List of allowed service addresses
        /// </summary>
        public BluetoothLEServiceAddress ServiceAddress { get; } = new(service.Uuid);

        /// <summary>
        /// UUID of the characteristic
        /// </summary>
        public GattCharacteristic Characteristic { get; init; } = characteristic;

        /// <summary>
        /// GATT service on the device
        /// </summary>
        public GattDeviceService Service { get; } = service;

        /// <summary>
        /// Check if notifications are active
        /// </summary>
        public bool AreNotificationsActive { get; private set; }

        /// <summary>
        /// Called when notification is received (must be set beforehand)
        /// </summary>
        public NotificationReceivedHandler? NotificationReceived { get; set; } = delegate { };

        /// <summary>
        /// Called when communication fails
        /// </summary>
        public CommunicationFailedHandler? CommunicationFailed { get; set; } = delegate { };

#region READ_WRITE_FUNCTIONS

        public async Task<long?> ReadLong()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to long
            return DataReader.FromBuffer(buffer).ReadInt64();
        }

        public async Task<bool> WriteLong(long data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteInt64(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<bool> WriteByte(byte data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteByte(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<byte?> ReadByte()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to byte
            return DataReader.FromBuffer(buffer).ReadByte();
        }

        public async Task<bool> WriteUShort(ushort data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteUInt16(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<ushort?> ReadUShort()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to ushort
            return DataReader.FromBuffer(buffer).ReadUInt16();
        }

        public async Task<bool> WriteShort(short data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteInt16(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<short?> ReadShort()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to short
            return DataReader.FromBuffer(buffer).ReadInt16();
        }

        public async Task<bool> WriteUInt(uint data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteUInt32(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<uint?> ReadUInt()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to uint
            return DataReader.FromBuffer(buffer).ReadUInt32();
        }

        public async Task<bool> WriteFloat(float data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteSingle(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<float?> ReadFloat()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to float
            return DataReader.FromBuffer(buffer).ReadSingle();
        }

        public async Task<bool> WriteDouble(double data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteDouble(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<double?> ReadDouble()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to double
            return DataReader.FromBuffer(buffer).ReadDouble();
        }

        public async Task<bool> WriteChar(char data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteUInt16(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<char?> ReadChar()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to char
            return (char) DataReader.FromBuffer(buffer).ReadUInt16();
        }

        public async Task<bool> WriteUlong(ulong data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteUInt64(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<ulong?> ReadUlong()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to ulong
            return DataReader.FromBuffer(buffer).ReadUInt64();
        }

        public async Task<bool> WriteDateTime(DateTimeOffset data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteDateTime(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<DateTimeOffset?> ReadDateTime()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to datetime
            return DataReader.FromBuffer(buffer).ReadDateTime();
        }

        public async Task<bool> WriteGuid(Guid data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteGuid(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<Guid?> ReadGuid()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to guid
            return DataReader.FromBuffer(buffer).ReadGuid();
        }

        public async Task<bool> WriteTimeSpan(TimeSpan data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteTimeSpan(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<TimeSpan?> ReadTimeSpan()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to timespan
            return DataReader.FromBuffer(buffer).ReadTimeSpan();
        }

        public async Task<bool> WriteBool(bool data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteBoolean(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<bool?> ReadBool()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to bool
            return DataReader.FromBuffer(buffer).ReadBoolean();
        }

        public async Task<bool> WriteInt(int data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteInt32(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<int?> ReadInt()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to int
            return DataReader.FromBuffer(buffer).ReadInt32();
        }

        public async Task<bool> WriteString(string data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteString(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<string?> ReadString()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to string
            return DataReader.FromBuffer(buffer).ReadString(buffer.Length);
        }

        public async Task<bool> WriteByteArray(byte[] data)
        {
            // Convert to buffer
            DataWriter writer = new();
            writer.WriteBytes(data);
            IBuffer buffer = writer.DetachBuffer();

            // Write value
            return await WriteRawValue(buffer);
        }

        public async Task<byte[]?> ReadByteArray()
        {
            // Read value
            IBuffer? buffer = await ReadRawValue();
            if (buffer == null) return null;

            // Convert to byte array
            byte[] data = new byte[buffer.Length];
            DataReader.FromBuffer(buffer).ReadBytes(data);
            return data;
        }

#endregion

        /// <summary>
        /// Read value from the characteristic
        /// </summary>
        public async Task<IBuffer?> ReadRawValue()
        {
            GattReadResult result = await Characteristic.ReadValueAsync();
            if (result.Status != GattCommunicationStatus.Success)
            {
                CommunicationFailed?.Invoke();
                return null;
            }

            return result.Value;
        }

        /// <summary>
        /// Write value to the characteristic
        /// </summary>
        public async Task<bool> WriteRawValue(IBuffer buffer)
        {
            GattCommunicationStatus status = await Characteristic.WriteValueAsync(buffer);
            if (status != GattCommunicationStatus.Success)
            {
                CommunicationFailed?.Invoke();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Set notify for this characteristic
        /// </summary>
        public async Task<bool> SetNotify(bool shallNotify)
        {
            // Set notify
            GattCommunicationStatus status =
                await Characteristic.WriteClientCharacteristicConfigurationDescriptorAsync(
                    shallNotify
                        ? GattClientCharacteristicConfigurationDescriptorValue.Notify
                        : GattClientCharacteristicConfigurationDescriptorValue.None);

            // Check if status is OK
            if (status != GattCommunicationStatus.Success)
            {
                CommunicationFailed?.Invoke();
                return false;
            }

            // Set notification handler
            if (shallNotify)
                Characteristic.ValueChanged += OnNotificationReceivedHandler;
            else
                Characteristic.ValueChanged -= OnNotificationReceivedHandler;

            AreNotificationsActive = shallNotify;
            return true;
        }

        private void OnNotificationReceivedHandler(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // Notify all listeners
            NotificationReceived?.Invoke(sender, args);
        }
    }
}