namespace EnergyDataAccess
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a source of energy data.
    /// </summary>
    public interface IEnergyRepository
    {
        /// <summary>
        /// Gets energy data from the provider.
        /// </summary>
        /// <param name="usagePointId">ID of the requested usage point.</param>
        /// <param name="fromDate">Beginning of the requested time frame.</param>
        /// <param name="endDate">End of the requested time frame.</param>
        /// <returns>A list of measurements from the provider.</returns>
        public IEnumerable<Measure> GetHistoricalData(string usagePointId, DateTime fromDate, DateTime endDate);

        /// <summary>
        /// Gets all possible data from the provider.
        /// </summary>
        /// <param name="usagePointId">ID of the requested usage point.</param>
        /// <param name="endDate">End of the requested time frame.</param>
        /// <returns>A list of measurements from the provider.</returns>
        public IEnumerable<Measure> GetAllData(string usagePointId, DateTime endDate);
    }
}
