using D4_Ex2_RandomNumberGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Interfaces
{
    interface IModel
    {
        DataModelStateInfo DataModelState { get; }

        void SetNewPlayer(string playerName, decimal moneyAmount);
        // Wallet
        void AddMoney(decimal moneyAmount);
        // Bet
        void Bet(decimal moneyAmount);
        void IncreaseBet(decimal moneyAmount);
        void DecreaseBet(decimal moneyAmount);
        void ResetBet();
        // Bet numbers
        void AddBetNumber(int number);
        void RemoveBetNumber(int number);
        // Game
        DrawResult Spin();
        void ResetGame(bool fullReset);
        void CashOutEarnings();
    }
}
