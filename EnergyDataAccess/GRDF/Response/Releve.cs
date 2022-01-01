namespace EnergyDataAccess.GRDF.Response
{
    using System;

    internal record Releve
    {
            public DateTime DateDebutReleve { get; init; }

            public DateTime DateFinReleve { get; init; }

            public string JourneeGaziere { get; init; }

            public int IndexDebut { get; init; }

            public int IndexFin { get; init; }

            public int VolumeBrutConsomme { get; init; }

            public int EnergieConsomme { get; init; }

            public object Pcs { get; init; }

            public object VolumeConverti { get; init; }

            public object Pta { get; init; }

            public string NatureReleve { get; init; }

            public string QualificationReleve { get; init; }

            public object Status { get; init; }

            public double CoeffConversion { get; init; }

            public object FrequenceReleve { get; init; }

            public object Temperature { get; init; }
        }
    }
