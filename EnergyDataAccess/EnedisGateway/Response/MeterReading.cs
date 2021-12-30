namespace EnergyDataAccess.EnedisGateway.Response
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Represents a reading from an energy meter.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal record MeterReading
    {
        /// <summary>
        /// Gets the ID of the requested Usage Point.
        /// </summary>
        public string UsagePointId { get; init; }

        /// <summary>
        /// Gets the start of the requested time frame.
        /// </summary>
        public DateTime Start { get; init; }

        /// <summary>
        /// Gets the end of the requested time frame.
        /// </summary>
        public DateTime End { get; init; }

        /// <summary>
        /// Gets the quality of the reading.
        /// </summary>
        public QualityEnum Quality { get; init; }

        /// <summary>
        /// Gets a list of readings between the start and end time.
        /// </summary>
        [JsonProperty("interval_reading")]
        public IEnumerable<IntervalReading> IntervalReadings { get; init; }

        /// <summary>
        /// Gets the kind of reading.
        /// </summary>
        public ReadingType ReadingType { get; init; }
    }
}
