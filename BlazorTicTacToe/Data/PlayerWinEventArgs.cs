namespace BlazorTicTacToe.Data
{
    public class PlayerWinEventArgs : EventArgs
    {
        public PlayerSign WinnerSign { get; set; }

        public string WinnerName { get; set; }

        public string LoserName { get; set; }

        public Tuple<int, int>[]? WinPoints { get; set; }
    }
}
