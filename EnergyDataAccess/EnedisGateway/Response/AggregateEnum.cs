namespace EnergyDataAccess.EnedisGateway.Response
{
    /// <summary>
    /// Enumerates the different types of aggregates returned by Enedis API.
    /// </summary>
    internal enum AggregateEnum
    {
        /// <summary>
        /// Averaged data.
        /// </summary>
        AVERAGE,

        /// <summary>
        /// Sum over a period.
        /// </summary>
        SUM,
    }
}
