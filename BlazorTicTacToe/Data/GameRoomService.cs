namespace BlazorTicTacToe.Data
{
    public class GameRoomService : IGameRoomService
    {
        public event EventHandler<RoomIsReadyEventArgs>? RoomIsReady;

        private List<GameRoom> _gameRooms = new List<GameRoom>();

        private int _currentRoomId = 1;

        private readonly IGameFieldService _gameFieldService;
        private readonly IStatsService _statsService;

        public GameRoomService(IGameFieldService gameFieldService, IStatsService statsService)
        {
            _gameFieldService = gameFieldService;
            _statsService = statsService;
        }

        public int CreateRoom(string creatorName, PlayerSign creatorSign)
        {
            var gameRoom = new GameRoom(creatorName, _gameFieldService.CreateGameField())
            {
                Id = _currentRoomId++,
            };
            gameRoom.SetPlayerSign(creatorName, creatorSign);

            _gameRooms.Add(gameRoom);

            gameRoom.PlayerWin += PlayerWinEventHandler;

            return gameRoom.Id;
        }

        private void PlayerWinEventHandler(object? sender, PlayerWinEventArgs e)
        {
            _statsService.AddGameResult(e.WinnerName, e.LoserName);

            ((GameRoom)sender).PlayerWin -= PlayerWinEventHandler;
        }

        public void JoinRoom(int roomId, string userName, PlayerSign playerSign)
        {
            var roomToJoin = _gameRooms.FirstOrDefault(gameRoom => gameRoom.Id == roomId);
            if (roomToJoin != null)
            {
                roomToJoin.SetPlayerSign(userName, playerSign);

                RoomIsReady?.Invoke(this, new RoomIsReadyEventArgs
                {
                    RoomId = roomId,
                });
            }
        }

        public IEnumerable<GameRoom> GetActiveRooms() => new List<GameRoom>(_gameRooms.Where(gameRoom => !gameRoom.GameFinished));

        public GameRoom? GetGameRoomById(int roomId) => _gameRooms.FirstOrDefault(gameRoom => gameRoom.Id == roomId);

        public void CloseRoom(int roomId)
        {
            var closingRoom = _gameRooms.FirstOrDefault(gameRoom => gameRoom.Id == roomId);
            if (closingRoom != null)
            {
                closingRoom.GameFinished = true;
            }
        }
    }
}
