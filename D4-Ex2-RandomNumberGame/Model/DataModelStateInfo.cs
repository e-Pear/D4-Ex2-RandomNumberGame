using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Model
{
    // This one holds all data obout model that are needed to upgrade view;
    public class DataModelStateInfo
    {
        public string PlayerName { get; }
        public decimal? PlayerMoneyAmount { get; }
        public decimal? PlayerScore { get; }
        public decimal? CurrentBet { get; }
        public List<int> ActualBetNumbers { get; }
        public List<int> DrawResults { get; }
        public List<int> Matchings { get; }
        public decimal CurrentWin { get; }
        public decimal TotalEarnings { get; }

        public DataModelStateInfo(string playerName, decimal? playerMoneyAmount, decimal? playerScore, decimal? currentBet, List<int> actualBetNumbers, List<int> drawResults, List<int> matchings, decimal currentWin, decimal earnings)
        {
            PlayerName = playerName;
            PlayerMoneyAmount = playerMoneyAmount;
            PlayerScore = playerScore;
            CurrentBet = currentBet;
            ActualBetNumbers = actualBetNumbers;
            DrawResults = drawResults;
            Matchings = matchings;
            CurrentWin = currentWin;
            TotalEarnings = earnings;
        }
    }
}
