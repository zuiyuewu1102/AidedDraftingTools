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

        public static RibbonButtonEX PSet 
        {
            get 
            {
                pSet = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\pSet_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                pSet.Text = "偏好";
                pSet.ShowText = true;
                //命令图标
                pSet.Image = bitmapImage;
                pSet.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                pSet.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                pSet.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                pSet.AddRibbonToolTip("PreferencesSet", "偏好设置", "PSet", "设置一些常用参数，图十字光标大小，图纸保存格式，图纸快速打开模板等等", null);

                //给按钮关联命令,注意后面加空格
                pSet.CommandHandler = new RibbonCommandHandler();
                pSet.CommandParameter = "PSet ";
                //鼠标进入离开是的图片
                pSet.ImagePath = Tools.GetCurrentPath() + @"\Images\pSet_32.png";
                pSet.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\pSet_32_hover.png";
                return pSet;
            }
        }

        public static RibbonButtonEX Hthj 
        { 
            get 
            {
                hthj = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\LaySet_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                hthj.Text = "图层";
                hthj.ShowText = true;
                //命令图标
                hthj.Image = bitmapImage;
                hthj.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                hthj.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                hthj.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                hthj.AddRibbonToolTip("LayerLoad", "图层加载", "HTHJ", "将文件中保存的常用图层信息加载到当前文档中……", null);

                //给按钮关联命令,注意后面加空格
                hthj.CommandHandler = new RibbonCommandHandler();
                hthj.CommandParameter = "HTHJ ";
                //鼠标进入离开是的图片
                hthj.ImagePath = Tools.GetCurrentPath() + @"\Images\LaySet_32.png";
                hthj.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\LaySet_32_Hover.png";
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
                hthj.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                hthj.Orientation = System.Windows.Controls.Orientation.Vertical;
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

        public static RibbonButtonEX Tk 
        { 
            get
            {
                tk = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\tk_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                tk.Text = "图框";
                tk.ShowText = true;
                //命令图标
                tk.Image = bitmapImage;
                tk.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                tk.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                tk.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                tk.AddRibbonToolTip("图框", "图框", "TK", "插入对应当前比例的图框块", null);

                //给按钮关联命令,注意后面加空格
                tk.CommandHandler = new RibbonCommandHandler();
                tk.CommandParameter = "TK ";
                //鼠标进入离开是的图片
                tk.ImagePath = Tools.GetCurrentPath() + @"\Images\tk_32.png";
                tk.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\tk_32_Hover.png";
                return tk;
            }
        }

        public static RibbonButtonEX Ymdp 
        {
            get
            {
                ymdp = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\ymdp_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                ymdp.Text = "移门顶剖";
                ymdp.ShowText = true;
                //命令图标
                ymdp.Image = bitmapImage;
                ymdp.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                ymdp.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                ymdp.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                ymdp.AddRibbonToolTip("绘移门顶剖", "绘移门顶剖", "ymdp", "根据输入的起始点和终止点绘制铝材玻璃移门顶剖图", null);

                //给按钮关联命令,注意后面加空格
                ymdp.CommandHandler = new RibbonCommandHandler();
                ymdp.CommandParameter = "ymdp ";
                //鼠标进入离开是的图片
                ymdp.ImagePath = Tools.GetCurrentPath() + @"\Images\ymdp_32.png";
                ymdp.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\ymdp_32_Hover.png";
                return ymdp;
            }
        }
        
        public static RibbonButtonEX Ymcp 
        {
            get
            {
                ymcp = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\ymcp_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                ymcp.Text = "移门侧剖";
                ymcp.ShowText = true;
                //命令图标
                ymcp.Image = bitmapImage;
                ymcp.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                ymcp.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                ymcp.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                ymcp.AddRibbonToolTip("绘移门侧剖", "绘移门侧剖", "ymdp", "根据输入的起始点和终止点绘制铝材玻璃移门侧剖图", null);

                //给按钮关联命令,注意后面加空格
                ymcp.CommandHandler = new RibbonCommandHandler();
                ymcp.CommandParameter = "ymcp ";
                //鼠标进入离开是的图片
                ymcp.ImagePath = Tools.GetCurrentPath() + @"\Images\ymcp_32.png";
                ymcp.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\ymcp_32_Hover.png";
                return ymcp;
            }
        }

        public static RibbonButtonEX Cdj 
        {
            get
            {
                cdj = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\dj_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                cdj.Text = "灯具";
                cdj.ShowText = true;
                //命令图标
                cdj.Image = bitmapImage;
                cdj.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                cdj.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                cdj.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                cdj.AddRibbonToolTip("插入灯具图块", "插入灯具图块", "CDJ", "插入灯具块", null);

                //给按钮关联命令,注意后面加空格
                cdj.CommandHandler = new RibbonCommandHandler();
                cdj.CommandParameter = "CDJ ";
                //鼠标进入离开是的图片
                cdj.ImagePath = Tools.GetCurrentPath() + @"\Images\dj_32.png";
                cdj.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\dj_32_Hover.png";
                return cdj;
            }
        }

        public static RibbonButtonEX Zdgc 
        {
            get
            {
                zdgc = new RibbonButtonEX();
                Uri uri = new Uri(Tools.GetCurrentPath() + @"\Images\zdgc_32.png");
                BitmapImage bitmapImage = new BitmapImage(uri);

                //Name = "SetCurLayer",
                //可见的显示命令名称
                zdgc.Text = "归层";
                zdgc.ShowText = true;
                //命令图标
                zdgc.Image = bitmapImage;
                zdgc.LargeImage = bitmapImage;
                //图片显示尺寸，默认为standard
                zdgc.Size = RibbonItemSize.Large;
                //控件或布局的方向，默认为水平方向
                zdgc.Orientation = System.Windows.Controls.Orientation.Vertical;
                //增加提示信息
                zdgc.AddRibbonToolTip("自动归层", "自动归层", "ZDGC", "检索当前文档中所有的图元，将文字、标注、填充自动修改为相应的图层", null);

                //给按钮关联命令,注意后面加空格
                zdgc.CommandHandler = new RibbonCommandHandler();
                zdgc.CommandParameter = "ZDGC ";
                //鼠标进入离开是的图片
                zdgc.ImagePath = Tools.GetCurrentPath() + @"\Images\zdgc_32.png";
                zdgc.ImageHoverPath = Tools.GetCurrentPath() + @"\Images\zdgc_32_Hover.png";
                return zdgc;
            }
        }

        //偏好设置命令
        private static RibbonButtonEX pSet;

        //图层加载命令（绘图环境）
        private static RibbonButtonEX hthj;

        //选取图元以设定其所在图层为当前图层命令
        private static RibbonButtonEX curLayer;

        //绘制中心线
        private static RibbonButtonEX szc;

        //插入图框
        private static RibbonButtonEX tk;

        //绘制移门顶剖图
        private static RibbonButtonEX ymdp;

        //绘制移门侧剖
        private static RibbonButtonEX ymcp;

        //插入灯具图块
        private static RibbonButtonEX cdj;

        //自动归层
        private static RibbonButtonEX zdgc;
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
