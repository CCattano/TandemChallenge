using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class PlayerController : BaseController<IPlayerAdapter>
    {
        public PlayerController(IPlayerAdapter adapter, IMapper mapper) : base(adapter, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<bool>> PlayerNameIsAvailable([FromQuery]string playerName)
        {
            bool isAvailable = await Adapter.NameIsAvailable(playerName);
            return isAvailable;
        }

        [HttpPost]
        public async Task<ActionResult<NewPlayer>> CreateAccount([FromQuery]string username, [FromQuery]string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            PlayerBE playerBE = await Adapter.CreateAccount(username, password);
            NewPlayer newPlayer = Mapper.Map<NewPlayer>(playerBE);
            return newPlayer;
        }
    }
}
