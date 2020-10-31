using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Player
{
    public class PlayerAnswerEntity_PlayerAnswerBE : ITypeConverter<PlayerAnswerEntity, PlayerAnswerBE>, ITypeConverter<PlayerAnswerBE, PlayerAnswerEntity>
    {
        public PlayerAnswerEntity Convert(PlayerAnswerBE source, PlayerAnswerEntity destination, ResolutionContext context)
        {
            PlayerAnswerEntity result = destination ?? new PlayerAnswerEntity();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.AnswerID = source.AnswerID;
            return result;
        }

        public PlayerAnswerBE Convert(PlayerAnswerEntity source, PlayerAnswerBE destination, ResolutionContext context)
        {
            PlayerAnswerBE result = destination ?? new PlayerAnswerBE();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.AnswerID = source.AnswerID;
            return result;
        }
    }
}