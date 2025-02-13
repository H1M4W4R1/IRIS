using System.Diagnostics;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using IRIS.Communication.Bluetooth;
using IRIS.Devices;
using IRIS.Devices.Bluetooth;
using IRIS.Implementations.BluetoothLE.Data;

namespace IRIS.Implementations.BluetoothLE
{
    public sealed class HeartRateBandBluetoothLEDevice() : BluetoothLEDevice(GattServiceUuids.HeartRate)
    {
        /// <summary>
        /// Handler for when a heart rate is received
        /// </summary>
        public delegate void HeartRateReceivedHandler(HeartRateReadout heartRate);

        /// <summary>
        /// Event for when a heart rate is received
        /// </summary>
        public event HeartRateReceivedHandler OnHeartRateReceived = delegate { };

        /// <summary>
        /// Endpoint to connect to heart rate sensor
        /// </summary>
        private BluetoothEndpoint? HeartRateEndpoint { get; set; }

        public override async Task Connect()
        {
            await base.Connect();
            HardwareAccess.OnDeviceDisconnected += HandleCommunicationFailed;

            // Open the endpoint
            HeartRateEndpoint = await HardwareAccess.GetEndpoint([GattServiceUuids.HeartRate], 0);

            // Set notify for the endpoint
            if (HeartRateEndpoint != null && await HeartRateEndpoint.SetNotify(true))
            {
                HeartRateEndpoint.NotificationReceived += HandleHeartRateNotification;
            }
            else
            {
                Debug.WriteLine("Heart rate endpoint is null");
                Debugger.Break();
            }
        }

        private async void HandleCommunicationFailed(ulong address, Windows.Devices.Bluetooth.BluetoothLEDevice device)
        {
            await Disconnect(true);
        }

        public override async Task Disconnect() => await Disconnect(false);

        private async Task Disconnect(bool failedConnection)
        {
            HardwareAccess.OnDeviceDisconnected -= HandleCommunicationFailed;

            
            // Close the endpoint if it is open
            if (HeartRateEndpoint is {AreNotificationsActive: true})
            {
                // Remove notification handlers
                HeartRateEndpoint.NotificationReceived -= HandleHeartRateNotification;
                
                // Set notify to false if connection has not yet failed
                if(!failedConnection) await HeartRateEndpoint.SetNotify(false);
                
                // Close the endpoint
                HeartRateEndpoint = null;
            }

            // Disconnect from the device
            await base.Disconnect();
        }

        /// <summary>
        /// Process the raw data received from the device into application usable data,
        /// </summary>
        private async void HandleHeartRateNotification(GattCharacteristic sender, GattValueChangedEventArgs args)
        {
            // If endpoint failed, return
            if (HeartRateEndpoint == null) return;
            
            // Read the data from the endpoint and return if it is null
            byte[]? data = await HeartRateEndpoint.ReadData<byte[]>();
            if(data == null) return;

            // Process the data
            HeartRateReadout heartRate = ProcessData(data);

            // Notify listeners
            OnHeartRateReceived(heartRate);
        }

        /// <summary>
        /// Process the raw data received from the device into application usable data, 
        /// according the the Bluetooth Heart Rate Profile.
        /// </summary>
        /// <param name="data">Raw data received from the heart rate monitor.</param>
        /// <returns>The heart rate measurement value.</returns>
        private HeartRateReadout ProcessData(byte[] data)
        {
            // Heart Rate profile defined flag values
            const byte HEART_RATE_VALUE_FORMAT = 0x01;
            const byte ENERGY_EXPANDED_STATUS = 0x08;

            byte currentOffset = 0;
            byte flags = data[currentOffset];
            bool isHeartRateValueSizeLong = ((flags & HEART_RATE_VALUE_FORMAT) != 0);
            bool hasEnergyExpended = ((flags & ENERGY_EXPANDED_STATUS) != 0);

            currentOffset++;

            ushort heartRateMeasurementValue;

            if (isHeartRateValueSizeLong)
            {
                heartRateMeasurementValue = (ushort) ((data[currentOffset + 1] << 8) + data[currentOffset]);
                currentOffset += 2;
            }
            else
            {
                heartRateMeasurementValue = data[currentOffset];
                currentOffset++;
            }

            ushort expendedEnergyValue = 0;

            if (hasEnergyExpended)
            {
                expendedEnergyValue = (ushort) ((data[currentOffset + 1] << 8) + data[currentOffset]);
                // currentOffset += 2;
            }

            // The Heart Rate Bluetooth profile can also contain sensor contact status information,
            // and R-Wave interval measurements, which can also be processed here. 
            // For the purpose of this sample, we don't need to interpret that data.
            return new HeartRateReadout
            {
                HeartRate = heartRateMeasurementValue,
                HasExpendedEnergy = hasEnergyExpended,
                ExpendedEnergy = expendedEnergyValue
            };
        }
    }
}