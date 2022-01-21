namespace EnergyDataAccess
{
    using System;

    /// <summary>
    /// Reprensents a measurement of energy consumption.
    /// </summary>
    public record Measure
    {
        /// <summary>
        /// Gets the value of the measurement.
        /// </summary>
        public float Value { get; init; }

        /// <summary>
        /// Gets the unit of the measurement.
        /// </summary>
        public string? Unit { get; init; }

        /// <summary>
        /// Gets the timestamp of the measurement.
        /// </summary>
        public DateTime Timestamp { get; init; }

        /// <summary>
        /// Gets the ID of the usage point related to the measurement.
        /// </summary>
        public string UsagePointId { get; init; }

        /// <summary>
        /// Gets the energy which usage is measured.
        /// </summary>
        public MeasurementEnum Kind { get; init; }

        public AggregateTimeEnum? Aggregate { get; init; }
    }
}
