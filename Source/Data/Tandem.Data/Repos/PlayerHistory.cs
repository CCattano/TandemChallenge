using System.Collections.Generic;
using System.Linq;
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
            PlayerHistoryEntity lastHistory = (await GetAsync())?.OrderByDescending(h => h.PlayerHistoryID)?.FirstOrDefault();
            entity.PlayerHistoryID = lastHistory?.PlayerHistoryID ?? 0 + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<PlayerHistoryEntity> GetByIDAsync(int playerHistoryID)
        {
            List<PlayerHistoryEntity> historyEntities = await GetAsync();
            PlayerHistoryEntity historyEntity = historyEntities?.SingleOrDefault(h => h.PlayerHistoryID == playerHistoryID);
            return historyEntity;
        }

        public async Task<List<PlayerHistoryEntity>> GetAsync()
        {
            List<PlayerHistoryEntity> response = await GetAsync<PlayerHistoryEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerHistoryEntity entity)
        {
            List<PlayerHistoryEntity> histories = await GetAsync();
            PlayerHistoryEntity history = histories.SingleOrDefault(h => h.PlayerHistoryID == entity.PlayerHistoryID);

            //PlayerHistoryEntity fields that support manipulation
            history.CompletedDateTime = entity.CompletedDateTime;
            history.LastModifiedBy = entity.LastModifiedBy;
            history.LastModifiedDateTime = entity.LastModifiedDateTime;

            bool response = await base.UpdateAsync(histories);
            return response;
        }
    }
}