using System;
using System.Collections.Generic;
using System.Text;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;

namespace Tandem.Web.Apps.Trivia.BusinessEntities.Trivia.Composite
{
    public class QuestionDetailBE : QuestionBE
    {
        public List<AnswerBE> Answers { get; set; }
        public int QuestionSequence { get; set; }
    }
}
