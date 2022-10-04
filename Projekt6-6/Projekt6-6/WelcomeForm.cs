using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Projekt6_6
{
    public partial class WelcomeForm : Form
    {
        public WelcomeForm()
        {
            InitializeComponent();
        }

        public Boolean zacina_hrac;
        public int pocet_karet;

        public void buttonExit_Click(object sender, EventArgs e)
        {
            if (radioButtonZacinaHrac.Checked)
            {
                zacina_hrac = true;
                pocet_karet = (int)numericUpDown1.Value;
                this.Close();

            }
            else if (radioButtonZacinaPocitac.Checked)
            {
                zacina_hrac = false;
                pocet_karet = (int)numericUpDown1.Value;
                this.Close();
            }


        }

        private void WelcomeForm_Load(object sender, EventArgs e)
        {

        }
    }
}
