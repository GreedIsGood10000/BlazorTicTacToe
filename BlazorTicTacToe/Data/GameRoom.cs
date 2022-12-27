namespace BlazorTicTacToe.Data
{
    public class GameRoom
    {
        public event EventHandler<PlayerSignAddedEventArgs>? PlayerSignAdded;

        public event EventHandler<PlayerWinEventArgs>? PlayerWin;

        public int Id { get; set; }

        public string CreatorName { get; set; }

        public PlayerSign CreatorSign => _signByPlayer[CreatorName];

        public bool GameFinished { get; set; }

        private readonly Dictionary<string, PlayerSign> _signByPlayer = new Dictionary<string, PlayerSign>();

        private GameField? _gameField;

        public PlayerSign? CurrentSignTurn { get; private set; } = PlayerSign.Cross;

        public GameRoom(string creatorName, GameField gameField)
        {
            CreatorName = creatorName;
            _gameField = gameField;
        }

        public void PutSignOnField(int colId, int rowId, string playerName)
        {
            PlayerSign playerSign = _signByPlayer[playerName];

            if (playerSign != CurrentSignTurn)
            {
                return;
            }

            _gameField?.PutPlayerSign(colId, rowId, playerSign);

            if (CurrentSignTurn == PlayerSign.Cross)
            {
                CurrentSignTurn = PlayerSign.Zero;
            }
            else
            {
                CurrentSignTurn = PlayerSign.Cross;
            }

            PlayerSignAdded?.Invoke(this, new PlayerSignAddedEventArgs
            {
                ColId = colId,
                RowId = rowId,
                PlayerSign = playerSign,
            });

            if (_gameField?.PlayerWin ?? false)
            {
                CurrentSignTurn = null;

                PlayerWin?.Invoke(this, new PlayerWinEventArgs
                {
                    WinPoints = _gameField.WinPoints,
                    WinnerSign = _gameField.WinnerSign ?? PlayerSign.Cross,
                    WinnerName = playerName,
                    LoserName = _signByPlayer.Keys.First(name => name != playerName),
                });
            }
        }

        public FieldSign[,]? GetGameScreen() => _gameField?.GetGameField();

        public void SetPlayerSign(string playerName, PlayerSign playerSign)
        {
            _signByPlayer[playerName] = playerSign;
        }
    }
}
