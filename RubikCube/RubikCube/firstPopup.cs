using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RubikCube
{
    public partial class FirstPopup : Form
    {
        readonly SwitchGameState gameState;

        public FirstPopup(SwitchGameState state)
        {
            gameState = state;
            InitializeComponent();
        }

        private void firstPopup_Load(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            gameState.SwitchToTutorial();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
