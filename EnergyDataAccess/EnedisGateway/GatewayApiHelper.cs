namespace EnergyDataAccess.EnedisGateway
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using EnergyDataAccess.EnedisGateway.Response;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Represents a set of helper functions to deal with Api responses.
    /// </summary>
    internal static class GatewayApiHelper
    {
        /// <summary>
        /// Creates a <see cref="MeterReading"/> object from a JSON response.
        /// </summary>
        /// <param name="jsonString">API's JSON response.</param>
        /// <returns>Same response as a MeterReading.</returns>
        internal static MeterReading GetMeterReading(string jsonString)
        {
            const string rootNodeName = "meter_reading";
            return JObject.Parse(jsonString).SelectToken(rootNodeName).ToObject<MeterReading>();
        }

        /// <summary>
        /// Creates a set of <see cref="Measure"/>s from the APIs <see cref="MeterReading"/>.
        /// </summary>
        /// <param name="meterReading">API MeterReading.</param>
        /// <param name="timeZoneInfo">Time zone of the timestamp.</param>
        /// <returns>A set of agnostic measures.</returns>
        internal static IEnumerable<Measure> GetMeasures(MeterReading meterReading, TimeZoneInfo timeZoneInfo)
        {
            return meterReading.IntervalReadings.Select(m => new Measure { 
                Timestamp = TimeZoneInfo.ConvertTimeToUtc(m.Date, timeZoneInfo),
                Value = m.Value,
                Energy = EnergyEnum.ELECTRICITY,
                UsagePointId = meterReading.UsagePointId,
                Aggregate = GetAggregateFromInterval(
                    m.IntervalLength == IntervalLengthEnum.UNDEFINED ?
                    meterReading.ReadingType.AggregateTime :
                    m.IntervalLength),
                Unit = meterReading.ReadingType.Unit, });
        }

        private static AggregateTimeEnum GetAggregateFromInterval(IntervalLengthEnum interval)
        {
            return interval switch
            {
                IntervalLengthEnum.PT30M => AggregateTimeEnum.HALF_HOUR,
                IntervalLengthEnum.P1D => AggregateTimeEnum.DAILY,
                _ => throw new Exception("impossible"),
            };
        }
    }
}
