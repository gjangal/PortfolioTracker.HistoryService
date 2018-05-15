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
                cumMktValue += await marketDataClient.GetMarketPrice(holding.Ric, asOf) * holding.Qty; 
            }

            return new MarketValue()
            {
                Date = asOf,
                MktValue = cumMktValue + cash.Amount,
                PortfolioId = portfolio.Id
            };
        }
    }
}
