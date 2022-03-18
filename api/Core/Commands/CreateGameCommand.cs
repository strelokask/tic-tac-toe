using Core.DAL;
using Core.Models;
using MediatR;

namespace Core.Commands
{
    public class CreateGameCommand : IRequest<GameModel>
    {
        public GameModel Model { get; }

        public CreateGameCommand(GameModel model)
        {
            Model = model;
        }
    }

    public class CreateGameHandler : IRequestHandler<CreateGameCommand, GameModel>
    {
        private readonly IGameContext _context;

        public CreateGameHandler(IGameContext context)
        {
            _context = context;
        }
        public async Task<GameModel> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            await _context.Games.AddAsync(request.Model, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return request.Model;
        }
    }
}
