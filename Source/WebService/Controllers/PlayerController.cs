using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player;
using Tandem.Web.Apps.Trivia.BusinessModels.Player;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class PlayerController : BaseController<IPlayerAdapter>
    {
        public PlayerController(IPlayerAdapter adapter, IMapper mapper) : base(adapter, mapper)
        {
        }

        [HttpGet]
        [NoToken]
        public async Task<ActionResult<bool>> PlayerNameIsAvailable([FromQuery] string playerName)
        {
            bool isAvailable = await Adapter.NameIsAvailable(playerName);
            return isAvailable;
        }

        [HttpPost]
        [NoToken]
        public async Task<ActionResult<string>> CreateAccount([FromQuery] string username, [FromQuery] string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            string newPlayerToken = await Adapter.CreateAccount(username, password);
            return newPlayerToken;
        }

        [HttpPost]
        [NoToken]
        public async Task<ActionResult<string>> Login([FromQuery] string username, [FromQuery] string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            string newLoginToken = await Adapter.Login(username, password);
            return newLoginToken;
        }

        [HttpGet]
        public async Task<ActionResult<Player>> GetPlayerByID([FromQuery]int playerID, [FromHeader] string TandemToken = @"eyJBbGdvcml0aG0iOiJTSEEyNTYiLCJUeXBlIjoiSldUIn0.eyJQbGF5ZXJJRCI6MSwiRXhwaXJhdGlvbkRhdGVUaW1lIjoiMjAyMC0xMS0xMFQyMTozMzo0OC4zNjc4ODI4WiJ9.MTdERTVBRDg5MDVBRDQ4QzFFRTI3MzA3MUU1NTZGRDRDMTg0QjZDRDJGNzc3RTIyMkUxMzI4M0NCRUNGQ0NGQg")
        {
            PlayerBE playerBE = await Adapter.GetPlayerByID(playerID);
            Player player = playerBE != null ? base.Mapper.Map<Player>(playerBE) : null;
            return player;
        }
    }
}