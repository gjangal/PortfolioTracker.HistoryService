using PortfolioTracker.HistoryService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.MarketData
{
    public interface IMarketValueCalculator
    {
        Task<MarketValue> Calculate(IPortfolio portfolio, ICash cash, DateTime asOf);
    }
}
