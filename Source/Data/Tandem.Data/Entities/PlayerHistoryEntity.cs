﻿using System;
using System.Collections.Generic;

namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class PlayerHistoryEntity : BaseEntity
    {
        public int PlayerHistoryID { get; set; }
        public int PlayerID { get; set; }
        public int GameNumber { get; set; }
        public DateTime StartedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public List<QuestionEntity> Questions { get; set; }
        public List<PlayerAnswerEntity> Answers { get; set; }
    }
}