using System.ComponentModel.DataAnnotations;
using Core.Domain;

namespace Core.Models
{
    public class GameModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Player1 { get; set; } = string.Empty;

        [Required]
        public string Player2 { get; set; } = string.Empty;

        [Required]
        public string StatusString { get; set; } = "         ";
        [Required]
        public int NextPlayer { get; set; } = 0;

        public int Winner { get; set; } = 0;

        public Game ToDomain()
        {
            return Game.FromString(StatusString);
        }
    }
}
