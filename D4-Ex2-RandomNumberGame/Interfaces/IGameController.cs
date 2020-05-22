using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame.Interfaces
{
    public interface IGameController
    {
        // Player
        void SetNewPlayer(string playerName);
        // Wallet
        void AddMoney();
        // Bet
        void Bet(decimal moneyAmount);
        void IncreaseBet(decimal moneyAmount);
        void DecreaseBet(decimal moneyAmount);
        void ResetBet();
        // Bet numbers
        void AddBetNumber(int number);
        void RemoveBetNumber(int number);
        // Game
        void Spin();
        void Reset(bool fullReset);
        void CashOutEarnings();
        // Events
        event PerformDrawAminationRequestedHandler PerformDrawAnimationRequested;
        event UpdateHallOfFameRequestedHandler UpdateHallOfFameRequested;
        // App
        void CloseCircus();
    }
}
