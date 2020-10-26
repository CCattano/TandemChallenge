using Microsoft.AspNetCore.Mvc;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    public abstract class BaseController<TAdapter> : Controller where TAdapter : class
    {
        public BaseController(TAdapter adapter) => Adapter = adapter;

        protected TAdapter Adapter;
    }
}