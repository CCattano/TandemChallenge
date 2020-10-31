using AutoMapper;
using Tandem.Web.Apps.Trivia.Adapter.Translators.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;
using Tandem.Web.Apps.Trivia.Infrastructure;

namespace Tandem.Web.Apps.Trivia.Adapter.Translators
{
    public class BusinessEntity_BusinessEntity : Profile
    {
        public BusinessEntity_BusinessEntity()
        {
            this.RegisterTranslator<PlayerRoundBE, PlayerHistoryBE, PlayerRoundBE_PlayerHistoryBE>();
        }
    }
}
