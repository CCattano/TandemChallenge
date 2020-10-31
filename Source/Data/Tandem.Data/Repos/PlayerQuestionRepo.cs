using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Common.DataProxy;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Data.Repos.Contracts;

namespace Tandem.Web.Apps.Trivia.Data.Repos
{
    public class PlayerQuestionRepo : BaseRepository, IPlayerQuestionRepo
    {
        public PlayerQuestionRepo(BaseDataService dataSvc) : base(dataSvc)
        {
        }

        protected override string DataFileName => "PlayerQuestion.json";

        public async Task<bool> InsertAsync(PlayerQuestionEntity entity)
        {
            PlayerQuestionEntity lastQuestion = (await GetAsync())?.OrderByDescending(question => question.PlayerQuestionID)?.FirstOrDefault();
            entity.PlayerQuestionID = (lastQuestion?.PlayerQuestionID ?? 0) + 1;
            bool response = await base.InsertAsync(entity);
            return response;
        }

        public async Task<List<PlayerQuestionEntity>> GetByPlayerHistoryIDAsync(int playerHistoryID)
        {
            List<PlayerQuestionEntity> allEntities = await GetAsync();
            List<PlayerQuestionEntity> playerEntities =
                allEntities?.Where(pqe => pqe.PlayerHistoryID == playerHistoryID).ToList();
            return playerEntities;
        }

        public async Task<List<PlayerQuestionEntity>> GetAsync()
        {
            List<PlayerQuestionEntity> response = await GetAsync<PlayerQuestionEntity>();
            return response;
        }

        public async Task<bool> UpdateAsync(PlayerQuestionEntity entity)
        {
            List<PlayerQuestionEntity> playerQuestions = await GetAsync();
            bool response = await base.UpdateAsync(playerQuestions);
            return response;
        }
    }
}
