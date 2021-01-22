using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CommonClassLibrary;
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
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace BF_CustomTools
{
    public partial class SetGlassThicknessForm : Form
    {
        public SetGlassThicknessForm()
        {
            InitializeComponent();
            TxtGlassThickness.Text = ConData();
        }
        private string ConData()
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            cmd.CommandText = "select Thickness from MaterialThickness Where MaterialName = 'Glass'";
            con.Open();
            string t = cmd.ExecuteScalar().ToString();
            con.Close();
            return t;
        }
        private void ChangDataValue(string t)
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            string myUpdata = "update MaterialThickness set Thickness  = '" + t + "'  Where MaterialName = 'Glass'";
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
            PublicValue.thickness = Convert.ToDouble(TxtGlassThickness.Text);
            this.Hide();
            //修改数据库中的值
            ChangDataValue(TxtGlassThickness.Text);
            PromptPointOptions optPoint = new PromptPointOptions("\n请拾取玻璃起点坐标");
            PromptPointResult resPoint = ed.GetPoint(optPoint);
            if (resPoint.Status != PromptStatus.OK) return;
            Point3d spt = resPoint.Value;
            PromptPointOptions ppo = new PromptPointOptions("\n请拾取玻璃终点坐标");
            ppo.UseBasePoint = true;
            ppo.BasePoint = spt;
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK) return;
            Point3d ept = ppr.Value;
            BoLiJig boliJig = new BoLiJig(spt, ept, PublicValue.thickness);
            PromptResult resJig = ed.Drag(boliJig);
            if (resJig.Status == PromptStatus.OK || resJig.Status == PromptStatus.Keyword)
            {
                Tools.AddToModelSpace(db, boliJig.GetEntity());
            }
            db.SetCurrentLayer(PublicValue.curLayerName);
            this.Close();
        }

        private void Btn5_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 5.0;
            this.TxtGlassThickness.Text = "5.0";
        }

        private void Btn6_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 6.0;
            this.TxtGlassThickness.Text = "6.0";
        }

        private void Btn8_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 8.0;
            this.TxtGlassThickness.Text = "8.0";
        }

        private void Btn10_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 10.0;
            this.TxtGlassThickness.Text = "10.0";
        }

        private void Btn12_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 12.0;
            this.TxtGlassThickness.Text = "12.0";
        }

        private void Btn15_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 15.0;
            this.TxtGlassThickness.Text = "15.0";
        }

        private void Btn16_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 16.0;
            this.TxtGlassThickness.Text = "16.0";
        }

        private void Btn18_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 18.0;
            this.TxtGlassThickness.Text = "18.0";
        }

        private void Btn20_Click(object sender, EventArgs e)
        {
            PublicValue.thickness = 20.0;
            this.TxtGlassThickness.Text = "20.0";
        }
    }
}
