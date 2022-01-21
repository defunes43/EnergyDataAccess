namespace EnergyDataAccess.Netatmo
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal record Credentials
    {
        /// <summary>
        /// Gets the access token for the user.
        /// </summary>
        public string AccessToken { get; init; }

        /// <summary>
        /// Gets the validity time of the token in seconds.
        /// </summary>
        public int ExpiresIn { get; init; }

        /// <summary>
        /// Gets the token used to refresh the access token.
        /// </summary>
        public string RefreshToken { get; init; }
    }
}
