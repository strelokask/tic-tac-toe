using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.DAL
{
    public class GameContext : DbContext, IGameContext
    {
        public GameContext(DbContextOptions<GameContext> options)
            : base(options)
        {
        }

        public DbSet<GameModel> Games { get; set; }

        public void MarkAsModified(GameModel game)
        {
            Entry(game).State = EntityState.Modified;
        }
    }
}
