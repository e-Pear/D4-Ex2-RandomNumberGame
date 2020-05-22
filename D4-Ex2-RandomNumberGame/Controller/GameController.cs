using D4_Ex2_RandomNumberGame.Interfaces;
using D4_Ex2_RandomNumberGame.Model;
using D4_Ex2_RandomNumberGame.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D4_Ex2_RandomNumberGame.Controller
{
    class GameController : IGameController, IModelStatePublisher
    {
        // Fields
        private IModel model;
        private IGameView view;
        private FileHandler fileHandler;
        private int lostGames;
        // Event Handlers
        public event ModelStateChangedHandler ModelStateChanged;
        public event PerformDrawAminationRequestedHandler PerformDrawAnimationRequested;
        public event UpdateHallOfFameRequestedHandler UpdateHallOfFameRequested;
        // Constructor
        public GameController(IModel model, IGameView view)
        {
            this.model = model;
            this.view = view;
            this.fileHandler = new FileHandler();
            this.lostGames = 0;

            view.SetController(this);

            this.ModelStateChanged += view.UpdateModelView;
            this.PerformDrawAnimationRequested += view.PerformSpinResultsPresentation;
            this.UpdateHallOfFameRequested += view.UpdateHallOfFame;
            // view initial synchronization with model
            view.SetViewTo_PreGame_State();
            OnUpdateHallOfFameRequested();
        }
        // Event Raisers
        protected virtual void OnModelStateChanged()
        {
            ModelStateChanged?.Invoke(model.DataModelState);
        }
        protected virtual void OnPerformDrawAnimationRequested()
        {
            PerformDrawAnimationRequested?.Invoke(model.DataModelState);
        }
        protected virtual void OnUpdateHallOfFameRequested()
        {
            List<Player>[] hallOfFamers = GetHallOfFamers();
            UpdateHallOfFameRequested?.Invoke(hallOfFamers[0], hallOfFamers[1]);
        }
        // Interface Methods
        public void SetNewPlayer(string playerName)
        {
            decimal playerMoneyAmount = MoneyTerminalView.GetMoneyAmmount();
            if(playerMoneyAmount == 0)
            {
                this.Reset(true);
                return;
            }
            model.SetNewPlayer(playerName, playerMoneyAmount);
            view.SetViewTo_InGame_State();
            OnModelStateChanged();
        }

        public void AddMoney()
        {
            decimal playerMoneyAmount = MoneyTerminalView.GetMoneyAmmount();
            model.AddMoney(playerMoneyAmount);
            OnModelStateChanged();
            EvaluateIfPlayerHasMoneyToPlay(model.DataModelState);
        }

        public void Bet(decimal moneyAmount)
        {
            model.Bet(moneyAmount);
            OnModelStateChanged();
            EvaluateIfSpinActionCanBePerformed(model.DataModelState);
        }

        public void IncreaseBet(decimal moneyAmount)
        {
            model.IncreaseBet(moneyAmount);
            OnModelStateChanged();
            EvaluateIfSpinActionCanBePerformed(model.DataModelState);
        }

        public void DecreaseBet(decimal moneyAmount)
        {
            model.DecreaseBet(moneyAmount);
            OnModelStateChanged();
            EvaluateIfSpinActionCanBePerformed(model.DataModelState);
        }

        public void ResetBet()
        {
            model.ResetBet();
            OnModelStateChanged();
            EvaluateIfSpinActionCanBePerformed(model.DataModelState);
        }

        public void AddBetNumber(int number)
        {
            DataModelStateInfo modelData;
            model.AddBetNumber(number);
            OnModelStateChanged();
            modelData = model.DataModelState;
            EvaluateIfBetNumbersCanBeAdded(modelData);
            EvaluateIfSpinActionCanBePerformed(modelData);
        }

        public void RemoveBetNumber(int number)
        {
            model.RemoveBetNumber(number);
            OnModelStateChanged();
            EvaluateIfBetNumbersCanBeAdded(model.DataModelState);
        }

        public void Spin()
        {
            if (model.Spin() == DrawResult.Loss) this.lostGames++;
            else this.lostGames = 0;
            OnPerformDrawAnimationRequested();
        }

        public void Reset(bool full)
        {
            DataModelStateInfo modelStateBefore, modelStateAfter;

            if (this.lostGames == 3)
            {
                DialogResult result = MessageBox.Show("This was the third loss in a row.\nMaybe you should take a break.", "Little Suggestion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch (result)
                {
                    case DialogResult.Yes:
                        full = true;
                        break;

                    default:
                        this.lostGames = 0;
                        break;
                }
            }
            
            modelStateBefore = model.DataModelState;
            
            model.ResetGame(full);
            modelStateAfter = model.DataModelState;

            if(full)
            {
                fileHandler.LogToFile($"{modelStateBefore.PlayerName};{modelStateBefore.PlayerScore.GetValueOrDefault(0).ToUsString()}");
                view.SetViewTo_PreGame_State();
                view.SetViewTo_BetNumbersAreaUnLocked_SubState(true);
                OnUpdateHallOfFameRequested();
            }
            else
            {
                OnModelStateChanged();
                EvaluateIfPlayerHasMoneyToPlay(modelStateAfter);
                EvaluateIfSpinActionCanBePerformed(modelStateAfter);
                
            }
        }
        public void CloseCircus()
        {
            fileHandler.StopSession($"Session total earnings: {model.DataModelState.TotalEarnings.ToUsString()}");
        }
        // There is no in view reference for this sofar. For now it'll stay as not implemented.
        public void CashOutEarnings()
        {
            throw new NotImplementedException();
        }
        // Auxiliary Methods
        // -> checks:
        private void EvaluateIfPlayerHasMoneyToPlay(DataModelStateInfo modelInfo)
        {
            if (modelInfo.CurrentBet == 0 && modelInfo.PlayerMoneyAmount == 0) view.SwitchView_NoMoney_SubState(true);
            else view.SwitchView_NoMoney_SubState(false);
        }
        private void EvaluateIfSpinActionCanBePerformed(DataModelStateInfo modelInfo)
        {
            if (modelInfo.CurrentBet != 0 && modelInfo.ActualBetNumbers.Count == 3) view.SwitchView_ReadyToSpin_SubState(true);
            else view.SwitchView_ReadyToSpin_SubState(false);
        }
        private void EvaluateIfBetNumbersCanBeAdded(DataModelStateInfo modelInfo)
        {
            if (modelInfo.ActualBetNumbers.Count == 3) view.SetViewTo_BetNumbersAreaLocked_SubState(modelInfo.ActualBetNumbers);
            else view.SetViewTo_BetNumbersAreaUnLocked_SubState();
        }
        // -> other:
        private List<Player>[] GetHallOfFamers() // I like that what's beeing returned here :) - an array of lists :D cool...
        {
            List<Player> fullList = fileHandler.ReadAllPlayersFromLog();
            List<Player> bestOnes = fullList.OrderByDescending(p => p.Score).Where(p => p.Score > 0).Take(5).ToList();
            List<Player> worstOnes = fullList.OrderBy(p => p.Score).Where(p => p.Score < 0).Take(5).ToList();

            return new List<Player>[] { bestOnes, worstOnes };
        }

    }
}
