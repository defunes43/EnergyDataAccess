namespace EnergyDataAccess.Netatmo
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using System;

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class MeasureRequest
    {
        public string DeviceId { get; init; }

        public string ModuleId { get; init; }

        public string Scale { get; init; } = "30min";

        public string Type { get; init; } = "temperature";

        public string Optimize { get; init; } = "False";

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateBegin { get; init; }

        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime DateEnd { get; init; }
    }
}
