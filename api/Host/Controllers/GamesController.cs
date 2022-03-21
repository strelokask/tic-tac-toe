using Core.Commands;
using Core.DAL;
using Core.Domain;
using Core.Dtos;
using Core.Models;
using Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameContext _context;
        private readonly IMediator _mediator;

        public GamesController(IGameContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }
        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameModel>>> GetGames(CancellationToken cancellationToken)
        {
            var query = new GetGamesQuery();

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameModel>> GetGame(int id, CancellationToken cancellationToken)
        {
            var query = new GetGameByIdQuery(id);

            var game = await _mediator.Send(query, cancellationToken);

            if (game == null)
            {
                return NotFound();
            }

            return game;
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(long id, GameModel game)
        {
            if (id != game.Id)
            {
                return BadRequest();
            }

            _context.MarkAsModified(game);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            //_mediator.PublishUpdate(game);
            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GameModel>> PostGame(GameModel game, CancellationToken cancellationToken)
        {
            var cmd = new CreateGameCommand(game);

            var result = await _mediator.Send(cmd, cancellationToken);

            //_mediator.PublishUpdate(game);
            return CreatedAtAction(nameof(GetGame), new { id = result.Id }, game);
        }
        
        // POST: api/games/new
        [HttpPost("new")]
        public async Task<IActionResult> NewGame(PlayerDto player)
        {
            GameModel game = new GameModel
            {
                Player1 = player.Name
            };

            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            //_mediator.PublishUpdate(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        // PUT: api/games/join/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("join/{id}")]
        public async Task<IActionResult> JoinGame(int id, PlayerDto player)
        {
            var cmd = new JoinGameCommand(id, player);

            await _mediator.Send(cmd);

            //_mediator.PublishUpdate(game);
            
            return NoContent();
        }

        [HttpPut("move/{id}")]
        public async Task<IActionResult> MakeMove(int id, MoveDto move)
        {
            var cmd = new MakeMoveCommand(id, move);

            var model = await _mediator.Send(cmd);

            //_mediator.PublishUpdate(model);

            return Ok(model);
        }

        //[HttpGet("stream/{id}")]
        //public async Task StreamCounter(long id, CancellationToken cancellationToken)
        //{
        //    Response.StatusCode = 200;
        //    Response.Headers.Add("Content-Type", "text/event-stream");

        //    _mediator.GameUpdated += StreamValue;

        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        await Task.Delay(1000);
        //    }

        //    _mediator.GameUpdated -= StreamValue;

        //    async void StreamValue(object sender, GameModel game)
        //    {
        //        if (game.Id == id)
        //        {
        //            var messageJson = JsonConvert.SerializeObject(game, _jsonSettings);
        //            await Response.WriteAsync($"data:{messageJson}\n\n");
        //            await Response.Body.FlushAsync();
        //        }
        //    }
        //}

        private bool GameExists(long id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
