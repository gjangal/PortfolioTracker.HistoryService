using MassTransit;
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

        public void Start()
        {
            bus = Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri("rabbitmq://localhost/"), h=> { }));
            bus.Start();
        }

        public void Stop()
        {
            bus.Stop();
        }

    }
}
