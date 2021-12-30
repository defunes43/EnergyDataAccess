namespace EnergyDataAccess.EnedisGateway.Response
{
    using System;

    /// <summary>
    /// Represents a power reading over an interval..
    /// </summary>
    internal record IntervalReading
    {
        /// <summary>
        /// Gets the value read.
        /// </summary>
        public float Value { get; init; }

        /// <summary>
        /// Gets the date of the reading.
        /// </summary>
        public DateTime Date { get; init; }

        /// <summary>
        /// Gets the interval over which the reading has been done.
        /// </summary>
        public IntervalLengthEnum IntervalLength { get; init; }

        /// <summary>
        /// Gets the type of measure returned.
        /// </summary>
        public QualityEnum MeasureType { get; init; }
    }
}
