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
    public class ChangeTools
    {
        //改比例(ok)
        [CommandMethod("GBL")]
        public void GBL()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——修改图块比例大小");
            //创建选择集
            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\n请选择需要修改的图块"
            };
            TypedValue[] values = new TypedValue[]
            {
                //new TypedValue((int)DxfCode.BlockName,"BFA3H")
                new TypedValue((int)DxfCode.Start,"Insert")
            };
            SelectionFilter filter = new SelectionFilter(values);
            PromptSelectionResult pst;
            do
            {
                pst = ed.GetSelection(pso, filter);
                pso.MessageForAdding = "\n未选择任何有效图块，请选择需要修改的图块";
                
            }
            while (pst.Status == PromptStatus.Error || pst.Status == PromptStatus.Cancel);

            SelectionSet ss = pst.Value;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                foreach (ObjectId id in ss.GetObjectIds())
                {
                    BlockReference blockRef = (BlockReference)trans.GetObject(id, OpenMode.ForWrite);
                    Point3d pt0 = blockRef.Position;
                    double yScale = blockRef.ScaleFactors.X;
                    PromptDoubleOptions pdo = new PromptDoubleOptions("\n原图框比例为 1：<" + yScale.ToString() + ">,需修改为1")
                    {
                        AllowNegative = false,//不允许输入负数
                        AllowNone = false
                    };
                    PromptDoubleResult pds = ed.GetDouble(pdo);
                    if (pds.Status == PromptStatus.OK)
                    {
                        double scale = pds.Value / yScale;
                        blockRef.TransformBy(Matrix3d.Scaling(scale, pt0));
                        //更新当前页面
                        Application.DocumentManager.MdiActiveDocument.Editor.Regen();
                    }
                }
                trans.Commit();
            }
        }
        
        //连接两根曲线的顶点
        [CommandMethod("LJX")]
        public void LJX()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            
            string layerName = db.GetCurrentLayerName();
            ed.WriteMessage("\n百福工具箱——连接两根多段线的顶点");

            PromptEntityOptions peo1 = new PromptEntityOptions("\n请选择第一根多端线");
            peo1.SetRejectMessage("\n选择的多段线!");
            peo1.AddAllowedClass(typeof(Polyline), false);
            PromptEntityResult ent1;
            do
            {
                ent1 = ed.GetEntity(peo1);
            } while (ent1.Status != PromptStatus.OK);

            PromptEntityOptions peo2 = new PromptEntityOptions("\n请选择第一根多端线");
            peo2.SetRejectMessage("\n选择的多段线!");
            peo2.AddAllowedClass(typeof(Polyline), false);
            PromptEntityResult ent2 = ed.GetEntity(peo2);
            if (ent1.Status == PromptStatus.OK)
            {
                db.SetCurrentLayer("BF-细线");
                using(Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    Polyline pl1 = (Polyline)trans.GetObject(ent1.ObjectId, OpenMode.ForRead);
                    Polyline pl2 = (Polyline)trans.GetObject(ent2.ObjectId, OpenMode.ForRead);

                    int vertexNum = Math.Min(pl1.NumberOfVertices,pl2.NumberOfVertices);

                    for (int i = 0; i < vertexNum; i++)
                    {
                        BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForWrite);                        

                        Point3d pt1 = pl1.GetPoint3dAt(i);
                        Point3d pt2 = pl2.GetPoint3dAt(i);
                        Line l1 = new Line(pt1, pt2);
                        btr.AppendEntity(l1);
                        trans.AddNewlyCreatedDBObject(l1, true);
                        btr.DowngradeOpen();
                    }
                    trans.Commit();
                }
                db.SetCurrentLayer(layerName);
            }
            else return;
        }

    }
}
