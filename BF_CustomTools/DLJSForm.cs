using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BF_CustomTools
{
    public partial class DLJSForm : Form
    {
        public DLJSForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox4.Text = String.Format("{0:N2} ", (double.Parse(textBox2.Text) * double.Parse(textBox3.Text)));
            double dianliu = (double.Parse(textBox4.Text) / (Math.Sqrt(3) * double.Parse(comboBox1.Text) * double.Parse(textBox5.Text))) * 1000;            
            textBox6.Text = String.Format("{0:N2} ", dianliu);
            dianliu = dianliu * 1.5;

            if (dianliu < 6 || dianliu == 6)
            {
                PubVal.edingdianliu = "6";
            }
            else if (dianliu < 10 || dianliu == 10)
            {
                PubVal.edingdianliu = "10";
            }
            else if (dianliu < 16 || dianliu == 16)
            {
                PubVal.edingdianliu = "16";
            }
            else if (dianliu < 20 || dianliu == 20)
            {
                PubVal.edingdianliu = "20";
            }
            else if (dianliu < 25 || dianliu == 25)
            {
                PubVal.edingdianliu = "25";
            }
            else if (dianliu < 32 || dianliu == 32)
            {
                PubVal.edingdianliu = "32";
            }
            else if (dianliu < 40 || dianliu == 40)
            {
                PubVal.edingdianliu = "40";
            }
            else if (dianliu < 50 || dianliu == 50)
            {
                PubVal.edingdianliu = "50";
            }
            else if (dianliu < 63 || dianliu == 63)
            {
                PubVal.edingdianliu = "63";
            }
            else
            {
                PubVal.edingdianliu = "没有合适的型号";
                MessageBox.Show("没有合适的断路器，建议分成2个配电箱！！！");
            }

            textBox1.Text = "EA9RN" +PubVal.jishu + PubVal.edingdianliu + "30C";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    PubVal.jishu = "4C";
                    break;
                case 1:
                    PubVal.jishu = "2C";
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PubVal.duanluqi = textBox1.Text;
            PubVal.pe = "Pe  = " + textBox2.Text + "Kw";
            PubVal.kx = "Kx  = " + textBox3.Text;
            PubVal.pj = "Pj  = " + textBox4.Text + "Kw";
            PubVal.ue = "Ue  = " + comboBox1.Text;
            PubVal.cos = @"Cos%%C = " + textBox5.Text;
            PubVal.ij = "Ij  = " + textBox6.Text + "A";
            this.Close();
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            PromptPointOptions ppo = new PromptPointOptions("\n输入插入点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                Point3d pt1 = new Point3d(ppr.Value.X +150,ppr.Value.Y + 50,0);
                Point3d pt2 = new Point3d(pt1.X, pt1.Y - 300, 0);
                Point3d pt3 = new Point3d(pt1.X, pt2.Y - 150, 0);
                Point3d pt4 = new Point3d(pt1.X, pt3.Y - 150, 0);
                Point3d pt5 = new Point3d(pt1.X, pt4.Y - 150, 0);
                Point3d pt6 = new Point3d(pt1.X, pt5.Y - 150, 0);
                Point3d pt7 = new Point3d(pt1.X, pt6.Y - 150, 0);
                db.AddDText(PubVal.duanluqi, pt1);
                db.AddDText(PubVal.pe, pt2);
                db.AddDText(PubVal.kx, pt3);
                db.AddDText(PubVal.pj, pt4);
                db.AddDText(PubVal.ue, pt5);
                db.AddDText(PubVal.cos, pt6);
                db.AddDText(PubVal.ij, pt7);
            }
        }

        
    }
    public static class PubVal
    {
        public static double zongdianliang = 0;
        public static string duanluqi;
        public static string pe;
        public static string kx;
        public static string pj;
        public static string ue;
        public static string cos;
        public static string ij;
        public static string jishu = "4C";
        public static string edingdianliu;

        public static void AddDText(this Database db, string neirong, Point3d insertPoint)
        {
            string oldLayerName = db.GetCurrentLayerName();
            db.SetCurrentLayer("BF-文字说明");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt;
                bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr;
                btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                DBText text0 = new DBText(); // 新建单行文本对象
                text0.Position = insertPoint; // 设置文本位置 
                text0.TextString = neirong; // 设置文本内容
                text0.Height = 75;  // 设置文本高度
                text0.Rotation = 0;  // 设置文本选择角度
                text0.IsMirroredInX = false; // 在X轴镜像
                //text0.HorizontalMode = TextHorizontalMode.TextLeft; // 设置对齐方式
                //text0.AlignmentPoint = text0.Position; //设置对齐点
                btr.AppendEntity(text0);
                trans.AddNewlyCreatedDBObject(text0, true);

                trans.Commit();
            }
            db.SetCurrentLayer(oldLayerName);
        }
    }
}
