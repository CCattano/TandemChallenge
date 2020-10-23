using Microsoft.AspNetCore.Mvc;

namespace Tandem.Web.Apps.ServiceImpl.Controllers
{
    public abstract class BaseController<TFacade> : Controller where TFacade : class
    {
        public BaseController(TFacade facade) => Facade = facade;

        protected TFacade Facade;
    }
}
