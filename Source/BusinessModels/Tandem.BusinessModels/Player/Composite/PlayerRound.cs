using System.Collections.Generic;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia.Composite;

namespace Tandem.Web.Apps.Trivia.BusinessModels.Player.Composite
{
    public class PlayerRound : PlayerHistory
    {
        public List<QuestionDetail> QuestionDetails { get; set; }
        public List<PlayerAnswer> PlayerAnswers { get; set; }
    }
}
