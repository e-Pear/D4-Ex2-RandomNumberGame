using D4_Ex2_RandomNumberGame.Controller;
using D4_Ex2_RandomNumberGame.Interfaces;
using D4_Ex2_RandomNumberGame.Model;
using D4_Ex2_RandomNumberGame.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D4_Ex2_RandomNumberGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            IModel model = new GameModel();
            IGameView view = new GameView();
            IGameController controller = new GameController(model, view);
            
            Application.EnableVisualStyles();
            Application.Run((Form)view);
        }
    }
}
