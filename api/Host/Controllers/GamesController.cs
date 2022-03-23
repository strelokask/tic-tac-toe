using Core.Commands;
using Core.DAL;
using Core.Dtos;
using Core.Models;
using Core.Notifications;
using Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameContext _context;
        private readonly IMediator _mediator;
        private readonly IGameNotification _gameNotification;

        public GamesController(IGameContext context, IMediator mediator, IGameNotification gameNotification)
        {
            _context = context;
            _mediator = mediator;
            _gameNotification = gameNotification;
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
        
        // POST: api/games/new
        [HttpPost("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<GameModel>> NewGame(PlayerDto player)
        {
            GameModel game = new GameModel
            {
                Player1 = player.Name
            };

            _context.Games.Add(game);

            await _context.SaveChangesAsync();

            _gameNotification.NotifyChanges(game);

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
        }

        // PUT: api/games/join/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("join/{id}")]
        public async Task<IActionResult> JoinGame(int id, PlayerDto player)
        {
            var cmd = new JoinGameCommand(id, player);

            var game = await _mediator.Send(cmd);

            _gameNotification.NotifyChanges(game);

            return NoContent();
        }

        [HttpPut("move/{id}")]
        public async Task<IActionResult> MakeMove(int id, MoveDto move)
        {
            var cmd = new MakeMoveCommand(id, move);

            var model = await _mediator.Send(cmd);

            _gameNotification.NotifyChanges(model);

            return Ok(model);
        }


        [HttpGet("stream/{id}")]
        [Produces("text/event-stream")]
        public async Task StreamCounter(long id, CancellationToken cancellationToken)
        {
            Response.StatusCode = 200;
            Response.Headers.Add("Content-Type", "text/event-stream");

            async void OnNotification(object sender, GameModel game)
            {
                if (game.Id == id)
                {
                    var messageJson = JsonConvert.SerializeObject(game, new JsonSerializerSettings()
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    await Response.WriteAsync($"data:{messageJson}\n\n");
                    await Response.Body.FlushAsync();
                }
            }

            try
            {
                _gameNotification.NotificationEvent += OnNotification;
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, cancellationToken);
                }
            }
            finally
            {
                _gameNotification.NotificationEvent -= OnNotification;
            }

        }
    }
}
