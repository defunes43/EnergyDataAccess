using System;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: InternalsVisibleTo("EnergyDataAccessTests")]

namespace EnergyDataAccess.EnedisGateway
{
    /// <summary>
    /// Represents a payload for EnedisGatewayPostQueries.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class EnedisGatewayPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnedisGatewayPayload"/> class.
        /// </summary>
        /// <param name="usagePointId">ID of the usage point to be requested.</param>
        /// <param name="start">Start of the requested time-frame.</param>
        /// <param name="end">End of the requested time-frame.</param>
        public EnedisGatewayPayload(string usagePointId, DateTime start, DateTime end)
        {
            this.Type = TypeEnum.ConsumptionLoadCurve;
            this.UsagePointId = usagePointId;
            this.Start = start;
            this.End = end;
        }

        /// <summary>
        /// Gets the type of data requested.
        /// </summary>
        [JsonProperty]
        internal TypeEnum Type { get; }

        /// <summary>
        /// Gets the ID of the usage point requested.
        /// </summary>
        [JsonProperty]
        internal string UsagePointId { get; }

        /// <summary>
        /// Gets the start of the requested time-frame.
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        internal DateTime Start { get; }

        /// <summary>
        /// Gets the end the requested time-frame.
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        internal DateTime End { get; }
    }
}
