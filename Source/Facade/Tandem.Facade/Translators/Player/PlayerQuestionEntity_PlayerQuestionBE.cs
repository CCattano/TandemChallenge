using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Player
{
    public class PlayerQuestionEntity_PlayerQuestionBE : ITypeConverter<PlayerQuestionEntity, PlayerQuestionBE>, ITypeConverter<PlayerQuestionBE, PlayerQuestionEntity>
    {
        public PlayerQuestionEntity Convert(PlayerQuestionBE source, PlayerQuestionEntity destination, ResolutionContext context)
        {
            PlayerQuestionEntity result = destination ?? new PlayerQuestionEntity();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.QuestionSequence = source.QuestionSequence;
            return result;
        }

        public PlayerQuestionBE Convert(PlayerQuestionEntity source, PlayerQuestionBE destination, ResolutionContext context)
        {
            PlayerQuestionBE result = destination ?? new PlayerQuestionBE();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.QuestionSequence = source.QuestionSequence;
            return result;
        }
    }
}
