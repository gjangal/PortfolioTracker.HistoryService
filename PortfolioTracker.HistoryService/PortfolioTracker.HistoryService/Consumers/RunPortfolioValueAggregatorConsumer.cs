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
        private readonly ICashRepository cashRepository;

        public RunPortfolioValueAggregatorConsumer(
            IPortfolioRepository portfolioRepository, 
            IMarketValueRepository marketValueRepository, 
            ICashRepository cashRepository)
        {
            this.portfolioRepository = portfolioRepository;
            this.marketValueRepository = marketValueRepository;
            this.cashRepository = cashRepository;
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
                        var marketValue = new MarketValue()
                        {
                            PortfolioId = portfolio.Id,
                            MktValue = portfolio.MarketValue(startDate) + (await cashRepository.GetCashValue(startDate, portfolio.Id)).Amount,
                            Date = startDate
                        };

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
