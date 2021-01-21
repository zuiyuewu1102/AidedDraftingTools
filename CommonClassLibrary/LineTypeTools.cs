using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonClassLibrary
{
    public static class LineTypeTools
    {
        public static ObjectId AddLineType(this Database db,string typeName)
        {
            LinetypeTable lt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);

            if (!lt.Has(typeName))
            {
                lt.UpgradeOpen();

                LinetypeTableRecord ltr = new LinetypeTableRecord
                {
                    Name = typeName
                };

                lt.Add(ltr);

                db.TransactionManager.AddNewlyCreatedDBObject(ltr, true);

                lt.DowngradeOpen();
            }

            return lt[typeName];
        }

        public static ObjectId LoadLineType(this Database db,string typeName)
        {
            LinetypeTable lt = (LinetypeTable)db.LinetypeTableId.GetObject(OpenMode.ForRead);

            if (!lt.Has(typeName))
            {
                db.LoadLineTypeFile(typeName, "acadiso.lin");
            }

            return lt[typeName];
        }
    }
}
