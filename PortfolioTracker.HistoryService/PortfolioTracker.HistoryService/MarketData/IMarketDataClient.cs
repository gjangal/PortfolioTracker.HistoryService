
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.MarketData
{
    public interface IMarketDataClient
    {
        Task<double> GetMarketPriceAsync(string ticker, DateTime asOf);
    }

    public class MarketDataClient : IMarketDataClient
    {
        HttpClient httpClient;
        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings{ContractResolver = new CamelCasePropertyNamesContractResolver()};

        public MarketDataClient()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://query1.finance.yahoo.com/");
        }
            
        public async Task<double> GetMarketPriceAsync(string ticker, DateTime asOf)
        {
            string apiUrl = $"v7/finance/quote?symbols={ticker}";
            var response = await httpClient.GetAsync(apiUrl);
            var json = await response.Content.ReadAsStringAsync();
            
            var quote = JsonConvert.DeserializeObject<YahooQuote>(json, jsonSerializerSettings);

            return quote.quoteResponse.result[0].regularMarketPrice;
        }
    }
}
