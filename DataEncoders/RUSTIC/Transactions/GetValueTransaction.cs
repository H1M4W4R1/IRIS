﻿using IRIS.Communication;
using IRIS.DataEncoders.RUSTIC.Abstract;
using IRIS.DataEncoders.RUSTIC.Data;
using IRIS.Devices;
using IRIS.Transactions;

namespace IRIS.DataEncoders.RUSTIC.Transactions
{
    /// <summary>
    /// Represents a transaction that retrieves a value from the device.
    /// </summary>
    public struct GetValueTransaction : IRusticTransaction,
        IDataExchangeTransaction<GetValueTransaction, GetValueRequestData, GetValueResponseData>
    {
        /// <summary>
        /// Expected byte that indicates the end of the response.
        /// </summary>
        public byte ExpectedByte => 0x0A;

        /// <summary>
        /// Perform the exchange with the device.
        /// </summary>
        public async Task<GetValueResponseData> _ExchangeAsync<TDevice, TCommunicationInterface>(TDevice device,
            GetValueRequestData requestData,
            CancellationToken cancellationToken = default) where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            // Get communication interface
            TCommunicationInterface communicationInterface = device.GetCommunicationInterface();

            // Send request to acquire value
            await communicationInterface.SendDataAsync(this, requestData, cancellationToken);

            // Read response
            return await communicationInterface
                .ReceiveDataAsync<GetValueTransaction, GetValueResponseData>(this, cancellationToken);
        }
    }
}