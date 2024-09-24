﻿using IRIS.Utility;

namespace IRIS.Protocols.RUSTIC.Data
{
    /// <summary>
    /// Used to request a value from the device using the RUSTIC protocol.
    /// </summary>
    public readonly struct GetValueRequestData(string propertyName)
    {
        /// <summary>
        /// Name of the property to get
        /// </summary>
        public readonly UnmanagedString128 name = new(propertyName);
    }
}