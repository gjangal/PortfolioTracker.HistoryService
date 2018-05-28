using Autofac;
using AutofacSerilogIntegration;
using MassTransit;
using PortfolioTracker.Contracts.Events;
using PortfolioTracker.HistoryService.Consumers;
using PortfolioTracker.HistoryService.MarketData;
using PortfolioTracker.HistoryService.Repository;
using Serilog;
using Topshelf;

namespace PortfolioTracker.HistoryService.Service
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console().CreateLogger();

            var containerbuilder = new ContainerBuilder();
            containerbuilder.RegisterType<CashRepository>().As<ICashRepository>();
            containerbuilder.RegisterType<PortfolioRepository>().As<IPortfolioRepository>();
            containerbuilder.RegisterType<MarketValueRepository>().As<IMarketValueRepository>();
            containerbuilder.RegisterType<MarketValueCalulator>().As<IMarketValueCalculator>();
            containerbuilder.RegisterType<MarketDataClient>().As<IMarketDataClient>();
            
            containerbuilder.RegisterType<RunPortfolioValueAggregatorConsumer>().AsSelf();
            containerbuilder.RegisterLogger();

            var container = containerbuilder.Build();

            HostFactory.Run((c) =>
            {
                c.Service<HistoryService>(service =>
                {
                    service.ConstructUsing(s => new HistoryService(container));
                    service.WhenStarted((s) => s.Start());
                    service.WhenStopped((s) => s.Stop());
                });

                c.RunAsLocalSystem();
                c.SetServiceName("PortfolioTrackerHistoryService");
                c.SetDisplayName("PortfolioTrackerHistoryService");
                c.SetDescription(".NET Service to keep track of daily portfolio value");
            });
        }
    }
}
