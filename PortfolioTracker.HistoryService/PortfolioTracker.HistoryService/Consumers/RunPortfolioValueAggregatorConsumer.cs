using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.MarketData;
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
        private readonly IMarketValueCalculator mktValueCalculator;

        public RunPortfolioValueAggregatorConsumer()
        {

        }
        public RunPortfolioValueAggregatorConsumer(
            IPortfolioRepository portfolioRepository, 
            IMarketValueRepository marketValueRepository, 
            ICashRepository cashRepository,
            IMarketValueCalculator mktValueCalculator)
        {
            this.portfolioRepository = portfolioRepository;
            this.marketValueRepository = marketValueRepository;
            this.cashRepository = cashRepository;
            this.mktValueCalculator = mktValueCalculator;
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
                        await InsertMarketValue(startDate, cashValues, portfolio);
                    }

                    return ;

                case PortfolioValueRunMode.SpecificPortfolios:
                    startDate = context.Message.Date;
                    cashValues = (await cashRepository.GetCashValueForDate(startDate)).ToLookup(c => Tuple.Create(c.PortfolioId, c.Date));

                    foreach (var portfolioId in context.Message.PortfolioIds)
                    {
                        var portfolio = await portfolioRepository.GetSingleAsync(portfolioId);

                        await InsertMarketValue(startDate, cashValues, portfolio);
                    }
                    return;

                default:
                    return;
            }
        }

        private async Task InsertMarketValue(DateTime startDate, ILookup<Tuple<int, DateTime>, Cash> cashValues, Portfolio portfolio)
        {
            var cashValue = cashValues.Contains(Tuple.Create(portfolio.Id, startDate)) ? cashValues[Tuple.Create(portfolio.Id, startDate)].FirstOrDefault() : null;

            var marketValue = await mktValueCalculator.Calculate(portfolio, cashValue, startDate);

            await marketValueRepository.InsertAsync(marketValue);
        }
    }
}
