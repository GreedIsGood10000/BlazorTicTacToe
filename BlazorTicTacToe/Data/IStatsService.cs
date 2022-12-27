namespace BlazorTicTacToe.Data
{
    public interface IStatsService
    {
        void AddGameResult(string winnerName, string loserName);
        IEnumerable<StatLine> GetStatistics(string nameFilter, string gameCountFilter, string winCountFilter);
    }
}