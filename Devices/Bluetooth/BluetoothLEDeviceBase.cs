using System.Diagnostics;
using System.Reflection;
using Windows.Devices.Bluetooth;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using IRIS.Communication.Bluetooth;
using IRIS.Devices.Abstract;

namespace IRIS.Devices.Bluetooth
{
    /// <summary>
    /// Represents a Bluetooth LE device
    /// </summary>
    public abstract class BluetoothLEDeviceBase : DeviceBase<BluetoothLEInterface>
    {
        /// <summary>
        /// List of endpoints for the device
        /// </summary>
        private Dictionary<uint, BluetoothEndpoint?> Endpoints { get; } =
            new Dictionary<uint, BluetoothEndpoint?>();

        /// <summary>
        /// Determines if the device is connected
        /// </summary>
        public bool IsConnected { get; private set; }

        public BluetoothLEDeviceBase(string deviceNameRegex)
        {
            HardwareAccess = new BluetoothLEInterface(deviceNameRegex);
        }

        public BluetoothLEDeviceBase(Guid serviceUUID)
        {
            HardwareAccess = new BluetoothLEInterface(serviceUUID);
        }

        /// <summary>
        /// Used to attach to endpoints events
        /// </summary>
        protected virtual Task AttachEndpoints(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        /// <summary>
        /// Used to detach from endpoints events
        /// </summary>
        protected async Task DetachEndpoints(CancellationToken cancellationToken = default)
        {
            // Loop through all endpoints and detach from events
            foreach (KeyValuePair<uint, BluetoothEndpoint?> endpoint in Endpoints)
            {
                // Detach only from notification-enabled endpoints
                if (endpoint.Value is {AreNotificationsActive: true} notificationEndpoint)
                    await notificationEndpoint.SetNotify(false, false);
            }
        }

        public sealed override async Task<bool> Connect(CancellationToken cancellationToken = default)
        {
            // Check if device is already connected
            if (IsConnected) return true;

            // Try to connect to device
            // We don't need to subscribe to device connected event as
            // interface will wait for device to be connected
            if (!await base.Connect(cancellationToken)) return false;

            // Handle disconnection
            HardwareAccess.OnDeviceDisconnected += HandleCommunicationFailed;

            // Attach to endpoints
            await AttachEndpoints(cancellationToken);

            // Device shall now be connected
            IsConnected = true;
            return true;
        }

        /// <summary>
        /// Disconnect from the device
        /// </summary>
        public sealed override async Task<bool> Disconnect(CancellationToken cancellationToken = default)
        {
            return await Disconnect(false, cancellationToken);
        }

        private async Task<bool> Disconnect(
            bool failedConnection = false,
            CancellationToken cancellationToken = default)
        {
            HardwareAccess.OnDeviceDisconnected -= HandleCommunicationFailed;

            // Detach from endpoints
            await DetachEndpoints(cancellationToken);
            Endpoints.Clear();

            if (!await base.Disconnect(cancellationToken)) return false;

            // Device shall now be disconnected
            IsConnected = false;
            return true;
        }

        private async void HandleCommunicationFailed(ulong address, BluetoothLEDevice device)
        {
            await Disconnect(true);
        }
        
        /// <summary>
        /// Get an endpoint by index
        /// </summary>
        /// <param name="endpointIndex">Endpoint index</param>
        /// <returns>Endpoint if found, null otherwise</returns>
        protected BluetoothEndpoint? GetEndpoint(uint endpointIndex) =>
            Endpoints.GetValueOrDefault(endpointIndex);

        /// <summary>
        /// Attach to an endpoint
        /// </summary>
        /// <param name="endpointIndex">Endpoint index to create</param>
        /// <param name="endpointService">Service UUID</param>
        /// <param name="endpointCharacteristicIndex">Characteristic index</param>
        /// <param name="notificationHandler">Notification handler</param>
        /// <returns>True if successful, false otherwise</returns>
        protected async Task<bool> AttachEndpoint(
            uint endpointIndex,
            Guid endpointService,
            int endpointCharacteristicIndex,
            BluetoothEndpoint.NotificationReceivedHandler notificationHandler)
        {
            BluetoothEndpoint? endpoint = await FindEndpoint(endpointService, endpointCharacteristicIndex);
            return await _AttachEndpoint(endpointIndex, endpoint, notificationHandler);
        }
        
        /// <summary>
        /// Attach to an endpoint
        /// </summary>
        /// <param name="endpointIndex">Endpoint index to create</param>
        /// <param name="endpointService">Service UUID</param>
        /// <param name="endpointCharacteristic">Characteristic UUID</param>
        /// <param name="notificationHandler">Notification handler</param>
        /// <returns>True if successful, false otherwise</returns>
        protected async Task<bool> AttachEndpoint(
            uint endpointIndex,
            Guid endpointService,
            Guid endpointCharacteristic,
            BluetoothEndpoint.NotificationReceivedHandler notificationHandler)
        {
            // Get endpoint
            BluetoothEndpoint? endpoint = await FindEndpoint(endpointService, endpointCharacteristic);
            return await _AttachEndpoint(endpointIndex, endpoint, notificationHandler);
        }

        /// <summary>
        /// Internal method to attach to an endpoint
        /// </summary>
        private async Task<bool> _AttachEndpoint(
            uint endpointIndex,
            BluetoothEndpoint? endpoint,
            BluetoothEndpoint.NotificationReceivedHandler notificationHandler)
        {
            // Check if endpoint already exists
            if (!Endpoints.TryAdd(endpointIndex, endpoint))
            {
                // Update endpoint if it is not null and the current endpoint is null
                if (endpoint != null && Endpoints[endpointIndex] == null)
                    Endpoints[endpointIndex] = endpoint;
                else
                {
                    Debug.WriteLine($"Endpoint {endpointIndex} already exists on device {GetType().Name}");
                    return false;
                }
            }

            // Attach to endpoint events if endpoint is not null and supports notify
            if (endpoint == null || !await endpoint.SetNotify(true)) return false;
            endpoint.NotificationReceived += notificationHandler;
            return true;
        }

        /// <summary>
        /// Load endpoint
        /// </summary>
        /// <param name="endpointIndex">Index of the endpoint</param>
        /// <param name="endpointService">Service UUID</param>
        /// <param name="endpointCharacteristic">Characteristic UUID</param>
        protected async Task LoadEndpoint(uint endpointIndex, Guid endpointService, Guid endpointCharacteristic)
        {
            // Get endpoint
            BluetoothEndpoint? endpoint = await FindEndpoint(endpointService, endpointCharacteristic);

            // Load endpoint
            await LoadEndpoint(endpointIndex, endpoint);
        }

        /// <summary>
        /// Internal method to load an endpoint
        /// </summary>
        protected Task LoadEndpoint(uint endpointIndex, BluetoothEndpoint? endpoint)
        {
            // Check if endpoint already exists
            if (!Endpoints.TryAdd(endpointIndex, endpoint))
            {
                // Update endpoint if it is not null and the current endpoint is null
                if (endpoint != null && Endpoints[endpointIndex] == null)
                    Endpoints[endpointIndex] = endpoint;
                else
                    Debug.WriteLine($"Endpoint {endpointIndex} already exists on device {GetType().Name}");
            }
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Searches for a service on the device
        /// </summary>
        /// <param name="serviceUUID">Service UUID</param>
        /// <returns>Service if found, null otherwise</returns>
        private async Task<GattDeviceService?> FindService(Guid serviceUUID)
        {
            // Check if interface is connected
            if (!HardwareAccess.IsConnected) return null;

            // Check if interface has valid device
            if (HardwareAccess.ConnectedDevice == null) return null;

            // Get all services for device
            GattDeviceServicesResult services = await HardwareAccess.ConnectedDevice.GetGattServicesAsync();

            // Check if services are valid
            if (services.Status != GattCommunicationStatus.Success)
            {
                await HardwareAccess.NotifyDeviceIsUnreachable();
                return null;
            }

            // Get the service with the correct UUID
            GattDeviceService? service = services.Services.FirstOrDefault(s => s.Uuid == serviceUUID);

            // Check if service is valid
            return service == null ? null : service;
        }

        /// <summary>
        /// Find the endpoint on a Bluetooth device
        /// </summary>
        /// <param name="serviceUUID">Service UUID</param>
        /// <param name="endpointIndex">Characteristic index</param>
        /// <returns>Endpoint if found, null otherwise</returns>
        private async Task<BluetoothEndpoint?> FindEndpoint(
            Guid serviceUUID,
            int endpointIndex)
        {
            // Get service
            GattDeviceService? service = await FindService(serviceUUID);

            // Check if service is valid
            if (service == null) return null;

            // Get all characteristics for service
            GattCharacteristicsResult characteristics = await service.GetCharacteristicsAsync();

            // Check if characteristics are valid
            if (characteristics.Status != GattCommunicationStatus.Success)
            {
                await HardwareAccess.NotifyDeviceIsUnreachable();
                return null;
            }

            // Get the characteristic with the correct index
            GattCharacteristic? characteristic = characteristics.Characteristics.ElementAtOrDefault(endpointIndex);

            // Check if characteristic is valid
            return characteristic == null ? null : new BluetoothEndpoint(HardwareAccess, service, characteristic);
        }

        /// <summary>
        /// Get the endpoint on a Bluetooth device
        /// </summary>
        /// <param name="serviceUUID">Service UUID</param>
        /// <param name="characteristicUUID">Characteristic UUID</param>
        /// <returns>Endpoint if found, null otherwise</returns>
        private async Task<BluetoothEndpoint?> FindEndpoint(
            Guid serviceUUID,
            Guid characteristicUUID)
        {
            // Get service
            GattDeviceService? service = await FindService(serviceUUID);

            // Check if service is valid
            if (service == null) return null;

            // Get all characteristics for service
            GattCharacteristicsResult characteristics = await service.GetCharacteristicsAsync();

            // Check if characteristics are valid
            if (characteristics.Status != GattCommunicationStatus.Success)
            {
                await HardwareAccess.NotifyDeviceIsUnreachable();
                return null;
            }

            // Get the characteristic with the correct UUID
            GattCharacteristic? characteristic =
                characteristics.Characteristics.FirstOrDefault(c => c.Uuid == characteristicUUID);

            // Check if characteristic is valid
            return characteristic == null
                ? null
                : new BluetoothEndpoint(HardwareAccess, service, characteristic);
        }
    }
}