using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.Advertisement;
using IRIS.Addressing.Bluetooth;

namespace IRIS.Communication.Bluetooth
{
    /// <summary>
    /// Base Interface for Bluetooth Low Energy communication
    /// </summary>
    public sealed class BluetoothLEInterface : ICommunicationInterface
    {
        /// <summary>
        /// Address of current device
        /// </summary>
        private ulong DeviceBluetoothAddress { get; set; }

        /// <summary>
        /// Connected device
        /// </summary>
        public BluetoothLEDevice? ConnectedDevice { get; private set; }

        /// <summary>
        /// List of all known connected device addresses
        /// used to connect to multiple devices
        /// </summary>
        private static List<ulong> ConnectedDevices { get; } = new();

        /// <summary>
        /// Service address to connect to
        /// </summary>
        public IBluetoothLEAddress DeviceAddress { get; init; }

        /// <summary>
        /// True if connected to device, false otherwise
        /// </summary>
        public bool IsConnected => ConnectedDevice != null || DeviceBluetoothAddress != 0;

        /// <summary>
        /// Device watcher for scanning for devices
        /// </summary>
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        public event DeviceConnected OnDeviceConnected = delegate { };
        public event DeviceDisconnected OnDeviceDisconnected = delegate { };
        
        public async Task<bool> Connect(CancellationToken cancellationToken = default)
        {
            // Check if device is already connected
            if (IsConnected) return true;
            
            // Start scanning for devices
            _watcher.Received += OnAdvertisementReceived;
            _watcher.Start();

            // Wait for connection
            while (!IsConnected)
            {
                if(cancellationToken.IsCancellationRequested) return false;
                await Task.Yield();
            }

            // Stop scanning for devices
            _watcher.Stop();
            _watcher.Received -= OnAdvertisementReceived;
            
            return true;
        }

        public Task<bool> Disconnect(CancellationToken cancellationToken = default)
        {
            // Check if device is connected, if not - return
            if (!IsConnected) return Task.FromResult(true);

            lock (ConnectedDevices)
            {
                // Remove device from connected devices
                ConnectedDevices.Remove(DeviceBluetoothAddress);
                DeviceBluetoothAddress = 0;

                // Disconnect from device if connected
                if (ConnectedDevice == null) return Task.FromResult(true);
                
                // Send events
                OnDeviceDisconnected(DeviceBluetoothAddress, ConnectedDevice);
                ConnectedDevice.Dispose();
                ConnectedDevice = null;
            }

            return Task.FromResult(true);
        }

        /// <summary>
        /// Create Bluetooth Low Energy interface for given device name (use regex to match wildcards)
        /// </summary>
        public BluetoothLEInterface(string deviceNameRegex)
        {
            // Create new service address
            DeviceAddress = new BluetoothLENameAddress(deviceNameRegex);

            // Create new watcher for service address
            _watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active,
                AdvertisementFilter = DeviceAddress.GetAdvertisementFilter(),
                SignalStrengthFilter = DeviceAddress.GetSignalStrengthFilter()
            };
        }

        /// <summary>
        /// Create Bluetooth Low Energy interface for given service addresses
        /// </summary>
        public BluetoothLEInterface(Guid serviceAddress)
        {
            // Create new service address
            DeviceAddress = new BluetoothLEServiceAddress(serviceAddress);

            // Create new watcher for service address
            _watcher = new BluetoothLEAdvertisementWatcher()
            {
                ScanningMode = BluetoothLEScanningMode.Active,
                AdvertisementFilter = DeviceAddress.GetAdvertisementFilter(),
                SignalStrengthFilter = DeviceAddress.GetSignalStrengthFilter()
            };
        }

        private async void OnAdvertisementReceived(
            BluetoothLEAdvertisementWatcher watcher,
            BluetoothLEAdvertisementReceivedEventArgs args)
        {
            // Check if device is already connected
            if (IsConnected) return;

            // Check if device is already connected, if so - ignore
            // we don't need to lock this as it's a read-only operation
            if (ConnectedDevices.Contains(args.BluetoothAddress)) return;

            // Connect to device
            BluetoothLEDevice device =
                await BluetoothLEDevice.FromBluetoothAddressAsync(args.BluetoothAddress);

            // If the device is not found, ignore
            if (device == null) return;

            // Check if device matches expected address
            if (!await DeviceAddress.IsDeviceValid(device)) return;

            lock (ConnectedDevices)
            {
                // Additional check just in case nothing went wrong in meanwhile
                if (IsConnected) return;

                // Add device to connected devices
                ConnectedDevices.Add(args.BluetoothAddress);
                DeviceBluetoothAddress = args.BluetoothAddress;
                ConnectedDevice = device;
                OnDeviceConnected(DeviceBluetoothAddress, device);
            }
        }
        
        /// <summary>
        /// Notify that device is unreachable
        /// </summary>
        internal async Task NotifyDeviceIsUnreachable()
        {
            // If device is unreachable, disconnect
            if (IsConnected) await Disconnect();
        }
    }
}