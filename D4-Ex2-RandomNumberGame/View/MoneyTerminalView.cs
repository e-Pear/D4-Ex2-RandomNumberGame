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
    public partial class MoneyTerminalView : Form
    {
        private static decimal userValue = 0.00m;
        private bool isSeparatorActive;
        private MoneyTerminalView()
        {
            isSeparatorActive = false;
            InitializeComponent();
            this.label1.Text = "";
        }
        /// <summary>
        /// Fires up a modal window which allows user to enter his money ammount.
        /// </summary>
        /// <returns></returns>
        public static decimal GetMoneyAmmount()
        {
            Form moneyTerminal = new MoneyTerminalView();
            moneyTerminal.ShowDialog();
            return userValue;
        }
        // Helpers
        private void AppentTextToDisplay(string text)
        {
            string mantissa;
            if(isSeparatorActive)
            {
                mantissa = label1.Text.Split('.')[1];
                if (mantissa.Length < 2) this.label1.Text += text;
                else return;
            }
            else this.label1.Text += text;
        }
        // Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("2");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("3");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("4");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("5");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("6");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("7");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("8");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("9");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay("0");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AppentTextToDisplay(".");
            isSeparatorActive = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (isSeparatorActive && (label1.Text.Contains(".0")||label1.Text.Contains(".00"))) return;
            AppentTextToDisplay("00");
        }

        private void button_clearAll_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            isSeparatorActive = false;
        }

        private void button_undo_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(label1.Text))
            {
                // take out separator flag if removed character is separator
                if (label1.Text.Last() == '.')
                {
                    isSeparatorActive = false;
                }
                // remove last character from displayed string
                label1.Text = label1.Text.Remove(label1.Text.Length - 1);
            }
        }

        private void button_accept_Click(object sender, EventArgs e)
        {

            userValue = Convert.ToDecimal(label1.Text,CultureInfo.InvariantCulture);
            this.DialogResult = DialogResult.OK; // not needed here, but ...
            this.Close();
        }
    }
}
