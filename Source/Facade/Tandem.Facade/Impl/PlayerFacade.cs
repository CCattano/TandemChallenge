using Tandem.Web.Apps.Trivia.Data;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Facade.Impl
{
    public class PlayerFacade : BaseFacade, IPlayerFacade
    {
        public PlayerFacade(ITriviaDataService dataSvc) : base(dataSvc)
        {
        }
    }
}