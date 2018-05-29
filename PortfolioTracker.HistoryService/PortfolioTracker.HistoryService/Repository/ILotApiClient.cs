using Newtonsoft.Json;
using PortfolioTracker.HistoryService.Util;
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
    public interface ILotApiClient
    {
        Task<IEnumerable<ILot>> GetLotsForPortfolio(int portfolioId, DateTime asOf);
    }

    public class LotApiClient : ILotApiClient
    {
        private ILogger logger;
        HttpClient httpClient;

        public LotApiClient(ILogger logger)
        {
            this.logger = logger;
            this.httpClient = new HttpClient();
            var uri = ConfigurationManager.AppSettings["LotsApi"].ToString();
            httpClient.BaseAddress = new Uri(uri);
        }
        
        public async Task<IEnumerable<ILot>> GetLotsForPortfolio(int portfolioId, DateTime asOf)
        {
            string uri = $"api/Lots/Portfolio/{portfolioId}?asOf={asOf}";

            var response = await httpClient.GetAsync(uri);

            var json = await response.Content.ReadAsStringAsync();

            var lots = JsonHelper.DeserializeToList<Lot>(json);

            return lots;
        }
    }
}
