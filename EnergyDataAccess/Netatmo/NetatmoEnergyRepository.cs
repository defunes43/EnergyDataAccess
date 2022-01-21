using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Web;

namespace EnergyDataAccess.Netatmo
{
    internal class NetatmoEnergyRepository :
        IEnergyRepository
    {
        private const string RequestScope = "read_station";
        private readonly Credentials credentials;

        public NetatmoEnergyRepository(
            string clientId,
            string clientSecret,
            string username,
            string password)
        {
            CredentialGrantRequest request = new()
            {
                Scope = RequestScope,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Username = username,
                Password = password,
            };

            this.credentials = this.GetCredentials(request);
        }

        public IEnumerable<Measure> GetAllData(string usagePointId, DateTime endDate)
        {
            var splitArray = usagePointId.Split("#");

            //TODO: limite de 1024 points par requete.

            var measureRequest = new MeasureRequest()
            {
                DeviceId = splitArray[0],
                ModuleId = splitArray.Length == 2 ? splitArray[1] : string.Empty,
                DateBegin = DateTimeOffset.FromUnixTimeSeconds(1).UtcDateTime,
                DateEnd = endDate,
            };

            return this.GetDataFromRequest(measureRequest, usagePointId);
        }

        private IEnumerable<Measure> GetDataFromRequest(MeasureRequest request, string usagePointId)
        {
            const string apiUrl = "https://api.netatmo.com/api/getmeasure";

            var requestJson = JsonConvert.SerializeObject(request);
            var requestDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(requestJson);

            var builder = new UriBuilder(apiUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var kvp in requestDict)
            {
                query.Add(kvp.Key, kvp.Value);
            }

            builder.Query = query.ToString();

            HttpRequestMessage message = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Get,
            };
            message.Headers.Add("Authorization", "Bearer " + this.credentials.AccessToken);

            using (HttpClient client = new HttpClient())
            {
                var httpQuery = client.SendAsync(message).Result;
                var rawResult = httpQuery.Content.ReadAsStringAsync().Result;
                var token = JObject.Parse(rawResult).SelectToken("body").Value<JObject>();
                var properties = token.Properties();
                var measureList =
                    properties.Select(p => this.ParseMeasureFromProperty(p, usagePointId));
                return measureList.ToList();
            }
        }

        public IEnumerable<Measure> GetHistoricalData(string usagePointId, DateTime fromDate, DateTime endDate)
        {
            var splitArray = usagePointId.Split("#");

            var measureRequest = new MeasureRequest()
            {
                DeviceId = splitArray[0],
                ModuleId = splitArray.Length == 2 ? splitArray[1] : string.Empty,
                DateBegin = fromDate,
                DateEnd = endDate,
            };

            return this.GetDataFromRequest(measureRequest, usagePointId);
        }

        private Measure ParseMeasureFromProperty(JProperty property, string usagePointId)
        {
            var timestamp =
                DateTimeOffset.FromUnixTimeSeconds(int.Parse(property.Name)).UtcDateTime;
            var value = (float)property.Value.ToArray()[0];

            return new Measure()
            {
                Timestamp = timestamp,
                Value = value,
                Aggregate = AggregateTimeEnum.HALF_HOUR,
                Kind = MeasurementEnum.TEMPERATURE,
                Unit = "°C",
                UsagePointId = usagePointId,
            };
        }

        private Credentials GetCredentials(CredentialGrantRequest credentialGrantRequest)
        {
            const string credentialsUrl = "https://api.netatmo.com/oauth2/token";
            var json = JsonConvert.SerializeObject(credentialGrantRequest);
            var requestDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            var postContent = new FormUrlEncodedContent(requestDict);

            using (HttpClient client = new HttpClient())
            {
                var authRequest = client.PostAsync(credentialsUrl, postContent);
                var authRawResult = authRequest.Result.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<Credentials>(authRawResult);
            }
        }
    }
}
