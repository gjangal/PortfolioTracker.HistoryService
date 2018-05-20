using PortfolioTracker.HistoryService.Repository;
using System;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.MarketData
{
    public class MarketValueCalulator : IMarketValueCalculator
    {
        private readonly IMarketDataClient marketDataClient;

        public MarketValueCalulator(
            IMarketDataClient marketDataClient)
        {
            this.marketDataClient = marketDataClient;
        }

        public async Task<MarketValue> Calculate(IPortfolio portfolio, ICash cash, DateTime asOf)
        {
            var holdings = portfolio.Holdings;
            float cumMktValue = 0;

            foreach (var holding in holdings)
            {
                var ticker = holding.Ric.Split('.');
                var price = await marketDataClient.GetMarketPriceAsync(ticker[0], asOf);
                cumMktValue += (float)(price) * holding.Qty; 
            }

            return new MarketValue()
            {
                Date = asOf,
                MktValue = cumMktValue + (cash == null ? 0: cash.Amount),
                PortfolioId = portfolio.Id
            };
        }
    }
}
