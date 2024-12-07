using IRIS.Addressing;

namespace IRIS.Recognition
{
    /// <summary>
    /// Represents a device watcher that watches for devices - it can be used to automatically connect to
    /// devices or to check if device is supported by the system while connecting to it. <br/>
    /// </summary>
    /// <typeparam name="THardwareAddress">Hardware address of the device, useful for USB CDC</typeparam>
    /// <typeparam name="TSoftwareAddress">Software address of the device</typeparam>
    /// <typeparam name="TSelf">Reference to the derived class type</typeparam>
    /// <remarks>
    /// This version of interface should be used when hardware and software addresses are different e.g.
    /// for USB CDC implementation where hardware address is the USB address (VID and PID) and software address
    /// is the COM port.
    /// </remarks>
    public abstract class DeviceWatcherBase<TSelf, THardwareAddress, TSoftwareAddress> : IDisposable
        where TSelf : DeviceWatcherBase<TSelf, THardwareAddress, TSoftwareAddress>
        where TSoftwareAddress : struct, IDeviceAddress
        where THardwareAddress : struct, IDeviceAddress
    {
        /// <summary>
        /// Local cancellation token source
        /// </summary>
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        /// <summary>
        /// Current cancellation token
        /// </summary>
        private CancellationToken _currentCancellationToken = CancellationToken.None;

        /// <summary>
        /// List of all software devices
        /// </summary>
        public List<TSoftwareAddress> AllSoftwareDevices { get; } = [];

        /// <summary>
        /// List of all hardware devices
        /// </summary>
        public List<THardwareAddress> AllHardwareDevices { get; } = [];

        /// <summary>
        /// Scan interval in milliseconds
        /// </summary>
        public int ScanInterval { get; set; } = 500;

        /// <summary>
        /// Event that is raised when a device is added to the system.
        /// </summary>
        public event DeviceAddedEventHandler<THardwareAddress, TSoftwareAddress> OnDeviceAdded = delegate { };

        /// <summary>
        /// Event that is raised when a device is removed from the system.
        /// </summary>
        public event DeviceRemovedEventHandler<THardwareAddress, TSoftwareAddress> OnDeviceRemoved = delegate { };

        /// <summary>
        /// Determines if the watcher is running
        /// </summary>
        public bool IsRunning { get; protected set; } = false;

        /// <summary>
        /// Converts hardware device address to software device
        /// </summary>
        public TSoftwareAddress TryToGetSoftwareAddress(THardwareAddress hardwareDevice)
        {
            // Check if device uses the same address for hardware and software
            if (typeof(TSoftwareAddress) == typeof(THardwareAddress))
                return (TSoftwareAddress) Convert.ChangeType(hardwareDevice, typeof(TSoftwareAddress));

            // Get index
            int index = AllHardwareDevices.IndexOf(hardwareDevice);

            // Check if index is valid
            if (index < 0 || index >= AllSoftwareDevices.Count)
                return default;

            // Return software device
            return AllSoftwareDevices[index];
        }

        /// <summary>
        /// Converts software device address to hardware device
        /// </summary>
        /// <returns></returns>
        public THardwareAddress TryToGetHardwareDevice(TSoftwareAddress softwareDevice)
        {
            // Check if device uses the same address for hardware and software
            if (typeof(TSoftwareAddress) == typeof(THardwareAddress))
                return (THardwareAddress) Convert.ChangeType(softwareDevice, typeof(THardwareAddress));

            // Get index
            int index = AllSoftwareDevices.IndexOf(softwareDevice);

            // Check if index is valid
            if (index < 0 || index >= AllHardwareDevices.Count)
                return default;

            // Return hardware device
            return AllHardwareDevices[index];
        }

        /// <summary>
        /// Start the device watcher.
        /// </summary>
        public void Start()
        {
            // Check if already running
            if (IsRunning) return;

            // Cancel previous token and create new one
            if (_currentCancellationToken != CancellationToken.None) _cancellationTokenSource.Cancel();
            _currentCancellationToken = _cancellationTokenSource.Token;

            // Start the watcher
            IsRunning = true;
            Task.Run(async () =>
            {
                // Run the watcher in infinite loop
                while (true)
                {
                    try
                    {
                        // Scan for device updates
                        await ScanForDeviceUpdatesAsync(_currentCancellationToken);

                        // Check if cancellation was requested
                        if (_currentCancellationToken.IsCancellationRequested) break;
                    }
                    // ReSharper disable once RedundantCatchClause
                    // This code is required to catch exceptions in release builds.
                    catch(Exception exception)
                    {
                        // Ignore exceptions, just continue as
                        // we prefer to have watcher working at all times
                        // because some rare cases may throw one-time exceptions

#if DEBUG
                        throw;
#endif
                    }
                }
            }, _currentCancellationToken);
        }

        /// <summary>
        /// Stop the device watcher.
        /// </summary>
        public void Stop()
        {
            // Check if already stopped
            if (!IsRunning) return;

            // Stop the watcher
            IsRunning = false;
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Scan and update the list of devices. Also, should raise events for devices that were added or removed.
        /// For example, if a device was added, the event <see cref="OnDeviceAdded"/> should be raised.
        /// </summary>
        protected async Task ScanForDeviceUpdatesAsync(CancellationToken cancellationToken)
        {
            // Scan for devices
            (List<THardwareAddress> hardwareDevices, List<TSoftwareAddress> softwareDevices) =
                await ScanForDevicesAsync(cancellationToken);

            // Check if devices were removed
            for (int hardwareAddressIndex = AllHardwareDevices.Count - 1;
                 hardwareAddressIndex >= 0;
                 hardwareAddressIndex--)
            {
                // Get hardware device address
                THardwareAddress hardwareDevice = AllHardwareDevices[hardwareAddressIndex];

                // If device is still connected, skip
                if (hardwareDevices.Contains(hardwareDevice)) continue;

                // Get software device
                TSoftwareAddress softwareDevice = TryToGetSoftwareAddress(hardwareDevice);

                // Remove device
                AllHardwareDevices.RemoveAt(hardwareAddressIndex);
                AllSoftwareDevices.RemoveAt(hardwareAddressIndex);

                // Raise event
                OnDeviceRemoved(hardwareDevice, softwareDevice);
            }

            // Check if devices were added
            for (int hardwareAddressIndex = 0; hardwareAddressIndex < hardwareDevices.Count; hardwareAddressIndex++)
            {
                // Get hardware device address
                THardwareAddress hardwareDevice = hardwareDevices[hardwareAddressIndex];

                // If device is already connected, skip
                if (AllHardwareDevices.Contains(hardwareDevice)) continue;

                // Get software device
                TSoftwareAddress softwareDevice = softwareDevices[hardwareAddressIndex];

                // Add device
                AllHardwareDevices.Add(hardwareDevice);
                AllSoftwareDevices.Add(softwareDevice);

                // Raise event
                OnDeviceAdded(hardwareDevice, softwareDevice);
            }

            // Wait for next scan
            await Task.Delay(ScanInterval, cancellationToken);
        }

        /// <summary>
        /// Scan for currently connected devices.
        /// </summary>
        protected abstract Task<(List<THardwareAddress>, List<TSoftwareAddress>)> ScanForDevicesAsync(
            CancellationToken cancellationToken);

        /// <summary>
        /// Dispose the device watcher.
        /// </summary>
        public void Dispose()
        {
            if(IsRunning) Stop();
            
            _cancellationTokenSource.Dispose();
        }
    }

    /// <summary>
    /// Represents a device watcher that uses the same address for hardware and software.
    /// </summary>
    /// <remarks>
    /// This version of interface should be used when hardware and software addresses are the same e.g.
    /// TCP/IP devices where the address is the IP address.
    /// </remarks>
    public abstract class
        DeviceWatcherBase<TSelf, TDeviceAddress> : DeviceWatcherBase<TSelf, TDeviceAddress, TDeviceAddress>
        where TSelf : DeviceWatcherBase<TSelf, TDeviceAddress>
        where TDeviceAddress : struct, IDeviceAddress
    {
    }
}