using Autodesk.AutoCAD.DatabaseServices;
using CommonClassLibrary;
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
    public partial class SetDwgScaleForm : Form
    {
        public SetDwgScaleForm()
        {
            InitializeComponent();
            

        }

        private void listBoxGlobalScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = listBoxGlobalScale.SelectedItem.ToString().Substring(2);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                listBoxLocalScale.Enabled = true;
                textBox2.Enabled = true;
            }                
            else 
            {
                listBoxLocalScale.Enabled = false;
                textBox2.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

            string message = string.Empty;

            if (checkBox1.Checked == true)
            {
                PublicValue.dimScale = double.Parse(textBox1.Text);
                PublicValue.scaleFactor = double.Parse(textBox2.Text) / PublicValue.dimScale;
                string dimStyleName = "BF" + textBox1.Text + "-" + textBox2.Text;
                
                using(Transaction trans = PublicValue.acDb.TransactionManager.StartTransaction())
                {
                    DimStyleTable dst = (DimStyleTable)trans.GetObject(PublicValue.acDb.DimStyleTableId, OpenMode.ForRead);
                    if (!dst.Has(dimStyleName))
                    {
                        DimStyleTools.CreateModifyDimStyle(dimStyleName, out message);
                        DimStyleTools.SetCurrentDimStyle(dimStyleName);
                        StatusBars.UpdateAppPane();
                    }
                    else
                    {
                        DimStyleTools.SetCurrentDimStyle(dimStyleName);
                        StatusBars.UpdateAppPane();
                    }
                }
            }
            else
            {
                PublicValue.dimScale = double.Parse(textBox1.Text);
                string dimStyleName = "BF" + textBox1.Text;
                PublicValue.scaleFactor = 1.0;
                using (Transaction trans = PublicValue.acDb.TransactionManager.StartTransaction())
                {
                    DimStyleTable dst = (DimStyleTable)trans.GetObject(PublicValue.acDb.DimStyleTableId, OpenMode.ForRead);
                    if (!dst.Has(dimStyleName))
                    {
                        DimStyleTools.CreateModifyDimStyle(dimStyleName,out message);
                        DimStyleTools.SetCurrentDimStyle(dimStyleName);
                        StatusBars.UpdateAppPane();
                    }
                    else
                    {
                        DimStyleTools.SetCurrentDimStyle(dimStyleName);
                        StatusBars.UpdateAppPane();
                    }
                }
            }
            this.Close();
        }
    }
}
