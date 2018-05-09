using PortfolioTracker.HistoryService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ICash:IDbEntity
    {
        DateTime Date   { get; }
        int PortfolioId { get; }
        float Amount    { get; }
    }

    public class Cash : ICash
    {
        public DateTime Date { get; set; }

        public int PortfolioId { get; set; }

        public float Amount { get; set; }

        public int Id { get; set; }
    }
}
