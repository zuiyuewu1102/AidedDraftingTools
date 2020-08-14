using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
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
using acadWin = Autodesk.AutoCAD.Windows;

namespace BF_CustomTools
{
    public partial class KxForm : Form
    {
        public KxForm()
        {
            InitializeComponent();
            Database db = HostApplicationServices.WorkingDatabase;
            List<string> lynames = new List<string>();
            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId id in lt) 
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(id, OpenMode.ForRead);
                    lynames.Add(ltr.Name);
                }
                trans.Commit();
            }
            foreach(string name in lynames)
            {
                LayercomboBox.Items.Add(name);
            }
        }
       
        private void TextradioButton_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void BlockradioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!BlockradioButton.Checked)
            { 
                BlocknamecheckBox.Enabled = false;
            }
            else
                BlocknamecheckBox.Enabled = true;
        }

        private void LayercheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!LayercheckBox.Checked) 
                LayercomboBox.Enabled = false;
            else
                LayercomboBox.Enabled = true;
        }

        private void BlocknamecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!BlocknamecheckBox.Checked)
                BlockcomboBox.Enabled = false;
            else
                BlockcomboBox.Enabled = true;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LayercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DimradioButton_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void Colorbtn_Click(object sender, EventArgs e)
        {
            acadWin.ColorDialog cdlg = new acadWin.ColorDialog();
            cdlg.ShowDialog();
            //Colorbtn.BackColor = cdlg.Color;            
        }

        private void OKbtn_Click(object sender, EventArgs e)
        {
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            string xx = "";
            if (TextradioButton.Checked) xx = "Text";
            else if (MtextradioButton.Checked) xx = "MText";
            else if (HatchradioButton.Checked) xx = "Hatch";
            else if (DimradioButton.Checked) xx = "Dim";
            else if (BlockradioButton.Checked) xx = "Insert";    //BlockReference

            //bool laylq = LayercheckBox.Checked;
            //bool collq = ColorcheckBox.Checked;
            //bool blocklq = BlocknamecheckBox.Checked;

            TypedValue[] values = new TypedValue[] 
            {
                new TypedValue((int)DxfCode.Start,xx)
            };            
            
            SelectionFilter filter1 = new SelectionFilter(values);
            PromptSelectionResult pst = ed.GetSelection(filter1);
            SelectionSet ss = pst.Value;
            ss.HighlightEntities();
        }

        
        private void KxForm_Load(object sender, EventArgs e)
        {

        }

     /*   private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // KxForm
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "KxForm";
            this.ResumeLayout(false);

        }*/
    }
}
