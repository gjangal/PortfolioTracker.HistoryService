using Autofac;
using MassTransit;
using PortfolioTracker.HistoryService.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Service
{
    public class HistoryService
    {
        private IBusControl bus;
        private IContainer container;

        public HistoryService(IContainer container)
        {
            this.container = container;
        }

        public void Start()
        {
            bus = Bus.Factory.CreateUsingRabbitMq(x => {
                var host = x.Host(new Uri("rabbitmq://localhost"), h => { });
                x.ReceiveEndpoint(host, "portfolio_mv_queue", e => e.Consumer(typeof(PortfolioValueAggregatorConsumer), type=>container.Resolve<PortfolioValueAggregatorConsumer>() ));
                });
            bus.Start();
        }

        public void Stop()
        {
            bus.Stop();
        }

    }
}
