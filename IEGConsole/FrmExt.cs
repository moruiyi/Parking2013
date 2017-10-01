using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IEGConsole
{
    public partial class FrmExt : Form
    {
        public FrmExt()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)   
            {
                this.DialogResult = DialogResult.OK;
            }
            else if (comboBox1.SelectedIndex == 1) 
            {
                this.DialogResult = DialogResult.Yes;
            }         
            this.Close();
        }


    }
}
