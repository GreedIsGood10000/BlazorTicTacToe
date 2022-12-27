namespace BlazorTicTacToe.Data
{

    public class StatsService : IStatsService
    {
        private readonly List<StatLine> _gameResults = new List<StatLine>();

        public void AddGameResult(string winnerName, string loserName)
        {
            var winnerStat = _gameResults.FirstOrDefault(x => string.Equals(x.PlayerName, winnerName, StringComparison.InvariantCultureIgnoreCase));

            if (winnerStat != null)
            {
                winnerStat.WinCount++;
                winnerStat.GamesCount++;
            }
            else
            {
                winnerStat = new StatLine
                {
                    PlayerName = winnerName,
                    GamesCount = 1,
                    WinCount = 1,
                };
                _gameResults.Add(winnerStat);
            }

            var loserStat = _gameResults.FirstOrDefault(x => string.Equals(x.PlayerName, loserName, StringComparison.InvariantCultureIgnoreCase));

            if (loserStat != null)
            {
                loserStat.GamesCount++;
            }
            else
            {
                loserStat = new StatLine
                {
                    PlayerName = loserName,
                    GamesCount = 1,
                    WinCount = 0,
                };
                _gameResults.Add(loserStat);
            }
        }

        public IEnumerable<StatLine> GetStatistics(string nameFilter, string gameCountFilter, string winCountFilter)
        {
            IEnumerable<StatLine> result = _gameResults;
            if (!string.IsNullOrEmpty(nameFilter))
            {
                result = result.Where(stat => stat.PlayerName.Contains(nameFilter));
            }
            if (!string.IsNullOrEmpty(gameCountFilter))
            {
                result = result.Where(stat => stat.GamesCount.ToString() == gameCountFilter);
            }
            if (!string.IsNullOrEmpty(winCountFilter))
            {
                result = result.Where(stat => stat.WinCount.ToString() == winCountFilter);
            }

            return new List<StatLine>(result);
        }
    }
}
