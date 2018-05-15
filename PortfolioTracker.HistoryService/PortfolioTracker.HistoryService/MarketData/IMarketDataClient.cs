
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.MarketData
{
    public interface IMarketDataClient
    {
        Task<float> GetMarketPrice(string ticker, DateTime asOf);
    }
}
