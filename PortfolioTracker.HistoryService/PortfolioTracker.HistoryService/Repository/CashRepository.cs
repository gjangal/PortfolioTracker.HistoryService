using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface ICashRepository : IRepository<Cash>
    {
        Task<Cash> GetCashValue(DateTime asOf, int portfolioId);
        Task<IEnumerable<Cash>> GetCashValueForPortfolio(int portfolioId);
        Task<IEnumerable<Cash>> GetCashValueForDate(DateTime asof);
    }

    public class CashRepository : ICashRepository
    {
        private readonly string connectionString;

        public CashRepository()
        {
            connectionString = ConfigurationManager.AppSettings["HistoryDb"].ToString();
        }

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<Cash> GetCashValue(DateTime asOf, int portfolioId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"SELECT Id, PortfolioId, Amount, Date from dbo.Cash where Date='{asOf}' and PortfolioId={portfolioId}";
                var cashValues = await connection.QueryAsync<Cash>(sql);

                return cashValues.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<Cash>> GetCashValueForDate(DateTime asof)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"SELECT Id, PortfolioId, Amount, Date from dbo.Cash where Date='{asof}'";
                return await connection.QueryAsync<Cash>(sql);
            }
        }

        public async Task<IEnumerable<Cash>> GetCashValueForPortfolio(int portfolioId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"SELECT Id, PortfolioId, Cash, Date from dbo.Cash where PortfolioId={portfolioId}";
                return await connection.QueryAsync<Cash>(sql);
            }
        }

        public async Task<IEnumerable<Cash>> GetListAsync()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"SELECT Id, PortfolioId, Cash, Date from dbo.Cash";
                return await connection.QueryAsync<Cash>(sql);
            }
        }

        public async Task<Cash> GetSingleAsync(int Id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var sql = $"SELECT Id, PortfolioId, Cash, Date from dbo.Cash where Id={Id}";
                var cashValues = await connection.QueryAsync<Cash>(sql);

                return cashValues.FirstOrDefault();
            }
        }

        public Task<bool> InsertAsync(Cash lot)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAysnc(Cash lot)
        {
            throw new NotImplementedException();
        }
    }
}
