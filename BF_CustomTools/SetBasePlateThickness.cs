using App = Autodesk.AutoCAD.ApplicationServices.Application;
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

namespace BF_CustomTools
{
    public partial class SetBasePlateThickness : Form
    {
        public SetBasePlateThickness()
        {
            InitializeComponent();
            TxtCurThickness.Text = ConData();
        }

        private string ConData()
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            SQLiteCommand cmd = new SQLiteCommand
            {
                Connection = con,
                CommandText = "select Thickness from MaterialTable Where Name = 'BasePlate'"
            };
            con.Open();
            string t = cmd.ExecuteScalar().ToString();
            con.Close();
            return t;
        }
        private void ChangDataValue(string t)
        {
            string dataPath = "DataSource=" + Tools.GetCurrentPath() + "\\BaseData.db";
            SQLiteConnection con = new SQLiteConnection(dataPath);
            string myUpdata = "update MaterialTable set Thickness  = '" + t + "'  Where Name = 'BasePlate'";
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
            Editor ed = App.DocumentManager.MdiActiveDocument.Editor;
            PublicValue.thickness = Convert.ToDouble(TxtCurThickness.Text);
            this.Hide();
            //修改数据库中的值
            ChangDataValue(TxtCurThickness.Text);

            string tishitxt = "\n当前基板厚度为" + PublicValue.thickness.ToString() + "\n请拾取起点坐标";
            PromptPointOptions optPoint = new PromptPointOptions(tishitxt);
            //optPoint.Keywords.Add("S");
            PromptPointResult resPoint = ed.GetPoint(optPoint);
            if (resPoint.Status != PromptStatus.OK) return;
            Point3d spt = resPoint.Value;
            PromptPointOptions ppo2 = new PromptPointOptions("\n给定终止点")
            {
                BasePoint = spt,
                UseBasePoint = true
            };
            PromptPointResult ppr2 = ed.GetPoint(ppo2);
            if (ppr2.Status == PromptStatus.OK)
            {
                Point3d ept = ppr2.Value;
                //初始化图形
                Polyline polyline1 = new Polyline();
                for (int i = 0; i < 4; i++)
                {
                    polyline1.AddVertexAt(i, Point2d.Origin, 0.0, 0.0, 0.0);
                }
                polyline1.Closed = true;
                polyline1.Layer = "BF-产品线";

                Polyline polyline2 = new Polyline();
                for (int i = 0; i < 8; i++)
                {
                    polyline2.AddVertexAt(i, Point2d.Origin, 0.0, 0.0, 0.0);
                }
                polyline2.Layer = PublicValue.layerName;
                //拖拽
                PiGeBanJig pigebanJig = new PiGeBanJig(spt, ept, PublicValue.thickness, polyline1, polyline2);
                PromptResult resJig = ed.Drag(pigebanJig);
                if (resJig.Status == PromptStatus.OK)
                {
                    Tools.AddToModelSpace(db, polyline1);
                    Tools.AddToModelSpace(db, polyline2);
                }
            }
            this.Close();
            return;
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
    }
}
