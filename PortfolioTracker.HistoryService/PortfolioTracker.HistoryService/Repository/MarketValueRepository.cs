using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface IMarketValueRepository : IRepository<MarketValue>    {    }

    public class MarketValueRepository : IMarketValueRepository
    {
        private readonly string connectionString;

        public MarketValueRepository()
        {
            connectionString = ConfigurationManager.AppSettings["HistoryDb"].ToString();
        }
        

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }
        
        public async Task<IEnumerable<MarketValue>> GetListAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();
                var sql = $"SELECT Id, PortfolioId, MarketValue from dbo.MarketValue";
                return await connection.QueryAsync<MarketValue>(sql);
            }
        }

        public async Task<MarketValue> GetSingleAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();

                var lookup = new Dictionary<int, Portfolio>();

                var sql = $"SELECT Id, PortfolioId, MarketValue from dbo.MarketValue where Id={Id}";
                var mktValue = await connection.QueryAsync<MarketValue>(sql);

                return mktValue.FirstOrDefault();
            }
        }

        public async Task<bool> InsertAsync(MarketValue lot)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"INSERT [dbo].[PortfolioValue] ([Id], [PortfolioId],[Value], [AsOf]) VALUES (@Id, @PortfolioId, @MktValue, @Date )";
                int rows = await connection.ExecuteAsync(sql, new { lot.Id, lot.PortfolioId, lot.MktValue, lot.Date });

                if (rows > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public Task<bool> UpdateAysnc(MarketValue lot)
        {
            throw new NotImplementedException();
        }
    }
}
