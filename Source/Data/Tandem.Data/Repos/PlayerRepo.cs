using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class PlayerRepo : BaseRepository, IPlayerRepo
    {
        public PlayerRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "Player.json";

        public async Task<bool> InsertAsync(PlayerEntity entity)
        {
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<PlayerEntity>> GetAsync()
        {
            List<PlayerEntity> response = await GetAsync<PlayerEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerEntity entity)
        {
            bool response = await base.UpdateAsync(entity);
            return response;
        }
    }
}