using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BF_CustomTools
{
    public class RibbonPanels
    {
        [CommandMethod("Btn")]
        public void Btn()
        {
            RibbonControl ribbonControl = ComponentManager.Ribbon;//获取CAD的Ribbon界面
            RibbonTab ribbonTab = ribbonControl.AddTab("百福工具箱", "Acad.MyRibbonId1", true);//给Ribbon界面添加一个选项卡
            ribbonTab.Panels.Add(ribbonControl.Tabs[0].FindPanel("ID_PanelLayers"));
            ribbonTab.Panels.Add(ribbonControl.Tabs[2].FindPanel("ID_PanelDimensions"));
            //RibbonPanelSource ribbonPanelSource0 = ribbonControl.Tabs[2].FindPanel("ID_PanelDimensions").Source;

            RibbonPanelSource ribbonPanelSource1 = ribbonTab.AddPanel("设置");//给选项卡添加面板
             //添加**命令按钮
            ribbonPanelSource1.Items.Add(RibbonButtonInfos.PSet);
            ribbonPanelSource1.Items.Add(RibbonButtonInfos.Hthj);

            RibbonPanelSource ribbonPanelSource2 = ribbonTab.AddPanel("图层工具");//给选项卡添加面板
            ribbonPanelSource2.Items.Add(RibbonButtonInfos.Zdgc);
            ribbonPanelSource2.Items.Add(RibbonButtonInfos.CurLayer);

            RibbonPanelSource ribbonPanelSource3 = ribbonTab.AddPanel("绘图工具");//给选项卡添加面板
            ribbonPanelSource3.Items.Add(RibbonButtonInfos.Szc);
            ribbonPanelSource3.Items.Add(RibbonButtonInfos.Ymdp);
            ribbonPanelSource3.Items.Add(RibbonButtonInfos.Ymcp);

            RibbonPanelSource ribbonPanelSource4 = ribbonTab.AddPanel("图块工具");//给选项卡添加面板
            ribbonPanelSource4.Items.Add(RibbonButtonInfos.Tk);
            ribbonPanelSource4.Items.Add(RibbonButtonInfos.Cdj);

        }
    }
    public class RibbonButtonEX : RibbonButton
    {
        //正常显示图片
        private string imagePath = "";

        public string ImagePath { get => imagePath; set => imagePath = value; }

        //鼠标进入显示图片
        private string imageHoverPath = "";

        public string ImageHoverPath { get => imageHoverPath; set => imageHoverPath = value; }

        public RibbonButtonEX()
            : base()
        {
            //鼠标进入事件
            this.MouseEntered += Ribbon_MouseEntered;
            //鼠标离开事件
            this.MouseLeft += Ribbon_MouseLeft;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ribbon_MouseLeft(object sender, EventArgs e)
        {
            if (this.imagePath != "")
            {
                RibbonButton btn = (RibbonButton)sender;
                Uri uri = new Uri(this.imagePath);
                BitmapImage bitmapImage = new BitmapImage(uri);
                btn.Image = bitmapImage;
                btn.LargeImage = bitmapImage;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private void Ribbon_MouseEntered(object sender, EventArgs e)
        {
            if (this.imageHoverPath != "")
            {
                RibbonButton btn = (RibbonButton)sender;

                Uri uri = new Uri(this.imageHoverPath);
                BitmapImage bitmapImage = new BitmapImage(uri);
                btn.Image = bitmapImage;
                btn.LargeImage = bitmapImage;
            }
        }
    }
}
