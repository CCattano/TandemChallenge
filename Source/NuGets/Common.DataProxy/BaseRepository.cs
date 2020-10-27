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
            List<TEntity> fileEntities = new List<TEntity>();
            if (fileContents.Length > 0)
            {
                fileEntities = JsonSerializer.Deserialize<List<TEntity>>(fileContents);
            }
            fileEntities.Add(entity);
            string newFileContents = JsonSerializer.Serialize(fileEntities);
            bool response = await _dataSvc.TryWriteFileContents(DataFileName, newFileContents);
            return response;
        }

        protected async Task<List<TEntity>> GetAsync<TEntity>()
        {
            List<TEntity> response = null;
            string fileContents = await _dataSvc.GetFileContents(DataFileName);
            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                response = JsonSerializer.Deserialize<List<TEntity>>(fileContents);
            }
            return response;
        }

        /// <summary>
        ///     Due to the nature of the data source,
        ///     ensure you perform the following steps to invoke an update
        /// </summary>
        /// <remarks>
        ///     1: Perform a full data source pull
        ///     <br />2: Isolate the entity you need to update in the list
        ///     <br />3: Update the field of the isolated entity that support modification
        ///     <br />4: Pass the full list that contains the updated entity to this method
        /// </remarks>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <returns>A <see langword="bool"/> indicating if the content passed in
        /// was successfully written to the data source</returns>
        protected async Task<bool> UpdateAsync<TEntity>(List<TEntity> entity)
        {
            string updatedFileContents = JsonSerializer.Serialize(entity);
            bool response = await _dataSvc.TryWriteFileContents(DataFileName, updatedFileContents);
            return response;
        }
        #endregion
    }
}