using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.Contracts.Events
{
    public interface IPortfolioValueAggregator
    {
        PortfolioValueRunMode Mode { get; }
        DateTime Date { get; }
        IEnumerable<int> PortfolioIds { get; }
    }

    public interface ICashValueAggregator
    {
        PortfolioValueRunMode Mode { get; }
        DateTime Date { get; }
        IEnumerable<int> PortfolioIds { get; }
    }
}
