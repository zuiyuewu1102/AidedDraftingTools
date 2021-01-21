using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CommonClassLibrary;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace BF_CustomTools
{
    public partial class SetWoodPlateThickness : Form
    {
        public SetWoodPlateThickness()
        {
            InitializeComponent();
            TxtCurThickness.Text = ConData();
        }
        private string ConData()
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandText = "select Thickness from MaterialThickness Where MaterialName = 'WoodPlate'";
            con.Open();
            string t = cmd.ExecuteScalar().ToString();
            con.Close();
            return t;
        }

        private void ChangDataValue(string t)
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            string myUpdata = "update MaterialThickness set Thickness  = '" + t + "'  Where MaterialName = 'WoodPlate'";
            SQLiteCommand cmd = new SQLiteCommand(myUpdata, con);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnOK_Click(object sender, EventArgs e)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PublicValue.thickness = Convert.ToDouble(TxtCurThickness.Text);            
            this.Hide();
            //修改数据库中的值
            ChangDataValue(TxtCurThickness.Text); 
            PromptPointOptions optPoint = new PromptPointOptions("\n请拾取夹层板起点坐标");            
            PromptPointResult resPoint = ed.GetPoint(optPoint);
            if (resPoint.Status != PromptStatus.OK) return;
            Point3d spt = resPoint.Value;
            PromptPointOptions ppo = new PromptPointOptions("\n请拾取夹层板终点坐标");
            ppo.UseBasePoint = true;
            ppo.BasePoint = spt;
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK) return;
            Point3d ept = ppr.Value;
            JiaCengBanJig mGBJig = new JiaCengBanJig(spt, ept, PublicValue.thickness);
            PromptResult resJig = ed.Drag(mGBJig);
            if (resJig.Status == PromptStatus.OK)
            {
                Tools.AddToModelSpace(db, mGBJig.GetEntity());
            }
            db.SetCurrentLayer(PublicValue.curLayerName);
            this.Close();
        }

        private void Btn3_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 3.0;
            this.TxtCurThickness.Text = "3.0";
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 5.0;
            this.TxtCurThickness.Text = "5.0";
        }

        private void Btn9_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 9.0;
            this.TxtCurThickness.Text = "9.0";
        }

        private void Btn12_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 12.0;
            this.TxtCurThickness.Text = "12.0";
        }

        private void Btn15_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 15.0;
            this.TxtCurThickness.Text = "15.0";
        }

        private void Btn18_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 18.0;
            this.TxtCurThickness.Text = "18.0";
        }

        private void Btn20_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 20.0;
            this.TxtCurThickness.Text = "20.0";
        }

        private void Btn25_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 25.0;
            this.TxtCurThickness.Text = "25.0";
        }
    }
}
