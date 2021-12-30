namespace EnergyDataAccess.GRDF.Response
{
    using System;

    internal class WhoAmIResponse
    {
            public int Id { get; set; }

            public string Type { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string SofitId { get; set; }

            public string Profile { get; set; }

            public Espace Espace { get; set; }

            public object Rev { get; set; }

            public object Status { get; set; }

            public string AcceptBeContacted { get; set; }

            public string AcceptCollectInfo { get; set; }

            public DateTime AcceptBeContactedUpdatedAt { get; set; }

            public DateTime AcceptCollectInfoUpdatedAt { get; set; }

            public DateTime LastLogin { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime UpdatedAt { get; set; }
    }
}
