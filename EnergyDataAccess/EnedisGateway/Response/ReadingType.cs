namespace EnergyDataAccess.EnedisGateway.Response
{
    /// <summary>
    /// Represents a reading type offered by the Enedis API.
    /// </summary>
    internal record ReadingType
    {
        /// <summary>
        /// Gets the unit of the reading.
        /// </summary>
        public string Unit { get; init; }

        /// <summary>
        /// Gets the kind of data measured.
        /// </summary>
        public MeasurementEnum MeasurementKind { get; init; }

        /// <summary>
        /// Gets the way the data is aggregated over the intervals.
        /// </summary>
        public AggregateEnum Aggregate { get; init; }
    }
}
