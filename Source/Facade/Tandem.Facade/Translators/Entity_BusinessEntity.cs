using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Translators.Player;
using Tandem.Web.Apps.Trivia.Facade.Translators.Trivia;
using Tandem.Web.Apps.Trivia.Infrastructure;

namespace Tandem.Web.Apps.Trivia.Facade.Translators
{
    public class Entity_BusinessEntity : Profile
    {
        public Entity_BusinessEntity()
        {
            //PLAYER TRANSLATORS
            this.RegisterTranslator<PlayerEntity, PlayerBE, PlayerEntity_PlayerBE>();
            this.RegisterTranslator<PlayerHistoryEntity, PlayerHistoryBE, PlayerHistoryEntity_PlayerHistoryBE>();
            this.RegisterTranslator<PlayerAnswerEntity, PlayerAnswerBE, PlayerAnswerEntity_PlayerAnswerBE>();
            this.RegisterTranslator<PlayerQuestionEntity, PlayerQuestionBE, PlayerQuestionEntity_PlayerQuestionBE>();

            //TRIVIA TRANSLATORS
            this.RegisterTranslator<QuestionEntity, QuestionBE, QuestionEntity_QuestionBE>();
            this.RegisterTranslator<AnswerEntity, AnswerBE, AnswerEntity_AnswerBE>();
        }
    }
}
