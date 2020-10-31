using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class PlayerAnswerRepo : BaseRepository, IPlayerAnswerRepo
    {
        public PlayerAnswerRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "PlayerAnswer.json";

        public async Task<bool> InsertAsync(PlayerAnswerEntity entity)
        {
            PlayerAnswerEntity lastPlayerAnswer = (await GetAsync())?.OrderByDescending(pa => pa.PlayerAnswerID)?.FirstOrDefault();
            entity.PlayerAnswerID = (lastPlayerAnswer?.PlayerAnswerID ?? 0) + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<PlayerAnswerEntity>> GetByPlayerHistoryIDAsync(int playerHistoryID)
        {
            List<PlayerAnswerEntity> answerEntities = await GetAsync();
            List<PlayerAnswerEntity> response = answerEntities?.Where(a => a.PlayerHistoryID == playerHistoryID).ToList();
            return response;
        }

        public async Task<List<PlayerAnswerEntity>> GetAsync()
        {
            List<PlayerAnswerEntity> response = await GetAsync<PlayerAnswerEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerAnswerEntity entity)
        {
            List<PlayerAnswerEntity> playerAnswers = await GetAsync();
            bool response = await base.UpdateAsync(playerAnswers);
            return response;
        }
    }
}
