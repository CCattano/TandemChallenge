using System;
using System.Collections.Generic;
using System.Text;

namespace Tandem.Web.Apps.Trivia.BusinessModels.Player
{
    public class NewPlayer
    {
        public int PlayerID { get; set; }
        public string LoginToken { get; set; }
        public DateTime LoginTokenExpireDateTime { get; set; }
    }
}
