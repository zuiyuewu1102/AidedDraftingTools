using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
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
            FormDIC form = new FormDIC();
            Application.ShowModalDialog(form);
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
                PromptSelectionOptions pso = new PromptSelectionOptions();
                pso.MessageForAdding = "\n请选择所有需要求和的文字项";
                PromptSelectionResult psr = ed.GetSelection(pso,sf);
                if (psr.Status == PromptStatus.OK)
                {
                    SelectionSet ss = psr.Value;

                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        DBText dBText = trans.GetObject(id, OpenMode.ForRead) as DBText;
                        val = double.Parse(Tools.IntegerString(dBText.TextString));
                        zongVal = zongVal + val;
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
        //[CommandMethod("DZ")]
        //public void DZ()
        //{

        //}
    }
}
