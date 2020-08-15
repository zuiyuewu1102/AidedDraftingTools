using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonClassLibrary;

namespace BF_CustomTools
{
    public class StatisticalTools
    {
        //统计led灯条长度
        [CommandMethod("LEDC")]
        public void LEDC()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具——统计LED灯条长度");

            TypedValue[] values = new TypedValue[]
            {
                new TypedValue((int)DxfCode.LinetypeName,"LED_LINE"),
                new TypedValue((int)DxfCode.LayerName,"BF-灯具")
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr;
            do
            {
                psr = ed.GetSelection(filter);
            }
            while (psr.Status != PromptStatus.OK);

            double ledLenght = 0.0;

            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId,OpenMode.ForRead);

                SelectionSet ss = psr.Value;

                foreach (ObjectId id in ss.GetObjectIds())
                {
                    Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);

                    switch (ent.GetType().Name.ToString())
                    {
                        case "Line":
                            Line line = (Line)trans.GetObject(id, OpenMode.ForRead);
                            ledLenght += line.Length;
                            break;
                        case "Arc":
                            Arc arc = (Arc)trans.GetObject(id, OpenMode.ForRead);
                            ledLenght += arc.Length;
                            break;
                        case "Circle":
                            Circle c = (Circle)trans.GetObject(id, OpenMode.ForRead);
                            ledLenght += c.Circumference;
                            break;
                        case "Polyline":
                            Polyline pline = (Polyline)trans.GetObject(id, OpenMode.ForRead);
                            ledLenght += pline.Length;
                            break;
                        default:
                            ledLenght += 1;
                            break;
                    }
                }
                trans.Commit();
            }
            ledLenght /= 1000;
            string strLen = String.Format("{0:N2} ", ledLenght);
            ed.WriteMessage("\nLED灯条总长为：" + strLen + "m");
        }

        //统计灯具数量
        [CommandMethod("DJSL")]
        public void DJSL()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具——统计灯具数量");

            TypedValue[] values = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"Insert"),
                new TypedValue((int)DxfCode.LayerName,"BF-灯具")
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult psr;
            do
            {
                psr = ed.GetSelection(filter);
            }
            while (psr.Status != PromptStatus.OK);

            int thdsl=0, tdsl=0,sksdsl=0,fxjldsl=0;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);

                SelectionSet ss = psr.Value;

                foreach (ObjectId id in ss.GetObjectIds())
                {
                    BlockReference br = (BlockReference)trans.GetObject(id, OpenMode.ForRead);

                    switch (br.Name)
                    {
                        case "天花灯":
                            thdsl += 1;
                            break;
                        case "LED筒灯":
                            tdsl += 1;
                            break;
                        case "百福-3孔射灯":
                            sksdsl += 1;
                            break;
                        case "百福-户外方金卤灯":
                            fxjldsl += 1;
                            break;
                        default:                            
                            break;
                    }
                }
                trans.Commit();
            }
            if (thdsl != 0)
            {
                ed.WriteMessage("\n天花灯数量：" + thdsl.ToString() + "盏");
            }

            if (tdsl != 0)
            {
                ed.WriteMessage("\n筒灯数量：" + tdsl.ToString() + "盏");
            }

            if (sksdsl != 0)
            {
                ed.WriteMessage("\n射灯数量：" + sksdsl.ToString() + "盏");
            }

            if (fxjldsl != 0)
            {
                ed.WriteMessage("\n金卤灯数量：" + fxjldsl.ToString() + "盏");
            }
        }

        //电力计算
        [CommandMethod("DLJS")]
        public void DLJS()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PubVal.zongdianliang = 0;
            double val;

            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                TypedValue[] typeval = new TypedValue[1];
                typeval.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);

                SelectionFilter sf = new SelectionFilter(typeval);

                PromptSelectionResult psr = ed.GetSelection(sf);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;

                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        DBText dBText = trans.GetObject(id, OpenMode.ForRead) as DBText;
                        val = double.Parse(Tools.IntegerString(dBText.TextString)) / 1000;
                        PubVal.zongdianliang += val;
                    }
                }
                trans.Commit();
            }

            DLJSForm f1 = new DLJSForm();
            f1.ShowDialog();
        }
    }
}
