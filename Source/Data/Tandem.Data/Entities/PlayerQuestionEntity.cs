namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class PlayerQuestionEntity : BaseEntity
    {
        public int PlayerQuestionID { get; set; }
        public int PlayerHistoryID { get; set; }
        public int QuestionID { get; set; }
        public int QuestionSequence { get; set; }
    }
}