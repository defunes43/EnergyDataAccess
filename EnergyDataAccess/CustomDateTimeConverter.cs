namespace EnergyDataAccess
{
    using Newtonsoft.Json.Converters;

    internal class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            this.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
