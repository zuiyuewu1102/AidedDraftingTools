using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows.ToolBars;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF_CustomTools
{

    public class TextTools
    {
        //用户词库管理
        //[CommandMethod("DIC")]
        public void DIC()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——用户词库管理");
            //加载显示窗体
            //FormDIC form = new FormDIC();
            //Application.ShowModalDialog(form);
        }

        //提取文本中的数字的和
        [CommandMethod("+")]
        public void Add()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——求和");
            double val;
            double zongVal = 0;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TypedValue[] typeval = new TypedValue[1];
                typeval.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);

                SelectionFilter sf = new SelectionFilter(typeval);
                PromptSelectionOptions pso = new PromptSelectionOptions
                {
                    MessageForAdding = "\n请选择所有需要求和的文字项"
                };
                PromptSelectionResult psr = ed.GetSelection(pso,sf);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;

                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        DBText dBText = trans.GetObject(id, OpenMode.ForRead) as DBText;
                        val = double.Parse(dBText.TextString.IntegerString());
                        zongVal += val;
                    }
                }

                PromptPointOptions ppo = new PromptPointOptions("\n输入插入点");
                PromptPointResult ppr = ed.GetPoint(ppo);
                if (ppr.Status == PromptStatus.OK)
                {
                    Point3d pt1 = ppr.Value;
                    db.AddDText(zongVal.ToString(), pt1);
                }
                trans.Commit();
            }
        }

        //递增文本
        [CommandMethod("DZ")]
        public void DZ()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——递增文本");
            //输入增量值
            //PromptIntegerOptions pio = new PromptIntegerOptions("\n请输入增量值<1>")
            //{
            //    DefaultValue = 1,
            //    AllowZero = false,
            //    AllowNone = true
            //};
            //PromptIntegerResult pir = ed.GetInteger(pio);
            //int zl = pir.Value;
            //选择文本
            TypedValue[] typeval = new TypedValue[1];
            typeval.SetValue(new TypedValue((int)DxfCode.Start, "TEXT"), 0);

            SelectionFilter sf = new SelectionFilter(typeval);

            PromptSelectionResult psr = ed.GetSelection(sf);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                foreach (ObjectId id in ss.GetObjectIds())
                {
                    PromptPointOptions ppo1 = new PromptPointOptions("\n选择起始点");
                    PromptPointResult ppr1 = ed.GetPoint(ppo1);
                    if (ppr1.Status == PromptStatus.OK)
                    {
                        Point3d spt = ppr1.Value;
                        PromptPointOptions ppo2 = new PromptPointOptions("\n选择下一点");
                        PromptPointResult ppr2;
                        Entity ent;
                        Point3d ept;
                        do
                        {
                            ppo2.UseBasePoint = true;
                            ppo2.BasePoint = spt;
                            ppr2 = ed.GetPoint(ppo2);
                            ept = ppr2.Value;
                            ent = id.CopyEntity(spt, ept);
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                                Entity ent1 = ent.CopyEntity(spt, ept);
                                DBText dbt = (DBText)ent1;
                                trans.Commit();
                            }
                            spt = ept;
                        } while (ppr2.Status == PromptStatus.OK);
                    }
                }
            }
        }
    }
}
