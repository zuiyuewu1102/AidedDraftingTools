using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Windows;

namespace BF_CustomTools
{
    public class StatusBars
    {
        void OnAppMouseDown(object sender,StatusBarMouseDownEventArgs e)
        {

            Pane paneButton = (Pane)sender;

            //Document doc = Application.DocumentManager.MdiActiveDocument;

            //Database db = doc.Database;

            int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));

            String dimStyle = System.Convert.ToString(Application.GetSystemVariable("DIMSTYLE"));

            //string curScale = db.Dimstyle.
            string curTextStyleName = " DimScale (1:" + dimScale.ToString() + " ) DimStyle:( " + dimStyle + " ) ";
            //string alertMessage;

            if (e.Button != System.Windows.Forms.MouseButtons.Left) return;

            paneButton.Text = curTextStyleName;

            //if (paneButton.Style == PaneStyles.PopOut)
            //{
            //    paneButton.Style = PaneStyles.Normal;
                
            //    alertMessage = "程序窗格按钮被按下";
            //}
            //else
            //{
            //    paneButton.Style = PaneStyles.PopOut;
            //    alertMessage = "程序窗格按钮没有被按下";
            //}

            Application.StatusBar.Update();

            //Application.ShowAlertDialog(alertMessage);
        }
        public static void UpdateAppPane()
        {
            //Document doc = Application.DocumentManager.MdiActiveDocument;

            int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));

            String dimStyle = System.Convert.ToString(Application.GetSystemVariable("DIMSTYLE"));

            string curTextStyleName = " DimScale (1:" + dimScale.ToString() + " ) DimStyle:( " + dimStyle + " ) ";

            Pane pane = Application.StatusBar.Panes[6];

            pane.Text = curTextStyleName;

            Application.StatusBar.Update();
        }

        [CommandMethod("CreateAppPane")]
        public static void CreateAppPane()
        {
            //int num = Application.StatusBar.Panes.Count - 1;

            int dimScale = System.Convert.ToInt32(Application.GetSystemVariable("DIMSCALE"));

            String dimStyle = System.Convert.ToString(Application.GetSystemVariable("DIMSTYLE"));

            Pane appPaneButton = new Pane
            {
                Enabled = true,

                Visible = true,

                Style = PaneStyles.Normal,

                Text = " DimScale (1:" + dimScale.ToString() + " ) DimStyle:( " + dimStyle + " ) ",

                ToolTipText = "[百福工具箱]你值得拥有"
            };

            //Application.StatusBar.Update();

            //appPaneButton.MouseDown += OnAppMouseDown;

            Application.StatusBar.Panes.Insert(6, appPaneButton);
        }

        [CommandMethod("StatusBarBalloon")]
        public void StatusBarBalloon()
        {

            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            ObjectId id = ed.GetEntity("请选择需要改变颜色的对象").ObjectId;

            TrayItem trayItem = new TrayItem
            {
                ToolTipText = "change color of Entity"
            };

            //trayItem.Icon = doc.StatusBar.TrayItems[0].Icon;//有问题报错

            Application.StatusBar.TrayItems.Add(trayItem);

            TrayItemBubbleWindow window = new TrayItemBubbleWindow
            {
                Title = "change color of Entity",

                HyperText = "对象颜色修改为红色",

                Text = "点击改变对象的颜色",

                IconType = IconType.Information
            };

            trayItem.ShowBubbleWindow(window);

            Application.StatusBar.Update();

            window.Closed += (sender, e) =>
            {
                if (e.CloseReason == TrayItemBubbleWindowCloseReason.HyperlinkClicked)
                {
                    using (doc.LockDocument())
                    using (Transaction trans = doc.TransactionManager.StartTransaction())
                    {
                        Entity ent = (Entity)trans.GetObject(id, OpenMode.ForWrite);
                        ent.ColorIndex = 1;
                        trans.Commit();
                    }
                }
                Application.StatusBar.TrayItems.Remove(trayItem);

                Application.StatusBar.Update();
            };
        }


    }
}
