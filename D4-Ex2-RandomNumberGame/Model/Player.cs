using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Model
{
    public class Player
    {
        private string _playerName;
        private decimal _playerScore;

        public string Name { get { return _playerName; } }
        public decimal Score { get { return _playerScore; } set { _playerScore = value; } }

        public Player (string name)
        {
            _playerName = name;
            _playerScore = 0;
        }

        public override string ToString()
        {
            return $"{_playerScore}\n{_playerName}"; 
        }
        public string ToFileString()
        {
            return $"{_playerName};{_playerScore}";
        }
    }
}
