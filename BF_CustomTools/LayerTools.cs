using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF_CustomTools
{
    public class LayerTools
    {
        //设置绘图环境1、设置图层
        
        [CommandMethod("HTHJ")]
        public void HTHJ()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            bool isPlot;
            LineWeight lineWeight;

            ed.WriteMessage("\n百福工具箱——设置绘图环境(图层)");

            string filePath = Tools.GetCurrentPath() + "\\SetFiles\\Layers.txt";
            StreamReader sr = new StreamReader(filePath, Encoding.Default);
            string text = sr.ReadToEnd();
            sr.Close();
            string[] data = text.Split(new char[] { '\n', '\r' });
            foreach (string str in data)
            {
                if (!str.Contains(";") & str != "")
                {
                    string[] lyArray = str.Split(new char[] { ',' });
                    short col = (short)int.Parse(lyArray[1]);

                    switch (lyArray[3])
                    {
                        case "9":
                            lineWeight = Autodesk.AutoCAD.DatabaseServices.LineWeight.LineWeight009;
                            break;
                        case "15":
                            lineWeight = Autodesk.AutoCAD.DatabaseServices.LineWeight.LineWeight015;
                            break;
                        case "40":
                            lineWeight = Autodesk.AutoCAD.DatabaseServices.LineWeight.LineWeight040;
                            break;
                        default:
                            lineWeight = Autodesk.AutoCAD.DatabaseServices.LineWeight.LineWeight025;
                            break;
                    }

                    if (lyArray[4] == "yes")
                        isPlot = true;
                    else
                        isPlot = false;
                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        db.AddLayer(lyArray[0], col, lyArray[2], lineWeight, isPlot, lyArray[5]);
                        trans.Commit();
                    }
                }
            }
        }

        //自动归层(ok)
        [CommandMethod("ZDGC")]
        public void ZDGC()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor acEd = doc.Editor;
            acEd.WriteMessage("\n百福工具箱——自动归层");
            //确定是否有标注、文字、填充3个图层，没有则添加
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has("BF-标注"))
                {
                    db.AddLayer("BF-标注", 0);
                }
                if (!lt.Has("BF-文字说明"))
                {
                    db.AddLayer("BF-文字说明", 43);
                }
                if (!lt.Has("BF-填充"))
                {
                    db.AddLayer("BF-填充", 250);
                }
                trans.Commit();
            }
            //定义一个过滤器
            TypedValue[] value1 = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"Hatch")
            };
            SelectionFilter filter1 = new SelectionFilter(value1);
            //创建选择集            
            PromptSelectionResult psr1 = acEd.SelectAll(filter1);
            if (psr1.Status == PromptStatus.OK)
            {
                SelectionSet ss1 = psr1.Value;
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    //遍历选择集中的对象
                    foreach (ObjectId id in ss1.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                        ent.Layer = "BF-填充";
                        ent.DowngradeOpen();
                    }
                    trans.Commit();
                }
            }

            //定义一个过滤器
            TypedValue[] value2 = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"Text")
            };
            SelectionFilter filter2 = new SelectionFilter(value2);
            //创建选择集
            PromptSelectionResult psr2 = acEd.SelectAll(filter2);
            if (psr2.Status == PromptStatus.OK)
            {
                SelectionSet ss2 = psr2.Value;
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    //遍历选择集中的对象
                    foreach (ObjectId id in ss2.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                        ent.Layer = "BF-文字说明";
                        ent.DowngradeOpen();
                    }
                    trans.Commit();
                }
            }

            //定义一个过滤器
            TypedValue[] value2a = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"MText")
            };
            SelectionFilter filter2a = new SelectionFilter(value2a);
            //创建选择集
            PromptSelectionResult psr2a = acEd.SelectAll(filter2a);
            if (psr2a.Status == PromptStatus.OK)
            {
                SelectionSet ss2a = psr2a.Value;
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    //遍历选择集中的对象
                    foreach (ObjectId id in ss2a.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                        ent.Layer = "BF-文字说明";
                        ent.DowngradeOpen();
                    }
                    trans.Commit();
                }
            }

            //定义一个过滤器
            TypedValue[] value3 = new TypedValue[]
            {
                new TypedValue((int)DxfCode.Start,"DIMENSION")
            };
            SelectionFilter filter3 = new SelectionFilter(value3);
            //创建选择集
            PromptSelectionResult psr3 = acEd.SelectAll(filter3);
            if (psr3.Status == PromptStatus.OK)
            {
                SelectionSet ss3 = psr3.Value;
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    //遍历选择集中的对象
                    foreach (ObjectId id in ss3.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                        ent.Layer = "BF-标注";
                        ent.DowngradeOpen();
                    }
                    trans.Commit();
                }
            }
        }

    }
}
