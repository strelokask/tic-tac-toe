using Core.DAL;
using Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Core.Queries
{
    public class GetGamesQuery : IRequest<GameModel[]>
    {
    }

    public class GetGamesHandler : IRequestHandler<GetGamesQuery, GameModel[]>
    {
        private readonly IGameContext _context;

        public GetGamesHandler(IGameContext context)
        {
            _context = context;
        }

        public Task<GameModel[]> Handle(GetGamesQuery request, CancellationToken cancellationToken)
        {
            return _context.Games.ToArrayAsync(cancellationToken);
        }
    }
}
