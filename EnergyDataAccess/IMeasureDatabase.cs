using System.Collections.Generic;

namespace EnergyDataAccess
{
    public interface IMeasureDatabase
    {
        void SaveMeasures(IEnumerable<Measure> measure);
    }
}
