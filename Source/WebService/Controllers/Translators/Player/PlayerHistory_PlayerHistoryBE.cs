using AutoMapper;
using System;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player
{
    public class PlayerHistory_PlayerHistoryBE : ITypeConverter<PlayerHistory, PlayerHistoryBE>, ITypeConverter<PlayerHistoryBE, PlayerHistory>
    {
        public PlayerHistory Convert(PlayerHistoryBE source, PlayerHistory destination, ResolutionContext context)
        {
            PlayerHistory result = destination ?? new PlayerHistory();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.PlayerID = source.PlayerID;
            result.RoundNumber = source.RoundNumber;
            result.StartedDateTime = source.StartedDateTime;
            result.CompletedDateTime = source.CompletedDateTime;
            return result;
        }

        public PlayerHistoryBE Convert(PlayerHistory source, PlayerHistoryBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}