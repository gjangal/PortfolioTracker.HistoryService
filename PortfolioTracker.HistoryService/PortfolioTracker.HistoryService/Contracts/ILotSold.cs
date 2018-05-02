using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Contracts
{
    interface ILotSold
    {
        int PortfolioId { get; set; }
        int LotId { get; set; }
        DateTime Date { get; set; }
    }
}
