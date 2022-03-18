using Core.Domain;

namespace Core.Models
{
    public class GameModel
    {
        public long Id { get; set; }

        public string Player1 { get; set; } = string.Empty;

        public string Player2 { get; set; } = string.Empty;

        public string StatusString { get; set; } = "         ";

        public int NextPlayer { get; set; } = 0;

        public int Winner { get; set; } = 0;

        public Game ToDomain()
        {
            return Game.FromString(StatusString);
        }
    }
}
