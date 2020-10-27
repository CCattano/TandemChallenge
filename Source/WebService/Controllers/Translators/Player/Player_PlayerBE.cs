using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using BM = Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player
{
    public class Player_PlayerBE : ITypeConverter<BM.Player, PlayerBE>, ITypeConverter<PlayerBE, BM.Player>
    {
        public PlayerBE Convert(BM.Player source, PlayerBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }

        public BM.Player Convert(PlayerBE source, BM.Player destination, ResolutionContext context)
        {
            BM.Player result = destination ?? new BM.Player();
            result.PlayerID = source.PlayerID;
            result.Name = source.Name;
            return result;
        }
    }
}
