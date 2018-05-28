using Newtonsoft.Json;
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
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("portfolioId")]
        public int PortfolioId { get; set; }

        [JsonProperty("amount")]
        public float Amount { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
