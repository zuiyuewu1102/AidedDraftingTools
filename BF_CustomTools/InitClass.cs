using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using Autodesk.AutoCAD.Windows;
using System.Collections.Specialized;
using Autodesk.AutoCAD.Customization;
using CommonClassLibrary;

[assembly: ExtensionApplication(typeof(BF_CustomTools.InitClass))] 
//启动时加载工具栏，注意typeof括号里的类库名

namespace BF_CustomTools
{
    public class InitClass : IExtensionApplication
    {
        public void Initialize()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            
            ed.WriteMessage("\n程序开始初始化请耐心等待……………………………………");


            //MyRibbonPanels.Ribbtn();
            //AddMenu();报错

            //string cuiFile = Tools.GetCurrentPath() + "\\BF Custom Tools.cuix";

            //string menuGroupName = "BF Custom Tools";

            //CustomizationSection cs = doc.AddCui(cuiFile, menuGroupName);

            //cs.LoadCui();

        }

        public void Terminate()
        {
            
        }

        public void AddMenu()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            
            AcadApplication acadApp = Application.AcadApplication as AcadApplication;

            //创建建菜单栏的对象
            AcadPopupMenu myMenu = null;

            // 创建菜单
            if (myMenu == null)
            {
                // 菜单名称
                myMenu = acadApp.MenuGroups.Item(0).Menus.Add("BF Custom Tools");

                myMenu.AddMenuItem(myMenu.Count, "改比例", "GBL "); //每个命令后面有一个空格，相当于咱们输入命令按空格
                myMenu.AddMenuItem(myMenu.Count, "自动归层", "ZDGC ");
                myMenu.AddMenuItem(myMenu.Count, "绘图环境", "HTHJ ");
                //开始加子菜单栏
                AcadPopupMenu subMenu = myMenu.AddSubMenu(myMenu.Count, "BlockTools");  //子菜单对象
                subMenu.AddMenuItem(myMenu.Count, "插入灯具", "CDJ ");
                subMenu.AddMenuItem(myMenu.Count, "插入图框", "TK ");
                subMenu.AddMenuItem(myMenu.Count, "插入铝材", "LXC ");
                myMenu.AddSeparator(myMenu.Count); //加入分割符号
                //结束加子菜单栏
            }

            // 菜单是否显示  看看已经显示的菜单栏里面有没有这一栏
            bool isShowed = false;  //初始化没有显示
            foreach (AcadPopupMenu menu in acadApp.MenuBar)  //遍历现有所有菜单栏
            {
                if (menu == myMenu)
                {
                    isShowed = true;
                    break;
                }
            }

            // 显示菜单 加载自定义的菜单栏
            if (!isShowed)
            {
                myMenu.InsertInMenuBar(acadApp.MenuBar.Count);
            }

            doc.Editor.WriteMessage("\n添加菜单栏成功！………………………………………………");
        }


    }
}
