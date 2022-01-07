namespace DatabaseUpdate
{
    internal record AccessPointConfiguration
    {
        public string AccessPointId { get; init; }

        public EnergyDataAccess.EnergyEnum Energy { get; init; }
    }
}
