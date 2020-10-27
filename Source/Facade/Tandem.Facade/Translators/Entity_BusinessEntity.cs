using AutoMapper;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.Data.Entities;
using Tandem.Web.Apps.Trivia.Facade.Translators.Player;
using Tandem.Web.Apps.Trivia.Infrastructure;

namespace Tandem.Web.Apps.Trivia.Facade.Translators
{
    public class Entity_BusinessEntity : Profile
    {
        public Entity_BusinessEntity()
        {
            //PLAYER TRANSLATORS
            this.RegisterTranslator<PlayerEntity, PlayerBE, PlayerEntity_PlayerBE>();
        }
    }
}
