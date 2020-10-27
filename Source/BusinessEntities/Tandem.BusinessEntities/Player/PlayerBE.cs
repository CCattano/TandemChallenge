using System;

namespace Tandem.Web.Apps.Trivia.BusinessEntities.Player
{
    public class PlayerBE
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string NameHash { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordPepper { get; set; }
        public string LoginToken { get; set; }
        public DateTime LoginTokenExpireDateTime { get; set; }
    }
}
