using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAL
{
    public interface IGameContext
    {
        DbSet<GameModel> Games { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void MarkAsModified(GameModel game);
    }
}
