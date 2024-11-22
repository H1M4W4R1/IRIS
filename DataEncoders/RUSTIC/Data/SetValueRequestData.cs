﻿namespace IRIS.DataEncoders.RUSTIC.Data
{
    /// <summary>
    /// Used to set value on the device using the RUSTIC protocol.
    /// </summary>
    /// <remarks>
    /// Be sure that <see cref="value"/> supports <see cref="object.ToString"/> method
    /// that returns properly-formatted data for the device. <br/>
    /// If <see cref="value"/> is null, it will be set to "0".
    /// </remarks>
    public struct SetValueRequestData(string propertyName, object? value)
    {
        /// <summary>
        /// Name of the property to get
        /// </summary>
        public readonly string name = propertyName;

        /// <summary>
        /// Value of the property to set
        /// </summary>
        public readonly string value = value?.ToString() ?? "0";
    }
}