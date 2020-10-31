using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player
{
    public class PlayerAnswer_PlayerAnswerBE : ITypeConverter<PlayerAnswer, PlayerAnswerBE>, ITypeConverter<PlayerAnswerBE, PlayerAnswer>
    {
        public PlayerAnswer Convert(PlayerAnswerBE source, PlayerAnswer destination, ResolutionContext context)
        {
            PlayerAnswer result = destination ?? new PlayerAnswer();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.AnswerID = source.AnswerID;
            return result;
        }

        public PlayerAnswerBE Convert(PlayerAnswer source, PlayerAnswerBE destination, ResolutionContext context)
        {
            PlayerAnswerBE result = destination ?? new PlayerAnswerBE();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.QuestionID = source.QuestionID;
            result.AnswerID = source.AnswerID;
            return result;
        }
    }
}