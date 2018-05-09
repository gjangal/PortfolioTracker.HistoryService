using System;
using System.Collections.Generic;
using System.Linq;

namespace PortfolioTracker.HistoryService.Repository
{
    public class Portfolio : IPortfolio
    {
        public string Name { get ; set; }

        public IEnumerable<Lot> Holdings { get; set; }

        public float Cash { get; set; }

        public int Id { get; set; }

        public float MarketValue(DateTime asOf)
        {
            return Holdings.Where(l => l.Date <= asOf).Sum(l => l.Qty);
        }
    }

}
