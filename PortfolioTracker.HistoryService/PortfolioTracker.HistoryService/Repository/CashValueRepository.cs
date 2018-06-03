using System;
using System.Collections.Generic;
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
        public Task<bool> DeleteAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ICashValue>> GetListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICashValue> GetSingleAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> InsertAsync(ICashValue lot)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAysnc(ICashValue lot)
        {
            throw new NotImplementedException();
        }
    }
}
