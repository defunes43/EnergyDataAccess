namespace EnergyDataAccess.EnedisGateway
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Polly;

    /// <summary>
    /// Represents a repository of energy data using EnedisGateway (https://enedisgateway.tech/).
    /// </summary>
    public class EnedisGatewayEnergyRepository :
        IEnergyRepository
    {
        private const string ApiAddress = "https://enedisgateway.tech/api";
        private readonly HttpClient httpClient;
        private readonly TimeZoneInfo enedisTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        private readonly string apiToken;
        private ILogger logger;

        public EnedisGatewayEnergyRepository(ILogger logger, string apiToken)
        {
            this.httpClient = new HttpClient();
            this.httpClient.Timeout = TimeSpan.FromMinutes(5);
            this.logger = logger;
            this.apiToken = apiToken;
        }

        public EnedisGatewayEnergyRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <inheritdoc/>
        public IEnumerable<Measure> GetHistoricalData(string usagePointId, DateTime fromDate, DateTime endDate)
        {
            if (fromDate > endDate)
            {
                throw new ArgumentOutOfRangeException("Begin date must be before end date");
            }

            var payload = new EnedisGatewayPayload(usagePointId, fromDate, endDate);

            return this.SendPayloadWithRetries(payload);
        }

        /// <inheritdoc/>
        public IEnumerable<Measure> GetAllData(string usagePointId, DateTime endDate)
        {
            TimeSpan enedisMaxOffset = new TimeSpan(1000, 0, 0, 0);
            var payload = new EnedisGatewayPayload(usagePointId, TypeEnum.DailyConsumption, endDate - enedisMaxOffset, endDate);

            return this.SendPayloadWithRetries(payload);
        }

        private IEnumerable<Measure> SendPayloadWithRetries(EnedisGatewayPayload payload)
        {
            HttpResponseMessage result;

            result = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .WaitAndRetry(
                3,
                retryAttempt => TimeSpan.FromSeconds(10),
                (exception, timeSpan, retryCount, context) =>
                {
                    this.logger.LogError(
                        $"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.  CorrelationId: {context.CorrelationId}");
                })
                .Execute(() =>
                {
                    HttpResponseMessage result;
                    result = this.SendQueryFromPayloadAsync(payload);
                    this.logger.LogDebug(result.Content.ReadAsStringAsync().Result);
                    this.logger.LogDebug(JsonConvert.SerializeObject(payload));
                    result.EnsureSuccessStatusCode();
                    return result;
                })
                ;

            string responseBody = result.Content.ReadAsStringAsync().Result;
            var meterReading = GatewayApiHelper.GetMeterReading(responseBody);
            return GatewayApiHelper.GetMeasures(meterReading, this.enedisTimeZoneInfo);
        }

        private HttpResponseMessage SendQueryFromPayloadAsync(EnedisGatewayPayload payload)
        {
            var query = this.BuildQuery(payload);
            return this.httpClient.SendAsync(query).Result;
        }

        private HttpRequestMessage BuildQuery(EnedisGatewayPayload payload)
        {
            HttpRequestMessage result = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(ApiAddress),
                Headers =
                {
                    { HttpRequestHeader.Authorization.ToString(), this.apiToken },
                    { HttpRequestHeader.ContentType.ToString(), "application/json" },
                },
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
            };

            return result;
        }
    }
}
