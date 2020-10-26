using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Tandem.Common.DataProxy
{
    public abstract class BaseRepository
    {
        private readonly BaseDataService _dataSvc;

        protected BaseRepository(BaseDataService dataSvc)
        {
            _dataSvc = dataSvc;
        }

        /* MISC IMPL NOTES
         * If this were a Proxy used to connect to a MongoDb server
         * This would effectively be a Collection Name
         * This is so each individual Repo impl'ing the BaseRepo class
         * can specify what Collection it is responsible for interacting with
         *
         * If this Proxy were used to connect to a SQL server
         * I'd made everything CRUD sproc driven and use Dapper
         * to invoke said sprocs making this class member
         * unnecessary
         */
        protected abstract string DataFileName { get; }

        #region SUPPORTED REPO METHODS
        protected async Task<bool> InsertAsync<TEntity>(TEntity entity)
        {
            string fileContents = await _dataSvc.GetFileContents(DataFileName);
            string entityStr = JsonSerializer.Serialize(entity);
            fileContents += entityStr;
            bool response = await _dataSvc.TryWriteFileContents(DataFileName, fileContents);
            return response;
        }

        protected async Task<List<TEntity>> GetAsync<TEntity>()
        {
            string fileContents = await _dataSvc.GetFileContents(DataFileName);
            List<TEntity> response = JsonSerializer.Deserialize<List<TEntity>>(fileContents);
            return response;
        }

        protected async Task<bool> UpdateAsync<TEntity>(TEntity entity)
        {
            string updatedFileContents = JsonSerializer.Serialize(entity);
            bool response = await _dataSvc.TryWriteFileContents(DataFileName, updatedFileContents);
            return response;
        }
        #endregion
    }
}