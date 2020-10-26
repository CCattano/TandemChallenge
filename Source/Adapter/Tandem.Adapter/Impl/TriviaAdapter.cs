using System.Threading.Tasks;
using Tandem.Common.StatusResponse.Model.Contracts;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.Facade.Contracts;

namespace Tandem.Web.Apps.Trivia.Adapter.Impl
{
    public class TriviaAdapter : BaseAdapter<ITriviaFacade>, ITriviaAdapter
    {
        public TriviaAdapter(ITriviaFacade facade, IStatusResponse statusResp) : base(facade, statusResp)
        {
        }

        public async Task<bool> E2ETest() => await Facade.E2ETest(); //TEST
    }
}