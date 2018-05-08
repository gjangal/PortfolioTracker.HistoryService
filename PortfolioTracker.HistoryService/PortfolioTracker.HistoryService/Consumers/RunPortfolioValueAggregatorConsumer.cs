using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Consumers
{
    public class RunPortfolioValueAggregatorConsumer : IConsumer<IRunPortfolioValueAggregator>
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IMarketValueRepository marketValueRepository;

        public RunPortfolioValueAggregatorConsumer(
            IPortfolioRepository portfolioRepository, 
            IMarketValueRepository marketValueRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.marketValueRepository = marketValueRepository;
        }

        public async Task Consume(ConsumeContext<IRunPortfolioValueAggregator> context)
        {
            switch (context.Message.Mode)
            {
                case PortfolioValueRunMode.AllPorfolios:

                    var startDate = context.Message.Date;
                    var portfolios = await portfolioRepository.GetListAsync();
                    foreach (var portfolio in portfolios)
                    {
                        // Calculate the market value of each portfolio
                        var marketValue = new MarketValue() { PortfolioId = portfolio.Id, MktValue = (portfolio.Cash + portfolio.Holdings.Sum(h => h.Qty)) };
                        await marketValueRepository.InsertAsync(marketValue);
                    }

                    return ;
                case PortfolioValueRunMode.SpecificPortfolios:
                    return ;
                default:
                    return;
            }
        }
    }
}
