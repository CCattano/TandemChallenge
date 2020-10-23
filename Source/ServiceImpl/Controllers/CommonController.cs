using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tandem.Web.App.ViewModels.ViewModels;
using Tandem.Web.Apps.Facade.Contracts;

namespace Tandem.Web.Apps.ServiceImpl.Controllers
{
    [Route("[controller]/[action]")]
    public class CommonController : BaseController<ICommonFacade>
    {
        public CommonController(ICommonFacade facade) : base(facade)
        {
        }

        [HttpGet]
        public async Task<ActionResult<BaseViewModel>> E2ETest() => await base.Facade.E2ETest();
    }
}
