namespace EnergyDataAccess.GRDF.Response
{
    internal class Espace
    {
        public int Id { get; set; }

        public string MobilePhone { get; set; }

        public object ValidatedMobilePhone { get; set; }

        public object MobilePhoneExpirationDate { get; set; }

        public object HomePhone { get; set; }

        public bool NotificationInfoCoupureSms { get; set; }

        public bool NotificationInfoCoupureEmail { get; set; }

        public Adresse Adresse { get; set; }
    }
}
