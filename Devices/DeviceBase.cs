using IRIS.Addressing.Abstract;
using IRIS.Communication;

namespace IRIS.Devices
{
    /// <summary>
    /// Represents device connected to computer
    /// </summary>
    /// <typeparam name="TCommunicationInterface">Communication interface between device and computer</typeparam>
    /// <typeparam name="TAddressType">Type of device address</typeparam>
    public abstract class DeviceBase<TCommunicationInterface, TAddressType> : DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
        where TAddressType : struct, IDeviceAddress
    {
    }

    
    public abstract class DeviceBase<TCommunicationInterface>
        where TCommunicationInterface : ICommunicationInterface
    {
        /// <summary>
        /// Connect to device (waits for the task to complete)
        /// </summary>
        public bool Connect(CancellationToken cancellationToken = default)
        {
            try
            {
                // Get task to connect to device
                ValueTask<bool> connectionTask = ConnectAsync(cancellationToken);

                // Wait for the task to complete
                while (!connectionTask.IsCompleted) ;

                // Return the result of the task
                return connectionTask.Result;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Disconnect from device (waits for the task to complete)
        /// </summary>
        public bool Disconnect(CancellationToken cancellationToken = default)
        {
            try
            {
                // Get task to disconnect from device
                ValueTask<bool> disconnectionTask = DisconnectAsync(cancellationToken);

                // Wait for the task to complete
                while (!disconnectionTask.IsCompleted) ;

                // Return the result of the task
                return disconnectionTask.Result;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Connect to device
        /// </summary>
        public virtual ValueTask<bool> ConnectAsync(CancellationToken cancellationToken = default)
            => HardwareAccess.ConnectAsync(cancellationToken);

        /// <summary>
        /// Disconnect from device
        /// </summary>
        public virtual ValueTask<bool> DisconnectAsync(CancellationToken cancellationToken = default)
            => HardwareAccess.DisconnectAsync();

        /// <summary>
        /// Communication interface between device and computer
        /// <b>Beware: this is not initialized in constructor, as it is not known at this point
        /// You must have a constructor in derived class that initializes this property</b>
        /// </summary>
        // ReSharper disable once NullableWarningSuppressionIsUsed
        protected TCommunicationInterface HardwareAccess { get; init; } = default!;

        /// <summary>
        /// Used to get communication interface, should be only implemented in transactions
        /// </summary>
        public TCommunicationInterface AccessHardwareInterface() => HardwareAccess;
    }
}