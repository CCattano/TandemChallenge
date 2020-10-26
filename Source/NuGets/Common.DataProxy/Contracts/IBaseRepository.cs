using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tandem.Common.DataProxy.Contracts
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> InsertAsync(TEntity entity);
        Task<List<TEntity>> GetAsync();
        Task<bool> UpdateAsync(TEntity entity);
    }
}