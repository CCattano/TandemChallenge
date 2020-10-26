using Tandem.Common.StatusResponse.Model.Contracts;

namespace Tandem.Web.Apps.Trivia.Adapter
{
    public abstract class BaseAdapter<TFacade>
    {
        protected BaseAdapter(TFacade facade, IStatusResponse statusResp)
        {
            Facade = facade;
            StatusResp = statusResp;
        }

        protected readonly TFacade Facade;
        protected readonly IStatusResponse StatusResp;
    }
}