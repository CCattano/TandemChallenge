using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Contracts;
using Tandem.Web.Apps.Trivia.Infrastructure;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class PlayerFacade : BaseFacade, IPlayerFacade
    {
        public PlayerFacade(ITriviaDataService dataSvc, IMapper mapper) : base(dataSvc, mapper)
        {
        }

        #region PLAYER METHODS
        public async Task<PlayerBE> GetPlayerByNameHash(string nameHash)
        {
            PlayerEntity playerEntity = await base.DataSvc.PlayerRepo.GetByNameHashAsync(nameHash);
            PlayerBE playerBE = playerEntity != null ? base.Mapper.Map<PlayerBE>(playerEntity) : null;
            return playerBE;
        }

        public async Task<PlayerBE> GetPlayerByPlayerID(int playerID)
        {
            PlayerEntity playerEntity = await base.DataSvc.PlayerRepo.GetByPlayerIDAsync(playerID);
            PlayerBE playerBE = playerEntity != null ? base.Mapper.Map<PlayerBE>(playerEntity) : null;
            return playerBE;
        }

        public async Task InsertNewPlayer(PlayerBE newPlayer)
        {
            PlayerEntity playerEntity = base.Mapper.Map<PlayerEntity>(newPlayer);
            playerEntity.CreatedBy = playerEntity.LastModifiedBy = SystemConstants.DefaultUser;
            playerEntity.CreatedDateTime = playerEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerRepo.InsertAsync(playerEntity);

            newPlayer.PlayerID = playerEntity.PlayerID;
        }

        public async Task<PlayerBE> UpdatePlayer(PlayerBE playerBE)
        {
            PlayerEntity playerEntity = base.Mapper.Map<PlayerEntity>(playerBE);
            playerEntity.LastModifiedBy = SystemConstants.DefaultUser;
            playerEntity.LastModifiedDateTime = DateTime.UtcNow;

            bool updatedSuccessfully = await base.DataSvc.PlayerRepo.UpdateAsync(playerEntity);

            if (!updatedSuccessfully) return null;

            PlayerBE updatedPlayerBE = base.Mapper.Map<PlayerBE>(playerEntity);
            return updatedPlayerBE;
        }
        #endregion

        #region PLAYER HISTORY METHODS
        public async Task<int> GetRoundNumberByPlayerID(int playerID)
        {
            List<PlayerHistoryEntity> historyEntities = await base.DataSvc.PlayerHistoryRepo.GetAsync();
            int? lastRound = historyEntities?.OrderByDescending(history => history.RoundNumber)?.FirstOrDefault()?.RoundNumber;
            return lastRound != null ? lastRound.Value + 1 : 1;
        }

        public async Task InsertNewHistory(PlayerHistoryBE historyBE)
        {
            PlayerHistoryEntity historyEntity = base.Mapper.Map<PlayerHistoryEntity>(historyBE);
            historyEntity.CreatedBy = historyEntity.LastModifiedBy = SystemConstants.DefaultUser;
            historyEntity.CreatedDateTime = historyEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerHistoryRepo.InsertAsync(historyEntity);

            historyBE.PlayerHistoryID = historyEntity.PlayerHistoryID;
        }

        public async Task<PlayerHistoryBE> GetPlayerHistory(int playerHistoryID)
        {
            PlayerHistoryEntity historyEntity = await base.DataSvc.PlayerHistoryRepo.GetByIDAsync(playerHistoryID);
            PlayerHistoryBE historyBE = historyEntity != null
                ? base.Mapper.Map<PlayerHistoryBE>(historyEntity)
                : null;
            return historyBE;
        }

        public async Task<List<PlayerHistoryBE>>GetAllPlayerHistory(int playerID)
        {
            List<PlayerHistoryEntity> playerHistories =
                await base.DataSvc.PlayerHistoryRepo.GetAllByPlayerIDAsync(playerID);
            List<PlayerHistoryBE> historyBEs =
                playerHistories?.Select(h => base.Mapper.Map<PlayerHistoryBE>(h)).ToList();
            return historyBEs;
        }

        public async Task UpdatePlayerHistory(PlayerHistoryBE historyBE)
        {
            PlayerHistoryEntity historyEntity = base.Mapper.Map<PlayerHistoryEntity>(historyBE);
            historyEntity.LastModifiedBy = SystemConstants.DefaultUser;
            historyEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerHistoryRepo.UpdateAsync(historyEntity);
        }
        #endregion

        #region PLAYER QUESTION METHODS
        public async Task InsertNewPlayerQuestion(PlayerQuestionBE playerQuestionBE)
        {
            PlayerQuestionEntity questionEntity = base.Mapper.Map<PlayerQuestionEntity>(playerQuestionBE);
            questionEntity.CreatedBy = questionEntity.LastModifiedBy = SystemConstants.DefaultUser;
            questionEntity.CreatedDateTime = questionEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerQuestionRepo.InsertAsync(questionEntity);
        }
        public async Task<List<PlayerQuestionBE>> GetPlayerQuestions(int playerHistoryID)
        {
            List<PlayerQuestionEntity> questionEntities =
                await base.DataSvc.PlayerQuestionRepo.GetByPlayerHistoryIDAsync(playerHistoryID);
            List<PlayerQuestionBE> questionBEs =
                questionEntities?.Select(qe => base.Mapper.Map<PlayerQuestionBE>(qe)).ToList();
            return questionBEs;
        }
        #endregion

        #region PLAYER ANSWER QUESTIONS
        public async Task InsertPlayerAnswer(PlayerAnswerBE playerAnswerBE)
        {
            PlayerAnswerEntity answerEntity = base.Mapper.Map<PlayerAnswerEntity>(playerAnswerBE);
            answerEntity.CreatedBy = answerEntity.LastModifiedBy = SystemConstants.DefaultUser;
            answerEntity.CreatedDateTime = answerEntity.LastModifiedDateTime = DateTime.UtcNow;

            await base.DataSvc.PlayerAnswerRepo.InsertAsync(answerEntity);
        }
        public async Task<List<PlayerAnswerBE>> GetPlayerAnswers(int playerHistoryID)
        {
            List<PlayerAnswerEntity> answerEntities =
                await base.DataSvc.PlayerAnswerRepo.GetByPlayerHistoryIDAsync(playerHistoryID);
            List<PlayerAnswerBE> answerBEs =
                answerEntities?.Select(qe => base.Mapper.Map<PlayerAnswerBE>(qe)).ToList();
            return answerBEs;
        }

        #endregion
    }
}