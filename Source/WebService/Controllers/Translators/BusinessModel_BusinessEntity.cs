using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Infrastructure;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player;
using BM = Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators
{
    public class BusinessModel_BusinessEntity : Profile
    {
        public BusinessModel_BusinessEntity()
        {
            this.RegisterTranslator<BM.Player, PlayerBE, Player_PlayerBE>();
        }
    }
}
