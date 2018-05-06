using MassTransit;
using PortfolioTracker.Contracts.Events;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Consumers
{
    public class RunPortfolioValueAggregatorConsumer : IConsumer<IRunPortfolioValueAggregator>
    {
        public Task Consume(ConsumeContext<IRunPortfolioValueAggregator> context)
        {
            switch (context.Message.Mode)
            {
                case PortfolioValueRunMode.AllPorfolios:
                    return Task.FromResult(0);
                case PortfolioValueRunMode.SpecificPortfolios:
                    return Task.FromResult(0);
                default:
                    return Task.FromResult(0);
            }
        }
    }
}
