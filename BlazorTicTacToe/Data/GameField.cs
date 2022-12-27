namespace BlazorTicTacToe.Data
{

    public class GameField
    {
        private class WinLineInfo
        {
            public Tuple<int, int>[] WinPoints { get; set; }
        }

        private readonly FieldSign[,] _playerFieldValues = new FieldSign[10, 10];

        public bool PlayerWin => WinnerSign != null;

        public PlayerSign? WinnerSign { get; set; }

        public Tuple<int, int>[]? WinPoints { get; private set; }

        public FieldSign[,] GetGameField()
        {
            FieldSign[,] result = new FieldSign[10, 10];
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = _playerFieldValues[i, j];
                }
            }

            return result;
        }

        public void PutPlayerSign(int colId, int rowId, PlayerSign playerSign)
        {
            FieldSign fieldSign = GetFieldSign(playerSign);
            _playerFieldValues[colId, rowId] = fieldSign;

            WinLineInfo? winLine = TryGetWinLine(colId, rowId, fieldSign);

            if (winLine != null)
            {
                WinnerSign = playerSign;
                WinPoints = winLine.WinPoints;
            }
        }

        private WinLineInfo? TryGetWinLine(int colId, int rowId, FieldSign fieldSign) =>
            TryGetWinLineOfDirection(colId, rowId, 0, 1, fieldSign) ??
            TryGetWinLineOfDirection(colId, rowId, 1, 0, fieldSign) ??
            TryGetWinLineOfDirection(colId, rowId, 1, 1, fieldSign) ??
            TryGetWinLineOfDirection(colId, rowId, -1, 1, fieldSign);

        private FieldSign GetFieldSign(PlayerSign player)
        {
            switch (player)
            {
                case PlayerSign.Cross:
                    return FieldSign.Cross;
                case PlayerSign.Zero:
                    return FieldSign.Zero;
                default:
                    throw new NotSupportedException(player.ToString());
            }
        }

        private WinLineInfo? TryGetWinLineOfDirection(int colId, int rowId, int colDirection, int rowDirection, FieldSign fieldSign)
        {
            int lineLenght = 1;
            int upLineLenght = GetSignLineLenght(colId, rowId, colDirection, rowDirection, fieldSign);
            int downLineLenght = GetSignLineLenght(colId, rowId, -colDirection, -rowDirection, fieldSign);

            lineLenght += upLineLenght + downLineLenght;
            if (lineLenght < 5)
            {
                return null;
            }

            List<Tuple<int, int>> winPoints = new List<Tuple<int, int>>();
            winPoints.Add(new Tuple<int, int>(colId, rowId));

            int currentCol = colId;
            int currentRow = rowId;
            for (int i = 0; i < upLineLenght; i++)
            {
                currentCol += colDirection;
                currentRow += rowDirection;
                winPoints.Add(new Tuple<int, int>(currentCol, currentRow));
            }

            currentCol = colId;
            currentRow = rowId;
            for (int i = 0; i < downLineLenght; i++)
            {
                currentCol -= colDirection;
                currentRow -= rowDirection;
                winPoints.Add(new Tuple<int, int>(currentCol, currentRow));
            }

            return new WinLineInfo
            {
                WinPoints = winPoints.ToArray(),
            };
        }

        private int GetSignLineLenght(int colId, int rowId, int colDirection, int rowDirection, FieldSign fieldSign)
        {
            for (int counter = 0, currentCol = colId, currentRow = rowId; ; counter++)
            {
                currentCol += colDirection;
                currentRow += rowDirection;

                if (currentCol > 10 || currentCol < 0 || currentRow > 10 || currentRow < 0)
                {
                    return counter;
                }
                if (_playerFieldValues[currentCol, currentRow] != fieldSign)
                {
                    return counter;
                }
            }
        }
    }
}
