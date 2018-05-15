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
        

        public async Task<bool> DeleteAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {

                connection.Open();
                var sql = $"DELETE FROM dbo.PortfolioValue WHERE Id={Id}";
                int rows =  await connection.ExecuteAsync(sql);

                if (rows > 0)
                {
                    return true;
                }

                return false;
            }
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

                // Check if exists else update
                var selectSql = $@"SELECT count(*) from [dbo].[PortfolioValue] WHERE PortfolioId={lot.PortfolioId} and AsOf='{lot.Date}'";

                int rows = (int)(await connection.ExecuteScalarAsync(selectSql));

                if(rows > 0)
                {
                    return await Update(lot, connection);
                }

                var sql = $"INSERT [dbo].[PortfolioValue] ([PortfolioId],[Value], [AsOf]) VALUES (@PortfolioId, @MktValue, @Date )";
                rows = await connection.ExecuteAsync(sql, new { lot.Id, lot.PortfolioId, lot.MktValue, lot.Date });

                if (rows > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> UpdateAysnc(MarketValue lot)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await Update(lot, connection);
            }
        }

        private static async Task<bool> Update(MarketValue lot, SqlConnection connection)
        {
            var sql = $@"UPDATE [dbo].[PortfolioValue] 
                                SET [Value]={lot.MktValue} 
                            WHERE PortfolioId={lot.PortfolioId} and AsOf='{lot.Date}'";
            int rows = await connection.ExecuteAsync(sql, new { lot.Id, lot.PortfolioId, lot.MktValue, lot.Date });

            if (rows > 0)
            {
                return true;
            }

            return false;
        }
    }
}
