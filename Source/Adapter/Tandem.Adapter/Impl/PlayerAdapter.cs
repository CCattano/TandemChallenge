using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Adapter.Impl
{
    public class PlayerAdapter : BaseAdapter<IPlayerFacade>, IPlayerAdapter
    {
        public PlayerAdapter(IPlayerFacade facade, IStatusResponse statusResp) : base(facade, statusResp)
        {
        }
    }
}
