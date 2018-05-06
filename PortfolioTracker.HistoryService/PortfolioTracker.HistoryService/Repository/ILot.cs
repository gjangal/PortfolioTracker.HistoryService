using System;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ILot : IDbEntity
    {
        int PortfolioId { get; set; }
        string Ric { get; set; }
        float Price { get; set; }
        float Qty { get; set; }
        DateTime Date { get; set; }
        float Commission { get; set; }
        string Side { get; set; }
    }

}
