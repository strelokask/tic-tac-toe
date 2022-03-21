﻿using Core.DAL;
using Core.Dtos;
using MediatR;

namespace Core.Commands
{
    public class JoinGameCommand : IRequest
    {
        public int GameId { get; }
        public PlayerDto Player { get; }

        public JoinGameCommand(int gameId, PlayerDto player)
        {
            GameId = gameId;
            Player = player;
        }
    }

    public class JoinGameHandler : IRequestHandler<JoinGameCommand, Unit>
    {
        private readonly IGameContext _context;

        public JoinGameHandler(IGameContext context)
        {
            _context = context;
        }
        public async Task<Unit> Handle(JoinGameCommand request, CancellationToken cancellationToken)
        {
            var game = _context.Games.FirstOrDefault(g => g.Id.Equals(request.GameId));

            if (game == null)
            {
                throw new("Not found");
            }

            var player = request.Player;
            if (game.Player1 == player.Name)
            {
                throw new("Player1 has the same name");
            }

            game.Player2 = player.Name;

            _context.MarkAsModified(game);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
