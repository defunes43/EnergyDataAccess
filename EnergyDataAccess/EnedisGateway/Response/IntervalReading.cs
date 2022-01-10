namespace EnergyDataAccess.EnedisGateway.Response
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Represents a power reading over an interval..
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
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
        public MeasureTypeEnum MeasureType { get; init; }
    }
}
