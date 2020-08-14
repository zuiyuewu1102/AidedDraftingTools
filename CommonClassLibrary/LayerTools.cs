using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class LayerTools
    {
        /// <summary>
        /// 新建图层
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="layername">图层名</param>
        /// <param name="colorIndex">颜色索引</param>
        /// <returns>ObjectId</returns>
        public static ObjectId AddLayer(this Database db, string layername, short colorIndex)
        {
            //打开层表
            LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
            if (!lt.Has(layername))
            {
                LayerTableRecord ltr = new LayerTableRecord
                {
                    Name = layername,
                    Color = Color.FromColorIndex(ColorMethod.ByAci, colorIndex)
                };//定义一个新的层表记录
                lt.UpgradeOpen();
                lt.Add(ltr);
                db.TransactionManager.AddNewlyCreatedDBObject(ltr, true);
                lt.DowngradeOpen();
            }
            return lt[layername];
        }
        /// <summary>
        /// 新建图层
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="layername">图层名</param>
        /// <param name="colorIndex">颜色索引</param>
        /// <param name="linetype">线型</param>
        /// <param name="lineWeight">线宽</param>
        /// <param name="isprint">是否打印</param>
        /// <param name="zs">注释</param>
        /// <returns></returns>
        public static ObjectId AddLayer(this Database db, string layername, short colorIndex, string linetype, LineWeight lineWeight, bool isprint, string zs)
        {
            //打开层表
            LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
            if (!lt.Has(layername))
            {
                LayerTableRecord ltr = new LayerTableRecord
                {
                    Name = layername,
                    Color = Color.FromColorIndex(ColorMethod.ByAci, colorIndex)
                };//定义一个新的层表记录
                LinetypeTable ltt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);
                if (ltt.Has(linetype))
                    ltr.LinetypeObjectId = ltt[linetype];
                else
                {
                    db.LoadLineTypeFile(linetype, "acadiso.lin");
                    ltr.LinetypeObjectId = ltt[linetype];
                }
                ltr.LineWeight = lineWeight;
                ltr.IsPlottable = isprint;
                ltr.Description = zs;
                lt.UpgradeOpen();
                lt.Add(ltr);
                db.TransactionManager.AddNewlyCreatedDBObject(ltr, true);
                lt.DowngradeOpen();
            }
            return lt[layername];
        }

        /// <summary>
        /// 得到当前图层名
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <returns>当前图层名</returns>
        public static string GetCurrentLayerName(this Database db)
        {
            string currentLayerName;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId,OpenMode.ForRead);
                LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(db.Clayer, OpenMode.ForRead);
                currentLayerName = ltr.Name;
                trans.Commit();
            }
            return currentLayerName;
        }

        /// <summary>
        /// 设置图层为当前
        /// </summary>
        /// <param name="db">图形数据库</param>
        /// <param name="layerName">图层名</param>
        /// <returns>bool</returns>
        /*public static bool SetCurrentLayer(this Database db, string layerName)
        {
            //打开层表
            LayerTable lt = (LayerTable)db.LayerTableId.GetObject(OpenMode.ForRead);
            //如果不存在名为layerName的图层，则返回
            if (!lt.Has(layerName)) return false;
            //获取名为layerName的层表记录的Id
            ObjectId layerId = lt[layerName];
            //如果指定的图层为当前层，则返回
            if (db.Clayer == layerId) return true;
            //指定当前层
            db.Clayer = layerId;
            return true;
        }*/

        public static void SetCurrentLayer(this Database db, string layerName)
        {
            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                //打开层表
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId,OpenMode.ForRead);
                //如果不存在名为layerName的图层，则返回
                if (!lt.Has(layerName)) return;
                //获取名为layerName的层表记录的Id
                ObjectId layerId = lt[layerName];
                //如果指定的图层为当前层，则返回
                if (db.Clayer == layerId) return;
                //指定当前层
                db.Clayer = layerId;
                trans.Commit();
            }
        }       

    }
}
