namespace Core.Domain
{
    public class Game
    {
        public static Game FromString(string boardString)
        {
            if (boardString.Length != 9)
            {
                throw new ArgumentException("Invalid string representation");

            }

            Game game = new Game();

            char[] chars = boardString.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                game.Board[i / 3, i % 3] = FromChar(chars[i]);
            }

            return game;
        }

        private static CellStatus FromChar(char repr)
        {
            switch (repr)
            {
                case ' ':
                    return CellStatus.Empty;
                case 'X':
                    return CellStatus.Cross;
                case 'O':
                    return CellStatus.Nought;
                default:
                    throw new ArgumentException($"Invalid cell representation {repr}");
            }
        }

        private static char ToChar(CellStatus cell)
        {
            switch (cell)
            {
                case CellStatus.Cross:
                    return 'X';
                case CellStatus.Nought:
                    return 'O';
                case CellStatus.Empty:
                default:
                    return ' ';
            }
        }

        public CellStatus[,] Board { get; }

        public Game()
        {
            Board = new CellStatus[3, 3];
        }

        public override string ToString()
        {
            return new string((
                from i in Enumerable.Range(0, 3)
                from j in Enumerable.Range(0, 3)
                select ToChar(Board[i, j])).ToArray());
        }

        public CellStatus GetWinner()
        {
            CellStatus winner = CellStatus.Empty;
            for (int i = 0; i < 3; i++)
            {
                if (CheckLine(Row(i), out winner))
                {
                    return winner;
                }
                if (CheckLine(Column(i), out winner))
                {
                    return winner;
                }
            }
            if (CheckLine(Diagonal(), out winner))
            {
                return winner;
            }
            if (CheckLine(AntiDiagonal(), out winner))
            {
                return winner;
            }
            return winner;

            IEnumerable<CellStatus> Row(int i)
                => Enumerable.Range(0, 3).Select(j => Board[i, j]);

            IEnumerable<CellStatus> Column(int j)
                => Enumerable.Range(0, 3).Select(i => Board[i, j]);

            IEnumerable<CellStatus> Diagonal()
                => Enumerable.Range(0, 3).Select(i => Board[i, i]);

            IEnumerable<CellStatus> AntiDiagonal()
                => Enumerable.Range(0, 3).Select(i => Board[i, 2 - i]);

            bool CheckLine(IEnumerable<CellStatus> line, out CellStatus winner)
            {
                CellStatus candidate = line.First();
                if (candidate != CellStatus.Empty && line.Skip(1).All(c => c == candidate))
                {
                    winner = candidate;
                    return true;
                }
                winner = CellStatus.Empty;
                return false;
            }
        }
    }
}
