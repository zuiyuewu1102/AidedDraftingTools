using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace CommonClassLibrary
{
    public static class DimStyleTools
    {
        //public static ObjectId AddDimStyle(this Database db,string styleName)
        //{
        //    DimStyleTable dst = (DimStyleTable)db.DimStyleTableId.GetObject(OpenMode.ForRead);

        //    if (!dst.Has(styleName))
        //    {
        //        DimStyleTableRecord dstr = new DimStyleTableRecord();

        //        dstr.Name = styleName;
        //        dstr.Dimscale = 12;
        //        dst.UpgradeOpen();
        //        dst.Add(dstr);
        //        db.TransactionManager.AddNewlyCreatedDBObject(dstr, true);
        //        dst.DowngradeOpen();
        //    }
        //    return dst[styleName];
        //}

        public static void AddDimStyle(this Database db, string styleName)
        {
            DimStyleTable dst = (DimStyleTable)db.DimStyleTableId.GetObject(OpenMode.ForRead);

            if (!dst.Has(styleName))
            {
                DimStyleTableRecord dstr = new DimStyleTableRecord();

                dstr.Name = styleName;
                dstr.Dimscale = 12;
                dst.UpgradeOpen();
                dst.Add(dstr);
                db.TransactionManager.AddNewlyCreatedDBObject(dstr, true);
                dst.DowngradeOpen();
            }
            
        }

        public static ObjectId AddDimStyle(this Database db, string styleName,double dimasz,double dimexe,int dimtad,double dimtxt)
        {
            DimStyleTable dst = (DimStyleTable)db.DimStyleTableId.GetObject(OpenMode.ForRead);

            if (!dst.Has(styleName))
            {
                DimStyleTableRecord dstr = new DimStyleTableRecord();

                dstr.Name = styleName;
                dstr.Dimasz = dimasz;
                dstr.Dimexe = dimexe;
                dstr.Dimtad = dimtad;
                dstr.Dimtxt = dimtxt;

                dst.UpgradeOpen();
                dst.Add(dstr);
                db.TransactionManager.AddNewlyCreatedDBObject(dstr, true);
                dst.DowngradeOpen();
            }
            return dst[styleName];
        }


        public static bool CheckDimStyleExists(string DimStyleName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction tr = doc.TransactionManager.StartTransaction())
            {
                DimStyleTableRecord dstr = new DimStyleTableRecord();
                DimStyleTable dst = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead, true);
                if (dst.Has(DimStyleName))
                    return true;

                return false;
            }
        }


        public static void SetCurrentDimStyle(string DimStyleName)
        {
            // Establish connections to the document and it's database
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            // Establish a transaction
            using (Transaction tr = doc.TransactionManager.StartTransaction())
            {
                DimStyleTable dst = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForRead);
                ObjectId dimId = ObjectId.Null;

                string message = string.Empty;
                if (!dst.Has(DimStyleName))
                {
                    CreateModifyDimStyle(DimStyleName, out message);
                    dimId = dst[DimStyleName];
                }
                else
                    dimId = dst[DimStyleName];

                DimStyleTableRecord dstr = (DimStyleTableRecord)tr.GetObject(dimId, OpenMode.ForRead);

                /* NOTE:
                 * If this code is used, and the updated style is current,
                 * an override is created for that style.
                 * This is not what I wanted.
                 */
                //if (dstr.ObjectId != db.Dimstyle)
                //{
                //    db.Dimstyle = dstr.ObjectId;
                //    db.SetDimstyleData(dstr);
                //}

                /* Simply by running these two lines all the time, any overrides to updated dimstyles get 
                 * cleared away as happens when you select the parent dimstyle in AutoCAD.
                 */
                db.Dimstyle = dstr.ObjectId;
                db.SetDimstyleData(dstr);

                tr.Commit();
            }
        }

        public static void CreateModifyDimStyle(string DimStyleName, out string message)
        {
            // Initialise the message value that gets returned by an exception (or not!)
            message = string.Empty;
            try
            {
                using (Transaction tr = Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
                {
                    Database db = Application.DocumentManager.MdiActiveDocument.Database;
                    DimStyleTable dst = (DimStyleTable)tr.GetObject(db.DimStyleTableId, OpenMode.ForWrite, true);

                    // Initialise a DimStyleTableRecord
                    DimStyleTableRecord dstr = null;
                    // If the required dimension style exists
                    if (dst.Has(DimStyleName))
                    {
                        // get the dimension style table record open for writing
                        dstr = (DimStyleTableRecord)tr.GetObject(dst[DimStyleName], OpenMode.ForWrite);
                    }
                    else
                        // Initialise as a new dimension style table record
                        dstr = new DimStyleTableRecord();

                    // Set all the available dimension style properties
                    // Most/all of these match the variables in AutoCAD.
                    dstr.Name = DimStyleName;
                    dstr.Annotative = AnnotativeStates.True;
                    dstr.Dimadec = 2;
                    dstr.Dimalt = false;
                    dstr.Dimaltd = 2;
                    dstr.Dimaltf = 25.4;
                    dstr.Dimaltrnd = 0;
                    dstr.Dimalttd = 2;
                    dstr.Dimalttz = 0;
                    dstr.Dimaltu = 2;
                    dstr.Dimaltz = 0;
                    dstr.Dimapost = "";
                    dstr.Dimarcsym = 0;
                    dstr.Dimasz = 3.5;
                    dstr.Dimatfit = 3;
                    dstr.Dimaunit = 0;
                    dstr.Dimazin = 2;
                    dstr.Dimblk = ObjectId.Null;
                    dstr.Dimblk1 = ObjectId.Null;
                    dstr.Dimblk2 = ObjectId.Null;
                    dstr.Dimcen = 0.09;
                    dstr.Dimclrd = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 7);
                    dstr.Dimclre = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 7); ;
                    dstr.Dimclrt = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 2); ;
                    dstr.Dimdec = 2;
                    dstr.Dimdle = 0;
                    dstr.Dimdli = 7;
                    dstr.Dimdsep = Convert.ToChar(".");
                    dstr.Dimexe = 1;
                    dstr.Dimexo = 2;
                    dstr.Dimfrac = 0;
                    dstr.Dimfxlen = 0.18;
                    dstr.DimfxlenOn = false;
                    dstr.Dimgap = 1;
                    dstr.Dimjogang = 0;
                    dstr.Dimjust = 0;
                    dstr.Dimldrblk = ObjectId.Null;
                    dstr.Dimlfac = 1;
                    dstr.Dimlim = false;

                    ObjectId ltId = GetLinestyleID("Continuous");
                    dstr.Dimltex1 = ltId;
                    dstr.Dimltex2 = ltId;
                    dstr.Dimltype = ltId;

                    dstr.Dimlunit = 2;
                    dstr.Dimlwd = LineWeight.LineWeight015;
                    dstr.Dimlwe = LineWeight.LineWeight015;
                    dstr.Dimpost = "";
                    dstr.Dimrnd = 0;
                    dstr.Dimsah = false;
                    dstr.Dimscale = 1;
                    dstr.Dimsd1 = false;
                    dstr.Dimsd2 = false;
                    dstr.Dimse1 = false;
                    dstr.Dimse2 = false;
                    dstr.Dimsoxd = false;
                    dstr.Dimtad = 2;
                    dstr.Dimtdec = 2;
                    dstr.Dimtfac = 1;
                    dstr.Dimtfill = 0;
                    dstr.Dimtfillclr = Autodesk.AutoCAD.Colors.Color.FromColorIndex(ColorMethod.ByAci, 0);
                    dstr.Dimtih = false;
                    dstr.Dimtix = false;
                    dstr.Dimtm = 0;
                    dstr.Dimtmove = 0;
                    dstr.Dimtofl = false;
                    dstr.Dimtoh = false;
                    dstr.Dimtol = false;
                    dstr.Dimtolj = 1;
                    dstr.Dimtp = 0;
                    dstr.Dimtsz = 0;
                    dstr.Dimtvp = 0;

                    // Test for the text style to be used
                    ObjectId tsId = ObjectId.Null;
                    // If it doesn't exist
                    if (!CheckTextStyle("BF-标注文字"))
                    {
                        // Create the required text style
                        CreateTextStyle("BF-标注文字", "simplex.shx", 0,out string massage);
                        tsId = GetTextStyleId("BF-标注文字");
                    }
                    else
                        // Get the ObjectId of the text style
                        tsId = GetTextStyleId("BF-标注文字");

                    dstr.Dimtxsty = tsId;
                    dstr.Dimtxt = 2.5;
                    dstr.Dimtxtdirection = false;
                    dstr.Dimtzin = 0;
                    dstr.Dimupt = false;
                    dstr.Dimzin = 0;

                    // If the dimension style doesn't exist
                    if (!dst.Has(DimStyleName))
                    {
                        // Add it to the dimension style table and collect its Id
                        Object dsId = dst.Add(dstr);
                        // Add the new dimension style table record to the document
                        tr.AddNewlyCreatedDBObject(dstr, true);
                    }

                    // Commit the changes.
                    tr.Commit();
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                message = e.Message.ToString();
            }
        }

        private static ObjectId GetTextStyleId(string v)
        {
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId id;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                TextStyleTable tst = (TextStyleTable)trans.GetObject(db.TextStyleTableId, OpenMode.ForRead);
                
                id = tst[v];

                trans.Commit();
            }
            return id;
        }

        public static ObjectId GetLinestyleID(string LineStyleName)
        {
            ObjectId result = ObjectId.Null;

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                LinetypeTable ltt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead);
                result = ltt[LineStyleName];
                tr.Commit();
            }

            return result;
        }

        public static bool CheckTextStyle(string TextStyleName)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDb = acDoc.Database;

            // Ensure that the MR_ROMANS text style exists
            using (Transaction AcTrans = Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
            {
                TextStyleTableRecord tstr = new TextStyleTableRecord();
                TextStyleTable tst = (TextStyleTable)AcTrans.GetObject(acDb.TextStyleTableId, OpenMode.ForRead, true, true);

                if (tst.Has(TextStyleName) == true)
                    //if (tst.Has(tst[TextStyleName]) == true)
                    return true;
                return false;
            }
        }

        public static void CreateTextStyle(string TextStyleName, string FontName, double ObliqueAng, out string message)
        {
            try
            {
                using (Transaction transaction = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.TransactionManager.StartTransaction())
                {
                    Database db = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.Database;
                    TextStyleTable tst1 = (TextStyleTable)transaction.GetObject(db.TextStyleTableId, OpenMode.ForWrite, true, true);
                    TextStyleTableRecord tstr1 = new TextStyleTableRecord();
                    tstr1.Name = TextStyleName;
                    tstr1.FileName = FontName;
                    tstr1.XScale = 1;
                    tstr1.ObliquingAngle = Tools.Deg2Rad(ObliqueAng);
                    tstr1.Annotative = AnnotativeStates.True;
                    tst1.Add(tstr1);
                    transaction.TransactionManager.AddNewlyCreatedDBObject(tstr1, true);
                    transaction.Commit();
                    //RmTSid = tstr1.ObjectId;
                    //return true;
                    message = string.Empty;
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception e)
            {
                message = e.Message.ToString();
            }
        }

        public static bool CheckLinestyleExists(string LineStyleName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            using (Transaction tr = doc.TransactionManager.StartTransaction())
            {
                //LinetypeTableRecord lttr = new LinetypeTableRecord();
                LinetypeTable ltt = (LinetypeTable)tr.GetObject(db.LinetypeTableId, OpenMode.ForRead, true);
                if (ltt.Has(LineStyleName))
                    return true;

                return false;
            }
        }

        public static void LoadLinetypes(string LinFile, string LinType)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Linetype table for read
                LinetypeTable acLineTypTbl;
                acLineTypTbl = acTrans.GetObject(acCurDb.LinetypeTableId,
                    OpenMode.ForRead) as LinetypeTable;

                if (acLineTypTbl.Has(LinType) == false)
                {
                    // Load the requested Linetype
                    acCurDb.LoadLineTypeFile(LinType, LinFile);
                }

                // Save the changes and dispose of the transaction
                acTrans.Commit();
            }
        }


        static ObjectId GetArrowObjectId(string newArrowName)
        {
            ObjectId result = ObjectId.Null;

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            string oldArrowName = Application.GetSystemVariable("DIMBLK").ToString();
            Application.SetSystemVariable("DIMBLK", newArrowName);
            if (oldArrowName.Length != 0)
                Application.SetSystemVariable("DIMBLK", oldArrowName);

            Transaction tr = db.TransactionManager.StartTransaction();
            using (tr)
            {
                BlockTable bt = (BlockTable)tr.GetObject(db.BlockTableId, OpenMode.ForRead);
                result = bt[newArrowName];
                tr.Commit();
            }

            return result;
        }





    }
}
