namespace Core.Dtos
{
    public class MoveDto
    {
        public string Player { get; set; }
        public int I { get; set; }

        public int J { get; set; }

        public bool Validate(params string[] players)
        {
            return I >= 0
                   && I < 3
                   && J >= 0
                   && J < 3
                   && players.Contains(Player);
        }
    }
}
