using Autofac;
using PortfolioTracker.HistoryService.Repository;
using Topshelf;

namespace PortfolioTracker.HistoryService.Service
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
            var containerbuilder = new ContainerBuilder();
            containerbuilder.RegisterType<CashRepository>().As<ICashRepository>();
            containerbuilder.RegisterType<PortfolioRepository>().As<IPortfolioRepository>();
            containerbuilder.RegisterType<MarketValueRepository>().As<IMarketValueRepository>();

            HostFactory.Run((c) =>
            {
                c.Service<HistoryService>(service =>
                {
                    service.ConstructUsing(s => new HistoryService());
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
