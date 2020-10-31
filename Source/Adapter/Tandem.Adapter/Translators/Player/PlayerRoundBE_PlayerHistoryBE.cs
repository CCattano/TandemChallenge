using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;

namespace Tandem.Web.Apps.Trivia.Adapter.Translators.Player
{
    public class PlayerRoundBE_PlayerHistoryBE : ITypeConverter<PlayerRoundBE, PlayerHistoryBE>, ITypeConverter<PlayerHistoryBE, PlayerRoundBE>
    {
        public PlayerRoundBE Convert(PlayerHistoryBE source, PlayerRoundBE destination, ResolutionContext context)
        {
            PlayerRoundBE result = destination ?? new PlayerRoundBE();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.PlayerID = source.PlayerID;
            result.RoundNumber = source.RoundNumber;
            result.StartedDateTime = source.StartedDateTime;
            result.CompletedDateTime = source.CompletedDateTime;
            return result;
        }

        public PlayerHistoryBE Convert(PlayerRoundBE source, PlayerHistoryBE destination, ResolutionContext context)
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
