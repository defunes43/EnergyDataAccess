namespace EnergyDataAccess.Netatmo
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal record CredentialGrantRequest
    {
        /// <summary>
        /// Gets the application client ID.
        /// </summary>
        public string ClientId { get; init; }

        /// <summary>
        /// Gets the application client secret.
        /// </summary>
        public string ClientSecret { get; init; }

        /// <summary>
        /// Gets the grant type.
        /// </summary>
        public string GrantType { get; init; } = "password";

        /// <summary>
        /// Gets the username of the user requesting access.
        /// </summary>
        public string Username { get; init; }

        /// <summary>
        /// Gets the password of the user requesting access.
        /// </summary>
        public string Password { get; init; }

        /// <summary>
        /// Gets the scope (space separated) requested by the credential.
        /// </summary>
        public string Scope { get; init; }
    }
}
