using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;
using Tandem.Web.Apps.Trivia.Infrastructure;
using Tandem.Web.Apps.Trivia.WebService.Controllers.Translators.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers.Translators
{
    public class BusinessModel_BusinessEntity : Profile
    {
        public BusinessModel_BusinessEntity()
        {
            this.RegisterTranslator<NewPlayer, PlayerBE, NewPlayer_PlayerBE>();
        }
    }
}
