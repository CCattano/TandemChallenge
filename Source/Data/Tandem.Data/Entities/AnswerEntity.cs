namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class AnswerEntity : BaseEntity
    {
        public int AnswerID { get; set; }
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
