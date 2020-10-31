namespace Tandem.Web.Apps.Trivia.BusinessEntities.Trivia
{
    public class AnswerBE
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}