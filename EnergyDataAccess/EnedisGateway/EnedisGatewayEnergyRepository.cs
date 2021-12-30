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
        private ILogger logger;

        public EnedisGatewayEnergyRepository(ILogger logger)
        {
            this.httpClient = new HttpClient();
            this.logger = logger;
        }

        public EnedisGatewayEnergyRepository(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <inheritdoc/>
        public IEnumerable<Measure> GetHistoricalData(string usagePointId, DateTime fromDate, DateTime endDate)
        {
            var payload = new EnedisGatewayPayload(usagePointId, fromDate, endDate);

            if (fromDate > endDate)
            {
                throw new ArgumentOutOfRangeException("Begin date must be before end date");
            }

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
                    { HttpRequestHeader.Authorization.ToString(), "s36Ib4gZGUziT9SkTjkRrxkLqP41cEKamhkf0Dw9drKK4uOSPFRTjh" },
                    { HttpRequestHeader.ContentType.ToString(), "application/json" },
                },
                Content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"),
            };

            return result;
        }
    }
}
