﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;

namespace Tandem.Web.Apps.Trivia.Facade.Contracts
{
    public interface IPlayerFacade
    {
        //PLAYER METHODS
        Task<PlayerBE> GetPlayerByNameHash(string nameHash);
        Task<PlayerBE> GetPlayerByPlayerID(int playerID);
        Task InsertNewPlayer(PlayerBE newPlayer);
        Task<PlayerBE> UpdatePlayer(PlayerBE playerBE);
        //PLAYER HISTORY METHODS
        Task<int> GetRoundNumberByPlayerID(int playerID);
        Task InsertNewHistory(PlayerHistoryBE historyBE);
        Task<PlayerHistoryBE> GetPlayerHistory(int playerHistoryID);
        Task<List<PlayerHistoryBE>> GetAllPlayerHistory(int playerID);
        Task UpdatePlayerHistory(PlayerHistoryBE historyBE);
        //PLAYER QUESTION METHODS
        Task InsertNewPlayerQuestion(PlayerQuestionBE playerQuestionBE);
        Task<List<PlayerQuestionBE>> GetPlayerQuestions(int playerHistoryID);
        //PLAYER ANSWER METHODS
        Task InsertPlayerAnswer(PlayerAnswerBE playerAnswerBE);
        Task<List<PlayerAnswerBE>> GetPlayerAnswers(int playerHistoryID);
    }
}
