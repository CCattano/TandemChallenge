using System.Collections.Generic;

namespace Tandem.Web.Apps.Trivia.BusinessModels.Trivia.Composite
{
    public class QuestionDetail : Question
    {
        public List<Answer> Answers { get; set; }
        public int QuestionSequence { get; set; }
    }
}
