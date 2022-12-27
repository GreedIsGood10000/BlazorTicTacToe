namespace BlazorTicTacToe.Data
{
    public class GameFieldService : IGameFieldService
    {
        public GameField CreateGameField()
        {
            return new GameField();
        }
    }
}
