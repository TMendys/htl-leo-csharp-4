using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using TournamentPlanner.Data;

namespace TournamentPlanner.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly TournamentPlannerDbContext context;

        public PlayersController(TournamentPlannerDbContext context) 
            => this.context = context;

        [HttpGet]
        public async Task<IEnumerable<Player>> GetPlayers([FromQuery] string name = null) 
            => await context.GetFilteredPlayers(name);

        [HttpPost]
        public async Task<Player> AddPlayer([FromBody] Player player) => await context.AddPlayer(player);


    }
}
