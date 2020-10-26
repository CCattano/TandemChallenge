using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class TriviaController : BaseController<ITriviaAdapter>
    {
        public TriviaController(ITriviaAdapter adapter) : base(adapter)
        {
        }

        [HttpGet]
        public async Task<ActionResult<bool>> E2ETest() => await Adapter.E2ETest(); //TEST
    }
}