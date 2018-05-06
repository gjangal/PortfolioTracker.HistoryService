using System.Collections.Generic;

namespace PortfolioTracker.HistoryService.Repository
{
    public class Portfolio : IPortfolio
    {
        public string Name { get ; set; }

        public IEnumerable<Lot> Holdings { get; set; }

        public float Cash { get; set; }

        public int Id { get; set; }
    }

}
