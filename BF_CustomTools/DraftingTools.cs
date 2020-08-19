using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BF_CustomTools
{
    public class DraftingTools
    {
        //绘制圆的十字中心线(ok)
        [CommandMethod("SZC")]
        public void SZC()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDB = acDoc.Database;
            Editor acEd = acDoc.Editor;

            //acDB.SetCurrentLayer("BF-中心线");
            //acDB.Clayer = "BF-中心线";
            //设置选择过滤器
            TypedValue[] value1 = new TypedValue[]{
                new TypedValue((int)DxfCode.Start,"Circle")
            };
            //创建选择集
            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——绘制圆的十字中心线"
            };
            SelectionFilter filter1 = new SelectionFilter(value1);
            PromptSelectionResult psr = acEd.GetSelection(pso, filter1);
            //判断选择集是否有值
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet acSSet = psr.Value;
                using (Transaction acTran = acDB.TransactionManager.StartTransaction())
                {
                    string ylayname;
                    //设定当前图层
                    LayerTable lt = (LayerTable)acTran.GetObject(acDB.LayerTableId, OpenMode.ForRead);
                    if (lt.Has("BF-中心线"))
                    {
                        LayerTableRecord ltr = (LayerTableRecord)acTran.GetObject(acDB.Clayer, OpenMode.ForRead);
                        ylayname = ltr.Name;
                        ObjectId layerId = lt["BF-中心线"];
                        if (acDB.Clayer != layerId)
                        {
                            acDB.Clayer = layerId;
                            //循环选择集中的ObjectId
                            foreach (ObjectId acId in acSSet.GetObjectIds())
                            {
                                //提取圆的半径和中心点
                                Circle cir = acTran.GetObject(acId, OpenMode.ForRead) as Circle;
                                double r = cir.Radius * 1.2;
                                Point3d pt0 = cir.Center;
                                //计算十字中心的起始点坐标
                                Point3d pt1 = new Point3d(pt0.X - r, pt0.Y, 0);
                                Point3d pt2 = new Point3d(pt0.X + r, pt0.Y, 0);
                                Point3d pt3 = new Point3d(pt0.X, pt0.Y + r, 0);
                                Point3d pt4 = new Point3d(pt0.X, pt0.Y - r, 0);
                                //绘制十字中心线，并添加到块表记录
                                BlockTable acBt = acTran.GetObject(acDB.BlockTableId, OpenMode.ForRead) as BlockTable;
                                BlockTableRecord acBtr = acTran.GetObject(acBt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                                Line l1 = new Line(pt1, pt2);
                                acBtr.AppendEntity(l1);
                                acTran.AddNewlyCreatedDBObject(l1, true);
                                Line l2 = new Line(pt3, pt4);
                                acBtr.AppendEntity(l2);
                                acTran.AddNewlyCreatedDBObject(l2, true);
                            }
                            acDB.Clayer = lt[ylayname];
                        }
                        else
                        {
                            //循环选择集中的ObjectId
                            foreach (ObjectId acId in acSSet.GetObjectIds())
                            {
                                //提取圆的半径和中心点
                                Circle cir = acTran.GetObject(acId, OpenMode.ForRead) as Circle;
                                double r = cir.Radius * 1.2;
                                Point3d pt0 = cir.Center;
                                //计算十字中心的起始点坐标
                                Point3d pt1 = new Point3d(pt0.X - r, pt0.Y, 0);
                                Point3d pt2 = new Point3d(pt0.X + r, pt0.Y, 0);
                                Point3d pt3 = new Point3d(pt0.X, pt0.Y + r, 0);
                                Point3d pt4 = new Point3d(pt0.X, pt0.Y - r, 0);
                                //绘制十字中心线，并添加到块表记录
                                BlockTable acBt = acTran.GetObject(acDB.BlockTableId, OpenMode.ForRead) as BlockTable;
                                BlockTableRecord acBtr = acTran.GetObject(acBt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                                Line l1 = new Line(pt1, pt2);
                                acBtr.AppendEntity(l1);
                                acTran.AddNewlyCreatedDBObject(l1, true);
                                Line l2 = new Line(pt3, pt4);
                                acBtr.AppendEntity(l2);
                                acTran.AddNewlyCreatedDBObject(l2, true);
                            }
                        }
                    }
                    else
                        acEd.WriteMessage("\n不存在图层“BF-中心线”");
                    acTran.Commit();
                }
            }
            else
            {
                Application.ShowAlertDialog("\nNumber of Circle selected: 0");
            }
        }

        //[CommandMethod("DD")]
        public void DD()
        {
           
        }

        //有问题
        //[CommandMethod("JB")]
        public void JB()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            string curLayerName = db.GetCurrentLayerName();
            ed.WriteMessage("\n百福工具箱——绘制夹层板截面");

            db.SetCurrentLayer("BF-产品线");

            //Matrix3d mt = ed.CurrentUserCoordinateSystem;
            //Vector3d normal = mt.CoordinateSystem3d.Zaxis;
            
            db.SetCurrentLayer(curLayerName);
        }

        //绘制皮革板截面
        [CommandMethod("PGB")]
        public void PGB()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3d spt;
            Point3d ept;
            List<Point2d> pl1 = new List<Point2d>();
            List<Point2d> pl2 = new List<Point2d>();
            Double t = 9;
            double ang;
            //double dis;
            PromptPointOptions ppo1 = new PromptPointOptions("\n百福绘图工具——绘制皮革板剖面")
            {
                Message = "\n给定起始点"
            };
            PromptPointResult ppr1 = ed.GetPoint(ppo1);
            if (ppr1.Status == PromptStatus.OK)
            {
                spt = ppr1.Value;
                PromptPointOptions ppo2 = new PromptPointOptions("\n给定终止点")
                {
                    BasePoint = spt,
                    UseBasePoint = true
                };
                PromptPointResult ppr2 = ed.GetPoint(ppo2);
                if (ppr2.Status == PromptStatus.OK)
                {
                    ept = ppr2.Value;
                    Vector2d vec = new Point2d(ept.X, ept.Y) - new Point2d(spt.X, spt.Y);
                    ang = vec.Angle;

                    PromptDoubleOptions pdo = new PromptDoubleOptions("\n给定板厚t=<9>")
                    {
                        AllowNone = true
                    };
                    PromptDoubleResult pdr = ed.GetDouble(pdo);
                    if (pdr.Status == PromptStatus.OK)
                    {
                        t = pdr.Value;
                    }
                    else if (pdr.Status == PromptStatus.None) t = 9;

                    //计算相关点坐标
                    Point2d p2 = new Point2d(spt.X + 20 * Math.Cos(ang), spt.Y + 20 * Math.Sin(ang));
                    Point2d p1 = new Point2d(p2.X + 1 * Math.Cos(ang + Math.PI / 2), p2.Y + 1 * Math.Sin(ang + Math.PI / 2));
                    Point2d p3 = new Point2d(spt.X, spt.Y);
                    Point2d p4 = new Point2d(spt.X + (t + 2) * Math.Cos(ang + Math.PI / 2), spt.Y + (t + 2) * Math.Sin(ang + Math.PI / 2));
                    Point2d p5 = new Point2d(ept.X + (t + 2) * Math.Cos(ang + Math.PI / 2), ept.Y + (t + 2) * Math.Sin(ang + Math.PI / 2));
                    Point2d p6 = new Point2d(ept.X, ept.Y);
                    Point2d p7 = new Point2d(ept.X - 20 * Math.Cos(ang), ept.Y - 20 * Math.Sin(ang));
                    Point2d p8 = new Point2d(p7.X + 1 * Math.Cos(ang + Math.PI / 2), p7.Y + 1 * Math.Sin(ang + Math.PI / 2));
                    pl1.Add(p1);
                    pl1.Add(p2);
                    pl1.Add(p3);
                    pl1.Add(p4);
                    pl1.Add(p5);
                    pl1.Add(p6);
                    pl1.Add(p7);
                    pl1.Add(p8);
                    Point2d pt1 = new Point2d(p1.X - 19 * Math.Cos(ang), p1.Y - 19 * Math.Sin(ang));
                    Point2d pt2 = new Point2d(pt1.X + t * Math.Cos(ang + Math.PI / 2), pt1.Y + t * Math.Sin(ang + Math.PI / 2));
                    Point2d pt4 = new Point2d(p8.X + 19 * Math.Cos(ang), p8.Y + 19 * Math.Sin(ang));
                    Point2d pt3 = new Point2d(pt4.X + t * Math.Cos(ang + Math.PI / 2), pt4.Y + t * Math.Sin(ang + Math.PI / 2));
                    pl2.Add(pt1);
                    pl2.Add(pt2);
                    pl2.Add(pt3);
                    pl2.Add(pt4);
                }
            }

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has("BF-皮革")) db.AddLayer("BF-皮革", 2);
                db.Clayer = lt["BF-皮革"];

                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr1 = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                Polyline polyline1 = new Polyline();
                for (int i = 0; i < pl1.Count; i++)
                {
                    polyline1.AddVertexAt(i, pl1[i], 0, 0, 0);
                }
                btr1.AppendEntity(polyline1);
                trans.AddNewlyCreatedDBObject(polyline1, true);
                btr1.DowngradeOpen();

                if (!lt.Has("BF-产品线")) db.AddLayer("BF-产品线", 146);
                db.Clayer = lt["BF-产品线"];

                BlockTableRecord btr2 = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                Polyline polyline2 = new Polyline();
                for (int i = 0; i < pl2.Count; i++)
                {
                    polyline2.AddVertexAt(i, pl2[i], 0, 0, 0);
                }
                polyline2.Closed = true;
                btr2.AppendEntity(polyline2);
                trans.AddNewlyCreatedDBObject(polyline2, true);
                btr2.DowngradeOpen();

                trans.Commit();
            }
        }

        //绘制墙布板截面
        [CommandMethod("QBB")]
        public void QBB()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3d spt;
            Point3d ept;
            List<Point2d> pl1 = new List<Point2d>();
            List<Point2d> pl2 = new List<Point2d>();
            Double t = 9;
            double ang;
            //double dis;
            PromptPointOptions ppo1 = new PromptPointOptions("\n百福绘图工具——绘制墙布板剖面")
            {
                Message = "\n给定起始点"
            };
            PromptPointResult ppr1 = ed.GetPoint(ppo1);
            if (ppr1.Status == PromptStatus.OK)
            {
                spt = ppr1.Value;
                PromptPointOptions ppo2 = new PromptPointOptions("\n给定终止点")
                {
                    BasePoint = spt,
                    UseBasePoint = true
                };
                PromptPointResult ppr2 = ed.GetPoint(ppo2);
                if (ppr2.Status == PromptStatus.OK)
                {
                    ept = ppr2.Value;
                    Vector2d vec = new Point2d(ept.X, ept.Y) - new Point2d(spt.X, spt.Y);
                    ang = vec.Angle;

                    PromptDoubleOptions pdo = new PromptDoubleOptions("\n给定板厚t=<9>")
                    {
                        AllowNone = true
                    };
                    PromptDoubleResult pdr = ed.GetDouble(pdo);
                    if (pdr.Status == PromptStatus.OK)
                    {
                        t = pdr.Value;
                    }
                    else if (pdr.Status == PromptStatus.None) t = 9;

                    //计算相关点坐标
                    Point2d p2 = new Point2d(spt.X + 20 * Math.Cos(ang), spt.Y + 20 * Math.Sin(ang));
                    Point2d p1 = new Point2d(p2.X + 1 * Math.Cos(ang + Math.PI / 2), p2.Y + 1 * Math.Sin(ang + Math.PI / 2));
                    Point2d p3 = new Point2d(spt.X, spt.Y);
                    Point2d p4 = new Point2d(spt.X + (t + 2) * Math.Cos(ang + Math.PI / 2), spt.Y + (t + 2) * Math.Sin(ang + Math.PI / 2));
                    Point2d p5 = new Point2d(ept.X + (t + 2) * Math.Cos(ang + Math.PI / 2), ept.Y + (t + 2) * Math.Sin(ang + Math.PI / 2));
                    Point2d p6 = new Point2d(ept.X, ept.Y);
                    Point2d p7 = new Point2d(ept.X - 20 * Math.Cos(ang), ept.Y - 20 * Math.Sin(ang));
                    Point2d p8 = new Point2d(p7.X + 1 * Math.Cos(ang + Math.PI / 2), p7.Y + 1 * Math.Sin(ang + Math.PI / 2));
                    pl1.Add(p1);
                    pl1.Add(p2);
                    pl1.Add(p3);
                    pl1.Add(p4);
                    pl1.Add(p5);
                    pl1.Add(p6);
                    pl1.Add(p7);
                    pl1.Add(p8);
                    Point2d pt1 = new Point2d(p1.X - 19 * Math.Cos(ang), p1.Y - 19 * Math.Sin(ang));
                    Point2d pt2 = new Point2d(pt1.X + t * Math.Cos(ang + Math.PI / 2), pt1.Y + t * Math.Sin(ang + Math.PI / 2));
                    Point2d pt4 = new Point2d(p8.X + 19 * Math.Cos(ang), p8.Y + 19 * Math.Sin(ang));
                    Point2d pt3 = new Point2d(pt4.X + t * Math.Cos(ang + Math.PI / 2), pt4.Y + t * Math.Sin(ang + Math.PI / 2));
                    pl2.Add(pt1);
                    pl2.Add(pt2);
                    pl2.Add(pt3);
                    pl2.Add(pt4);
                }
            }

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (!lt.Has("BF-墙布")) db.AddLayer("BF-墙布", 2);
                db.Clayer = lt["BF-墙布"];

                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr1 = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);

                Polyline polyline1 = new Polyline();
                for (int i = 0; i < pl1.Count; i++)
                {
                    polyline1.AddVertexAt(i, pl1[i], 0, 0, 0);
                }
                btr1.AppendEntity(polyline1);
                trans.AddNewlyCreatedDBObject(polyline1, true);
                btr1.DowngradeOpen();

                if (!lt.Has("BF-产品线")) db.AddLayer("BF-产品线", 146);
                db.Clayer = lt["BF-产品线"];

                BlockTableRecord btr2 = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                Polyline polyline2 = new Polyline();
                for (int i = 0; i < pl2.Count; i++)
                {
                    polyline2.AddVertexAt(i, pl2[i], 0, 0, 0);
                }
                polyline2.Closed = true;
                btr2.AppendEntity(polyline2);
                trans.AddNewlyCreatedDBObject(polyline2, true);
                btr2.DowngradeOpen();

                trans.Commit();
            }
        }

        //绘制玻璃(有点小Bug)
        [CommandMethod("BL")]
        public void BL()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——绘制玻璃截面");
            List<Point3d> points = new List<Point3d>();
            string layerNameOld = db.GetCurrentLayerName();
            int t = 10;

            PromptIntegerOptions pio = new PromptIntegerOptions("\n请输入玻璃厚度")
            {
                DefaultValue = 10
            };
            PromptIntegerResult pir = ed.GetInteger(pio);
            if (pir.Status == PromptStatus.OK)
            {
                t = pir.Value;
            }

            PromptPointOptions ppo = new PromptPointOptions("\n选择起点位置");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                points.Add(ppr.Value);
                //Point3d pt1 = ppr.Value;
                PromptPointOptions ppo1 = new PromptPointOptions("\n指定下一点")
                {
                    UseBasePoint = true,
                    AllowNone = true
                };
                PromptPointResult ppr1;
                int i = 0;
                ppo1.BasePoint = points[i];
                ppr1 = ed.GetPoint(ppo1);
                
                if(ppr1.Status == PromptStatus.OK)
                {
                    do
                    {
                        i++;
                        points.Add(ppr1.Value);
                        ppo1.Message = "\n指定下一点或结束";                        
                        ppo1.BasePoint = points[i];
                        ppr1 = ed.GetPoint(ppo1);
                    } while (ppr1.Status != PromptStatus.None);
                }
                
                //将Points中的所有元素反序
                //points.Reverse();

                if(points.Count == 2)
                {
                    db.OnlyOneGlass(points[0], points[1], t);
                }
                else if(points.Count == 3)
                {
                    db.No1Glass(points[0], points[1], points[2], t);
                    db.LastGlass(points[0], points[1], points[2], t);
                }
                else//大于3个点的情况
                {
                    db.No1Glass(points[0], points[1], points[2], t);
                    db.ZJGlass(points,t);
                    db.LastGlass(points[points.Count -3], points[points.Count - 2], points[points.Count - 1], t);
                }
            }
            db.SetCurrentLayer(layerNameOld);
        }
        
        //绘制石头板(有点小Bug)
        [CommandMethod("STB")]
        public void STB()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——绘制石材截面");
            List<Point3d> points = new List<Point3d>();
            string layerNameOld = db.GetCurrentLayerName();
            int t = 13;

            PromptIntegerOptions pio = new PromptIntegerOptions("\n请输入石材厚度")
            {
                DefaultValue = 13
            };
            PromptIntegerResult pir = ed.GetInteger(pio);
            if (pir.Status == PromptStatus.OK)
            {
                t = pir.Value;
            }

            PromptPointOptions ppo = new PromptPointOptions("\n选择起点位置");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                points.Add(ppr.Value);
                //Point3d pt1 = ppr.Value;
                PromptPointOptions ppo1 = new PromptPointOptions("\n指定下一点")
                {
                    UseBasePoint = true,
                    AllowNone = true
                };
                PromptPointResult ppr1;
                int i = 0;
                ppo1.BasePoint = points[i];
                ppr1 = ed.GetPoint(ppo1);

                if (ppr1.Status == PromptStatus.OK)
                {
                    do
                    {
                        i++;
                        points.Add(ppr1.Value);
                        ppo1.Message = "\n指定下一点或结束";
                        ppo1.BasePoint = points[i];
                        ppr1 = ed.GetPoint(ppo1);
                    } while (ppr1.Status != PromptStatus.None);
                }

                //将Points中的所有元素反序
                //points.Reverse();

                if (points.Count == 2)
                {
                    db.OnlyOneStb(points[0], points[1], t);
                }
                else if (points.Count == 3)
                {
                    db.No1Stb(points[0], points[1], points[2], t);
                    db.LastStb(points[0], points[1], points[2], t);
                }
                else//大于3个点的情况
                {
                    db.No1Stb(points[0], points[1], points[2], t);
                    db.ZJStb(points, t);
                    db.LastStb(points[points.Count - 3], points[points.Count - 2], points[points.Count - 1], t);
                }
            }

            db.SetCurrentLayer(layerNameOld);
        }

        //绘制柜桶后框
        [CommandMethod("GTHK")]
        public void GTHK()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point2d pt1, pt2, pt3, pt4;
            ed.WriteMessage("\n百福工具箱——绘制柜桶铝后框");

            string curLayerName = db.GetCurrentLayerName();
            db.SetCurrentLayer("BF-铝材");

            PromptPointOptions ppo1 = new PromptPointOptions("\n请绘制柜桶铝后框的起始点");
            PromptPointResult ppr1 = ed.GetPoint(ppo1);
            if (ppr1.Status == PromptStatus.OK)
            {
                Point3d spt = ppr1.Value;
                //PromptPointOptions ppo2 = new PromptPointOptions("\n请绘制柜桶铝后框的对角点");
                //ppo2.UseBasePoint = true;
                //ppo2.BasePoint = spt;
                //ppo2.UseDashedLine = true;
                PromptPointResult ppr2 = ed.GetCorner("\n请绘制柜桶铝后框的对角点",spt);
                if (ppr2.Status == PromptStatus.OK)
                {
                    Point3d ept = ppr2.Value;
                    pt1 = new Point2d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
                    pt2 = new Point2d(Math.Max(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
                    pt3 = new Point2d(Math.Max(spt.X, ept.X), Math.Max(spt.Y, ept.Y));
                    pt4 = new Point2d(Math.Min(spt.X, ept.X), Math.Max(spt.Y, ept.Y));

                    using (Transaction trans = db.TransactionManager.StartTransaction())
                    {
                        BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                        BlockTableRecord btr;
                        btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                        Polyline pl1 = new Polyline();
                        pl1.AddVertexAt(0, pt1, 0, 0, 0);
                        pl1.AddVertexAt(1, pt2, 0, 0, 0);
                        pl1.AddVertexAt(2, pt3, 0, 0, 0);
                        pl1.AddVertexAt(3, pt4, 0, 0, 0);
                        pl1.Closed = true;

                        btr.AppendEntity(pl1);
                        trans.AddNewlyCreatedDBObject(pl1, true);

                        Point2d pt11 = new Point2d(pt1.X + 13, pt1.Y + 13);
                        Point2d pt12 = new Point2d(pt2.X - 13, pt2.Y + 13);
                        Point2d pt13 = new Point2d(pt3.X - 13, pt3.Y - 13);
                        Point2d pt14 = new Point2d(pt4.X + 13, pt4.Y - 13);

                        Polyline pl2 = new Polyline();
                        pl2.AddVertexAt(0, pt11, 0, 0, 0);
                        pl2.AddVertexAt(1, pt12, 0, 0, 0);
                        pl2.AddVertexAt(2, pt13, 0, 0, 0);
                        pl2.AddVertexAt(3, pt14, 0, 0, 0);
                        pl2.Closed = true;

                        btr.AppendEntity(pl2);
                        trans.AddNewlyCreatedDBObject(pl2, true);

                        db.SetCurrentLayer("BF-细线");

                        Point2d pt21 = new Point2d(pt1.X + 23, pt1.Y + 23);
                        Point2d pt22 = new Point2d(pt2.X - 23, pt2.Y + 23);
                        Point2d pt23 = new Point2d(pt3.X - 23, pt3.Y - 23);
                        Point2d pt24 = new Point2d(pt4.X + 23, pt4.Y - 23);

                        Line l1 = new Line(new Point3d(pt1.X, pt1.Y, 0), new Point3d(pt21.X, pt21.Y, 0));
                        Line l2 = new Line(new Point3d(pt2.X, pt2.Y, 0), new Point3d(pt22.X, pt22.Y, 0));
                        Line l3 = new Line(new Point3d(pt3.X, pt3.Y, 0), new Point3d(pt23.X, pt23.Y, 0));
                        Line l4 = new Line(new Point3d(pt4.X, pt4.Y, 0), new Point3d(pt24.X, pt24.Y, 0));

                        btr.AppendEntity(l1);
                        btr.AppendEntity(l2);
                        btr.AppendEntity(l3);
                        btr.AppendEntity(l4);
                        btr.DowngradeOpen();
                        trans.AddNewlyCreatedDBObject(l1, true);
                        trans.AddNewlyCreatedDBObject(l2, true);
                        trans.AddNewlyCreatedDBObject(l3, true);
                        trans.AddNewlyCreatedDBObject(l4, true);

                        trans.Commit();
                    }
                }
                else
                {
                    db.SetCurrentLayer(curLayerName);
                    return; 
                }
            }
            else
            {
                db.SetCurrentLayer(curLayerName);
                return;
            }
            db.SetCurrentLayer(curLayerName);
        }

        //绘制储物门
        [CommandMethod("CWM")]
        public void CWM()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blockName = "自关锁";
            Point3d zgsPt;

            Point2d pt1, pt2, pt3;
            Point3d insPt1, insPt2;
            Point2d xPt1, xPt2, xPt3;
            ed.WriteMessage("\n百福工具箱——绘制柜桶储物门立面");

            string curLayerName = db.GetCurrentLayerName();
            //db.SetCurrentLayer("BF-产品线");
            PromptPointOptions ppo1 = new PromptPointOptions("\n请绘制柜桶储物门的起始点");
            PromptPointResult ppr1 = ed.GetPoint(ppo1);
            if (ppr1.Status == PromptStatus.OK)
            {
                Point3d spt = ppr1.Value;                
                PromptPointResult ppr2 = ed.GetCorner("\n请绘制柜桶储物门的对角点", spt);

                if (ppr2.Status == PromptStatus.OK)
                {
                    Point3d ept = ppr2.Value;
                    
                    pt1 = new Point2d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
                    pt2 = new Point2d(Math.Max(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
                    pt3 = new Point2d(Math.Max(spt.X, ept.X), Math.Max(spt.Y, ept.Y));
                    //pt4 = new Point2d(Math.Min(spt.X, ept.X), Math.Max(spt.Y, ept.Y));
                    Vector2d vector = pt3 - pt1;
                    int cwmsl;
                    double cwmkd;
                    PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("\n是否有抽屉组");
                    pKeyOpts.Keywords.Add("Yes");
                    pKeyOpts.Keywords.Add("No");
                    pKeyOpts.Keywords.Default = "No";
                    pKeyOpts.AllowNone = false;
                    PromptResult promptResult = ed.GetKeywords(pKeyOpts);
                    if (promptResult.StringResult == "Yes")
                    {
                        PromptKeywordOptions pko = new PromptKeywordOptions("\n抽屉布置方向<H横向><V竖向>");
                        pko.Keywords.Add("H");
                        pko.Keywords.Add("V");
                        pko.Keywords.Default = "V";
                        pko.AllowNone = false;
                        PromptResult pr = ed.GetKeywords(pko);
                        if (pr.StringResult == "H")
                        {
                            //判断储物门数量                        
                            if (Math.Abs(vector.X) <= 450) cwmsl = 1;
                            else if (Math.Abs(vector.X) <= 903) cwmsl = 2;
                            else if (Math.Abs(vector.X) <= 1356) cwmsl = 3;
                            else cwmsl = 4;

                            //计算储物门宽度
                            if (cwmsl != 4)
                            {
                                cwmkd = (vector.X - (cwmsl - 1) * 3) / cwmsl;
                                //绘制储物门和抽屉边框线
                                insPt1 = new Point3d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y), 0);
                                insPt2 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd, Math.Min(spt.Y, ept.Y) + vector.Y - 130, 0);
                                for (int i = 0; i < cwmsl; i++)
                                {
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    insPt1 = new Point3d(insPt1.X, insPt2.Y + 10, 0);
                                    insPt2 = new Point3d(insPt2.X, insPt2.Y + 130, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    zgsPt = new Point3d(insPt1.X + cwmkd / 2, insPt2.Y - 37, 0);
                                    using (Transaction trans = db.TransactionManager.StartTransaction())
                                    {
                                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                        db.ImportBlocksFromDWG(blockPath, blockName);
                                        spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                        trans.Commit();
                                    }
                                    insPt1 = new Point3d(insPt1.X + cwmkd + 3, pt1.Y, 0);
                                    insPt2 = new Point3d(insPt2.X + cwmkd + 3, pt3.Y - 130, 0);
                                }
                                //绘制储物开门示意线
                                xPt1 = new Point2d(pt1.X + cwmkd, pt3.Y - 130);
                                xPt2 = new Point2d(pt1.X, xPt1.Y - (vector.Y - 130) / 2);
                                xPt3 = new Point2d(pt1.X + cwmkd, pt1.Y);
                                for (int i = 0; i < cwmsl; i++)
                                {
                                    db.AddPolyLineToModeSpace("BF-虚线", false, xPt1, xPt2, xPt3);
                                    switch (i)
                                    {
                                        case 0:
                                            xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                            zgsPt = new Point3d(xPt1.X + 50, xPt1.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            break;
                                        case 1:
                                            xPt1 = new Point2d(xPt1.X + 3 + cwmkd, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 3 + cwmkd, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 3 + cwmkd, xPt3.Y);
                                            if (cwmsl == 3)
                                            {
                                                zgsPt = new Point3d(xPt1.X + 50, xPt1.Y - 37, 0);
                                                using (Transaction trans = db.TransactionManager.StartTransaction())
                                                {
                                                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                    db.ImportBlocksFromDWG(blockPath, blockName);
                                                    spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                    trans.Commit();
                                                }
                                            }                                            
                                            break;
                                        case 2:
                                            xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                cwmkd = (vector.X - 17) / cwmsl;
                                //绘制储物门和抽屉边框线
                                insPt1 = new Point3d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y), 0);
                                insPt2 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd, Math.Min(spt.Y, ept.Y) + vector.Y - 130, 0);
                                for (int i = 0; i < cwmsl; i++)
                                {
                                    if (i == 2)
                                    {
                                        insPt1 = new Point3d(insPt1.X + 8, insPt1.Y, 0);
                                        insPt2 = new Point3d(insPt2.X + 8, insPt2.Y, 0);
                                    }
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    insPt1 = new Point3d(insPt1.X, insPt2.Y + 10, 0);
                                    insPt2 = new Point3d(insPt2.X, insPt2.Y + 130, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    insPt1 = new Point3d(insPt1.X + cwmkd + 3, pt1.Y, 0);
                                    insPt2 = new Point3d(insPt2.X + cwmkd + 3, pt3.Y - 130, 0);
                                }
                                //绘制储物开门示意线
                                xPt1 = new Point2d(pt1.X + cwmkd, pt3.Y - 130);
                                xPt2 = new Point2d(pt1.X, xPt1.Y - (vector.Y - 130) / 2);
                                xPt3 = new Point2d(pt1.X + cwmkd, pt1.Y);
                                for (int i = 0; i < cwmsl; i++)
                                {
                                    db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);
                                    switch (i)
                                    {
                                        case 0:
                                            xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                            zgsPt = new Point3d(xPt1.X + 50, xPt1.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            break;
                                        case 1:
                                            xPt1 = new Point2d(xPt1.X + 11 + 2 * cwmkd, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 11, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 11 + 2 * cwmkd, xPt3.Y);
                                            break;
                                        case 2:
                                            xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                            xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                            xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                            zgsPt = new Point3d(xPt1.X + 50, xPt1.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //判断储物门数量                            
                            if (Math.Abs(vector.X) <= 903) cwmsl = 1;
                            else if (Math.Abs(vector.X) <= 1356) cwmsl = 2;
                            else cwmsl = 3;
                            //计算储物门宽度
                            cwmkd = ((vector.X -460) - cwmsl * 3) / cwmsl;
                            //计算抽屉高度
                            double ctgd = (vector.Y - 6) / 3;
                            insPt1 = new Point3d(pt1.X, pt1.Y, 0);
                            insPt2 = new Point3d(insPt1.X + 460, insPt1.Y + ctgd, 0);
                            
                            switch (cwmsl)
                            {
                                case 1:
                                    //绘制抽屉和储物门边框线
                                    for (int i = 0; i < 3; i++)
                                    {
                                        db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                        zgsPt = new Point3d(insPt1.X + 230, insPt2.Y - 37, 0);
                                        using(Transaction trans = db.TransactionManager.StartTransaction())
                                        {
                                            ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                            db.ImportBlocksFromDWG(blockPath, blockName);
                                            spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                            trans.Commit();
                                        }
                                        insPt1 = new Point3d(insPt1.X, insPt1.Y + ctgd + 3, 0);
                                        insPt2 = new Point3d(insPt2.X, insPt2.Y + ctgd + 3, 0);
                                    }
                                    insPt1 = new Point3d(insPt1.X +463, pt2.Y, 0);
                                    insPt2 = new Point3d(pt3.X, pt3.Y, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    zgsPt = new Point3d(insPt1.X +50, insPt2.Y - 37, 0);
                                    using (Transaction trans = db.TransactionManager.StartTransaction())
                                    {
                                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                        db.ImportBlocksFromDWG(blockPath, blockName);
                                        spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                        trans.Commit();
                                    }
                                    //绘制开门示意线
                                    xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                    xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                    xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                    db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);
                                    break;
                                case 2:
                                    PromptKeywordOptions pko1 = new PromptKeywordOptions("\n请确定抽屉布置的位置");
                                    pko1.Keywords.Add("L左侧");
                                    pko1.Keywords.Add("M中间");
                                    pko1.Keywords.Add("R右侧");
                                    pko1.Keywords.Default = "L左侧";
                                    pko1.AllowNone = false;
                                    PromptResult promptResult1 = ed.GetKeywords(pko1);
                                    switch (promptResult1.StringResult)
                                    {
                                        case "L左侧":
                                            //绘制抽屉和储物门边框线和开门示意线
                                            for (int i = 0; i < 3; i++)
                                            {
                                                db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                                zgsPt = new Point3d(insPt1.X + 230, insPt2.Y - 37, 0);
                                                using (Transaction trans = db.TransactionManager.StartTransaction())
                                                {
                                                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                    db.ImportBlocksFromDWG(blockPath, blockName);
                                                    spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                    trans.Commit();
                                                }
                                                insPt1 = new Point3d(insPt1.X, insPt1.Y + ctgd + 3, 0);
                                                insPt2 = new Point3d(insPt2.X, insPt2.Y + ctgd + 3, 0);
                                            }
                                            insPt1 = new Point3d(insPt1.X + 463, pt2.Y, 0);
                                            insPt2 = new Point3d(insPt1.X+cwmkd, pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            xPt1 = new Point2d(insPt2.X, insPt1.Y);
                                            xPt2 = new Point2d(insPt1.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt2.X, insPt2.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);

                                            insPt1 = new Point3d(insPt1.X + cwmkd + 3, pt2.Y, 0);
                                            insPt2 = new Point3d(pt3.X , pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            zgsPt = new Point3d(insPt1.X + 50, insPt2.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                            xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);

                                            break;
                                        case "M中间":
                                            insPt2 = new Point3d(insPt1.X + cwmkd, pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            zgsPt = new Point3d(insPt2.X - 50, insPt2.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            xPt1 = new Point2d(insPt2.X, insPt2.Y);
                                            xPt2 = new Point2d(insPt1.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt2.X, insPt1.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);

                                            insPt1 = new Point3d(insPt2.X+3, pt1.Y, 0);
                                            insPt2 = new Point3d(insPt1.X + 460, pt1.Y + ctgd, 0);
                                            for (int i = 0; i < 3; i++)
                                            {
                                                db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                                zgsPt = new Point3d(insPt1.X + 230, insPt2.Y - 37, 0);
                                                using (Transaction trans = db.TransactionManager.StartTransaction())
                                                {
                                                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                    db.ImportBlocksFromDWG(blockPath, blockName);
                                                    spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                    trans.Commit();
                                                }
                                                insPt1 = new Point3d(insPt1.X, insPt1.Y + ctgd + 3, 0);
                                                insPt2 = new Point3d(insPt2.X, insPt2.Y + ctgd + 3, 0);
                                            }
                                            insPt1 = new Point3d(insPt1.X + 463, pt2.Y, 0);
                                            insPt2 = new Point3d(pt3.X, pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            zgsPt = new Point3d(insPt1.X + 50, insPt2.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                            xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);
                                            break;
                                        case "R右侧":
                                            insPt2 = new Point3d(insPt1.X + cwmkd, pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            xPt1 = new Point2d(insPt2.X, insPt2.Y);
                                            xPt2 = new Point2d(insPt1.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt2.X, insPt1.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线", false,xPt1, xPt2, xPt3);
                                            insPt1 = new Point3d(insPt1.X + 3 + cwmkd, pt1.Y, 0);
                                            insPt2 = new Point3d(insPt2.X + cwmkd + 3, pt3.Y, 0);
                                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                            zgsPt = new Point3d(insPt1.X + 50, insPt2.Y - 37, 0);
                                            using (Transaction trans = db.TransactionManager.StartTransaction())
                                            {
                                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                db.ImportBlocksFromDWG(blockPath, blockName);
                                                spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                trans.Commit();
                                            }
                                            xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                            xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                            xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                            db.AddPolyLineToModeSpace("BF-虚线", false,xPt1, xPt2, xPt3);
                                            insPt1 = new Point3d(insPt2.X + 3, pt1.Y, 0);
                                            insPt2 = new Point3d(insPt1.X + 460, pt1.Y + ctgd, 0);
                                            for (int i = 0; i < 3; i++)
                                            {
                                                db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                                zgsPt = new Point3d(insPt1.X + 230, insPt2.Y - 37, 0);
                                                using (Transaction trans = db.TransactionManager.StartTransaction())
                                                {
                                                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                                    db.ImportBlocksFromDWG(blockPath, blockName);
                                                    spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                                    trans.Commit();
                                                }
                                                insPt1 = new Point3d(insPt1.X, insPt1.Y + ctgd + 3, 0);
                                                insPt2 = new Point3d(insPt2.X, insPt2.Y + ctgd + 3, 0);
                                            }
                                            break;
                                    }
                                    break;
                                case 3:
                                    insPt2 = new Point3d(insPt1.X + cwmkd, pt3.Y, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    xPt1 = new Point2d(insPt2.X, insPt2.Y);
                                    xPt2 = new Point2d(insPt1.X, insPt1.Y + vector.Y / 2);
                                    xPt3 = new Point2d(insPt2.X, insPt1.Y);
                                    db.AddPolyLineToModeSpace("BF-虚线", false,xPt1, xPt2, xPt3);
                                    insPt1 = new Point3d(insPt1.X + 3 + cwmkd, pt1.Y, 0);
                                    insPt2 = new Point3d(insPt2.X + cwmkd + 3, pt3.Y, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    zgsPt = new Point3d(insPt1.X + 50, insPt2.Y - 37, 0);
                                    using (Transaction trans = db.TransactionManager.StartTransaction())
                                    {
                                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                        db.ImportBlocksFromDWG(blockPath, blockName);
                                        spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                        trans.Commit();
                                    }
                                    xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                    xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                    xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                    db.AddPolyLineToModeSpace("BF-虚线",false, xPt1, xPt2, xPt3);
                                    insPt1 = new Point3d(insPt2.X + 3, pt1.Y, 0);
                                    insPt2 = new Point3d(insPt1.X + 460, pt1.Y + ctgd, 0);
                                    for (int i = 0; i < 3; i++)
                                    {
                                        db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                        zgsPt = new Point3d(insPt1.X + 230, insPt2.Y - 37, 0);
                                        using (Transaction trans = db.TransactionManager.StartTransaction())
                                        {
                                            ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                            db.ImportBlocksFromDWG(blockPath, blockName);
                                            spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                            trans.Commit();
                                        }
                                        insPt1 = new Point3d(insPt1.X, insPt1.Y + ctgd + 3, 0);
                                        insPt2 = new Point3d(insPt2.X, insPt2.Y + ctgd + 3, 0);
                                    }
                                    insPt1 = new Point3d(insPt1.X + 463, pt2.Y, 0);
                                    insPt2 = new Point3d(pt3.X, pt3.Y, 0);
                                    db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                                    zgsPt = new Point3d(insPt1.X + 50, insPt2.Y - 37, 0);
                                    using (Transaction trans = db.TransactionManager.StartTransaction())
                                    {
                                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                        db.ImportBlocksFromDWG(blockPath, blockName);
                                        spaceId.InsertBlockReference("0", blockName, zgsPt, new Scale3d(1), 0);
                                        trans.Commit();
                                    }
                                    xPt1 = new Point2d(insPt1.X, insPt2.Y);
                                    xPt2 = new Point2d(insPt2.X, insPt1.Y + vector.Y / 2);
                                    xPt3 = new Point2d(insPt1.X, insPt1.Y);
                                    db.AddPolyLineToModeSpace("BF-虚线", false,xPt1, xPt2, xPt3);
                                    break;
                            }                            
                        }
                    }
                    else
                    {                       
                        //判断储物门数量                        
                        if (Math.Abs(vector.X) <= 450) cwmsl = 1;
                        else if (Math.Abs(vector.X) <= 903) cwmsl = 2;
                        else if (Math.Abs(vector.X) <= 1356) cwmsl = 3;
                        else cwmsl = 4;

                        //计算储物门宽度
                        cwmkd = (Math.Abs(vector.X) - (cwmsl - 1) * 3) / cwmsl;
                        
                        //绘制储物门边线
                        insPt1 = new Point3d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y), 0);
                        insPt2 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd, Math.Min(spt.Y, ept.Y) + Math.Abs(vector.Y), 0);
                        for (int i = 0; i < cwmsl; i++)
                        {
                            db.AddRecToModeSpace(insPt1, insPt2, "BF-产品线");
                            insPt1 = new Point3d(insPt1.X + cwmkd + 3, insPt1.Y, 0);
                            insPt2 = new Point3d(insPt2.X + cwmkd + 3, insPt2.Y, 0);
                        }
                        //绘制储物开门示意线
                        xPt1 = new Point2d(Math.Min(spt.X, ept.X) + cwmkd, Math.Min(spt.Y, ept.Y) + Math.Abs(vector.Y));
                        xPt2 = new Point2d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y) + Math.Abs(vector.Y) / 2);
                        xPt3 = new Point2d(Math.Min(spt.X, ept.X) + cwmkd, Math.Min(spt.Y, ept.Y));
                        for (int i = 0; i < cwmsl; i++)
                        {
                            db.AddPolyLineToModeSpace("BF-虚线", false,xPt1, xPt2, xPt3);
                            switch (i)
                            {
                                case 0:
                                    xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                    xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                    xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                    break;
                                case 1:
                                    if (cwmsl == 3)
                                    {
                                        xPt1 = new Point2d(xPt1.X + 3 + cwmkd, xPt1.Y);
                                        xPt2 = new Point2d(xPt2.X + 3 + cwmkd, xPt2.Y);
                                        xPt3 = new Point2d(xPt3.X + 3 + cwmkd, xPt3.Y);
                                    }
                                    else
                                    {
                                        xPt1 = new Point2d(xPt1.X + 3 + 2 * cwmkd, xPt1.Y);
                                        xPt2 = new Point2d(xPt2.X + 3, xPt2.Y);
                                        xPt3 = new Point2d(xPt3.X + 3 + 2 * cwmkd, xPt3.Y);
                                    }
                                    break;
                                case 2:
                                    xPt1 = new Point2d(xPt1.X + 3, xPt1.Y);
                                    xPt2 = new Point2d(xPt2.X + 3 + 2 * cwmkd, xPt2.Y);
                                    xPt3 = new Point2d(xPt3.X + 3, xPt3.Y);
                                    break;
                                default:
                                    break;
                            }
                        }

                        //插入储物门锁图块
                        using(Transaction trans = db.TransactionManager.StartTransaction())
                        {
                            //BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                            ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                            db.ImportBlocksFromDWG(blockPath, blockName);
                            switch (cwmsl)
                            {
                                case 1:
                                    insPt1 = new Point3d(Math.Min(spt.X, ept.X) + 50, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt1, new Scale3d(1), 0);
                                    break;
                                case 2:
                                    insPt1 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd + 53, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt1, new Scale3d(1), 0);
                                    break;
                                case 3:
                                    insPt1 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd + 53, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    insPt2 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd * 2 + 56, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt1, new Scale3d(1), 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt2, new Scale3d(1), 0);
                                    break;
                                case 4:
                                    insPt1 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd + 53, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    insPt2 = new Point3d(Math.Min(spt.X, ept.X) + cwmkd * 3 + 59, Math.Max(spt.Y, ept.Y) - 37, 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt1, new Scale3d(1), 0);
                                    spaceId.InsertBlockReference("0", blockName, insPt2, new Scale3d(1), 0);
                                    break;
                                default:
                                    break;
                            }
                            trans.Commit();
                        }                       
                    }
                }
                else
                {
                    db.SetCurrentLayer(curLayerName);
                    return;
                }
            }
            else
            {
                db.SetCurrentLayer(curLayerName);
                return;
            }
            db.SetCurrentLayer(curLayerName);
        }

        //卡布灯箱剖面
        [CommandMethod("DXPM")]
        public void DXPM()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("百福工具箱——快速绘制卡布灯箱顶剖图");
            string curLayerName = db.GetCurrentLayerName();
            Point3d pt1, pt2;
            PromptPointOptions ppo = new PromptPointOptions("\n请选择起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                pt1 = ppr.Value;
                PromptPointOptions ppo1;
                PromptPointResult ppr1;
                do
                {
                    ppo1 = new PromptPointOptions("\n请选择终止点")
                    {
                        UseBasePoint = true,
                        BasePoint = pt1
                    };
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                pt2 = ppr1.Value;
                Vector2d vector = new Point2d(pt2.X, pt2.Y) - new Point2d(pt1.X, pt1.Y);
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\铝型材标准块库.dwg";
                    string block1Path = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
                    db.ImportBlocksFromDWG(blockPath, "LC156");
                    db.ImportBlocksFromDWG(block1Path, "LED硬灯条_端面");
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (bt.Has("LC156"))
                    {
                        //插入第一个图块
                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                        spaceId.InsertBlockReference("BF-铝材", "LC156", pt1, new Scale3d(1), vector.Angle);

                        //插入第2个图块并镜像
                        spaceId.InsertBlockReference("BF-铝材", "LC156", pt2, new Scale3d(1), vector.Angle).MirrorEntity(pt2, pt2.Polar(vector.Angle + Math.PI * 0.5, 100), true);

                        //绘制底板
                        Point2d lspt1 = new Point2d(pt1.X, pt1.Y).Polar(vector.Angle + Math.Atan(45.0/8.0),Math.Sqrt(2089));
                        Point2d lspt2 = lspt1.Polar(vector.Angle, vector.Length - 16);
                        Point2d lspt3 = lspt2.Polar(vector.Angle + Math.PI * 0.5, 9);
                        Point2d lspt4 = lspt1.Polar(vector.Angle + Math.PI * 0.5, 9);
                        db.AddPolyLineToModeSpace("BF-产品线", true, lspt1, lspt2, lspt3, lspt4);
                        //插入硬灯条截面图块
                        Vector2d vector1 = lspt2 - lspt1;
                        double nums = vector1.Length / 100;
                        int num = (int)nums;
                        double qscd = (vector1.Length - num * 100) / 2;
                        if (qscd <= 35)
                        {
                            qscd += 50;
                            num -= 1;
                        }
                        else if (qscd >= 85)
                        {
                            qscd -= 50;
                            num += 1;
                        }
                        for (int i = 0; i <= num; i++)
                        {
                            Point3d insertPt = new Point3d(lspt1.Polar(vector1.Angle, qscd + i * 100).X, lspt1.Polar(vector1.Angle, qscd + i * 100).Y, 0);
                            spaceId.InsertBlockReference("BF-灯具", "LED硬灯条_端面", insertPt, new Scale3d(1), vector1.Angle);                            
                        }
                        //绘制灯箱布
                        lspt1 = new Point2d(pt1.X, pt1.Y).Polar(vector.Angle + Math.Atan(10.0 / 9), Math.Sqrt(181));
                        lspt2 = lspt1.Polar(vector.Angle - Math.PI * 0.5, 15);
                        lspt3 = lspt2.Polar(vector.Angle, vector.Length - 18);
                        lspt4 = lspt3.Polar(vector.Angle + Math.PI * 0.5, 15);
                        db.AddPolyLineDXBToModeSpace("BF-细线", lspt1, lspt2, lspt3, lspt4);
                    }
                    trans.Commit();
                }
            }
            db.SetCurrentLayer(curLayerName);
        }

        //绘制矩形卡布灯箱顶剖图
        [CommandMethod("JXDXDP")]
        public void JXDXDP()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("百福工具箱——快速绘制矩形卡布灯箱顶剖图");
            string curLayerName = db.GetCurrentLayerName();
            Point3d pt1, pt2;
            PromptPointOptions ppo = new PromptPointOptions("\n请选择起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                pt1 = ppr.Value;
                PromptPointOptions ppo1;
                PromptPointResult ppr1;
                do
                {
                    ppo1 = new PromptPointOptions("\n请选择终止点")
                    {
                        UseBasePoint = true,
                        BasePoint = pt1
                    };
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                pt2 = ppr1.Value;
                Vector2d vector = new Point2d(pt2.X, pt2.Y) - new Point2d(pt1.X, pt1.Y);
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\铝型材标准块库.dwg";
                    string block1Path = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
                    db.ImportBlocksFromDWG(blockPath, "LC51&52");
                    db.ImportBlocksFromDWG(block1Path, "LED硬灯条_端面");
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    if (bt.Has("LC51&52"))
                    {
                        //插入第一个图块
                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                        spaceId.InsertBlockReference("BF-铝材", "LC51&52", pt1, new Scale3d(1), vector.Angle);

                        //插入第2个图块并镜像
                        spaceId.InsertBlockReference("BF-铝材", "LC51&52", pt2, new Scale3d(1), vector.Angle).MirrorEntity(pt2, pt2.Polar(vector.Angle + Math.PI * 0.5, 100), true);

                        //绘制底板
                        Point2d lspt1 = new Point2d(pt1.X, pt1.Y).Polar(vector.Angle + Math.Atan(50.0 / 18.0), Math.Sqrt(2824));
                        Point2d lspt2 = lspt1.Polar(vector.Angle, vector.Length - 36);
                        Point2d lspt3 = lspt2.Polar(vector.Angle + Math.PI * 0.5, 9);
                        Point2d lspt4 = lspt1.Polar(vector.Angle + Math.PI * 0.5, 9);
                        db.AddPolyLineToModeSpace("BF-产品线", true, lspt1, lspt2, lspt3, lspt4);
                        //插入硬灯条截面图块
                        Vector2d vector1 = lspt2 - lspt1;
                        double nums = vector1.Length / 100;
                        int num = (int)nums;
                        double qscd = (vector1.Length - num * 100) / 2;
                        if (qscd <= 35)
                        {
                            qscd += 50;
                            num -= 1;
                        }
                        else if (qscd >= 85)
                        {
                            qscd -= 50;
                            num += 1;
                        }
                        for (int i = 0; i <= num; i++)
                        {
                            Point3d insertPt = new Point3d(lspt1.Polar(vector1.Angle, qscd + i * 100).X, lspt1.Polar(vector1.Angle, qscd + i * 100).Y, 0);
                            spaceId.InsertBlockReference("BF-灯具", "LED硬灯条_端面", insertPt, new Scale3d(1), vector1.Angle);
                        }
                        //绘制灯箱布
                        lspt1 = new Point2d(pt1.X, pt1.Y).Polar(vector.Angle + Math.Atan(14.0 / 21), Math.Sqrt(638));
                        lspt2 = lspt1.Polar(vector.Angle - Math.PI * 0.5, 15);
                        lspt3 = lspt2.Polar(vector.Angle, vector.Length - 42);
                        lspt4 = lspt3.Polar(vector.Angle + Math.PI * 0.5, 15);
                        db.AddPolyLineDXBToModeSpace("BF-细线", lspt1, lspt2, lspt3, lspt4);
                    }
                    trans.Commit();
                }
            }
            db.SetCurrentLayer(curLayerName);
        }

        //绘制橱窗移门侧剖
        [CommandMethod("YMCP")]
        public void YMCP()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——绘制橱窗移门侧剖");
            Point3d spt, ept;
            string layerNameOld = db.GetCurrentLayerName();
            PromptPointOptions ppo = new PromptPointOptions("\n给定移门下起点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                spt = ppr.Value;
                PromptPointOptions ppo1 = new PromptPointOptions("\n给定移门最高点")
                {
                    UseBasePoint = true,
                    BasePoint = spt
                };
                PromptPointResult ppr1;
                do
                {
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                ept = ppr1.Value;                
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    //计算铝材相关插入点坐标
                    Point3d pt1 = new Point3d(spt.X + 22, spt.Y + 17, spt.Z);
                    Point3d pt2 = new Point3d(spt.X + 64.5, spt.Y + 17, spt.Z);
                    Point3d pt3 = new Point3d(spt.X, ept.Y, spt.Z);
                    Point3d pt4 = new Point3d(pt3.X + 22, pt3.Y - 23, pt3.Z);
                    Point3d pt5 = new Point3d(pt3.X + 64.5, pt3.Y - 23, pt3.Z);
                    Point3d pt6 = new Point3d(spt.X + 17, spt.Y + 63, spt.Z);
                    Point3d pt7 = new Point3d(spt.X + 17, ept.Y - 44, spt.Z);
                    Point3d pt8 = new Point3d(spt.X + 59.5, spt.Y + 63, spt.Z);
                    Point3d pt9 = new Point3d(spt.X + 59.5, ept.Y - 44, spt.Z);

                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间)
                    //insert blocks
                    string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\铝型材标准块库.dwg";
                    db.ImportBlocksFromDWG(blockPath, "LC131");
                    if (bt.Has("LC131"))
                    {
                        spaceId.InsertBlockReference("BF-铝材", "LC131", spt, new Scale3d(1), 0);
                    }
                    else
                    {
                        ed.WriteMessage("\n未找到名为LC131的图块");
                    }

                    db.ImportBlocksFromDWG(blockPath, "LC127");
                    if (bt.Has("LC127"))
                    {
                        spaceId.InsertBlockReference("BF-铝材", "LC127", pt1, new Scale3d(1), 0);
                        spaceId.InsertBlockReference("BF-铝材", "LC127", pt2, new Scale3d(1), 0);
                    }
                    else
                    {
                        ed.WriteMessage("\n未找到名为LC127的图块");
                    }

                    db.ImportBlocksFromDWG(blockPath, "LC129");
                    if (bt.Has("LC129"))
                    {
                        spaceId.InsertBlockReference("BF-铝材", "LC129", pt4, new Scale3d(1), Math.PI * 0.5);
                        spaceId.InsertBlockReference("BF-铝材", "LC129", pt5, new Scale3d(1), Math.PI * 0.5);
                    }
                    else
                    { 
                       ed.WriteMessage("\n未找到名为LC129的图块");
                    }

                    db.ImportBlocksFromDWG(blockPath, "LC130");
                    if (bt.Has("LC130"))
                    {
                        spaceId.InsertBlockReference("BF-铝材", "LC130", pt3, new Scale3d(1), 0);
                    }
                    else
                    {
                        ed.WriteMessage("\n未找到名为LC130的图块");
                    }
                    //drawing glass
                    db.OnlyOneGlass(pt6 ,pt7, 10);
                    db.OnlyOneGlass(pt8, pt9, 10);
                    //提交事务处理
                    trans.Commit();
                }                
            }
            else
            {
                ed.WriteMessage("\n你给的点不是一个合格点");
            }
            db.SetCurrentLayer(layerNameOld);
        }

        //绘制橱窗移门顶剖图
        [CommandMethod("YMDP")]
        public void YMDP()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——绘制橱窗移门侧剖");
            Point3d spt, ept;
            string layerNameOld = db.GetCurrentLayerName();
            PromptPointOptions ppo = new PromptPointOptions("\n给定起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                spt = ppr.Value;
                PromptPointOptions ppo1 = new PromptPointOptions("\n给定终止点")
                {
                    UseBasePoint = true,
                    BasePoint = spt
                };
                PromptPointResult ppr1;
                do
                {
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                ept = ppr1.Value;
                //确定移门数量
                Vector2d vector = new Point2d(ept.X, ept.Y) - new Point2d(spt.X, spt.Y);
                double nums = vector.Length / 800;
                int num = (int)nums;
                double ymkd = (vector.Length + ((num - 1) * 55)) / num;
                if ((vector.Length /num) > 800)
                {
                    num += 1;
                    ymkd = (vector.Length + ((num - 1) * 55)) / num;
                }
                //绘制移门轨道外框
                Point2d lspt1 = new Point2d(spt.X, spt.Y);
                Point2d lspt2 = new Point2d(ept.X, ept.Y);
                Point2d lspt3 = lspt2.Polar(vector.Angle + Math.PI * 1.5, 86.5);
                Point2d lspt4 = lspt1.Polar(vector.Angle + Math.PI * 1.5, 86.5);
                db.AddPolyLineToModeSpace("BF-铝材", true, lspt1, lspt2, lspt3, lspt4);
                //绘制移门顶剖面
                using(Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                    BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间)
                    //insert blocks
                    string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\铝型材标准块库.dwg";
                    db.ImportBlocksFromDWG(blockPath, "LC126");
                    Point3d insertPt1 = spt.Polar(vector.Angle + Math.PI * 1.5, 22);
                    Point3d insertPt2 = insertPt1.Polar(vector.Angle, ymkd);
                    Point3d insertPt3 = insertPt1.Polar(vector.Angle + Math.Atan(5.0 / 40),Math.Sqrt(1625));
                    Point3d insertPt4 = insertPt3.Polar(vector.Angle, ymkd - 80);
                    for (int i = 0; i < num; i++)
                    {
                        spaceId.InsertBlockReference("BF-铝材", "LC126", insertPt1, new Scale3d(1), vector.Angle);
                        spaceId.InsertBlockReference("BF-铝材", "LC126", insertPt2, new Scale3d(1), vector.Angle + Math.PI);
                        db.OnlyOneGlass(insertPt3, insertPt4, 10);
                        if (i%2 != 0)
                        {
                            insertPt1 = insertPt1.Polar(vector.Angle + Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt2 = insertPt2.Polar(vector.Angle + Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt3 = insertPt3.Polar(vector.Angle + Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt4 = insertPt4.Polar(vector.Angle + Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                        }
                        else
                        {
                            insertPt1 = insertPt1.Polar(vector.Angle - Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt2 = insertPt2.Polar(vector.Angle - Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt3 = insertPt3.Polar(vector.Angle - Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                            insertPt4 = insertPt4.Polar(vector.Angle - Math.Atan(42.5 / (ymkd - 55)), Math.Sqrt(1806.25 + (ymkd - 55) * (ymkd - 55)));
                        }
                    }
                    trans.Commit();
                }
            }
            db.SetCurrentLayer(layerNameOld);
        }

        //绘制橱窗移门立面（玻璃填充未搞定，最好将图元的顺序调整一下）
        [CommandMethod("YM")]
        public void YM()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——绘制橱窗移门侧剖");
            Point3d spt, ept;
            string layerNameOld = db.GetCurrentLayerName();
            PromptPointOptions ppo = new PromptPointOptions("\n给定起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                spt = ppr.Value;
                PromptPointResult ppr1;
                do
                {
                    ppr1 = ed.GetCorner("\n给定终止点",spt);
                } while (ppr1.Status != PromptStatus.OK);
                ept = ppr1.Value;
                //确定移门数量                
                double kd = Math.Max(spt.X, ept.X) - Math.Min(spt.X, ept.X);
                double nums = kd / 800;
                int num = (int)nums;
                double ymkd = (kd + ((num - 1) * 55)) / num;
                if ((kd / num) > 800)
                {
                    num += 1;
                    ymkd = (kd + ((num - 1) * 55)) / num;
                }
                //绘制移门立面框架——底轨
                Point2d[] points = new Point2d[]
                {
                    new Point2d(Math.Min(spt.X,ept.X),Math.Min(spt.Y,ept.Y)),
                    new Point2d(Math.Max(spt.X,ept.X),Math.Min(spt.Y,ept.Y)),
                    new Point2d(Math.Max(spt.X,ept.X),Math.Min(spt.Y,ept.Y)+12),
                    new Point2d(Math.Min(spt.X,ept.X),Math.Min(spt.Y,ept.Y)+12)
                };
                db.AddPolyLineToModeSpace("BF-铝材",true,points);
                //绘制移门立面框架——顶轨
                points = new Point2d[]
                {
                    new Point2d(Math.Min(spt.X,ept.X),Math.Max(spt.Y,ept.Y)),
                    new Point2d(Math.Min(spt.X,ept.X),Math.Max(spt.Y,ept.Y)-35),
                    new Point2d(Math.Max(spt.X,ept.X),Math.Max(spt.Y,ept.Y)-35),
                    new Point2d(Math.Max(spt.X,ept.X),Math.Max(spt.Y,ept.Y))
                };
                db.AddPolyLineToModeSpace("BF-铝材", true, points);
                //绘制移门
                points = new Point2d[]
                {
                    new Point2d(Math.Min(spt.X,ept.X),Math.Min(spt.Y,ept.Y)+17),
                    new Point2d(Math.Min(spt.X,ept.X)+55,Math.Min(spt.Y,ept.Y)+17),
                    new Point2d(Math.Min(spt.X,ept.X)+55,Math.Max(spt.Y,ept.Y)-35),
                    new Point2d(Math.Min(spt.X,ept.X),Math.Max(spt.Y,ept.Y)-35)
                };
                db.AddPolyLineToModeSpace("BF-铝材", true, points);
                Point2d[] points1 = new Point2d[]
                {
                    points[1],
                    points[1].Polar(0,ymkd-110),
                    points[1].Polar(Math.Atan(60.0/(ymkd-110)),Math.Sqrt(3600+(ymkd-110)*(ymkd-110))),
                    points[1].Polar(Math.PI*0.5,60)
                };
                Point2d[] points2 = new Point2d[]
                {
                    points1[3],
                    points1[2],
                    points1[2].Polar(Math.PI*0.5,Math.Abs(spt.Y - ept.Y) -135),
                    points1[3].Polar(Math.PI*0.5,Math.Abs(spt.Y - ept.Y) -135)
                };
                Point2d[] points3 = new Point2d[]
                {
                    points2[3],
                    points2[2],
                    points2[2].Polar(Math.PI*0.5,23),
                    points2[3].Polar(Math.PI*0.5,23)
                };

                for (int i = 0; i < num; i++)
                {
                    db.AddPolyLineToModeSpace("BF-玻璃", true, points2);
                    points2[0] = points2[0].Polar(0, ymkd - 55);
                    points2[1] = points2[1].Polar(0, ymkd - 55);
                    points2[2] = points2[2].Polar(0, ymkd - 55);
                    points2[3] = points2[3].Polar(0, ymkd - 55);

                    db.AddPolyLineToModeSpace("BF-铝材", true, points1);
                    points1[0] = points1[0].Polar(0, ymkd - 55);
                    points1[1] = points1[1].Polar(0, ymkd - 55);
                    points1[2] = points1[2].Polar(0, ymkd - 55);
                    points1[3] = points1[3].Polar(0, ymkd - 55);

                    db.AddPolyLineToModeSpace("BF-铝材", true, points3);
                    points3[0] = points3[0].Polar(0, ymkd - 55);
                    points3[1] = points3[1].Polar(0, ymkd - 55);
                    points3[2] = points3[2].Polar(0, ymkd - 55);
                    points3[3] = points3[3].Polar(0, ymkd - 55);

                    points[0] = points[0].Polar(0, ymkd - 55);
                    points[1] = points[1].Polar(0, ymkd - 55);
                    points[2] = points[2].Polar(0, ymkd - 55);
                    points[3] = points[3].Polar(0, ymkd - 55);
                    db.AddPolyLineToModeSpace("BF-铝材", true, points);
                }
            }
            db.SetCurrentLayer(layerNameOld);
        }
    }
}
