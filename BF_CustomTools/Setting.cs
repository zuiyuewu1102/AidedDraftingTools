using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using Autodesk.AutoCAD.Runtime;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BF_CustomTools
{
    public class Setting
    {
        private readonly Document acDoc = Application.DocumentManager.MdiActiveDocument;
        //private Database acDb = Application.DocumentManager.MdiActiveDocument.Database;

        //设置一些基本的偏好设置
        [CommandMethod("PSet")]
        public void PSet()
        {            
            Editor ed = acDoc.Editor;
            ed.WriteMessage("[百福工具箱]——基本设置");
            AcadPreferences acPre = (AcadPreferences)Application.Preferences;
            //设置十字光标大小
            acPre.Display.CursorSize = 50;
            ed.WriteMessage("\n十字光标大小设置为50");
            //设置文件另存的版本
            acPre.OpenSave.SaveAsType = AcSaveAsType.ac2013_dwg;
            ed.WriteMessage("\n文件另存版本设置为AutoCAD 2013 图形 （*.dwg）");
            //设置自动保存地址
            acPre.Files.AutoSavePath = @"E:\CAD自动保存\";
            ed.WriteMessage("\n文件自动保存位置设置为" + @"E:\CAD自动保存\");
            //快速新建图纸样板文件
            acPre.Files.QNewTemplateFile = Tools.GetCurrentPath() + @"\SetFiles\BF_template.dwt";
            ed.WriteMessage("\n快速新建图纸样板文件设置为" + Tools.GetCurrentPath() + @"\SetFiles\BF_template.dwt");
            //acPre.Files.TempFilePath= Tools.GetCurrentPath() + @"\SetFiles\BF_template.dwt";
            //acPre.Files.TemplateDwgPath = Tools.GetCurrentPath() + @"\SetFiles\BF_template.dwt";
            //用于图纸创建和页面设置替代的默认样板
            //acPre.Files.PageSetupOverridesTemplateFile = Tools.GetCurrentPath()+ @"\SetFiles\BF_template.dwt";
            //设置不显示代理对话框
            acPre.OpenSave.ShowProxyDialogBox = false;
            ed.WriteMessage("\n设置不显示代理对话框");
            //设置不自动创建副本
            acPre.OpenSave.CreateBackup = false;
            ed.WriteMessage("\n文件设置为不创建副本");
            //acPre.Display.GraphicsWinModelBackgrndColor = 1;
        }

        [CommandMethod("SD")]
        public void SD()
        {
            Editor ed = acDoc.Editor;
            ed.WriteMessage("[百福工具箱]——设定图纸比例和缩放各标注");

            //int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));
            //String dimStyle = System.Convert.ToString(Application.GetSystemVariable("DIMSTYLE"));

            SetDwgScaleForm form1 = new SetDwgScaleForm();
            form1.ShowDialog();

            StatusBars.CreateAppPane();
        }
    }
}
