using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            return JsonSerializer.Serialize(newPlayerToken);
        }

        [HttpPost]
        [NoToken]
        public async Task<ActionResult<string>> Login([FromQuery] string username, [FromQuery] string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            string newLoginToken = await Adapter.Login(username, password);
            return JsonSerializer.Serialize(newLoginToken);
        }

        [HttpGet]
        public async Task<ActionResult<Player>> GetPlayerByID([FromQuery] int playerID)
        {
            PlayerBE playerBE = await Adapter.GetPlayerByID(playerID);
            Player player = playerBE != null ? base.Mapper.Map<Player>(playerBE) : null;
            return player;
        }

        [HttpPost]
        public async Task<ActionResult> SavePlayerAnswer([FromBody] PlayerAnswer playerAnswer)
        {
            PlayerAnswerBE answerBE = Mapper.Map<PlayerAnswerBE>(playerAnswer);
            await base.Adapter.SavePlayerAnswer(answerBE);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> MarkRoundCompleted([FromQuery] int playerHistoryID)
        {
            await base.Adapter.MarkRoundCompleted(playerHistoryID);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<List<PlayerHistory>>> GetPlayerHistory([FromQuery] int playerID)
        {
            List<PlayerHistoryBE> historyBEs = await base.Adapter.GetAllPlayerHistory(playerID);
            List<PlayerHistory> histories = historyBEs?.Select(h => base.Mapper.Map<PlayerHistory>(h)).ToList();
            return histories;
        }
    }
}