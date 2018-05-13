using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace PortfolioTracker.HistoryService.Repository
{
    public interface IPortfolioRepository : IRepository<Portfolio> { }

    public class PortfolioRepository : IPortfolioRepository
    {
        private string connectionString;

        public PortfolioRepository()
        {
            connectionString = ConfigurationManager.AppSettings["HistoryDb"].ToString();
        }

        public bool Delete(int Id)
        {
            
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }


        public async Task<IEnumerable<Portfolio>> GetListAsync()
        {
            using(var connection = new SqlConnection(connectionString)){

                connection.Open();

                var lookup = new Dictionary<int, Portfolio>();

                var sql = $"SELECT p.* , l.* from Portfolio p INNER JOIN Lot l on l.PortfolioId = p.Id";

                await connection.QueryAsync<Portfolio, Lot, Portfolio>(sql, (p, l) =>
                {
                    Portfolio portfolio;
                    if (!lookup.TryGetValue(p.Id, out portfolio))
                    {
                        lookup.Add(p.Id, portfolio = p);
                    }

                    if (portfolio.Holdings is null)
                        portfolio.Holdings = new List<Lot>();

                    ((IList<Lot>)portfolio.Holdings).Add(l);
                    return portfolio;
                });
                

                return lookup.Values;
                
            }
        }

        public async Task<Portfolio> GetSingleAsync(int Id)
        {
            var portfolios = await GetListAsync();
            return portfolios.Where(p => p.Id == Id).FirstOrDefault();
        }
        
        public bool Insert(Portfolio lot)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAsync(Portfolio lot)
        {
            throw new NotImplementedException();
        }

        public bool Update(Portfolio lot)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAysnc(Portfolio lot)
        {
            throw new NotImplementedException();
        }
    }
}
