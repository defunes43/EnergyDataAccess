using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

[assembly: InternalsVisibleTo("EnergyDataAccessTests")]

namespace EnergyDataAccess.Grdf
{
    /// <summary>
    /// Represents a payload for EnedisGatewayPostQueries.
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal class GrdfLoginPayload
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrdfLoginPayload"/> class.
        /// </summary>
        /// <param name="usagePointId">ID of the usage point to be requested.</param>
        /// <param name="start">Start of the requested time-frame.</param>
        /// <param name="end">End of the requested time-frame.</param>
        public GrdfLoginPayload(string email, string password)
        {
            this.Email = email;
            this.Password = password;
            this.Capp = "meg";
            this.Goto = "https://sofa-connexion.grdf.fr:443/openam/oauth2/externeGrdf/authorize";
        }

        /// <summary>
        /// Gets the email address used for authentication.
        /// </summary>
        [JsonProperty]
        internal string Email { get; }

        /// <summary>
        /// Gets the password used for authentication.
        /// </summary>
        [JsonProperty]
        internal string Password { get; }

        [JsonProperty]
        internal string Capp { get; }

        [JsonProperty]
        internal string Goto { get; }

        internal Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>()
            {
                { "email", this.Email },
                { "password", this.Password },
                { "capp", this.Capp },
                { "goto", this.Goto },
            };
        }
    }
}
