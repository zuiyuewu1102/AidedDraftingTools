using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BF_CustomTools
{
    public partial class ReBlockNameForm : Form
    {
        public ReBlockNameForm()
        {
            InitializeComponent();
            textBoxNewBlockName.Text = PublicValue.newBlockName;
            textBoxOldBlockName.Text = PublicValue.oldBlockName;
        } 

        private void buttonOk_Click(object sender, EventArgs e)
        {
            PublicValue.newBlockName = textBoxNewBlockName.Text;
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBoxNewBlockName.Enabled = false;
            }
            else
            {
                textBoxNewBlockName.Enabled = true;
            }
        }
    }
}
