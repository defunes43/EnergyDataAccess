namespace EnergyDataAccess.GRDF.Response
{
    using System;

    internal class Releve
    {
            public DateTime DateDebutReleve { get; set; }

            public DateTime DateFinReleve { get; set; }

            public string JourneeGaziere { get; set; }

            public int IndexDebut { get; set; }

            public int IndexFin { get; set; }

            public int VolumeBrutConsomme { get; set; }

            public int EnergieConsomme { get; set; }

            public object Pcs { get; set; }

            public object VolumeConverti { get; set; }

            public object Pta { get; set; }

            public string NatureReleve { get; set; }

            public string QualificationReleve { get; set; }

            public object Status { get; set; }

            public double CoeffConversion { get; set; }

            public object FrequenceReleve { get; set; }

            public object Temperature { get; set; }
        }
    }
