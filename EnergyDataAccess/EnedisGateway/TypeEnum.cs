namespace EnergyDataAccess.EnedisGateway
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Reprensents an enumeration of the type of data possible to request from the gateway.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter), typeof(SnakeCaseNamingStrategy))]

    internal enum TypeEnum
    {
        /// <summary>
        /// Gets the load curve
        /// </summary>
        ConsumptionLoadCurve,
    }
}
