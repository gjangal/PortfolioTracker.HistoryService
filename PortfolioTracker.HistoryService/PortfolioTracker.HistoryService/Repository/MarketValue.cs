using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface IMarketValue:IDbEntity
    {
         int PortfolioId { get;}
         float MktValue { get; }
         DateTime Date { get; set; }
    }

    public class MarketValue : IMarketValue
    {
        public int PortfolioId { get;  set ; }
        public int Id          { get;  set; }
        public float MktValue  { get;  set; }
        public DateTime Date { get; set; }
    }
}
