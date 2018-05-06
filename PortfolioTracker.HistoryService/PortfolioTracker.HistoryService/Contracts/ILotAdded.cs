using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Contracts
{
    public interface ILotAdded
    {
        int PortfolioId { get; set; }
        int LotId       { get; set; }
        DateTime Date   { get; set; }
        int Qty         { get; set; }
        decimal Amount  { get; set; }
        string Ric      { get; set; }
    }
}
