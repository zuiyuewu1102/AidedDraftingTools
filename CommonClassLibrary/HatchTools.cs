using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class HatchTools
    {
        public struct HatchPatterName
        {
            public static readonly string solid = "SOLID";
            public static readonly string angle = "ANGLE";
            public static readonly string ar_rroof = "AR-RROOF";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="PatternName"></param>
        /// <param name="scale"></param>
        /// <param name="degree"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static ObjectId HatchEntity(this Database db,string PatternName,double scale,double degree, ObjectIdCollection ids)
        {
            db.SetCurrentLayer("BF-填充");
            ObjectId id;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //声明图案填充对象
                Hatch hatch = new Hatch
                {
                    //设置填充比例
                    PatternScale = scale
                };
                //设置填充类型和图案名
                hatch.SetHatchPattern(HatchPatternType.PreDefined, PatternName);
                //加入图形数据库
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                id = btr.AppendEntity(hatch);
                trans.AddNewlyCreatedDBObject(hatch, true);
                //设置填充角度
                hatch.PatternAngle = degree;
                //设置关联
                hatch.Associative = true;
                //设置边界图形和填充方式
                hatch.AppendLoop(HatchLoopTypes.Outermost, ids);
                //计算填充并显示
                hatch.EvaluateHatch(true);
                //提交事务
                trans.Commit();
            }
            return id;
        }
    }
}
