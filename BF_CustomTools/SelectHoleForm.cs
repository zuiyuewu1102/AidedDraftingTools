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
    public partial class SelectHoleForm : Form
    {
        public SelectHoleForm()
        {
            InitializeComponent();
            PublicValue.blkname = "DK5";
        }

        private void RadioBtnH510S_CheckedChanged(object sender, EventArgs e)
        {
            PublicValue.blkname = "SK5_10S";
        }

        private void RadioBtnH510X_CheckedChanged(object sender, EventArgs e)
        {
            PublicValue.blkname = "SK5_10X";
        }

        private void RadioBtnH5_CheckedChanged(object sender, EventArgs e)
        {
            PublicValue.blkname = "DK5";
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
