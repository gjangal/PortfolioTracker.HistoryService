using System;

namespace PortfolioTracker.HistoryService.Repository
{
    public class Lot : ILot
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public string Ric { get; set; }
        public float Price { get; set; }
        public float Qty { get; set; }
        public DateTime Date { get; set; }
        public float Commission { get; set; }
        public string Side { get; set; }
    }

}
