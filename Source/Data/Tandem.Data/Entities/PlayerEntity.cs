namespace Tandem.Web.Apps.Trivia.Data.Entities
{
    public class PlayerEntity : BaseEntity
    {
        public int PlayerID { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}