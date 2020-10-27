﻿namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class PlayerAnswerEntity : BaseEntity
    {
        public int PlayerAnswerID { get; set; }
        public int QuestionID { get; set; }
        public int AnswerID { get; set; }
        public int Sequence { get; set; }
    }
}