using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ICashValue : IDbEntity
    {
        int PortfolioId { get; }
        float Cash { get; }
        DateTime Date { get;}
    }

    public class CashValue : ICashValue
    {
        public int PortfolioId { get; set; }
        public float Cash { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
    }
}
