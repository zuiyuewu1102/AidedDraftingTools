using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using Autodesk.AutoCAD.ApplicationServices;

using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using System.Windows.Media.Imaging;

namespace CommonClassLibrary
{
    /// <summary>
    /// 辅助操作类
    /// </summary>
    public static class Tools
    {
        public static double Deg2Rad(double deg)
        {
            double rad = deg * 180 / Math.PI;
            return rad;
        }
        /// <summary>
        /// 判断字符串是否为数字
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>如果字符串为数字，返回true，否则返回false</returns>
        public static bool IsNumeric(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        /// <summary>
        /// 判断字符串是否为整数
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>如果字符串为整数，返回true，否则返回false</returns>
        public static bool IsInt(this string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }

        /// <summary>
        /// 获取当前.NET程序所在的目录
        /// </summary>
        /// <returns>返回当前.NET程序所在的目录</returns>
        public static string GetCurrentPath()
        {
            var moudle = System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0];
            return System.IO.Path.GetDirectoryName(moudle.FullyQualifiedName);
        }

        /// <summary>
        /// 判断字符串是否为空或空白
        /// </summary>
        /// <param name="value">字符串</param>
        /// <returns>如果字符串为空或空白，返回true，否则返回false</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (value == null) return false;
            return string.IsNullOrEmpty(value.Trim());
        }

        /// <summary>
        /// 获取模型空间的ObjectId
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <returns>返回模型空间的ObjectId</returns>
        public static ObjectId GetModelSpaceId(this Database db)
        {
            return SymbolUtilityServices.GetBlockModelSpaceId(db);
        }

        /// <summary>
        /// 获取图纸空间的ObjectId
        /// </summary>
        /// <param name="db"></param>
        /// <returns>返回图纸空间的ObjectId</returns>
        public static ObjectId GetPaperSpaceId(this Database db)
        {
            return SymbolUtilityServices.GetBlockPaperSpaceId(db);
        }

        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ent">要添加的实体</param>
        /// <returns>返回添加到模型空间中的实体ObjectId</returns>
        public static ObjectId AddToModelSpace(this Database db, Entity ent)
        {
            ObjectId entId;//用于返回添加到模型空间中的实体ObjectId
            //定义一个指向当前数据库的事务处理，以添加直线
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //以读方式打开块表
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //以写方式打开模型空间块表记录.
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                entId = btr.AppendEntity(ent);//将图形对象的信息添加到块表记录中
                trans.AddNewlyCreatedDBObject(ent, true);//把对象添加到事务处理中
                trans.Commit();//提交事务处理
            }
            return entId; //返回实体的ObjectId
        }

        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到模型空间中的实体ObjectId集合</returns>
        public static ObjectIdCollection AddToModelSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite);
            foreach (var ent in ents)
            {
                ids.Add(btr.AppendEntity(ent));
                trans.AddNewlyCreatedDBObject(ent, true);
            }
            btr.DowngradeOpen();
            return ids;
        }
        /// <summary>
        /// 将矩形多段线添加到图纸空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="spt">矩形的起始点</param>
        /// <param name="ept">矩形的对角点</param>
        /// <param name="layerName">绘制图形所在图层名</param>
        public static void AddRecToModeSpace(this Database db,Point3d spt,Point3d ept,string layerName)
        {
            Point2d pt0 = new Point2d(Math.Min(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
            Point2d pt1 = new Point2d(Math.Max(spt.X, ept.X), Math.Min(spt.Y, ept.Y));
            Point2d pt2 = new Point2d(Math.Max(spt.X, ept.X), Math.Max(spt.Y, ept.Y));
            Point2d pt3 = new Point2d(Math.Min(spt.X, ept.X), Math.Max(spt.Y, ept.Y));
            db.SetCurrentLayer(layerName);
            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                Polyline pl = new Polyline();
                pl.AddVertexAt(0, pt0, 0, 0, 0);
                pl.AddVertexAt(1, pt1, 0, 0, 0);
                pl.AddVertexAt(2, pt2, 0, 0, 0);
                pl.AddVertexAt(3, pt3, 0, 0, 0);
                pl.Closed = true;
                btr.AppendEntity(pl);
                trans.AddNewlyCreatedDBObject(pl, true);
                trans.Commit();
            }
        }

        /// <summary>
        /// 将多段线添加到图纸空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="layerName">绘制图形所在图层名</param>
        /// <param name="points">点的集合</param>
        public static void AddPolyLineToModeSpace(this Database db,string layerName,bool isClose, params Point2d[] points)
        {
            db.SetCurrentLayer(layerName);
            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                Polyline pl = new Polyline();
                int i = 0;
                foreach (Point2d pt in points)
                {
                    pl.AddVertexAt(i, pt, 0, 0, 0);
                    i++;
                }
                pl.Closed = isClose;
                pl.LinetypeScale = 3.0;
                btr.AppendEntity(pl);
                trans.AddNewlyCreatedDBObject(pl, true);
                trans.Commit();
            }
        }

        /// <summary>
        /// 将实体添加到图纸空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ent">要添加的实体</param>
        /// <returns>返回添加到图纸空间中的实体ObjectId</returns>
        public static ObjectId AddToPaperSpace(this Database db, Entity ent)
        {
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockPaperSpaceId(db), OpenMode.ForWrite);
            ObjectId id = btr.AppendEntity(ent);
            trans.AddNewlyCreatedDBObject(ent, true);
            btr.DowngradeOpen();
            return id;
        }

        /// <summary>
        /// 将实体添加到图纸空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到图纸空间中的实体ObjectId集合</returns>
        public static ObjectIdCollection AddToPaperSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockPaperSpaceId(db), OpenMode.ForWrite);
            foreach (var ent in ents)
            {
                ids.Add(btr.AppendEntity(ent));
                trans.AddNewlyCreatedDBObject(ent, true);
            }
            btr.DowngradeOpen();
            return ids;
        }

        /// <summary>
        /// 将实体添加到当前空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ent">要添加的实体</param>
        /// <returns>返回添加到当前空间中的实体ObjectId</returns>
        public static ObjectId AddToCurrentSpace(this Database db, Entity ent)
        {
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
            ObjectId id = btr.AppendEntity(ent);
            trans.AddNewlyCreatedDBObject(ent, true);
            btr.DowngradeOpen();
            return id;
        }

        /// <summary>
        /// 将实体添加到当前空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到当前空间中的实体ObjectId集合</returns>
        public static ObjectIdCollection AddToCurrentSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(db.CurrentSpaceId, OpenMode.ForWrite);
            foreach (var ent in ents)
            {
                ids.Add(btr.AppendEntity(ent));
                trans.AddNewlyCreatedDBObject(ent, true);
            }
            btr.DowngradeOpen();
            return ids;
        }

        /// <summary>
        /// 将字符串形式的句柄转化为ObjectId
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="handleString">句柄字符串</param>
        /// <returns>返回实体的ObjectId</returns>
        public static ObjectId HandleToObjectId(this Database db, string handleString)
        {
            Handle handle = new Handle(Convert.ToInt64(handleString, 16));
            ObjectId id = db.GetObjectId(false, handle, 0);
            return id;
        }

        /// <summary>
        /// 亮显实体
        /// </summary>
        /// <param name="ids">要亮显的实体的Id集合</param>
        public static void HighlightEntities(this ObjectIdCollection ids)
        {
            if (ids.Count == 0) return;
            var trans = ids[0].Database.TransactionManager;
            foreach (ObjectId id in ids)
            {
                Entity ent = trans.GetObject(id, OpenMode.ForRead) as Entity;
                if (ent != null)
                {
                    ent.Highlight();
                }
            }
        }

        /// <summary>
        /// 亮显选择集中的实体
        /// </summary>
        /// <param name="selectionSet">选择集</param>
        public static void HighlightEntities(this SelectionSet selectionSet)
        {
            if (selectionSet == null) return;
            ObjectIdCollection ids = new ObjectIdCollection(selectionSet.GetObjectIds());
            ids.HighlightEntities();
        }

        /// <summary>
        /// 取消亮显实体
        /// </summary>
        /// <param name="ids">实体的Id集合</param>
        public static void UnHighlightEntities(this ObjectIdCollection ids)
        {
            if (ids.Count == 0) return;
            var trans = ids[0].Database.TransactionManager;
            foreach (ObjectId id in ids)
            {
                Entity ent = trans.GetObject(id, OpenMode.ForRead) as Entity;
                if (ent != null)
                {
                    ent.Unhighlight();
                }
            }
        }

        /// <summary>
        /// 将字符串格式的点转换为Point3d格式
        /// </summary>
        /// <param name="stringPoint">字符串格式的点</param>
        /// <returns>返回对应的Point3d</returns>
        public static Point3d StringToPoint3d(this string stringPoint)
        {
            string[] strPoint = stringPoint.Trim().Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
            double x = Convert.ToDouble(strPoint[0]);
            double y = Convert.ToDouble(strPoint[1]);
            double z = Convert.ToDouble(strPoint[2]);
            return new Point3d(x, y, z);
        }

        /// <summary>
        /// 获取数据库对应的文档对象
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <returns>返回数据库对应的文档对象</returns>
        public static Document GetDocument(this Database db)
        {
            return Application.DocumentManager.GetDocument(db);
        }

        /// <summary>
        /// 根据数据库获取命令行对象
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <returns>返回命令行对象</returns>
        public static Editor GetEditor(this Database db)
        {
            return Application.DocumentManager.GetDocument(db).Editor;
        }

        /// <summary>
        /// 在命令行输出信息
        /// </summary>
        /// <param name="ed">命令行对象</param>
        /// <param name="message">要输出的信息</param>
        public static void WriteMessage(this Editor ed, object message)
        {
            ed.WriteMessage(message.ToString());
        }

        /// <summary>
        /// 在命令行输出信息，信息显示在新行上
        /// </summary>
        /// <param name="ed">命令行对象</param>
        /// <param name="message">要输出的信息</param>
        public static void WriteMessageWithReturn(this Editor ed, object message)
        {
            ed.WriteMessage("\n" + message.ToString());
        }

        /// <summary>
        /// 绘制唯一一段玻璃截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="spt">起始点坐标</param>
        /// <param name="ept">终止点坐标</param>
        /// <param name="t">玻璃厚度</param>
        public static void OnlyOneGlass(this Database db, Point3d spt, Point3d ept, int t)
        {
            Point2d lsPt1, lsPt2;
            lsPt1 = new Point2d(spt.X, spt.Y);
            lsPt2 = new Point2d(ept.X, ept.Y);
            Vector2d vector = lsPt2 - lsPt1;

            double sina = Math.Sin(vector.Angle);
            double cosa = Math.Cos(vector.Angle);

            List<Point2d> points = new List<Point2d>
            {
            new Point2d(lsPt1.X + cosa, lsPt1.Y + sina)
            };
            points.Add(new Point2d(points[0].X + (vector.Length - 2) * cosa, points[0].Y + (vector.Length - 2) * sina));
            points.Add(new Point2d(lsPt2.X + sina, lsPt2.Y - cosa));
            points.Add(new Point2d(lsPt2.X + sina * (t - 1), lsPt2.Y - cosa * (t - 1)));
            points.Add(new Point2d(points[1].X + sina * t, points[1].Y - cosa * t));
            points.Add(new Point2d(points[0].X + sina * t, points[0].Y - cosa * t));
            points.Add(new Point2d(lsPt1.X + sina * (t - 1), lsPt1.Y - cosa * (t - 1)));
            points.Add(new Point2d(lsPt1.X + sina, lsPt1.Y - cosa)); 
            db.AddPolyLineToModeSpace("BF-玻璃", true,points[0], points[1], points[2], points[3], points[4], points[5], points[6], points[7]);
        }

        /// <summary>
        /// 绘制第一段玻璃截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="pt1">起始点坐标</param>
        /// <param name="pt2">中间点坐标</param>
        /// <param name="pt3">终止点坐标</param>
        /// <param name="t">玻璃厚度</param>
        public static void No1Glass(this Database db, Point3d pt1, Point3d pt2, Point3d pt3, int t)
        {
            Point2d lsPt1, lsPt2, lsPt3;
            lsPt1 = new Point2d(pt1.X, pt1.Y);
            lsPt2 = new Point2d(pt2.X, pt2.Y);
            lsPt3 = new Point2d(pt3.X, pt3.Y);
            Vector2d vector1 = lsPt2 - lsPt1;
            Vector2d vector2 = lsPt3 - lsPt2;
            double xangle = (vector1.Angle + vector2.Angle - Math.PI) / 2;
            double zjxc = Math.Abs(0.5 / Math.Sin(vector1.Angle - xangle));
            double sina1 = Math.Sin(vector1.Angle);
            double cosa1 = Math.Cos(vector1.Angle);
            double xc = t / Math.Sin(vector1.Angle - xangle);

            List<Point2d> points = new List<Point2d>
            {
                new Point2d(lsPt1.X + cosa1, lsPt1.Y + sina1),
                lsPt2.Polar(vector1.Angle - Math.PI,zjxc)
            };
            points.Add(points[1].Polar(xangle, xc));
            points.Add(points[0].Polar(vector1.Angle - 0.5 * Math.PI, t));
            points.Add(points[3].Polar(vector1.Angle - 1.25 * Math.PI, Math.Sqrt(1)));
            points.Add(points[4].Polar(vector1.Angle - 1.5 * Math.PI, t-2));

            db.AddPolyLineToModeSpace("BF-玻璃", true, points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        /// <summary>
        /// 绘制中间段玻璃截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="points">坐标点集合</param>
        /// <param name="t">玻璃截面厚度</param>
        public static void ZJGlass(this Database db, List<Point3d> points,int t)
        {
            Point2d lsPt1, lsPt2, lsPt3,lsPt4;
            for (int i = 0; i < points.Count -3; i++)
            {
                lsPt1 = new Point2d(points[i].X, points[i].Y);
                lsPt2 = new Point2d(points[i + 1].X, points[i + 1].Y);
                lsPt3 = new Point2d(points[i + 2].X, points[i + 2].Y);
                lsPt4 = new Point2d(points[i + 3].X, points[i + 3].Y);
                Vector2d vector1 = lsPt2 - lsPt1;
                Vector2d vector2 = lsPt3 - lsPt2;
                Vector2d vector3 = lsPt4 - lsPt3;
                double xangle1 = (vector2.Angle - vector1.Angle - Math.PI) / 2 + vector1.Angle;
                double xangle2 = (vector3.Angle - vector2.Angle - Math.PI) / 2 + vector2.Angle;
                double xc1 = t / Math.Sin(vector1.Angle - xangle1);
                double xc2 = t / Math.Sin(vector2.Angle - xangle2);
                double jxcd1 =Math.Abs( 0.5 / Math.Sin(vector1.Angle - xangle1));
                double jxcd2 = Math.Abs(0.5 / Math.Sin(vector2.Angle - xangle2));
                List<Point2d> lspoints = new List<Point2d>
                {
                    lsPt2.Polar(vector2.Angle ,jxcd1),
                    lsPt3.Polar(vector2.Angle,-jxcd2)
                };
                lspoints.Add(lspoints[1].Polar(xangle2, xc2));
                lspoints.Add(lspoints[0].Polar(xangle1, xc1));
                db.AddPolyLineToModeSpace("BF-玻璃", true, lspoints[0], lspoints[1], lspoints[2], lspoints[3]);
            }
        }

        /// <summary>
        /// 绘制最后一段玻璃截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="pt1">起始点坐标</param>
        /// <param name="pt2">中间点坐标</param>
        /// <param name="pt3">终止点坐标</param>
        /// <param name="t">玻璃厚度</param>
        public static void LastGlass(this Database db, Point3d pt1, Point3d pt2, Point3d pt3, int t)
        {
            Point2d lsPt1, lsPt2, lsPt3;
            lsPt1 = new Point2d(pt1.X, pt1.Y);
            lsPt2 = new Point2d(pt2.X, pt2.Y);
            lsPt3 = new Point2d(pt3.X, pt3.Y);
            Vector2d vector1 = lsPt2 - lsPt1;
            Vector2d vector2 = lsPt3 - lsPt2;
            double xangle = (vector2.Angle + vector1.Angle + Math.PI) / 2;
            double zjxc = Math.Abs(0.5 / Math.Sin(vector1.Angle - xangle));
            //double sina = Math.Sin(vector2.Angle);
            //double cosa = Math.Cos(vector2.Angle);
            
            double xc = t / Math.Sin(vector1.Angle - xangle);

            List<Point2d> points = new List<Point2d>
            {
                lsPt2.Polar(vector2.Angle,zjxc),
                lsPt3.Polar(vector2.Angle+Math.PI,1),
                lsPt3.Polar(vector2.Angle+Math.PI*1.5,1),
                lsPt3.Polar(vector2.Angle+Math.PI*1.5,t-1)
            };
            points.Add(points[1].Polar(vector2.Angle + Math.PI * 1.5, t));
            points.Add(points[0].Polar(xangle, xc));

            db.AddPolyLineToModeSpace("BF-玻璃", true, points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        /// <summary>
        /// 绘制唯一一段石头板截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="spt">起始点坐标</param>
        /// <param name="ept">终止点坐标</param>
        /// <param name="t">玻璃厚度</param>
        public static void OnlyOneStb(this Database db, Point3d spt, Point3d ept, int t)
        {
            Point2d lsPt1, lsPt2;
            lsPt1 = new Point2d(spt.X, spt.Y);
            lsPt2 = new Point2d(ept.X, ept.Y);
            Vector2d vector = lsPt2 - lsPt1;

            double sina = Math.Sin(vector.Angle);
            double cosa = Math.Cos(vector.Angle);

            List<Point2d> points = new List<Point2d>
            {
            lsPt1,
            lsPt2,
            new Point2d(lsPt2.X + sina * (t - 2),lsPt2.Y - cosa * (t-2)),
            new Point2d(lsPt2.X + sina * (t - 2) - Math.Cos(Math.PI *0.25 + vector.Angle) * Math.Sqrt(8),lsPt2.Y - cosa * (t - 2) - Math.Sin(Math.PI *0.25 + vector.Angle) * Math.Sqrt(8)),
            new Point2d(lsPt1.X + sina * (t - 2) + Math.Sin(Math.PI *0.25 + vector.Angle) * Math.Sqrt(8),lsPt1.Y - cosa * (t-2)- Math.Cos(Math.PI *0.25 + vector.Angle) * Math.Sqrt(8)),
            new Point2d(lsPt1.X + sina * (t - 2),lsPt1.Y - cosa * (t-2)),
            };
            db.AddPolyLineToModeSpace("BF-石材", true, points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        /// <summary>
        /// 绘制第一段石头板截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="pt1">起始点坐标</param>
        /// <param name="pt2">中间点坐标</param>
        /// <param name="pt3">终止点坐标</param>
        /// <param name="t">石材厚度</param>
        public static void No1Stb(this Database db, Point3d pt1, Point3d pt2, Point3d pt3, int t)
        {
            Point2d lsPt1, lsPt2, lsPt3;
            lsPt1 = new Point2d(pt1.X, pt1.Y);
            lsPt2 = new Point2d(pt2.X, pt2.Y);
            lsPt3 = new Point2d(pt3.X, pt3.Y);
            Vector2d vector1 = lsPt2 - lsPt1;
            Vector2d vector2 = lsPt3 - lsPt2;

            double sina1 = Math.Sin(vector1.Angle);
            double cosa1 = Math.Cos(vector1.Angle);
            double xangle = (vector2.Angle + vector1.Angle + Math.PI) / 2;
            double xc = (t - 2) / Math.Sin(vector1.Angle - xangle);

            List<Point2d> points = new List<Point2d>
            {
                lsPt1,
                lsPt2,
                lsPt2.Polar(xangle,xc)
            };
            points.Add(points[2].Polar(vector1.Angle - Math.PI * 0.5, 2));
            points.Add(new Point2d(lsPt1.X + sina1 * (t - 2) + Math.Sin(Math.PI * 0.25 + vector1.Angle) * Math.Sqrt(8), lsPt1.Y - cosa1 * (t - 2) - Math.Cos(Math.PI * 0.25 + vector1.Angle) * Math.Sqrt(8)));
            points.Add(lsPt1.Polar(vector1.Angle - Math.PI * 0.5, t - 2));
            db.AddPolyLineToModeSpace("BF-石材", true, points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        /// <summary>
        /// 绘制中间段石材截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="points">坐标点集合</param>
        /// <param name="t">石材截面厚度</param>
        public static void ZJStb(this Database db, List<Point3d> points, int t)
        {
            Point2d lsPt1, lsPt2, lsPt3, lsPt4;
            for (int i = 0; i < points.Count - 3; i++)
            {
                lsPt1 = new Point2d(points[i].X, points[i].Y);
                lsPt2 = new Point2d(points[i + 1].X, points[i + 1].Y);
                lsPt3 = new Point2d(points[i + 2].X, points[i + 2].Y);
                lsPt4 = new Point2d(points[i + 3].X, points[i + 3].Y);
                Vector2d vector1 = lsPt2 - lsPt1;
                Vector2d vector2 = lsPt3 - lsPt2;
                Vector2d vector3 = lsPt4 - lsPt3;
                double xangle1 = (vector2.Angle - vector1.Angle - Math.PI) / 2 + vector1.Angle;
                double xangle2 = (vector3.Angle - vector2.Angle - Math.PI) / 2 + vector2.Angle;
                double xc1 = (t - 2) / Math.Sin(vector1.Angle - xangle1);
                double xc2 = (t - 2) / Math.Sin(vector2.Angle - xangle2);
                List<Point2d> lspoints = new List<Point2d>
                {
                    lsPt2,
                    lsPt3,
                    lsPt3.Polar(xangle2,xc2)
                };
                lspoints.Add(lspoints[2].Polar(vector2.Angle - Math.PI * 0.5, 2));
                lspoints.Add(lsPt2.Polar(xangle1, xc1).Polar(vector2.Angle - Math.PI * 0.5, 2));
                lspoints.Add(lsPt2.Polar(xangle1, xc1));
                db.AddPolyLineToModeSpace("BF-石材", true, lspoints[0], lspoints[1], lspoints[2], lspoints[3], lspoints[4], lspoints[5]);
            }
        }

        /// <summary>
        /// 绘制最后段石头板截面
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="pt1">起始点坐标</param>
        /// <param name="pt2">中间点坐标</param>
        /// <param name="pt3">终止点坐标</param>
        /// <param name="t">石材厚度</param>
        public static void LastStb(this Database db, Point3d pt1, Point3d pt2, Point3d pt3, int t)
        {
            Point2d lsPt1, lsPt2, lsPt3;
            lsPt1 = new Point2d(pt1.X, pt1.Y);
            lsPt2 = new Point2d(pt2.X, pt2.Y);
            lsPt3 = new Point2d(pt3.X, pt3.Y);
            Vector2d vector1 = lsPt2 - lsPt1;
            Vector2d vector2 = lsPt3 - lsPt2;
            
            double xangle = (vector2.Angle + vector1.Angle + Math.PI) / 2;
            double xc = (t - 2) / Math.Sin(vector1.Angle - xangle);

            List<Point2d> points = new List<Point2d>
            {
                lsPt2,
                lsPt3,
                lsPt3.Polar(vector2.Angle -Math.PI*0.5,t-2)
            };
            points.Add(points[2].Polar(vector2.Angle - Math.PI * 0.75, Math.Sqrt(8)));
            points.Add(lsPt2.Polar(xangle, xc).Polar(vector2.Angle - Math.PI * 0.5, 2));
            points.Add(lsPt2.Polar(xangle, xc));
            db.AddPolyLineToModeSpace("BF-石材", true, points[0], points[1], points[2], points[3], points[4], points[5]);
        }

        /// <summary>
        /// 根据基点、角度、长度计算相对点
        /// </summary>
        /// <param name="basePt">基点</param>
        /// <param name="angle">角度（弧度）</param>
        /// <param name="len">长度</param>
        /// <returns>Point2d</returns>
        public static Point2d Polar(this Point2d basePt,double angle,double len)
        {            
            Point2d point = new Point2d(basePt.X + Math.Cos(angle) * len, basePt.Y + Math.Sin(angle) * len);

            return point;
        }

        /// <summary>
        /// 将字符串中的数字和小数点转换出来
        /// </summary>
        /// <param name="str">需转换的字符串</param>
        /// <returns></returns>
        public static string IntegerString(string str)
        {
            string b = string.Empty;
            for (int i = 0; i < str.Length; i++)
            {
                if (Char.IsDigit(str[i]) || str[i] == '.')
                    b += str[i];
            }
            return b;
        }

        /// <summary>
        /// 添加Ribbon选项卡
        /// </summary>
        /// <param name="ribbonControl">Ribbon控制器</param>
        /// <param name="title">选项卡标题</param>
        /// <param name="id">选项卡id</param>
        /// <param name="isActive">是否置为当前</param>
        /// <returns>RibbonTab</returns>
        public static RibbonTab AddTab(this RibbonControl ribbonControl, string title, string id, bool isActive)
        {
            RibbonTab tab = new RibbonTab
            {
                Title = title,
                Id = id
            };
            ribbonControl.Tabs.Add(tab);
            tab.IsActive = isActive;
            return tab;
        }

        /// <summary>
        /// 添加面板
        /// </summary>
        /// <param name="tab">Ribbon选项卡</param>
        /// <param name="title">面板标题</param>
        /// <returns>RibbonPanelSource</returns>
        public static RibbonPanelSource AddPanel(this RibbonTab tab,string title)
        {
            RibbonPanelSource ribbonPanelSource = new RibbonPanelSource()
            {
                Title = title
            };
            RibbonPanel ribbonPanel = new RibbonPanel()
            {
                Source = ribbonPanelSource
            };
            tab.Panels.Add(ribbonPanel);
            return ribbonPanelSource;
        }

        /// <summary>
        /// 给面板添加下拉组合按钮
        /// </summary>
        /// <param name="panelSource"></param>
        /// <param name="text"></param>
        /// <param name="size"></param>
        /// <param name="orient"></param>
        /// <returns></returns>
        public static RibbonSplitButton AddSplitButton(this RibbonPanelSource panelSource, string text, RibbonItemSize size, Orientation orient)
        {
            RibbonSplitButton splitBtn = new RibbonSplitButton
            {
                Text = text,
                ShowText = true,
                Size = size,
                ShowImage = true,
                Orientation = orient
            };
            panelSource.Items.Add(splitBtn);
            return splitBtn;
        }
        /// <summary>
        /// 添加命令按钮提示信息
        /// </summary>
        /// <param name="ribbon">命令按钮</param>
        /// <param name="title">命令名</param>
        /// <param name="content">命令解释</param>
        /// <param name="command">快捷命令</param>
        /// <param name="expandedContent">命令拓展说明</param>
        /// <param name="imgFilePath">说明图片</param>
        /// <returns>RibbonToolTip</returns>
        public static RibbonToolTip AddRibbonToolTip(this RibbonButton ribbon, string title, string content, string command, string expandedContent, string imgFilePath)
        {
            RibbonToolTip ribbonToolTip = new RibbonToolTip()
            {
                Title = title,
                Content = content,
                Command = command,
                ExpandedContent = expandedContent
            };

            if (imgFilePath != null)
            {
                Uri uri = new Uri(imgFilePath);
                BitmapImage bitmapImage = new BitmapImage(uri);
                ribbonToolTip.ExpandedImage = bitmapImage;
            }

            ribbon.ToolTip = ribbonToolTip;
            return ribbonToolTip;
        }

        /// <summary>
        /// 复制图形
        /// </summary>
        /// <param name="entId">图形的ObjectId</param>
        /// <param name="sourcePoint">参考起点</param>
        /// <param name="targetPoint">参考终点</param>
        /// <returns>图形对象</returns>
        public static Entity CopyEntity(this ObjectId entId,Point3d sourcePoint,Point3d targetPoint)
        {
            //声明一个图形对象
            Entity entR;
            Database db = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //打开块表
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                //打开块表记录
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                //打开图形
                Entity ent = (Entity)entId.GetObject(OpenMode.ForWrite);
                //计算变换矩阵
                Vector3d vector = sourcePoint.GetVectorTo(targetPoint);
                Matrix3d mt = Matrix3d.Displacement(vector);
                entR = ent.GetTransformedCopy(mt);
                //提交事务处理
                trans.Commit();
            }
            return entR;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entId"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="isEraseSoruce"></param>
        public static Entity MirrorEntity(this ObjectId entId,Point3d pt1,Point3d pt2,bool isEraseSoruce)
        {
            //声明一个图形对象用于返回
            Entity entR;
            //计算镜像的变换矩阵
            Matrix3d mt = Matrix3d.Mirroring(new Line3d(pt1, pt2));
            using(Transaction trans = entId.Database.TransactionManager.StartTransaction())
            {
                if (isEraseSoruce)
                {
                    //打开源对象
                    Entity ent = (Entity)trans.GetObject(entId, OpenMode.ForWrite);
                    ent.TransformBy(mt);
                    entR = ent;
                }
                else
                {
                    //打开源对象
                    Entity ent = (Entity)trans.GetObject(entId, OpenMode.ForRead);
                    entR = ent.GetTransformedCopy(mt);                    
                }
                trans.Commit();
            }
            return entR;
        }
    }
}
