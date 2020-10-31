using System;

namespace Tandem.Web.Apps.Trivia.BusinessModels.Player
{
    public class PlayerHistory
    {
        public int PlayerHistoryID { get; set; }
        public int PlayerID { get; set; }
        public int RoundNumber { get; set; }
        public DateTime StartedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}
