using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EnergyDataAccess.GRDF
{
    public class GrdfEnergyRepository
        : IEnergyRepository
    {
        private const string WelcomeUrl = "https://monespace.grdf.fr/client/particulier/accueil";
        private const string LoginUrl = "https://login.monespace.grdf.fr/sofit-account-api/api/v1/auth";
        private const string WhoAmIUrl = "https://monespace.grdf.fr/api/e-connexion/users/whoami";

        private readonly ILogger logger;
        private readonly string login;
        private readonly string password;

        public GrdfEnergyRepository(ILogger logger, string login, string password)
        {
            this.logger = logger;
            this.login = login;
            this.password = password;
        }

        /// <inheritdoc/>
        public IEnumerable<Measure> GetHistoricalData(string usagePointId, DateTime fromDate, DateTime endDate)
        {
            const string dailyDataQuery = "https://monespace.grdf.fr/api/e-conso/pce/consommation/informatives?dateDebut={0}&dateFin={1}&pceList[]={2}";
            const string grdfDateFormat = "yyyy-MM-dd";

            HttpClient client = this.SetupClient();

            this.Login(client, this.login, this.password);

            var queryResult = client.GetAsync(
                string.Format(
                    dailyDataQuery,
                    fromDate.ToString(grdfDateFormat),
                    endDate.ToString(grdfDateFormat),
                    usagePointId)).Result.Content.ReadAsStringAsync().Result;

            this.logger.LogDebug(queryResult);

            JObject grdfMeasures = JsonConvert.DeserializeObject<JObject>(queryResult, new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
            });
            var releves = grdfMeasures[usagePointId]["releves"].Children().ToList().Select(o => o.ToObject<Response.Releve>());
            return GrdfApiHelper.GetMeasures(releves, usagePointId);
        }

        /// <inheritdoc/>
        public IEnumerable<Measure> GetAllData(string usagePointId, DateTime endTime)
        {
            TimeSpan grdfMaxOffset = new TimeSpan(1000, 0, 0, 0);
            return this.GetHistoricalData(usagePointId, endTime - grdfMaxOffset, endTime);
        }

        private HttpClient SetupClient()
        {
            var httpCookieStore = new CookieContainer();
            var handler = new HttpClientHandler()
            {
                UseCookies = true,
                CookieContainer = httpCookieStore,
                AllowAutoRedirect = true,
            };
            return new HttpClient(handler);
        }

        private void Login(HttpClient client, string login, string password)
        {
            client.DefaultRequestHeaders.Add("Accept", "application/json, */*");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connexion", "keep-alive");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Mobile Safari/537.36");

            client.GetAsync(WelcomeUrl).Wait();

            var payload = new Grdf.GrdfLoginPayload(login, password);
            client.PostAsync(LoginUrl, new FormUrlEncodedContent(payload.ToDictionary())).Wait();
            client.GetAsync(WhoAmIUrl).Wait();
        }

        private IEnumerable<int> GetPce(HttpClient client)
        {
            var rawResult = client.GetAsync(WhoAmIUrl).Result.Content.ReadAsStringAsync().Result;
            var grdfObject = JsonConvert.DeserializeObject<Response.WhoAmIResponse>(rawResult);
            return new List<int> { grdfObject.Id };
        }
    }
}
