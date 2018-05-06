using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.Contracts.Events
{
    public interface IRunPortfolioValueAggregator
    {
        PortfolioValueRunMode Mode { get; set; }
        DateTime Date { get; set; }
        IEnumerable<int> PortfolioIds { get; set; }
    }
}
