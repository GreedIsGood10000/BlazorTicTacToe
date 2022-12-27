namespace BlazorTicTacToe.Data
{
    public interface IGameRoomService
    {
        event EventHandler<RoomIsReadyEventArgs> RoomIsReady;

        int CreateRoom(string creatorName, PlayerSign creatorSign);

        GameRoom? GetGameRoomById(int roomId);

        IEnumerable<GameRoom> GetActiveRooms();

        void CloseRoom(int roomId);

        void JoinRoom(int roomId, string userName, PlayerSign playerSign);
    }
}
