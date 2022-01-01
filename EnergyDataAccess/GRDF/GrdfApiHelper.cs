namespace EnergyDataAccess.GRDF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class GrdfApiHelper
    {
        private static readonly TimeZoneInfo GrdfTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");

        internal static IEnumerable<Measure> GetMeasures(IEnumerable<Response.Releve> releves, string pceId)
        {
            return releves.Select(m => new Measure { Timestamp = TimeZoneInfo.ConvertTimeToUtc(m.DateDebutReleve, GrdfTimeZoneInfo), Value = m.EnergieConsomme, Energy = EnergyEnum.GAS, UsagePointId = pceId });
        }
    }
}
