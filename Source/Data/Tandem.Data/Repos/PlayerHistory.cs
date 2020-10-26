using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class PlayerHistory : BaseRepository, IPlayerHistory
    {
        public PlayerHistory(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "PlayerHistory.json";

        public async Task<bool> InsertAsync(PlayerHistoryEntity entity)
        {
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<PlayerHistoryEntity>> GetAsync()
        {
            List<PlayerHistoryEntity> response = await GetAsync<PlayerHistoryEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerHistoryEntity entity)
        {
            bool response = await base.UpdateAsync(entity);
            return response;
        }
    }
}