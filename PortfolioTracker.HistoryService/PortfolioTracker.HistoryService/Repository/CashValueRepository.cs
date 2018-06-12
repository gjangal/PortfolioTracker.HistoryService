using Dapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ICashValueRepository : IRepository<ICashValue>
    {

    }
    public class CashValueRepository : ICashValueRepository
    {
        private readonly ILogger logger;
        private readonly string connectionString;
        public CashValueRepository(
            ILogger logger)
        {
            this.logger = logger;
            this.connectionString = ConfigurationManager.AppSettings["HistoryDb"].ToString();
        }

        public async Task<bool> DeleteAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE from dbo.CashValue where Id={Id}";

                var rows = await connection.ExecuteAsync(sql);

                if(rows > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<IEnumerable<ICashValue>> GetListAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, PortfolioId, Cash, Date from dbo.CashValue";

                return await connection.QueryAsync<ICashValue>(sql);
            }
        }

        public async Task<ICashValue> GetSingleAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT Id, PortfolioId, Cash, Date from dbo.CashValue where Id={Id}";

                return (await connection.QueryAsync<ICashValue>(sql)).FirstOrDefault();
            }
        }

        public async Task<bool> InsertAsync(ICashValue lot)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if exists else update
                var selectSql = $@"SELECT count(*) from [dbo].[CashValue] WHERE PortfolioId={lot.PortfolioId} and Date='{lot.Date}'";

                int rows = (int)(await connection.ExecuteScalarAsync(selectSql));

                if (rows > 0)
                {
                    return await Update(lot, connection);
                }

                var sql = $"INSERT [dbo].[CashValue] ([PortfolioId],[Cash], [Date]) VALUES (@PortfolioId, @Cash, @Date )";
                rows = await connection.ExecuteAsync(sql, new { lot.PortfolioId, lot.Cash, lot.Date });

                if (rows > 0)
                {
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> UpdateAysnc(ICashValue lot)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await Update(lot, connection);
            }
        }

        private static async Task<bool> Update(ICashValue lot, SqlConnection connection)
        {
            var sql = $@"UPDATE [dbo].[PortfolioValue] 
                                SET [Value]={lot.Cash} 
                            WHERE PortfolioId={lot.PortfolioId} and AsOf='{lot.Date}'";
            int rows = await connection.ExecuteAsync(sql, new { lot.PortfolioId, lot.Cash, lot.Date });

            if (rows > 0)
            {
                return true;
            }

            return false;
        }
    }
}
