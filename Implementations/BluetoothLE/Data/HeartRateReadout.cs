namespace IRIS.Implementations.BluetoothLE.Data
{
    public struct HeartRateReadout()
    {
        /// <summary>
        /// Current pulse in BPM.
        /// </summary>
        public ushort HeartRate { get; set; } = 0;
        
        /// <summary>
        /// Indicates if the device has expended energy.
        /// </summary>
        public bool HasExpendedEnergy { get; set; } = false;
        
        /// <summary>
        /// Amount of energy expended in kilo-calories.
        /// </summary>
        public ushort ExpendedEnergy { get; set; } = 0;
        
        /// <summary>
        /// Timestamp of the readout.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Returns the heart rate as a string.
        /// </summary>
        public override string ToString() => HeartRate.ToString();
    }
}