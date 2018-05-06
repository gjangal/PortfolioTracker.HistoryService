using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Contracts
{
    public interface ICashWithDrawn
    {
        int PortfolioId { get; set; }
        decimal Amount { get; set; }
    }
}
