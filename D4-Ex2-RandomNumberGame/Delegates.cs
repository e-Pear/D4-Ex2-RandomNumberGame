using D4_Ex2_RandomNumberGame.Controller;
using D4_Ex2_RandomNumberGame.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D4_Ex2_RandomNumberGame
{
    public delegate void ModelStateChangedHandler(DataModelStateInfo args);
    public delegate void PerformDrawAminationRequestedHandler(DataModelStateInfo args);
    public delegate void UpdateHallOfFameRequestedHandler(List<Player> bestOnes, List<Player> worstOnes);
}
