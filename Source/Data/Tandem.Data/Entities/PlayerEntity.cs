using System;

namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class PlayerEntity : BaseEntity
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string NameHash { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordPepper { get; set; }
        public DateTime LoginTokenExpireDateTime { get; set; }
    }
}