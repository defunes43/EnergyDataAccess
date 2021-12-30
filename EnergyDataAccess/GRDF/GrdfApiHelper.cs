using System.Collections.Generic;
using System.Linq;

namespace EnergyDataAccess.GRDF
{
    internal static class GrdfApiHelper
    {

        internal static IEnumerable<Measure> GetMeasures(IEnumerable<Response.Releve> releves, string pceId)
        {
            return releves.Select(m => new Measure { Timestamp = m.DateDebutReleve, Value = m.EnergieConsomme, Energy = EnergyEnum.GAS, UsagePointId = pceId });
        }
    }
}
