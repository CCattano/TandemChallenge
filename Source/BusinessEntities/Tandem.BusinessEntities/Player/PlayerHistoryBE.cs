using System;

namespace Tandem.Web.Apps.Trivia.BusinessEntities.Player
{
    public class PlayerHistoryBE
    {
        public int PlayerHistoryID { get; set; }
        public int PlayerID { get; set; }
        public int RoundNumber { get; set; }
        public DateTime StartedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
    }
}
