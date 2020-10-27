using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player
{
    public class NewPlayer_PlayerBE : ITypeConverter<NewPlayer, PlayerBE>, ITypeConverter<PlayerBE, NewPlayer>
    {
        public NewPlayer Convert(PlayerBE source, NewPlayer destination, ResolutionContext context)
        {
            NewPlayer result = destination ?? new NewPlayer();
            result.PlayerID = source.PlayerID;
            result.LoginToken = source.LoginToken;
            result.LoginTokenExpireDateTime = source.LoginTokenExpireDateTime;
            return result;
        }

        public PlayerBE Convert(NewPlayer source, PlayerBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
