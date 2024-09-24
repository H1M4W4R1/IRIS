﻿using IRIS.Communication;
using IRIS.Communication.Transactions;
using IRIS.Devices;
using IRIS.Protocols.LINE.Abstract;
using IRIS.Protocols.LINE.Data;

namespace IRIS.Protocols.LINE.Transactions
{
    public struct LineWriteTransaction : IWriteTransaction<LineWriteTransaction, LineTransactionData>, ILineTransaction
    {
        public async Task<bool> _WriteAsync<TDevice, TCommunicationInterface>(TDevice device,
            LineTransactionData requestData,
            CancellationToken cancellationToken = default) where TDevice : DeviceBase<TCommunicationInterface>
            where TCommunicationInterface : ICommunicationInterface
        {
            // Get the communication interface and send data
            ICommunicationInterface communicationInterface = device.GetCommunicationInterface();
            await communicationInterface.SendDataAsync<LineProtocol, LineWriteTransaction, LineTransactionData>(requestData, cancellationToken);
            return true;
        }
    }
}