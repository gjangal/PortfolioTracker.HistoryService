using PortfolioTracker.HistoryService.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.MarketData
{
    public class MarketValueCalulator : IMarketValueCalculator
    {
        private readonly ILogger logger;
        private readonly IMarketDataClient marketDataClient;
        private readonly ICashApiClient cashApi;

        public MarketValueCalulator(ILogger logger,
            IMarketDataClient marketDataClient,
            ICashApiClient cashApi)
        {
            this.logger = logger;
            this.marketDataClient = marketDataClient;
            this.cashApi = cashApi;
        }

        public async Task<MarketValue> Calculate(int portfolioId, DateTime asOf)
        {
            var holdings = new List<Lot>();//portfolio.Holdings;
            float cumMktValue = 0;

            foreach (var holding in holdings)
            {
                var ticker = holding.Ric.Split('.');

                logger.Information($"Getting market price for {ticker[0]}");
                var price = await marketDataClient.GetMarketPriceAsync(ticker[0], asOf);
                cumMktValue += (float)(price) * holding.Qty; 
            }

            logger.Information($"Fetching cash information for portfolio id {portfolioId}");

            var cashValue = await cashApi.GetCashForPortfolio(portfolioId, asOf);

            logger.Information($"Finished calculating market value for portfolioId:{portfolioId}");

            return new MarketValue()
            {
                Date = asOf,
                MktValue = cumMktValue + cashValue.Amount,
                PortfolioId = portfolioId
            };
            
        }
    }
}
