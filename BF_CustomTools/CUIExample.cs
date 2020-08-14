using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using CommonClassLibrary;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace BF_CustomTools
{
    public class CUIExample
    {

        /*
        //设置CUI文件的名字，将其路径设置为当前运行目录
        string cuiFile = Tools.GetCurrentPath() + "BF Tools.cuix";
        //菜单组名
        string menuGroupName = "百福工具箱";
        //获得活动文档
        Document activeDoc = Application.DocumentManager.MdiActiveDocument;

        public CUIExample()
        {
            //添加程序退出时事件处理
            Application.QuitWillStart += new EventHandler(Application_QuitWillStart);
        }
        void Application_QuitWillStart(object sender, EventArgs e)
        {
            //由于触发此事件前文档已关闭，所以需通过模板重建，以便命令能够执行
            Document doc = Application.DocumentManager.Add("acadiso.dwt");
            //获取FILEDIA系统变量的值
            object oldFileDia = Application.GetSystemVariable("FILEDIA");
            //设置FILEDIA=0，禁止显示文件对话框，这样可以通过程序输入文件名
            Application.SetSystemVariable("FILEDIA", 0);
            //获取主CUI
            CustomizationSection mainCs = doc.GetMainCustomizationSection();
            //如果存在指定的局部CUI文件，则进行卸载
            if (mainCs.PartialCuiFiles.Contains(cuiFile))
                doc.Editor.Command("cuiunload " + menuGroupName + " ");
            //恢复FILEDIA系统变量的值
            Application.SetSystemVariable("FILEDIA", oldFileDia);
        }

        [CommandMethod("AddMenu")]
        public void AddMenu()
        {
            //当前运行目录
            string currentPath = Tools.GetCurrentPath();

            CustomizationSection cs = activeDoc.AddCui(cuiFile, menuGroupName);

            //添加命令宏
            cs.AddMacro("铝型材", "^C^C_SZC ", "ID_MyLXC", "插入铝型材图块", currentPath + "\\Images\\center_of_Circle_64.png");
            cs.AddMacro("A3图框", "^C^C_A3 ", "ID_MyA3", "插入A3图块", currentPath + "\\Images\\frame_64.png");

            //设置用于下拉菜单别名的字符串集合
            StringCollection sc = new StringCollection();
            sc.Add("MyPop1");

            //添加名为“百福工具箱”的下拉菜单，如果存在则返回null
            PopMenu myMenu = cs.MenuGroup.AddPopMenu("百福工具箱", sc, "ID_MyMenu");

            if(myMenu != null)
            {
                //添加一个名为“图块工具”的子菜单
                PopMenu menuBlockTools = myMenu.AddSubMenu(-1, "图块工具", "ID_MyBlockTools");
                //从上到下为“图块工具”子菜单添加**的菜单项
                menuBlockTools.AddMenuItem(-1, null, "ID_MyLXC");
                menuBlockTools.AddMenuItem(-1, null, "ID_MyA3");
            }
            //必须装载CUI文件才能看到添加的菜单
            cs.LoadCui();
        }*/
    }
}
