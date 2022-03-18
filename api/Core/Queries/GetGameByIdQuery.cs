using Core.DAL;
using Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Queries
{
    public class GetGameByIdQuery : IRequest<GameModel>
    {
        public int Id { get; }

        public GetGameByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetGameByIdHandler : IRequestHandler<GetGameByIdQuery, GameModel>
    {
        private readonly IGameContext _context;

        public GetGameByIdHandler(IGameContext context)
        {
            _context = context;
        }

        public Task<GameModel> Handle(GetGameByIdQuery request, CancellationToken cancellationToken)
        {
            return _context.Games.FirstOrDefaultAsync(game => game.Id == request.Id, cancellationToken);
        }
    }
}
