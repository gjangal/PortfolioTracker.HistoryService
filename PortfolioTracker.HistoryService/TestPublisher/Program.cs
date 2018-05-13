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
                Console.Write("Enter a message: ");
                text = Console.ReadLine();

                var message = new RunPortfolioValueAggregator();
                bus.Publish(message);
            }

            bus.Stop();
        }
    }

    public class RunPortfolioValueAggregator : IRunPortfolioValueAggregator
    {
        public PortfolioValueRunMode Mode { get =>  PortfolioValueRunMode.AllPorfolios; }
        public DateTime Date { get => DateTime.Today; }
        public IEnumerable<int> PortfolioIds { get => null; }
    }
}
