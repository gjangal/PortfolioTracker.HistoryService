using System.Collections.Generic;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface IPortfolio : IDbEntity
    {
        string Name { get; set; }
        IEnumerable<Lot> Holdings { get; }
        float Cash { get;}
    }

}
