using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.Repository;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Consumers
{
    public class CashValueAggregatorConsumer : IConsumer<ICashValueAggregator>
    {
        private readonly ILogger logger;
        private readonly ICashApiClient cashApi;
        private readonly ICashValueRepository cashHistoryRepo;

        public CashValueAggregatorConsumer(
            ILogger logger, 
            ICashApiClient cashApi,
            ICashValueRepository cashHistoryRepo)
        {
            this.logger = logger;
            this.cashApi = cashApi;
            this.cashHistoryRepo = cashHistoryRepo;
        }

        public async Task Consume(ConsumeContext<ICashValueAggregator> context)
        {
            var startDate = context.Message.Date;

            foreach (var portfolioId in context.Message.PortfolioIds)
            {
                logger.Information($"Calculating cash value for {portfolioId}");

                var cash = await cashApi.GetCashForPortfolio(portfolioId, startDate);

                var rows = await cashHistoryRepo.InsertAsync(new CashValue() { Date = startDate, Cash = cash.Amount });
            }
            
        }
    }
}
