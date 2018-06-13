using MassTransit;
using MassTransit.Log4NetIntegration.Logging;
using PortfolioTracker.Contracts.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Log4NetLogger.Use();
            var bus = Bus.Factory.CreateUsingRabbitMq(x =>
              x.Host(new Uri("rabbitmq://localhost/"), h => { }));
            bus.Start();
            var text = "";

            while (text != "quit")
            {
                if(text=="cash")
                {
                    var message = new CashValueAggregator();
                    bus.Publish(message);
                }
                else
                {
                    var message = new RunPortfolioValueAggregator();
                    bus.Publish(message);
                }

                Console.Write("Enter a message: ");
                text = Console.ReadLine();
            }

            bus.Stop();
        }
    }

    public class RunPortfolioValueAggregator : IPortfolioValueAggregator
    {
        public PortfolioValueRunMode Mode { get =>  PortfolioValueRunMode.SpecificPortfolios; }
        public DateTime Date { get => DateTime.Today; }
        public IEnumerable<int> PortfolioIds { get => new[] { 1, 2 }; }
    }

    public class CashValueAggregator : ICashValueAggregator
    {
        public PortfolioValueRunMode Mode { get => PortfolioValueRunMode.Cash; }
        public DateTime Date { get => DateTime.Today; }
        public IEnumerable<int> PortfolioIds { get => new[] { 1, 2 }; }
    }
}
