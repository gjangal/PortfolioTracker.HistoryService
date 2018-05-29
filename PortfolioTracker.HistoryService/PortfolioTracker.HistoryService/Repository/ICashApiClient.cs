using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ICashApiClient
    {
        Task<ICash> GetCashForPortfolio(int portfolioId, DateTime asOf);
    }

    public class CashApiClient : ICashApiClient
    {
        private readonly ILogger logger;
        private HttpClient httpClient;
        private string baseUri;
        

        public CashApiClient(ILogger logger)
        {
            this.baseUri = ConfigurationManager.AppSettings["CashApi"].ToString();
            this.logger = logger;
            this.httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUri);

        }

        public async Task<ICash> GetCashForPortfolio(int portfolioId, DateTime asOf)
        {
            string uri = $"api/Cash/Portfolio/{portfolioId}?asOf={asOf.Date}";

            logger.Information($"Query uri {uri}");
            var response = await httpClient.GetAsync(uri);
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Cash>(json);
        }
    }

}
