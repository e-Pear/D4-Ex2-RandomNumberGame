using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Model
{
    class Wallet
    {
        private decimal _moneyAmount;
        public decimal MoneyAmount { get { return _moneyAmount; } }

        public Wallet(decimal moneyAmount = 0)
        {
            _moneyAmount = moneyAmount;
        }

        public void AddMoneyAmount(decimal moneyAmount)
        {
            _moneyAmount += moneyAmount;
        }
        public void TakeMoneyAmount(decimal moneyAmount) // No validation here. It will all be a controller's concern. 
        {
            _moneyAmount -= moneyAmount;
        }

    }
}
