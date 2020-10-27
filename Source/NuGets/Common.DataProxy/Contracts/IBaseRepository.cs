using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tandem.Common.DataProxy.Contracts
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<bool> InsertAsync(TEntity entity);
        Task<List<TEntity>> GetAsync();

        /* NOTES ON INTERFACE/IMPL SIGNATURE DIFFERENCE
         *
         * This interface signature does not match its concrete impl by design
         *
         * This interface is used on the upstream Repo, and forces it to support
         * a method of, "UpdateAsync" that takes 1 entity.
         *
         * This is so adapters can pass a single entity to UpdateAsync
         * and not be the part responsible for implementing unique/specific
         * update behaviour.
         *
         * Adapters should just know when it passes a entity to UpdateAsync
         * that the data is passed gets applied to the database.
         *
         * It is the repos responsibility to deal with the nitty gritty of
         * what that process looks like.
         *
         * The concrete impl of BaseRepository exposes a "UpdateAsync"
         * method that the Child-Repo must use to update the Db
         * and that BaseRepo UpdateAsync requires a List<TEntity>
         * so that it can, to the best of its ability, force the Child-Repo
         * to adhere to a specific convention/procedure for performing updates
         */
        Task<bool> UpdateAsync(TEntity entity);
    }
}