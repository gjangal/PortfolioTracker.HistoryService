using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Consumers
{
    public class RunPortfolioValueAggregatorConsumer : IConsumer<IRunPortfolioValueAggregator>
    {
        private readonly IPortfolioRepository portfolioRepository;
        private readonly IMarketValueRepository marketValueRepository;
        private readonly ICashRepository cashRepository;

        public RunPortfolioValueAggregatorConsumer()
        {

        }
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

                    var cashValues = (await cashRepository.GetCashValueForDate(startDate)).ToLookup(c=> Tuple.Create(c.PortfolioId, c.Date)); 

                    foreach (var portfolio in portfolios)
                    {
                        var cashValue = cashValues.Contains(Tuple.Create(portfolio.Id, startDate)) ? cashValues[Tuple.Create(portfolio.Id, startDate)].FirstOrDefault():null;
                        var marketValue = new MarketValue()
                        {
                            PortfolioId = portfolio.Id,
                            MktValue = portfolio.MarketValue(startDate) + (cashValue==null? 0: cashValue.Amount),
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
