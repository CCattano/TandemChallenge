using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Player
{
    public class PlayerEntity_PlayerBE : ITypeConverter<PlayerEntity, PlayerBE>, ITypeConverter<PlayerBE, PlayerEntity>
    {
        public PlayerEntity Convert(PlayerBE source, PlayerEntity destination, ResolutionContext context)
        {
            PlayerEntity result = destination ?? new PlayerEntity();
            result.PlayerID = source.PlayerID;
            result.Name = source.Name;
            result.NameHash = source.NameHash;
            result.PasswordHash = source.PasswordHash;
            result.PasswordSalt = source.PasswordSalt;
            result.PasswordPepper= source.PasswordPepper;
            result.LoginToken = source.LoginToken;
            result.LoginTokenExpireDateTime = source.LoginTokenExpireDateTime;
            return result;
        }

        public PlayerBE Convert(PlayerEntity source, PlayerBE destination, ResolutionContext context)
        {
            PlayerBE result = destination ?? new PlayerBE();
            result.PlayerID = source.PlayerID;
            result.Name = source.Name;
            result.NameHash = source.NameHash;
            result.PasswordHash = source.PasswordHash;
            result.PasswordSalt = source.PasswordSalt;
            result.PasswordPepper = source.PasswordPepper;
            result.LoginToken = source.LoginToken;
            result.LoginTokenExpireDateTime = source.LoginTokenExpireDateTime;
            return result;
        }
    }
}
