using Core;
using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public interface IGameContext
    {
        DbSet<GameModel> Games { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void MarkAsModified(GameModel game);
    }
}
