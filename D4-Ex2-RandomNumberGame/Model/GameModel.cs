using D4_Ex2_RandomNumberGame.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Model
{
    class GameModel : IModel
    {
        // non data model state variables
        private int maxNumberOfBetNumbers;
        // data model state variables:
        private Player _currentPlayer;
        private RandomGenerator _lotteryMachine;
        private Wallet _wallet;

        private decimal _currentBet;
        private List<int> _betNumbers;
        
        private List<int> _drawResults;
        private List<int> _matchings;

        private decimal _win;
        private decimal _earnings;

        public DataModelStateInfo DataModelState
        {
            get
            {
                string playerName;
                decimal? playerScore, playerMoneyAmount, currentBet;

                if (_currentPlayer == null)
                {
                    playerName = null;
                    playerScore = null;
                    playerMoneyAmount = null;
                    currentBet = null;
                }
                else
                {
                    playerName = _currentPlayer.Name;
                    playerScore = _currentPlayer.Score;
                    playerMoneyAmount = _wallet.MoneyAmount;
                    currentBet = _currentBet;
                }

                return new DataModelStateInfo(playerName, playerMoneyAmount, playerScore, currentBet, _betNumbers, _drawResults, _matchings, _win, _earnings);
            }
        }

        public GameModel()
        {
            maxNumberOfBetNumbers = 3;

            SetNewPlayer("", 0);
            _lotteryMachine = new RandomGenerator();
            _earnings = 0;
            _win = 0;
            _betNumbers = new List<int>();
            _drawResults = new List<int>();
            _matchings = new List<int>();
        }
        // Player
        public void SetNewPlayer(string playerName, decimal moneyAmount)
        {
            _currentPlayer = new Player(playerName);
            _wallet = new Wallet(moneyAmount);
            _currentBet = 0;
        }
        // Wallet
        public void AddMoney(decimal moneyAmount)
        {
            _wallet.AddMoneyAmount(moneyAmount);
        }
        // Bet
        public void Bet(decimal moneyAmount)
        {
            if(_wallet.MoneyAmount < moneyAmount)
            {
                _currentBet = 0;
            }
            else
            {
                _wallet.TakeMoneyAmount(moneyAmount);
                _currentBet = moneyAmount;
            }
        }
        public void IncreaseBet(decimal moneyAmount)
        {
            if (_wallet.MoneyAmount >= moneyAmount)
            {
                _wallet.TakeMoneyAmount(moneyAmount);
                _currentBet += moneyAmount;
            }
        }
        public void DecreaseBet(decimal moneyAmount)
        {
            if(_currentBet >= moneyAmount)
            {
                _currentBet = -moneyAmount;
                _wallet.AddMoneyAmount(moneyAmount);
            }
        }
        public void ResetBet()
        {
            _wallet.AddMoneyAmount(_currentBet);
            _currentBet = 0;
        }
        // Bet numbers
        public void AddBetNumber(int number)
        {
            if(_betNumbers.Count < maxNumberOfBetNumbers) _betNumbers.Add(number);
        }
        public void RemoveBetNumber(int number)
        {
            if (_betNumbers.Contains(number)) _betNumbers.Remove(number);
        }
        // Game
        public DrawResult Spin()
        {
            _drawResults = _lotteryMachine.DrawNumbersWithoutRepetition(6, 1, 25);
            // matching
            _matchings.Clear();
            foreach(int result in _drawResults)
            {
                if (_betNumbers.Contains(result)) _matchings.Add(result);
            }
            // evaluating result
            if(_matchings.Count >= 3)
            {
                _win = _currentBet * 2;

                _wallet.AddMoneyAmount(_win);
                _earnings -= _currentBet;
                _currentPlayer.Score += _currentBet;
                _currentBet = 0;
                return DrawResult.TotalWin;
            }
            else if (_matchings.Count == 2)
            {
                _win = Math.Round((_currentBet * 0.3m), 2);

                _wallet.AddMoneyAmount(_currentBet + _win);
                _earnings -= _win;
                _currentPlayer.Score += _win;
                _currentBet = 0;
                return DrawResult.PartialWin;
            }
            else
            {
                _win = 0;
                _earnings += _currentBet;
                _currentPlayer.Score -= _currentBet;
                _currentBet = 0;
                return DrawResult.Loss;
            }
        }
        public void ResetGame(bool totalReset)
        {
            if (totalReset)
            {
                _currentPlayer.Score = _earnings * -1; // no score board mechanics yet :)
                _currentPlayer = null;
                _betNumbers.Clear();
                _win = 0;
                
            }
            _drawResults.Clear();
            _matchings.Clear();
        }
        public void CashOutEarnings()
        {
            _earnings = 0;
        }
        // Model very own methods

    }
}
