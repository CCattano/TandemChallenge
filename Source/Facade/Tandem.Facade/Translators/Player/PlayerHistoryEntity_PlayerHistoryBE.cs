using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Trivia;
using Tandem.Web.Apps.Trivia.Data.Entities;

namespace Tandem.Web.Apps.Trivia.Facade.Translators.Player
{
    public class PlayerHistoryEntity_PlayerHistoryBE : ITypeConverter<PlayerHistoryEntity, PlayerHistoryBE>, ITypeConverter<PlayerHistoryBE, PlayerHistoryEntity>
    {
        public PlayerHistoryEntity Convert(PlayerHistoryBE source, PlayerHistoryEntity destination, ResolutionContext context)
        {
            PlayerHistoryEntity result = destination ?? new PlayerHistoryEntity();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.PlayerID = source.PlayerID;
            result.RoundNumber = source.RoundNumber;
            result.StartedDateTime = source.StartedDateTime;
            result.CompletedDateTime = source.CompletedDateTime;
            return result;
        }

        public PlayerHistoryBE Convert(PlayerHistoryEntity source, PlayerHistoryBE destination, ResolutionContext context)
        {
            PlayerHistoryBE result = destination ?? new PlayerHistoryBE();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.PlayerID = source.PlayerID;
            result.RoundNumber = source.RoundNumber;
            result.StartedDateTime = source.StartedDateTime;
            result.CompletedDateTime = source.CompletedDateTime;
            return result;
        }
    }
}
