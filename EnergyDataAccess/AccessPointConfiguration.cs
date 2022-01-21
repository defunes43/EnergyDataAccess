namespace DatabaseUpdate
{
    internal record AccessPointConfiguration
    {
        public string AccessPointId { get; init; }

        public EnergyDataAccess.MeasurementEnum Energy { get; init; }
    }
}
