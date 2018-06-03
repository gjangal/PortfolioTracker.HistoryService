using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.MarketData;
using PortfolioTracker.HistoryService.Repository;
using Serilog;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Consumers
{
    public class PortfolioValueAggregatorConsumer : IConsumer<IPortfolioValueAggregator>
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IMarketValueRepository marketValueRepository;
        private readonly ICashRepository cashRepository;
        private readonly IMarketValueCalculator mktValueCalculator;
        private readonly ILogger logger;

        public PortfolioValueAggregatorConsumer(
            IMarketValueRepository marketValueRepository, 
            IMarketValueCalculator mktValueCalculator,
            ILogger logger)
        {
            this.marketValueRepository = marketValueRepository;
            this.mktValueCalculator = mktValueCalculator;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<IPortfolioValueAggregator> context)
        {
            var startDate = context.Message.Date;

            foreach (var portfolioId in context.Message.PortfolioIds)
            {
                logger.Information($"Calculating portfolio value for {portfolioId}");

                var marketValue = await mktValueCalculator.Calculate(portfolioId, startDate);

                logger.Information($"Calculated portfolio value for {portfolioId}");

                logger.Information($"Inserting market value for {portfolioId}");

                await marketValueRepository.InsertAsync(marketValue);
                
                logger.Information($"Done inserting market value for {portfolioId} for date {startDate}");
            }
        }
    }
}
