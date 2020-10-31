﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tandem.Web.Apps.Trivia.Adapter.Contracts;
using Tandem.Web.Apps.Trivia.BusinessEntities.Player.Composite;
using Tandem.Web.Apps.Trivia.BusinessModels.Player.Composite;
using Tandem.Web.Apps.Trivia.WebService.Middleware.TokenValidation.TokenValidationResources;

namespace Tandem.Web.Apps.Trivia.WebService.Controllers
{
    [Route("[controller]/[action]")]
    public class TriviaController : BaseController<ITriviaAdapter>
    {
        public TriviaController(ITriviaAdapter adapter, IMapper mapper) : base(adapter, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult<PlayerRound>> GetTriviaRound(int playerID)
        {
            PlayerRoundBE playerRoundBE = await Adapter.GetTriviaRound(playerID);
            PlayerRound playerRound = Mapper.Map<PlayerRound>(playerRoundBE);
            return playerRound;
        }

        [HttpGet]
        [NoToken]
        public async Task<ActionResult<PlayerRound>> GetGuestTriviaRound()
        {
            PlayerRoundBE playerRoundBE = await Adapter.GetTriviaRound(null);
            PlayerRound playerRound = Mapper.Map<PlayerRound>(playerRoundBE);
            return playerRound;
        }

        [HttpGet]
        public async Task<ActionResult<PlayerRound>> GetExistingRound(int playerHistoryID)
        {
            PlayerRoundBE playerRoundBE = await Adapter.GetExistingRound(playerHistoryID);
            PlayerRound playerRound = Mapper.Map<PlayerRound>(playerRoundBE);
            return playerRound;
        }
    }
}