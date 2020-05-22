using D4_Ex2_RandomNumberGame.Controller;
using D4_Ex2_RandomNumberGame.Interfaces;
using D4_Ex2_RandomNumberGame.Model;
using D4_Ex2_RandomNumberGame.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D4_Ex2_RandomNumberGame.View
{
    public partial class GameView : Form, IGameView
    {
        private IGameController controller;
        // Demo animation helper
        Random colorRandomizer;
        // Draw animation buffer data part:
        int counter;
        bool[] drumsSpinningMap;
        private int[] drawnNumbers;
        private int[] matchings;
        private IEnumerable<int> betNumbers;
        private IEnumerator<int>[] drumsEnumerators;
        private IEnumerable<int>[] drumsRanges;
        private decimal currentWin;
        private decimal score;
        // Animation control collections little helpers
        TextBox[] drums;
        TextBox[] indicators;
        // Ui helpers
        CheckBox[] betNumbersButtons;
        public GameView()
        {
            InitializeComponent();

            colorRandomizer = new Random();

            drums = new TextBox[]   { 
                                        textBox_DrawnNumber1, 
                                        textBox_DrawnNumber2, 
                                        textBox_DrawnNumber3, 
                                        textBox_DrawnNumber4, 
                                        textBox_DrawnNumber5, 
                                        textBox_DrawnNumber6 
                                    };
            
            indicators = new TextBox[]  { 
                                            textBox_Indicator1, 
                                            textBox_Indicator2, 
                                            textBox_Indicator3, 
                                            textBox_Indicator4, 
                                            textBox_Indicator5, 
                                            textBox_Indicator6 
                                        };
            betNumbersButtons = new CheckBox[]  {
                                                    button_BetNumber1, button_BetNumber2, button_BetNumber3, button_BetNumber4, button_BetNumber5,
                                                    button_BetNumber6, button_BetNumber7, button_BetNumber8, button_BetNumber9, button_BetNumber10,
                                                    button_BetNumber11, button_BetNumber12, button_BetNumber13, button_BetNumber14, button_BetNumber15,
                                                    button_BetNumber16, button_BetNumber17, button_BetNumber18, button_BetNumber19, button_BetNumber20,
                                                    button_BetNumber21, button_BetNumber22, button_BetNumber23, button_BetNumber24, button_BetNumber25
                                                };

            drumsEnumerators = new IEnumerator<int>[6];
            drumsRanges = new IEnumerable<int>[6];
        }
        // IView interface implementation:
        public void SetController(IGameController controller)
        {
            this.controller = controller;
        }
        public void UpdateModelView(DataModelStateInfo args)
        {
            label_PlayerNameLabel.Text = args.PlayerName;
            ThrowOnScreen(args.ActualBetNumbers.ToArray());
            textBox_WalletFunds.Text = args.PlayerMoneyAmount.GetValueOrDefault(0m).ToUsString();
            textBox_BetBox.Text = args.CurrentBet.GetValueOrDefault(0m).ToUsString();
            // scroll bar 1 (characteristic) update
            trackBar_BetBar_Characteristic.Maximum = args.PlayerMoneyAmount.GetValueOrDefault(0.0m).SplitToCharacteristicAndMantissa()[0];
            trackBar_BetBar_Characteristic.Minimum = args.CurrentBet.GetValueOrDefault(0.0m).SplitToCharacteristicAndMantissa()[0] * -1;
            trackBar_BetBar_Characteristic.Value = 0;
            // scroll bar 2 (mantissa) update
            trackBar_BetBar_Mantissa.Maximum = args.PlayerMoneyAmount.GetValueOrDefault(0.0m).SplitToCharacteristicAndMantissa()[1];
            trackBar_BetBar_Mantissa.Minimum = args.CurrentBet.GetValueOrDefault(0.0m).SplitToCharacteristicAndMantissa()[1] * -1;
            trackBar_BetBar_Mantissa.Value = 0;
            label_BetAmount.Text = "0";
        }
        public void UpdateHallOfFame(List<Player> bestOnes, List<Player> worstOnes)
        {
            List<string> collection = new List<string>() { "Best Players:",""};
            foreach (var item in bestOnes)
            {
                collection.Add($"{item.Name}");
                collection.Add($"{item.Score.ToUsString()}");
            }
            collection.Add("");
            collection.Add("Worst Players:");
            collection.Add("");
            foreach (var item in worstOnes)
            {
                collection.Add($"{item.Name}");
                collection.Add($"{item.Score.ToUsString()}");
            }
            
            listBox_HallOfFame.DataSource = collection;
        }
        public void PerformSpinResultsPresentation(DataModelStateInfo args)
        {
            Random byRandom = new Random();
            
            this.betNumbers = args.ActualBetNumbers.ToArray();
            this.drawnNumbers = args.DrawResults.ToArray();
            this.matchings = args.Matchings.ToArray();
            this.currentWin = args.CurrentWin;
            this.score = args.PlayerScore.GetValueOrDefault(0);

            this.counter = 0;
            this.drumsSpinningMap = new bool[] { true, true, true, true, true, true };

            ThrowOnScreen(betNumbers, "Draw in process ...", true);

            for (int j = 0; j < 6; j++)
            {
                if (j == 0)
                {
                    drumsRanges[0] = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
                }
                else
                {
                    drumsRanges[j] = new List<int>(drumsRanges[j - 1].Except(new List<int>() { drawnNumbers[j - 1] }));
                }

                drumsEnumerators[j] = drumsRanges[j].GetEnumerator();
                drumsEnumerators[j].MoveNext();
            }

            //  scrambling of drums starting points a little bit

            foreach (IEnumerator<int> enumerator in drumsEnumerators)
            {
                for (int i = 0; i < byRandom.Next(0,26); i++)
                {
                    if(!enumerator.MoveNext())
                    {
                        enumerator.Reset();
                        enumerator.MoveNext();
                    }
                }
            }

            groupBox_BetArea.Enabled = false;
            groupBox_BetNumbers.Enabled = false;
            groupBox_WalletArea.Enabled = false;

            button_Resign.Enabled = false;
            button_Spin.Enabled = false;
            timer_drawNumber.Interval = 1;
            timer_drawNumber.Start();
        }
        public void SetViewTo_PreGame_State()
        {
            textBox_PlayerNameBox.Visible = true;
            button_StartAsNewPlayer.Visible = true;
            label_PlayerNameLabel.Visible = false;
            
            label_PlayerNameLabel.Text = "";
            textBox_PlayerNameBox.Text = "";

            groupBox_BetArea.Enabled = false;
            textBox_BetBox.Text = "";
            groupBox_WalletArea.Enabled = false;
            textBox_WalletFunds.Text = "";
            groupBox_BetNumbers.Enabled = false;
            
            button_Spin.Enabled = false;
            button_Resign.Enabled = false;

            ClearScreen();
            ThrowOnScreen(new int[0],"Set your name to play!");
            timer_animation.Start();
        }
        public void SetViewTo_InGame_State()
        {
            textBox_PlayerNameBox.Visible = false;
            button_StartAsNewPlayer.Visible = false;
            label_PlayerNameLabel.Visible = true;

            textBox_PlayerNameBox.Text = "";

            groupBox_BetArea.Enabled = true;
            groupBox_WalletArea.Enabled = true;
            groupBox_BetNumbers.Enabled = true;

            button_Resign.Enabled = true;
            timer_animation.Stop();
            ClearScreen();
        }
        public void SwitchView_NoMoney_SubState(bool switchIndicator)
        {
            groupBox_BetNumbers.Enabled = !switchIndicator;
            groupBox_BetArea.Enabled = !switchIndicator;
            groupBox_BetArea.Enabled = !switchIndicator;

            if (switchIndicator)
            {
                ThrowOnScreen("No money!\nPlease add more money.");
            }
        }
        public void SwitchView_ReadyToSpin_SubState(bool switchIndicator)
        {
            button_Spin.Enabled = switchIndicator;
        }
        public void SetViewTo_BetNumbersAreaLocked_SubState(IEnumerable<int> exclusions)
        {
            for (int i = 1; i <= 25; i++)
            {
                if (!exclusions.Contains(i)) betNumbersButtons[i - 1].Enabled = false;
            }

        }
        public void SetViewTo_BetNumbersAreaUnLocked_SubState(bool alsoUncheckAll)
        {
            for (int i = 1; i <= 25; i++)
            {
                betNumbersButtons[i - 1].Enabled = true;
                if (alsoUncheckAll) betNumbersButtons[i - 1].Checked = false;
            }
        }
        // Aux methods
        private void ClearScreen()
        {
            // Reseting drawn number boxes
            textBox_DrawnNumber1.Text = "";
            textBox_DrawnNumber2.Text = "";
            textBox_DrawnNumber3.Text = "";
            textBox_DrawnNumber4.Text = "";
            textBox_DrawnNumber5.Text = "";
            textBox_DrawnNumber6.Text = "";
            // Resteing matchingboxes
            textBox_Indicator1.BackColor = Color.DeepSkyBlue;
            textBox_Indicator2.BackColor = Color.DeepSkyBlue;
            textBox_Indicator3.BackColor = Color.DeepSkyBlue;
            textBox_Indicator4.BackColor = Color.DeepSkyBlue;
            textBox_Indicator5.BackColor = Color.DeepSkyBlue;
            textBox_Indicator6.BackColor = Color.DeepSkyBlue;
        }
        private void ThrowOnScreen(string messageForUser)
        {
            label_MainDisplay.Text = messageForUser;
        }
        private void ThrowOnScreen(IEnumerable<int> betNumbers, string messageForUser = "", bool numbersInBrackets = false)
        {
            // Populating main display content
            int counter = 0;
            if (messageForUser == "") label_MainDisplay.Text = "Bet your numbers:\n";
            else label_MainDisplay.Text = messageForUser + "\n";
            foreach  (int betNumber in betNumbers)
            {
                counter++;
                if(numbersInBrackets) label_MainDisplay.Text += $"[{betNumber}]";
                else label_MainDisplay.Text += betNumber;
                if(counter != betNumbers.Count()) label_MainDisplay.Text += " - ";
            }
        }
        private void UpdateBetLabel(int characteristic, int mantissa)
        {
            decimal value1 = (decimal)characteristic;
            decimal value2 = (decimal)(mantissa / 100m);

            label_BetAmount.Text = (value1 + value2).ToUsString();
        }
        // Internal Event callbacks
        private void OnDrawAnimationEnd()
        {
            if(this.currentWin > 0)
            {
                ThrowOnScreen($"You won {this.currentWin}!\nScore: {this.score}");
            }
            else ThrowOnScreen($"You lost your bet.\nScore: {this.score}");

            button_Spin.Visible = false;
            button_Continue.Visible = true;
        }
        // Independent Animations
        private void PlayOpening(bool onOFF)
        {
            if (onOFF) timer_animation.Start();
            else timer_animation.Stop();
        }
        // Control Events
        // -> new player
        private void button_StartAsNewPlayer_Click(object sender, EventArgs e)
        {
            controller.SetNewPlayer(textBox_PlayerNameBox.Text);
        }
        private void textBox_PlayerNameBox_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox_PlayerNameBox.Text)) button_StartAsNewPlayer.Enabled = false;
            else button_StartAsNewPlayer.Enabled = true;
        }
        // -> bets
        private void trackBar_BetBar_Scroll(object sender, EventArgs e)
        {
            UpdateBetLabel(trackBar_BetBar_Characteristic.Value, trackBar_BetBar_Mantissa.Value);
        }

        private void trackBar_BetBar_Mantissa_Scroll(object sender, EventArgs e)
        {
            UpdateBetLabel(trackBar_BetBar_Characteristic.Value, trackBar_BetBar_Mantissa.Value);
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            controller.IncreaseBet(Convert.ToDecimal(label_BetAmount.Text,CultureInfo.InvariantCulture));
        }

        private void button_ResetBet_Click(object sender, EventArgs e)
        {
            controller.ResetBet();
        }
        // -> wallet
        private void button_AddFundsToWallet_Click(object sender, EventArgs e)
        {
            controller.AddMoney();
        }
        // -> bet numbers
        private void button_BetNumber1_Click(object sender, EventArgs e)
        {
            if (button_BetNumber1.Checked) controller.AddBetNumber(1);
            else controller.RemoveBetNumber(1);
        }
        private void button_BetNumber2_Click(object sender, EventArgs e)
        {
            if (button_BetNumber2.Checked) controller.AddBetNumber(2);
            else controller.RemoveBetNumber(2);
        }
        private void button_BetNumber3_Click(object sender, EventArgs e)
        {
            if (button_BetNumber3.Checked) controller.AddBetNumber(3);
            else controller.RemoveBetNumber(3);
        }
        private void button_BetNumber4_Click(object sender, EventArgs e)
        {
            if (button_BetNumber4.Checked) controller.AddBetNumber(4);
            else controller.RemoveBetNumber(4);
        }
        private void button_BetNumber5_Click(object sender, EventArgs e)
        {
            if (button_BetNumber5.Checked) controller.AddBetNumber(5);
            else controller.RemoveBetNumber(5);
        }
        private void button_BetNumber6_Click(object sender, EventArgs e)
        {
            if (button_BetNumber6.Checked) controller.AddBetNumber(6);
            else controller.RemoveBetNumber(6);
        }
        private void button_BetNumber7_Click(object sender, EventArgs e)
        {
            if (button_BetNumber7.Checked) controller.AddBetNumber(7);
            else controller.RemoveBetNumber(7);
        }
        private void button_BetNumber8_Click(object sender, EventArgs e)
        {
            if (button_BetNumber8.Checked) controller.AddBetNumber(8);
            else controller.RemoveBetNumber(8);
        }
        private void button_BetNumber9_Click(object sender, EventArgs e)
        {
            if (button_BetNumber9.Checked) controller.AddBetNumber(9);
            else controller.RemoveBetNumber(9);
        }
        private void button_BetNumber10_Click(object sender, EventArgs e)
        {
            if (button_BetNumber10.Checked) controller.AddBetNumber(10);
            else controller.RemoveBetNumber(10);
        }
        private void button_BetNumber11_Click(object sender, EventArgs e)
        {
            if (button_BetNumber11.Checked) controller.AddBetNumber(11);
            else controller.RemoveBetNumber(11);
        }
        private void button_BetNumber12_Click(object sender, EventArgs e)
        {
            if (button_BetNumber12.Checked) controller.AddBetNumber(12);
            else controller.RemoveBetNumber(12);
        }
        private void button_BetNumber13_Click(object sender, EventArgs e)
        {
            if (button_BetNumber13.Checked) controller.AddBetNumber(13);
            else controller.RemoveBetNumber(13);
        }
        private void button_BetNumber14_Click(object sender, EventArgs e)
        {
            if (button_BetNumber14.Checked) controller.AddBetNumber(14);
            else controller.RemoveBetNumber(14);
        }
        private void button_BetNumber15_Click(object sender, EventArgs e)
        {
            if (button_BetNumber15.Checked) controller.AddBetNumber(15);
            else controller.RemoveBetNumber(15);
        }
        private void button_BetNumber16_Click(object sender, EventArgs e)
        {
            if (button_BetNumber16.Checked) controller.AddBetNumber(16);
            else controller.RemoveBetNumber(16);
        }
        private void button_BetNumber17_Click(object sender, EventArgs e)
        {
            if (button_BetNumber17.Checked) controller.AddBetNumber(17);
            else controller.RemoveBetNumber(17);
        }
        private void button_BetNumber18_Click(object sender, EventArgs e)
        {
            if (button_BetNumber18.Checked) controller.AddBetNumber(18);
            else controller.RemoveBetNumber(18);
        }
        private void button_BetNumber19_Click(object sender, EventArgs e)
        {
            if (button_BetNumber19.Checked) controller.AddBetNumber(19);
            else controller.RemoveBetNumber(19);
        }
        private void button_BetNumber20_Click(object sender, EventArgs e)
        {
            if (button_BetNumber20.Checked) controller.AddBetNumber(20);
            else controller.RemoveBetNumber(20);
        }
        private void button_BetNumber21_Click(object sender, EventArgs e)
        {
            if (button_BetNumber21.Checked) controller.AddBetNumber(21);
            else controller.RemoveBetNumber(21);
        }
        private void button_BetNumber22_Click(object sender, EventArgs e)
        {
            if (button_BetNumber22.Checked) controller.AddBetNumber(22);
            else controller.RemoveBetNumber(22);
        }
        private void button_BetNumber23_Click(object sender, EventArgs e)
        {
            if (button_BetNumber23.Checked) controller.AddBetNumber(23);
            else controller.RemoveBetNumber(23);
        }
        private void button_BetNumber24_Click(object sender, EventArgs e)
        {
            if (button_BetNumber24.Checked) controller.AddBetNumber(24);
            else controller.RemoveBetNumber(24);
        }
        private void button_BetNumber25_Click(object sender, EventArgs e)
        {
            if (button_BetNumber25.Checked) controller.AddBetNumber(25);
            else controller.RemoveBetNumber(25);
        }
        // -> main buttons
        private void button_Resign_Click(object sender, EventArgs e)
        {
            controller.Reset(true);
        }

        private void button_InfoButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.GameRules, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button_Spin_Click(object sender, EventArgs e)
        {
            controller.Spin();
        }
        private void button_Continue_Click(object sender, EventArgs e)
        {
            groupBox_BetArea.Enabled = true;
            groupBox_BetNumbers.Enabled = true;
            groupBox_WalletArea.Enabled = true;

            button_Continue.Visible = false;
            button_Resign.Enabled = true;
            button_Spin.Visible = true;

            ClearScreen();
            controller.Reset(false);
        }
        // Timers (Animations)
        private void timer_drawNumber_Tick(object sender, EventArgs e)
        {
            int timerStoppingCondition = 0;
            
            counter++;
            if (counter % 10 == 0) 
            {
                // iterate through all drums
                for (int i = 0; i < 6; i++)
                {
                    // if drum's current value is equal to drawn number, and drum has been marked to stop then it's stop
                    if (!drumsSpinningMap[i] && drumsEnumerators[i].Current == drawnNumbers[i])
                    {
                        drums[i].Text = drumsEnumerators[i].Current.ToString();
                        timerStoppingCondition++;
                        if (matchings.Contains(drumsEnumerators[i].Current))
                        {
                            indicators[i].Text = "";
                            indicators[i].BackColor = Color.Green;
                        }
                        else
                        {
                            indicators[i].Text = "";
                            indicators[i].BackColor = Color.Red;
                        }
                    }
                    // if drum has been not marked to stop or has but not reached it's treshold value yet then it's spinning further
                    else if (drumsSpinningMap[i] || drumsEnumerators[i].Current != drawnNumbers[i])
                    {
                        if (counter % 20 == 0) indicators[i].Text = "-_--_-";
                        else indicators[i].Text = "--_--";
                        
                        indicators[i].BackColor = Color.Yellow;
                        // if last element in range, jump to first one
                        if (!drumsEnumerators[i].MoveNext())
                        {
                            drumsEnumerators[i].Reset();
                            drumsEnumerators[i].MoveNext();
                        }
                        drums[i].Text = drumsEnumerators[i].Current.ToString();
                    }
                }
            }

            if (counter >= 300 && counter % 100 == 0) // <- for about 3s every drum is spinning, then they starting to stop every second.
            {
                // this loop marks next spinning drum to stop
                for (int p = 0; p < 6; p++)
                {
                    if(drumsSpinningMap[p])
                    {
                        drumsSpinningMap[p] = false;
                        break;
                    }
                }
            }

            if (timerStoppingCondition == 6)
            {
                timer_drawNumber.Stop();
                OnDrawAnimationEnd();
            }
        }

        private void timer_animation_Tick(object sender, EventArgs e)
        {
            int r, g, b;
            
            foreach (TextBox indicator in indicators)
            {
                r = colorRandomizer.Next(1, 256);
                g = colorRandomizer.Next(1, 256);
                b = colorRandomizer.Next(1, 256);
                
                indicator.BackColor = Color.FromArgb(r,g,b);
            }
        }

        private void GameView_FormClosing(object sender, FormClosingEventArgs e)
        {
            controller.CloseCircus();
        }
    }
}
