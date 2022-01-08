namespace EnergyDataAccess.InfluxDB
{
    using System;
    using System.Collections.Generic;
    using global::InfluxDB.Client;
    using global::InfluxDB.Client.Writes;

    public class InfluxDbMeasureDatabase :
        IMeasureDatabase
    {
        private readonly InfluxDBClient influxDBClient;
        const string influxDbBucketVariable = "INFLUXDB_BUCKET";
        const string influxDbOrganizationVariable = "INFLUXDB_ORG";
        const string influxDbUrlVariable = "INFLUXDB_URL";
        const string influxDbToken = "INFLUXDB_TOKEN";

        private readonly string bucket;
        private readonly string organization;

        public InfluxDbMeasureDatabase()
        {
            var token = Environment.GetEnvironmentVariable(influxDbToken);

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("InfluxDB token is not defined");
            }

            var url = Environment.GetEnvironmentVariable(influxDbUrlVariable);

            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("InfluxDB URL is not defined");
            }

            this.bucket = Environment.GetEnvironmentVariable(influxDbBucketVariable);

            if (string.IsNullOrEmpty(this.bucket))
            {
                throw new ArgumentException("InfluxDB bucket is not defined");
            }

            this.organization = Environment.GetEnvironmentVariable(influxDbOrganizationVariable);

            if (string.IsNullOrEmpty(this.organization))
            {
                throw new ArgumentException("InfluxDB organization is not defined");
            }

            this.influxDBClient =
                InfluxDBClientFactory.Create(url, token);
        }

        public void SaveMeasures(IEnumerable<Measure> measures)
        {
            using (var writeApi = this.influxDBClient.GetWriteApi())
            {
                foreach(Measure measure in measures)
                {
                    var point = PointData.Measurement("energy")
                                        .Tag("kind", measure.Energy.ToString())
                                        .Tag("unit", measure.Unit)
                                        .Tag("usagePointId", measure.UsagePointId)
                                        .Tag("aggregate", measure.Aggregate.ToString())
                                        .Field("value", measure.Value)
                                        .Timestamp(measure.Timestamp, global::InfluxDB.Client.Api.Domain.WritePrecision.S);

                    writeApi.WritePoint(this.bucket, this.organization, point);
                }
            }
        }
    }
}
