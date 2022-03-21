using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAL;
using Core.Domain;
using Core.Dtos;
using Core.Models;
using MediatR;

namespace Core.Commands
{
    public class MakeMoveCommand : IRequest<GameModel>
    {
        public int Id { get; }
        public MoveDto Move { get; }

        public MakeMoveCommand(int id, MoveDto move)
        {
            Id = id;
            Move = move;
        }
    }

    public class MakeMoveHandler : IRequestHandler<MakeMoveCommand, GameModel>
    {
        private readonly IGameContext _context;

        public MakeMoveHandler(IGameContext context)
        {
            _context = context;
        }
        public async Task<GameModel> Handle(MakeMoveCommand request, CancellationToken cancellationToken)
        {
            var model = _context.Games.FirstOrDefault(x => x.Id.Equals(request.Id));
            if (model == null) throw new ($"Not found {request.Id}");

            var move = request.Move;

            string nextPlayer = model.NextPlayer == 0 ? model.Player1 : model.Player2;
            if (!move.Validate(nextPlayer))
            {
                throw new ("Invalid move");
            }

            Game game = model.ToDomain();
            if (game.Board[move.I, move.J] != CellStatus.Empty)
            {
                throw new ("Invalid move");
            }

            game.Board[move.I, move.J] = move.Player == model.Player1
                ? CellStatus.Cross
                : CellStatus.Nought;

            model.StatusString = game.ToString();
            model.NextPlayer = (model.NextPlayer + 1) % 2;
            model.Winner = (int)game.GetWinner();

            await _context.SaveChangesAsync(cancellationToken);

            return model;
        }
    }
}
