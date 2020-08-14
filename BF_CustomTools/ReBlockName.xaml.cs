using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace BF_CustomTools
{
    /// <summary>
    /// ReBlockName.xaml 的交互逻辑
    /// </summary>
    public partial class ReBlockName : UserControl
    {
        public ReBlockName()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            Editor ed = doc.Editor;

            DateTime dt = DateTime.Now;
            string strTime = dt.ToString("yyMMddHHmmss");
            string blockName = "BF" + strTime;

            

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


                    btr2.Name = blockName;
                    trans.Commit();
                }
            }
        }
    }
}
