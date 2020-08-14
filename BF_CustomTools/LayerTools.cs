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

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        //选对象改为当前层
        [CommandMethod("ERC")]
        public void ERC()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            PromptSelectionResult psr = ed.GetSelection();
            SelectionSet ss = psr.Value;
            if (psr.Status != PromptStatus.OK) return;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                //获取当前层的块表记录
                LayerTableRecord ltr = (LayerTableRecord)db.Clayer.GetObject(OpenMode.ForRead);
                //循环选择集
                foreach (ObjectId id in ss.GetObjectIds())
                {
                    Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                    ent.Layer = ltr.Name;
                    ent.DowngradeOpen();
                }
                trans.Commit();
            }
        }

        //新建图层
        [CommandMethod("ERN")]
        public void ERN()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            PromptStringOptions pso = new PromptStringOptions("\nADT——新建图层\n请输入新图层名称")
            {
                AllowSpaces = true
            };
            PromptResult pr = ed.GetString(pso);
            if (pr.Status == PromptStatus.OK)
            {
                string layname = pr.StringResult;

                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);
                    if (!lt.Has(layname))
                    {
                        LayerTableRecord ltr = new LayerTableRecord
                        {
                            Name = layname
                        };
                        lt.Add(ltr);
                        trans.AddNewlyCreatedDBObject(ltr, true);
                        trans.Commit();
                    }
                }
            }
        }

        //设定当前图层
        [CommandMethod("ERS")]
        public void ERS()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peo = new PromptEntityOptions("\n选取图元以设定其所在图层为当前图层：\n")
            {
                AllowNone = false
            };
            PromptEntityResult per = ed.GetEntity(peo);
            if (per.Status == PromptStatus.OK)
            {
                using (Transaction tran = db.TransactionManager.StartTransaction())
                {
                    Entity ent = (Entity)tran.GetObject(per.ObjectId, OpenMode.ForRead);
                    db.Clayer = ent.LayerId;
                    tran.Commit();
                }
            }
        }

        //关闭非选图层ERD
        [CommandMethod("ERD")]
        public void ERD()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<string> laynames = new List<string>();
            List<string> alayname = new List<string>();

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——关闭非选图层：\n选取要保留图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    SelectionSet ss = psr.Value;
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        if (!laynames.Contains(ent.Layer))
                        {
                            laynames.Add(ent.Layer);
                        }
                    }

                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in lt)
                    {
                        LayerTableRecord ltr = trans.GetObject(id, OpenMode.ForRead) as LayerTableRecord;
                        alayname.Add(ltr.Name);
                    }

                    foreach (string name in laynames)
                    {
                        alayname.Remove(name);
                    }

                    foreach (string name in alayname)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[name], OpenMode.ForWrite);
                        ltr.IsOff = true;
                    }

                    trans.Commit();
                }
            }
        }

        //关闭图层ERF
        [CommandMethod("ERF")]
        public void ERF()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——关闭图层：\n选取要关闭的图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[ent.Layer], OpenMode.ForWrite);
                        if (!ltr.IsOff) ltr.IsOff = true;
                    }
                    trans.Commit();
                }
            }
        }
        //关闭所有图层ERAF
        [CommandMethod("ERAF")]
        public void ERAF()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\nADT——关闭全部图层");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId id in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(id, OpenMode.ForWrite);
                    if (!ltr.IsOff) ltr.IsOff = true;
                }
                trans.Commit();
            }
        }
        //打开图层ERA

        //打开全部图层ERAA
        [CommandMethod("ERAA")]
        public void ERAA()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\nADT——打开全部图层");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId id in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(id, OpenMode.ForWrite);
                    if (ltr.IsOff) ltr.IsOff = false;
                }
                trans.Commit();
            }
        }

        //切换全部图层开关ERV
        [CommandMethod("ERV")]
        public void ERV()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            ed.WriteMessage("\n百福工具箱——打开全部图层");
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                foreach (ObjectId id in lt)
                {
                    LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(id, OpenMode.ForWrite);
                    if (ltr.IsOff)
                    {
                        ltr.IsOff = false;
                    }
                    else
                    {
                        ltr.IsOff = true;
                    }
                }
                trans.Commit();
            }
        }

        //锁定图层ERL
        [CommandMethod("ERL")]
        public void ERL()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——锁定图层：\n选取要锁定的图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[ent.Layer], OpenMode.ForWrite);
                        if (!ltr.IsLocked) ltr.IsLocked = true;
                    }
                    trans.Commit();
                }
            }
        }

        //锁定非选图层ERDL
        [CommandMethod("ERDL")]
        public void ERDL()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<string> laynames = new List<string>();
            List<string> alayname = new List<string>();

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——锁定非选图层：\n选取要解锁图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    SelectionSet ss = psr.Value;
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        if (!laynames.Contains(ent.Layer))
                        {
                            laynames.Add(ent.Layer);
                        }
                    }

                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in lt)
                    {
                        LayerTableRecord ltr = trans.GetObject(id, OpenMode.ForRead) as LayerTableRecord;
                        alayname.Add(ltr.Name);
                    }

                    foreach (string name in laynames)
                    {
                        alayname.Remove(name);
                    }

                    foreach (string name in alayname)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[name], OpenMode.ForWrite);
                        ltr.IsLocked = true;
                    }

                    trans.Commit();
                }
            }
        }

        //解锁图层ERU
        [CommandMethod("ERU")]
        public void ERU()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——解锁图层：\n选取要解锁的图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[ent.Layer], OpenMode.ForWrite);
                        if (ltr.IsLocked) ltr.IsLocked = false;
                    }
                    trans.Commit();
                }
            }
        }

        //冻结图层ERZ
        [CommandMethod("ERZ")]
        public void ERZ()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——冻结图层：\n选取图元以冻结其所在的图层"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                SelectionSet ss = psr.Value;
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[ent.Layer], OpenMode.ForWrite);
                        if (!ltr.IsFrozen) ltr.IsLocked = false;
                    }
                    trans.Commit();
                }
            }
        }

        //冻结非选图层ERDZ
        [CommandMethod("ERDZ")]
        public void ERDZ()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            List<string> laynames = new List<string>();
            List<string> alayname = new List<string>();

            PromptSelectionOptions pso = new PromptSelectionOptions
            {
                MessageForAdding = "\nADT——冻结非选图层：\n选取要保留不冻结图层上的图元"
            };
            PromptSelectionResult psr = ed.GetSelection(pso);
            if (psr.Status == PromptStatus.OK)
            {
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    SelectionSet ss = psr.Value;
                    foreach (ObjectId id in ss.GetObjectIds())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForRead);
                        if (!laynames.Contains(ent.Layer))
                        {
                            laynames.Add(ent.Layer);
                        }
                    }

                    LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForRead);
                    foreach (ObjectId id in lt)
                    {
                        LayerTableRecord ltr = trans.GetObject(id, OpenMode.ForRead) as LayerTableRecord;
                        alayname.Add(ltr.Name);
                    }

                    foreach (string name in laynames)
                    {
                        alayname.Remove(name);
                    }

                    foreach (string name in alayname)
                    {
                        LayerTableRecord ltr = (LayerTableRecord)trans.GetObject(lt[name], OpenMode.ForWrite);
                        ltr.IsFrozen = true;
                    }

                    trans.Commit();
                }
            }
        }

        //解冻图层ERT

        //合并图层ERM
    }
}
