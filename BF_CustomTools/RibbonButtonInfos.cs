using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.Windows;
using CommonClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BF_CustomTools
{
    public static class RibbonButtonInfos
    {
        public static RibbonButtonEX CurLayer 
        {
            get
            {
                curLayer = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\ers_16.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                curLayer.Text = "SetCurLayer";
                curLayer.ShowText = true;
                //命令图标
                curLayer.Image = bitmapImage;
                curLayer.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                //curLayer.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                //curLayer.Orientation = System.Windows.Controls.Orientation.Horizontal;
                //增加提示信息
                curLayer.AddRibbonToolTip("SetCurLayer", "切换当前图层", "ERS", "选取图元以设定其所在图层为当前图层", null);

                //给按钮关联命令,注意后面加空格
                curLayer.CommandHandler = new RibbonCommandHandler();
                curLayer.CommandParameter = "ers ";
                //鼠标进入离开是的图片
                curLayer.ImagePath = Tools.GetCurrentPath() + @"\Images\ers_16.png";
                curLayer.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\ers_16.png";
                return curLayer;
            }            
        }

        //选取图元以设定其所在图层为当前图层命令
        private static RibbonButtonEX curLayer;

        public static RibbonButtonEX PSet 
        {
            get 
            {
                pSet = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\pSet_16.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                pSet.Text = "偏好设置";
                pSet.ShowText = true;
                //命令图标
                pSet.Image = bitmapImage;
                pSet.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                //pSet.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                //pSet.Orientation = System.Windows.Controls.Orientation.Horizontal;
                //增加提示信息
                pSet.AddRibbonToolTip("PreferencesSet", "偏好设置", "PSet", "设置一些常用参数，图十字光标大小，图纸保存格式，图纸快速打开模板等等", null);

                //给按钮关联命令,注意后面加空格
                pSet.CommandHandler = new RibbonCommandHandler();
                pSet.CommandParameter = "PSet ";
                //鼠标进入离开是的图片
                pSet.ImagePath = Tools.GetCurrentPath() + @"\Images\pSet_16.png";
                pSet.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\pSet_16.png";
                return pSet;
            }
        }

        //偏好设置命令
        private static RibbonButtonEX pSet;

        public static RibbonButtonEX Hthj 
        { 
            get 
            {
                hthj = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\LaySet_16.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                hthj.Text = "图层加载";
                hthj.ShowText = true;
                //命令图标
                hthj.Image = bitmapImage;
                hthj.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                //hthj.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                //hthj.Orientation = System.Windows.Controls.Orientation.Horizontal;
                //增加提示信息
                hthj.AddRibbonToolTip("LayerLoad", "图层加载", "HTHJ", "将文件中保存的常用图层信息加载到当前文档中……", null);

                //给按钮关联命令,注意后面加空格
                hthj.CommandHandler = new RibbonCommandHandler();
                hthj.CommandParameter = "HTHJ ";
                //鼠标进入离开是的图片
                hthj.ImagePath = Tools.GetCurrentPath() + @"\Images\LaySet_16.png";
                hthj.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\LaySet_16_Hover.png";
                return hthj;
            } 
        }

        public static RibbonButtonEX Szc 
        {
            get
            {
                szc = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\centerline_16.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                szc.Text = "中心线";
                szc.ShowText = true;
                //命令图标
                szc.Image = bitmapImage;
                szc.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                //hthj.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                //hthj.Orientation = System.Windows.Controls.Orientation.Horizontal;
                //增加提示信息
                szc.AddRibbonToolTip("CenterLine", "中心线", "SZC", "选择圆添加十字中心线", null);

                //给按钮关联命令,注意后面加空格
                szc.CommandHandler = new RibbonCommandHandler();
                szc.CommandParameter = "SZC ";
                //鼠标进入离开是的图片
                szc.ImagePath = Tools.GetCurrentPath() + @"\Images\centerline_16.png";
                szc.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\centerline_16_Hover.png";
                return szc;
            }
        }

        //图层加载命令（绘图环境）
        private static RibbonButtonEX hthj;

        //绘制中心线
        private static RibbonButtonEX szc;

    }
    public class RibbonCommandHandler : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is RibbonButton btn)
            {
                if (btn.CommandParameter != null)
                {
                    Document doc = Application.DocumentManager.MdiActiveDocument;
                    doc.SendStringToExecute(btn.CommandParameter.ToString(), true, false, false);
                }
            }
        }
    }
}
