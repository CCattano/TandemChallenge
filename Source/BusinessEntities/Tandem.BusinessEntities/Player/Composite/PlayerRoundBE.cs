using System.Collections.Generic;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia.Composite;

namespace Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite
{
    public class PlayerRoundBE : PlayerHistoryBE
    {
        public List<QuestionDetailBE> QuestionDetails { get; set; }
        public List<PlayerAnswerBE> PlayerAnswers { get; set; }
    }
}