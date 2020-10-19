using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
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
    public class PublicValue
    {
        public static string oldBlockName;
        public static string newBlockName;
        public static double dimScale;
        public static double scaleFactor;
        public static string DwgName;
        public static string scale;
        public static string syNo;
        public static string page;
        public static Database acDb = Application.DocumentManager.MdiActiveDocument.Database;
        public static string blkname;
        public static string[] zmk = new string[]
        {
            "a","b","c","d","e","f","g","h","j","k","l","m","n","p","q","r","s","t","u","v","w","x","y","z",
            "A","B","C","D","E","F","G","H","J","K","L","M","N","P","Q","R","S","T","U","V","W","X","Y","Z"
        };
        public static TypedValue[] values;
    }
    public class BlockTools
    {
        //快速新建图块,允许先选择后执行命令
        [CommandMethod("BBN",CommandFlags.UsePickSet)]
        public void BBN()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——快速建块");

            DateTime dt = DateTime.Now;
            string strTime = dt.ToString("yyyyMMddHHmmss");
            string blockName = "BF" + strTime;
            Point3d insertPt = new Point3d(0,0,0);

            using (DocumentLock docLock = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.LockDocument())
            {
                //用于返回所创建的块的对象Id
                ObjectId blockId = ObjectId.Null;
                //创建一个BlockTableRecord类的对象，表示所要创建的块
                BlockTableRecord btr = new BlockTableRecord
                {
                    //设置块名 
                    Name = blockName
                };
                //设置块的基点
                PromptPointOptions ppo = new PromptPointOptions("\n请指定块基点");
                PromptPointResult ppr = ed.GetPoint(ppo);
                if (ppr.Status == PromptStatus.OK)
                {
                    btr.Origin = insertPt = ppr.Value;
                }
                else
                    btr.Origin = new Point3d(0, 0, 0);
                
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForWrite);//以写的方式打开块表
                    if (!bt.Has(btr.Name))
                    {
                        //选择对象
                        PromptSelectionResult res = ed.GetSelection();
                        if (res.Status == PromptStatus.OK)
                        {
                            foreach (ObjectId id in res.Value.GetObjectIds())
                            {
                                Entity ent = trans.GetObject(id, OpenMode.ForWrite) as Entity;
                                Entity NewEnt = (Entity)ent.Clone();
                                btr.AppendEntity(NewEnt);
                                ent.Erase(true);
                            }
                            //btr.AssumeOwnershipOf(new ObjectIdCollection(res.Value.GetObjectIds()));
                        }
                        //在块表中加入块
                        bt.Add(btr);
                        //通知事务处理
                        trans.AddNewlyCreatedDBObject(btr, true);
                        ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                        spaceId.InsertBlockReference("0", blockName, insertPt, new Scale3d(1), 0);
                    }
                    else
                    {
                        ed.WriteMessage("此图块名已存在,请检查!");
                    }
                    trans.Commit();
                }
            }
        }
        
        //修改图块基点，现有同名块位置不变
        //[CommandMethod("BBI")]
        public void BBI()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——修改图块基点，现有同名块位置不变");

            Point3d basePoint;
            PromptEntityOptions peo = new PromptEntityOptions("\n请选择要修改的图块");
            //peo.AddAllowedClass(typeof(BlockReference),false);
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status == PromptStatus.OK)
            {
                ObjectId id = per.ObjectId;
                using(Transaction trans = db.TransactionManager.StartTransaction())
                {
                    BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                    BlockReference brf = (BlockReference)trans.GetObject(id, OpenMode.ForWrite);
                    basePoint = brf.Position;

                    PromptPointOptions ppo = new PromptPointOptions("\n请拾取新的插入点")
                    {
                        UseBasePoint = true,
                        BasePoint = basePoint
                    };
                    PromptPointResult ppr = ed.GetPoint(ppo);
                    if (ppr.Status == PromptStatus.OK)
                    {
                        Point3d newBasePoint = ppr.Value;
                        using(DocumentLock dl = doc.LockDocument())
                        {
                            BlockTableRecord btr =(BlockTableRecord) trans.GetObject(brf.BlockTableRecord, OpenMode.ForWrite);

                            //修改块基点的代码不知道怎么写…………
                        }
                    }
                    brf.DowngradeOpen();
                    trans.Commit();
                }
            }
            else return;
        }

        //修改块名
        [CommandMethod("BBR")]
        public void BBR()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            Editor ed = doc.Editor;

            DateTime dt = DateTime.Now;
            string strTime = dt.ToString("yyyyMMddHHmmss");
            PublicValue.newBlockName = "BF" + strTime;
            //string blockName = "BF" + strTime;

            ed.WriteMessage("\n百福工具箱——修改块名");

            PromptEntityOptions peo = new PromptEntityOptions("\n请选择图块");
            peo.SetRejectMessage("\n选择的必须是块!");
            peo.AddAllowedClass(typeof(BlockReference), false);
            PromptEntityResult entRes = ed.GetEntity(peo);
            if (entRes.Status != PromptStatus.OK) return;
            using (DocumentLock dl = doc.LockDocument())
            {
                using (Transaction trans = doc.TransactionManager.StartTransaction())
                {
                    BlockReference blkRef = (BlockReference)trans.GetObject(entRes.ObjectId, OpenMode.ForRead);
                    BlockTableRecord btr2 = (BlockTableRecord)trans.GetObject(blkRef.BlockTableRecord, OpenMode.ForWrite);
                    PublicValue.oldBlockName = btr2.Name;

                    ReBlockNameForm form1 = new ReBlockNameForm();
                    form1.ShowDialog();
                    
                    btr2.Name = PublicValue.newBlockName;
                    trans.Commit();
                }
            }

        }

        //用于插入图框
        [CommandMethod("TK")]
        public void TK()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;

            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——插入图框");
            string blkname = "BFA3H";
            //输入插入块的类型
            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("")
            {
                Message = "\n选择相应的图框类型"
            };
            pKeyOpts.Keywords.Add("F封面图框");
            pKeyOpts.Keywords.Add("M目录图框");
            pKeyOpts.Keywords.Add("C常用图框");
            pKeyOpts.Keywords.Default = "C常用图框";
            pKeyOpts.AllowNone = false;
            PromptResult pKeyRes = ed.GetKeywords(pKeyOpts);
            if (pKeyRes.Status == PromptStatus.Keyword || pKeyRes.Status == PromptStatus.OK)
            {
                switch (pKeyRes.StringResult)
                {
                    case "F封面图框":
                        blkname = "封面图框";
                        break;
                    case "M目录图框":
                        blkname = "目录图框";
                        break;
                    default:
                        break;
                }
            }

            //输入块参照插入的点
            PromptPointResult ppr = ed.GetPoint("\n请选择插入点");
            Point3d pt = ppr.Value;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                if (lt.Has("BF-图框"))
                {
                    //string xckPath = @"F:\Aided Drafting Tools\Aided Drafting Tools\BaseDwgs\图框.dwg";
                    string xckPath = Tools.GetCurrentPath() + @"\BaseDwgs\图框.dwg";
                    
                    db.ImportBlocksFromDWG(xckPath, blkname);
                    //提取当前图形的标注比例
                    int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));
                    Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(dimScale.ToString());

                    //表示属性的字典对象
                    Dictionary<string, string> atts = new Dictionary<string, string>();

                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                    spaceId.InsertBlockReference("BF-图框", blkname, pt, new Scale3d(dimScale), 0, atts);
                }
                else
                {
                    ed.WriteMessage("\n没有发现图层：\"BF-图框\"");
                }
                trans.Commit();
            }
        }

        //插入铝型材
        [CommandMethod("LXC")]
        public void LXC()
        {
            DocumentCollection dm = Application.DocumentManager;
            Editor ed = dm.MdiActiveDocument.Editor;
            Database db = dm.MdiActiveDocument.Database;

            //该图库地址后期应修改未变量
            string xckPath = Tools.GetCurrentPath() + @"\BaseDwgs\铝型材标准块库.dwg";
            ed.WriteMessage(xckPath);
            //根据编号生成需要的块名
            PromptStringOptions pso = new PromptStringOptions("\n百福工具箱——插入铝型材\n请输入型材编号")
            {
                AllowSpaces = false
            };
            PromptResult xcbhpr = ed.GetString(pso);

            string xcbh = "LC" + xcbhpr.StringResult;
            //输入块参照插入的点
            PromptPointResult ppr = ed.GetPoint("\n请选择插入点");
            Point3d pt = ppr.Value;
            //从外部DWG文件中插入图块
            //BlockTools.ImportBlocksFromDWG(xckPath, db);
            db.ImportBlocksFromDWG(xckPath, xcbh);

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btr = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                if (bt.Has(xcbh))
                {
                    using (BlockReference brf = new BlockReference(pt, bt[xcbh]))
                    {
                        btr.AppendEntity(brf);
                        trans.AddNewlyCreatedDBObject(brf, true);
                    }
                }
                else
                {
                    ed.WriteMessage("\n未在图库中找到图块" + xcbh);
                }
                trans.Commit();
            }
        }

        //插入灯具图块
        [CommandMethod("CDJ")]
        public void CDJ()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            string curLayerName = db.GetCurrentLayerName();
            db.SetCurrentLayer("BF-灯具");


            //该图库地址后期应修改未变量
            string xckPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blkname = "LED灯条";

            ed.WriteMessage("\n百福工具箱——插入灯具图块");
            //选择插入灯具的样式
            PromptPointOptions ppo = new PromptPointOptions("选择需要插入的灯具图块");
            ppo.Keywords.Add("Led灯条截面");
            ppo.Keywords.Add("Y硬灯条截面");
            ppo.Keywords.Add("TH天花灯");
            ppo.Keywords.Add("TD筒灯");
            ppo.Keywords.Add("S3孔射灯");
            ppo.Keywords.Add("F方形金卤灯");
            ppo.Keywords.Default = "Led灯条截面";
            PromptPointResult ppr = ed.GetPoint(ppo);

            Point3d pt;

            if (ppr.Status == PromptStatus.Keyword)

                switch (ppr.StringResult)
                {
                    case "Led灯条截面":
                        blkname = "LED灯条";
                        break;
                    case "Y硬灯条截面":
                        blkname = "LED硬灯条_端面";
                        break;
                    case "TH天花灯":
                        blkname = "天花灯";
                        break;
                    case "TD筒灯":
                        blkname = "LED筒灯";
                        break;
                    case "S3孔射灯":
                        blkname = "百福-3孔射灯";
                        break;
                    case "F方形金卤灯":
                        blkname = "百福-户外方金卤灯";
                        break;
                    default:
                        break;
                }
            PromptPointResult ppr1 = ed.GetPoint("\n请提供插入点");
            if (ppr1.Status == PromptStatus.OK)
            {
                pt = ppr1.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                   // LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);

                    db.ImportBlocksFromDWG(xckPath, blkname);

                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                    spaceId.InsertBlockReference("BF-灯具", blkname, pt, new Scale3d(1), 0);

                    trans.Commit();
                }
                db.SetCurrentLayer(curLayerName);
            }
            else 
            {
                db.SetCurrentLayer(curLayerName);
                return;
            }
            
        }

        //布置天花灯(ok)
        [CommandMethod("BD1")]
        public void BD1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            string clayername = db.GetCurrentLayerName();
            string xckPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blkname = "天花灯";

            ed.WriteMessage("\n百福工具箱——快速布置天花灯具");

            //输入天花灯具的起始点和终止点
            PromptPointResult ppr1 = ed.GetPoint("\n请选择起始点");
            if (ppr1.Status == PromptStatus.OK)
            {
                Point2d spt = new Point2d(ppr1.Value.X, ppr1.Value.Y);
                PromptPointOptions ppo = new PromptPointOptions("\n请选择终止点")
                {
                    BasePoint = ppr1.Value,
                    UseBasePoint = true
                };
                PromptPointResult ppr2 = ed.GetPoint(ppo);
                if (ppr2.Status == PromptStatus.OK)
                {
                    Point2d ept = new Point2d(ppr2.Value.X, ppr2.Value.Y);                    
                    double dis = spt.GetDistanceTo(ept);

                    PromptDoubleOptions pdo = new PromptDoubleOptions("\n请输入间距,默认为450mm")
                    {
                        AllowNegative = false,//不允许输入负数
                        DefaultValue = 450//设置默认值
                    };
                    PromptDoubleResult pdr = ed.GetDouble(pdo);
                    if (pdr.Status == PromptStatus.OK)
                    {
                        double jj = pdr.Value;
                        int n = (int)(dis / jj);
                        jj = dis / n;

                        while (jj >= 500)
                        {
                            n++;
                            jj = dis / n;
                        }

                        Vector2d vec = (ept - spt) / n;
                        Point3d insertPoint = new Point3d(spt.X ,spt.Y,0);

                        db.SetCurrentLayer("BF-灯具");

                        for (int i = 1; i <= n; i++)
                        {
                            insertPoint = new Point3d(insertPoint.X + vec.X,insertPoint.Y +vec.Y,0);
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                                db.ImportBlocksFromDWG(xckPath, blkname);
                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                spaceId.InsertBlockReference("0", blkname, insertPoint, new Scale3d(1), 0);
                                trans.Commit();
                            }
                        }
                        ed.WriteMessage($"\n天花灯间距是{string.Format("{0:F2}", jj)}mm");
                    }
                }
            }            
            db.SetCurrentLayer(clayername);
        }

        //布置筒灯(ok)
        [CommandMethod("BD2")]
        public void BD2()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;
            string clayername = db.GetCurrentLayerName();
            string xckPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blkname = "LED筒灯";

            ed.WriteMessage("\n百福工具箱——快速通道照明灯具");

            //输入天花灯具的起始点和终止点
            PromptPointResult ppr1 = ed.GetPoint("\n请选择起始点");
            if (ppr1.Status == PromptStatus.OK)
            {
                Point2d spt = new Point2d(ppr1.Value.X, ppr1.Value.Y);
                PromptPointOptions ppo = new PromptPointOptions("\n请选择终止点")
                {
                    BasePoint = ppr1.Value,
                    UseBasePoint = true
                };
                PromptPointResult ppr2 = ed.GetPoint(ppo);
                if (ppr2.Status == PromptStatus.OK)
                {
                    Point2d ept = new Point2d(ppr2.Value.X, ppr2.Value.Y);
                    double dis = spt.GetDistanceTo(ept);

                    PromptDoubleOptions pdo = new PromptDoubleOptions("\n请输入间距,默认为1200mm")
                    {
                        AllowNegative = false,//不允许输入负数
                        DefaultValue = 1200//设置默认值
                    };
                    PromptDoubleResult pdr = ed.GetDouble(pdo);
                    if (pdr.Status == PromptStatus.OK)
                    {
                        double jj = pdr.Value;
                        int n = (int)(dis / jj);
                        jj = dis / n;

                        while (jj >= 1200)
                        {
                            n++;
                            jj = dis / n;
                        }

                        Vector2d vec = (ept - spt) / n;
                        Point3d insertPoint = new Point3d(spt.X, spt.Y, 0);

                        db.SetCurrentLayer("BF-灯具");

                        for (int i = 1; i <= n; i++)
                        {
                            insertPoint = new Point3d(insertPoint.X + vec.X, insertPoint.Y + vec.Y, 0);
                            using (Transaction trans = db.TransactionManager.StartTransaction())
                            {
                                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                                db.ImportBlocksFromDWG(xckPath, blkname);
                                ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                                
                                spaceId.InsertBlockReference("BF-灯具", blkname, insertPoint, new Scale3d(1), 0);
                                trans.Commit();
                            }
                        }
                        ed.WriteMessage($"\n筒灯间距是{string.Format("{0:F2}", jj)}mm");
                    }
                }
            }
            db.SetCurrentLayer(clayername);
        }

        //插入标题
        //[CommandMethod("BT")]
        public void BT()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = doc.Editor;

            TitleForm form1 = new TitleForm();
            form1.ShowDialog();

            string blockPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            PromptPointOptions ppo = new PromptPointOptions("\n选择插入点")
            {
                AllowArbitraryInput = true
            };
            PromptPointResult ppr = ed.GetPoint(ppo);
            if(ppr.Status == PromptStatus.OK)
            {
                Point3d insertPoint = ppr.Value;
                double factor = System.Convert.ToDouble(Application.GetSystemVariable("DIMSCALE"));
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    db.ImportBlocksFromDWG(blockPath, PublicValue.newBlockName);
                    //获取当前空间（模型空间或图纸空间）
                    ObjectId spaceId = db.CurrentSpaceId;
                    //表示属性的字典对象
                    Dictionary<string, string> atts = new Dictionary<string, string>
                    {
                        { "CHINESENAME", PublicValue.DwgName },
                        { "SCALE", PublicValue.scale },
                        { "INDEXNO", PublicValue.syNo },
                        { "DRAWINGNO", "-" }
                    };
                    spaceId.InsertBlockReference("BF-索引", PublicValue.newBlockName, insertPoint, new Scale3d(factor), 0,atts);
                    trans.Commit();
                }
            }
        }

        //插入4合1索引符号
        [CommandMethod("I0")]
        public void I0()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——插入组合标题索引");
            PromptPointOptions ppo = new PromptPointOptions("\n给定索引组合的插入点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                Point3d insertPoint = ppr.Value;
                PromptStringOptions pso = new PromptStringOptions("\n给定起始索引符号<A>或者<1>")
                {                    
                    UseDefaultValue = true,
                    DefaultValue = "A"
                };
                
                PromptResult pr = ed.GetString(pso);
                if (pr.Status == PromptStatus.OK)
                {
                    PublicValue.syNo = pr.StringResult;                    
                }
                else
                {
                    if (PublicValue.syNo == "")
                    {
                        PublicValue.syNo = "A";
                    }
                }
                
                int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));
                db.AddIden0(insertPoint, dimScale, PublicValue.syNo);
            }
        }

        //插入索引符号
        [CommandMethod("I1")]
        public void I1()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            double angle = 0;
            ed.WriteMessage("\n百福工具箱——插入索引符号");
            //开启正交
            if (!db.Orthomode)
            {
                db.Orthomode = true;
            }

            PromptPointOptions ppo = new PromptPointOptions("\n请选择插入点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if(ppr.Status == PromptStatus.OK)
            {
                Point3d insertPt = ppr.Value;
                PromptAngleOptions pao = new PromptAngleOptions("\n请输入索引符号的指向角度")
                {
                    UseBasePoint = true,
                    BasePoint = insertPt,
                    AllowArbitraryInput = true,
                    DefaultValue = 0
                };

                PromptDoubleResult pdr = ed.GetAngle(pao);
                if (pdr.Status == PromptStatus.OK)
                {
                    //这里得到的角度值为弧度值，手动输入的值会自动转化
                     angle= pdr.Value;
                }
                PromptStringOptions pso = new PromptStringOptions("\n给定起始索引符号<A>或者<1>")
                {
                    UseDefaultValue = true,
                    DefaultValue = "A"
                };

                PromptResult pr = ed.GetString(pso);
                if (pr.Status == PromptStatus.OK)
                {
                    PublicValue.syNo = pr.StringResult;
                }
                else
                {
                    if (PublicValue.syNo == "")
                    {
                        PublicValue.syNo = "A";
                    }
                }

                int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));
                db.AddIden1(insertPt, dimScale, PublicValue.syNo,angle);
            }
        }

        //布置开孔
        [CommandMethod("BK1")]
        public void BK1()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——均布螺丝孔");

            string blkPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blkname = "DK5";

            PromptPointOptions ppo = new PromptPointOptions("\n给定起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                Point3d insertPoint = ppr.Value;
                Point2d spt = new Point2d(ppr.Value.X,ppr.Value.Y);
                PromptPointOptions ppo1 = new PromptPointOptions("\n给定终止点");
                PromptPointResult ppr1;
                do
                {
                    ppo1.UseBasePoint = true;
                    ppo1.BasePoint = ppr.Value;
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                Point2d ept = new Point2d(ppr1.Value.X,ppr1.Value.Y);
                Vector2d vector = ept - spt;
                double nums = vector.Length / 250;
                int num = (int)nums;
                double jj = vector.Length / num;
                while (jj>250)
                {
                    num += 1;
                    jj = vector.Length / num;
                }

                using(Transaction trans = db.TransactionManager.StartTransaction())
                {
                    db.ImportBlocksFromDWG(blkPath, blkname);
                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                    spaceId.InsertBlockReference("BF-不锈钢", blkname,insertPoint, new Scale3d(1), 0);
                    for (int i = 0; i < num; i++)
                    {
                        insertPoint = insertPoint.Polar(vector.Angle, jj);
                        spaceId.InsertBlockReference("BF-不锈钢", blkname, insertPoint, new Scale3d(1), 0);
                    }
                    trans.Commit();
                }
            }
        }

        //布置开孔
        [CommandMethod("BK2")]
        public void BK2()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\n百福工具箱——均布双螺丝孔");

            string blkPath = Tools.GetCurrentPath() + @"\BaseDwgs\常用图块.dwg";
            string blkname = "SK5_10";

            PromptPointOptions ppo = new PromptPointOptions("\n给定起始点");
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status == PromptStatus.OK)
            {
                Point3d insertPoint = ppr.Value;
                Point2d spt = new Point2d(ppr.Value.X, ppr.Value.Y);
                PromptPointOptions ppo1 = new PromptPointOptions("\n给定终止点");
                PromptPointResult ppr1;
                do
                {
                    ppo1.UseBasePoint = true;
                    ppo1.BasePoint = ppr.Value;
                    ppr1 = ed.GetPoint(ppo1);
                } while (ppr1.Status != PromptStatus.OK);
                Point2d ept = new Point2d(ppr1.Value.X, ppr1.Value.Y);
                Vector2d vector = ept - spt;
                double nums = vector.Length / 250;
                int num = (int)nums;
                double jj = vector.Length / num;
                while (jj > 250)
                {
                    num += 1;
                    jj = vector.Length / num;
                }

                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    db.ImportBlocksFromDWG(blkPath, blkname);
                    ObjectId spaceId = db.CurrentSpaceId;//获取当前空间（模型空间或图纸空间）
                    spaceId.InsertBlockReference("BF-不锈钢", blkname, insertPoint, new Scale3d(1), 0);
                    for (int i = 0; i < num; i++)
                    {
                        insertPoint = insertPoint.Polar(vector.Angle, jj);
                        spaceId.InsertBlockReference("BF-不锈钢", blkname, insertPoint, new Scale3d(1), 0);
                    }
                    trans.Commit();
                }
            }
        }

    }
}
