using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia.Composite;
using Tandem.Web.Apps.Trivia.Infrastructure;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player.Composite;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Trivia;
using BM = Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators
{
    public class BusinessModel_BusinessEntity : Profile
    {
        public BusinessModel_BusinessEntity()
        {
            //PLAYER TRANSLATORS
            this.RegisterTranslator<BM.Player, PlayerBE, Player_PlayerBE>();
            this.RegisterTranslator<PlayerAnswer, PlayerAnswerBE, PlayerAnswer_PlayerAnswerBE>();
            this.RegisterTranslator<QuestionDetail, QuestionDetailBE, QuestionDetail_QuestionDetailBE>();
            this.RegisterTranslator<PlayerRound, PlayerRoundBE, PlayerRound_PlayerRoundBE>();

            //TRIVIA TRANSLATORS
            this.RegisterTranslator<Question, QuestionBE, Question_QuestionBE>();
            this.RegisterTranslator<Answer, AnswerBE, Answer_AnswerBE>();
        }
    }
}