using D4_Ex2_RandomNumberGame.Controller;
using D4_Ex2_RandomNumberGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D4_Ex2_RandomNumberGame.Interfaces
{
    interface IGameView
    {
        // Base
        void UpdateModelView(DataModelStateInfo args);
        void UpdateHallOfFame(List<Player> bestOnes, List<Player> worstOnes);
        void PerformSpinResultsPresentation(DataModelStateInfo args); // yeah... this one will be playing a fancy animation :P
        void SetController(IGameController controller);
        // Main Program
        void Show();
        // View State Control
        void SetViewTo_PreGame_State();
        void SetViewTo_InGame_State();
        void SwitchView_NoMoney_SubState(bool switchIndicator);
        void SwitchView_ReadyToSpin_SubState(bool switchIndicator);
        void SetViewTo_BetNumbersAreaLocked_SubState(IEnumerable<int> exclusions);
        void SetViewTo_BetNumbersAreaUnLocked_SubState(bool alsoUncheckAll = false);
       
    }
}
