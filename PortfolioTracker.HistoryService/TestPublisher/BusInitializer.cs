using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.BusConfigurators;
using MassTransit.Log4NetIntegration.Logging;

namespace TestPublisher
{
    public class BusInitializer
    {
        public static IBusControl CreateBus(string queueName)
        {
            Log4NetLogger.Use();
            var bus = Bus.Factory.CreateUsingRabbitMq(x => x.Host(new Uri($"rabbitmq://localhost/"), h => {  }));
            return bus;
        }
    }
}
