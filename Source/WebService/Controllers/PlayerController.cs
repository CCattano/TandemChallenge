﻿using AutoMapper;
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
            bool isAvailable = await base.Adapter.NameIsAvailable(playerName);
            return isAvailable;
        }

        [HttpPost]
        [NoToken]
        public async Task<ActionResult<string>> CreateAccount([FromQuery] string username, [FromQuery] string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            string newPlayerToken = await base.Adapter.CreateAccount(username, password);
            return JsonSerializer.Serialize(newPlayerToken);
        }

        [HttpPost]
        [NoToken]
        public async Task<ActionResult<string>> Login([FromQuery] string username, [FromQuery] string password)
        {
            //Typically these param values would be transmitted over encrypted h2
            string newLoginToken = await base.Adapter.Login(username, password);
            return JsonSerializer.Serialize(newLoginToken);
        }

        [HttpPost]
        [Route("/[controller]/[action]/{playerID}")]
        public async Task<ActionResult<string>> ChangeUsername([FromRoute]int playerID, [FromQuery]string newUsername)
        {
            string newToken = await base.Adapter.ChangeUsername(playerID, newUsername);
            return JsonSerializer.Serialize(newToken);
        }

        [HttpPost]
        [Route("/[controller]/[action]/{playerID}")]
        public async Task<ActionResult> ChangePassword([FromRoute]int playerID, [FromQuery]string currentPassword, [FromQuery]string newPassword)
        {
            await base.Adapter.ChangePassword(playerID, currentPassword, newPassword);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<Player>> GetPlayerByID([FromQuery] int playerID)
        {
            PlayerBE playerBE = await base.Adapter.GetPlayerByID(playerID);
            Player player = playerBE != null ? base.Mapper.Map<Player>(playerBE) : null;
            return player;
        }

        [HttpPost]
        public async Task<ActionResult> SavePlayerAnswer([FromBody] PlayerAnswer playerAnswer)
        {
            PlayerAnswerBE answerBE = base.Mapper.Map<PlayerAnswerBE>(playerAnswer);
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