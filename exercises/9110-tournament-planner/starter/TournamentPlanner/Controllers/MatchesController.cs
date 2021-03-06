using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TournamentPlanner.Data;

namespace TournamentPlanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly TournamentPlannerDbContext context;

        public MatchesController(TournamentPlannerDbContext context) => this.context = context;

        [HttpGet("open")]
        public async Task<IList<Match>> GetIncompleteMatches() => await context.GetIncompleteMatches();

        [HttpPost("generate")]
        public async Task GenerateFirstRound() => await context.GenerateFirstRound();
    }
}
