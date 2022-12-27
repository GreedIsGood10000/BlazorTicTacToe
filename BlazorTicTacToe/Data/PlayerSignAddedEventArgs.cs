namespace BlazorTicTacToe.Data
{
    public class PlayerSignAddedEventArgs : EventArgs
    {
        public int ColId { get; set; }

        public int RowId { get; set; }

        public PlayerSign PlayerSign { get; set; }
    }
}
