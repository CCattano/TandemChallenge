using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Trivia.Composite;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player.Composite
{
    public class PlayerRound_PlayerRoundBE : ITypeConverter<PlayerRound, PlayerRoundBE>, ITypeConverter<PlayerRoundBE, PlayerRound>
    {
        public PlayerRound Convert(PlayerRoundBE source, PlayerRound destination, ResolutionContext context)
        {
            PlayerRound result = destination ?? new PlayerRound();
            result.PlayerHistoryID = source.PlayerHistoryID;
            result.PlayerID = source.PlayerID;
            result.RoundNumber = source.RoundNumber;
            result.StartedDateTime = source.StartedDateTime;
            result.CompletedDateTime = source.CompletedDateTime;
            result.QuestionDetails = source.QuestionDetails?.Select(qd => context.Mapper.Map<QuestionDetail>(qd)).ToList();
            result.PlayerAnswers = source.PlayerAnswers?.Select(pa => context.Mapper.Map<PlayerAnswer>(pa)).ToList();
            return result;
        }

        public PlayerRoundBE Convert(PlayerRound source, PlayerRoundBE destination, ResolutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
