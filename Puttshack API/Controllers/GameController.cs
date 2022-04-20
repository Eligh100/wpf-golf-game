using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Puttshack_API.Models;
using Puttshack_API.Services;

namespace Puttshack_API.Controllers
{
    [Route("api/game")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private GameStateService gameStateService;
        public GameController()
        {
            this.gameStateService = new GameStateService();
        }

        // GET: api/game/start
        [HttpGet("start")]
        public ActionResult<GameMove> StartGame()
        {
            return this.gameStateService.StartGame();
        }

        // GET: api/game/next/:currentMoveNumber
        [HttpGet("next")]
        public ActionResult<GameMove> NextMove([System.Web.Http.FromUri] string currentMoveNumber)
        {
            return this.gameStateService.NextMove(Int32.Parse(currentMoveNumber));
        }

    }
}
