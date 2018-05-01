using Topshelf;

namespace PortfolioTracker.HistoryService.Service
{
    internal static class ConfigureService
    {
        internal static void Configure()
        {
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
                c.SetDescription(".NET Service to keep tracof daily portfolio value");
            });
        }
    }
}
