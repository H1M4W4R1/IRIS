﻿using IRIS.Communication.Types;

namespace IRIS.Communication
{
    /// <summary>
    /// Represents communication interface between device and computer
    /// This can be for example serial port, ethernet, etc.
    /// </summary>
    public interface ICommunicationInterface
    {
        /// <summary>
        /// Connect to physical device
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnect from physical device
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Get raw data communication interface if available
        /// </summary>
        public IRawDataCommunicationInterface? Raw => this as IRawDataCommunicationInterface;
    }
}