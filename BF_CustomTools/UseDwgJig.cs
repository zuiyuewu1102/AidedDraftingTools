using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Text;
using Autodesk.AutoCAD.ApplicationServices;
using CommonClassLibrary;


namespace BF_CustomTools
{
    class CircleJig : EntityJig
    {
        private Point3d m_CenterPt;
        private double m_Radius = 100;
        //派生类构造函数
        public CircleJig(Vector3d normal):base(new Circle())
        {
            ((Circle)Entity).Center = m_CenterPt;
            ((Circle)Entity).Normal = normal;
            ((Circle)Entity).Radius = m_Radius;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {

            JigPromptPointOptions optJig = new JigPromptPointOptions("\n请指定圆心和半径");

            optJig.Keywords.Add("100");
            optJig.Keywords.Add("200");
            optJig.Keywords.Add("300");
            optJig.UserInputControls = UserInputControls.Accept3dCoordinates;
            PromptPointResult resJip = prompts.AcquirePoint(optJig);
            Point3d curPt = resJip.Value;

            if (resJip.Status == PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }

            if (resJip.Status == PromptStatus.Keyword)
            {
                switch (resJip.StringResult)
                {
                    case "100":
                        m_Radius = 100;
                        return SamplerStatus.NoChange;                        
                    case "200":
                        m_Radius = 200;
                        return SamplerStatus.NoChange;                        
                    case "300":
                        m_Radius = 300;
                        return SamplerStatus.NoChange;                                            
                }
            }

            if (m_CenterPt != curPt)
            {
                m_CenterPt = curPt;
                return SamplerStatus.OK;
            }
            else
            {
                return SamplerStatus.NoChange;
            }
        }

        protected override bool Update()
        {
            ((Circle)Entity).Center = m_CenterPt;
            ((Circle)Entity).Radius = m_Radius;
            return true;
        }

        public Entity GetEntity()
        {
            return Entity;
        }
    }

    class JBJig : EntityJig
    {
        private Point2d spt, ept;
        private Point2d m_Point2,m_Point3;        
        private double m_thickness = 12;

        public JBJig(Vector3d normal) : base(new Polyline())
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            
            PromptPointOptions opt1 = new PromptPointOptions("\n请指定绘制板材的起始点");
            //opt1.UserInputControls = UserInputControls.Accept3dCoordinates;
            //用AcquirePoint函数得到用户输入的点
            PromptPointResult res1 = ed.GetPoint(opt1);
            
            if (res1.Status == PromptStatus.Cancel) return;

            if (res1.Status == PromptStatus.OK)
            {
                Point3d m_spt = res1.Value;
                spt = new Point2d(m_spt.X, m_spt.Y);
                PromptPointOptions opt2 = new PromptPointOptions("\n请指定绘制板材的终止点");
                //optJig2.UserInputControls = UserInputControls.Accept3dCoordinates;
                opt2.UseBasePoint = true;
                opt2.BasePoint = m_spt;
                PromptPointResult res2 = ed.GetPoint(opt2);
                //拖拽取消
                if (res2.Status == PromptStatus.Cancel) return;
                if (res2.Status == PromptStatus.OK)
                {
                    Point3d m_ept = res2.Value;
                    ept = new Point2d(m_ept.X, m_ept.Y);
                    PromptIntegerOptions pio = new PromptIntegerOptions("\n请输入绘制板材的厚度");
                    pio.Keywords.Add("3");
                    pio.Keywords.Add("5");
                    pio.Keywords.Add("9");
                    pio.Keywords.Add("12");
                    pio.Keywords.Add("15");
                    pio.Keywords.Add("18");
                    pio.Keywords.Default = "12";
                    PromptIntegerResult pir = ed.GetInteger(pio);
                    if (pir.Status == PromptStatus.OK || pir.Status == PromptStatus.Keyword)
                    {
                        if (pir.Status == PromptStatus.Keyword) 
                        {
                            switch (pir.StringResult)
                            {
                                case "3":
                                    m_thickness = 3;
                                    break;
                                case "5":
                                    m_thickness = 5;
                                    break;
                                case "9":
                                    m_thickness = 9;
                                    break;
                                case "12":
                                    m_thickness = 12;
                                    break;
                                case "15":
                                    m_thickness = 15;
                                    break;
                                case "18":
                                    m_thickness = 18;
                                    break;
                            }
                        }
                        else
                            m_thickness = pir.Value;
                    }
                    double dis = Math.Sqrt((ept.Y - spt.Y) * (ept.Y - spt.Y) + (ept.X - spt.X) * (ept.X - spt.X));
                    double sina = (ept.Y - spt.Y) / dis;
                    double cosa = (ept.X - spt.X) / dis;

                    m_Point2 = new Point2d(ept.X - m_thickness * sina,ept.Y +m_thickness*cosa);
                    m_Point3 = new Point2d(spt.X - m_thickness * sina, spt.Y + m_thickness * cosa);

                    ((Polyline)Entity).Normal = normal;
                    ((Polyline)Entity).AddVertexAt(0, m_Point3, 0, 0, 0);
                    ((Polyline)Entity).AddVertexAt(1, spt, 0, 0, 0);
                    ((Polyline)Entity).AddVertexAt(2, ept, 0, 0, 0);
                    ((Polyline)Entity).AddVertexAt(3, m_Point2, 0, 0, 0);
                    ((Polyline)Entity).Closed = true;

                }
            }          

        }
        
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            
            JigPromptPointOptions jigPpo = new JigPromptPointOptions("\n滑动鼠标选择翻转的方向");
            PromptPointResult resJip = prompts.AcquirePoint(jigPpo);

            if (resJip.Status == PromptStatus.OK)
            {
                Point3d m_fxPt = resJip.Value;
                Point2d fxPt = new Point2d(m_fxPt.X, m_fxPt.Y);
                Point2d ls2, ls3;
                Vector2d vector1 = spt - ept;
                Vector2d vector2 = spt - fxPt;

                double a = vector1.X * vector2.Y - vector1.Y * vector2.X;

                if (a < 0)
                {
                    double dis = Math.Sqrt((ept.Y - spt.Y) * (ept.Y - spt.Y) + (ept.X - spt.X) * (ept.X - spt.X));
                    double sina = (ept.Y - spt.Y) / dis;
                    double cosa = (ept.X - spt.X) / dis;

                    ls2 = new Point2d(ept.X + m_thickness * sina, ept.Y - m_thickness * cosa);
                    ls3 = new Point2d(spt.X + m_thickness * sina, spt.Y - m_thickness * cosa);
                    
                }
                else
                {
                    double dis = Math.Sqrt((ept.Y - spt.Y) * (ept.Y - spt.Y) + (ept.X - spt.X) * (ept.X - spt.X));
                    double sina = (ept.Y - spt.Y) / dis;
                    double cosa = (ept.X - spt.X) / dis;

                    ls2 = new Point2d(ept.X - m_thickness * sina, ept.Y + m_thickness * cosa);
                    ls3 = new Point2d(spt.X - m_thickness * sina, spt.Y + m_thickness * cosa);
                    
                }

                if (ls2 != m_Point2 && ls3 != m_Point3)
                {
                    m_Point2 = ls2;
                    m_Point3 = ls3;
                    return SamplerStatus.OK;
                }
                else
                    return SamplerStatus.NoChange;
            }
            else if (resJip.Status == PromptStatus.Cancel)
                return SamplerStatus.Cancel;
            else
                return SamplerStatus.NoChange;
        }

        protected override bool Update()
        {

            ((Polyline)Entity).StartPoint = new Point3d(m_Point3.X, m_Point3.Y,0);
            ((Polyline)Entity).EndPoint = new Point3d(m_Point2.X, m_Point2.Y, 0);
            
            return true;
        }
        public Entity GetEntity()
        {
            return Entity;
        }
    }

    class MyLineJig : EntityJig//使用此基类来实现动态绘制只可以绘制一个实体
    {
        //基类EntityJig中有一个关键的成员变量Entity,也就是要绘制到模型空间的临时图形，在绘制完成后将其添加到模型空间以及数据库，既完成了动态绘制
        public MyLineJig(Point3d _basePt) : base(new Line(_basePt, _basePt))//在构造之前调用基类构造为基类成员Entity初始化
        {
            m_AcquirePoint = _basePt;
        }

        public static bool StartDrag()//调用此方法开始绘制
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Point3d startPt = Point3d.Origin;
            PromptPointOptions ppo = new PromptPointOptions("jafsf");//需自己实现（获取用户输入点）
            ppo.UseBasePoint = true;
            ppo.BasePoint = startPt;
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK)
                return false;

            MyLineJig lineJig = new MyLineJig(startPt);

            PromptResult PR = doc.Editor.Drag(lineJig);//开始绘制
            if (PR.Status != PromptStatus.OK)
                return false;

            doc.Database.AddToModelSpace(lineJig.Entity);//需自己实现（将实体添加进模型空间）
            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)//提取输入数据并处理(鼠标移动时一定频率调用)
        {
            JigPromptPointOptions JPPO = new JigPromptPointOptions();//定义点绘制的配置类
            JPPO.Message = "\n选择点";

            PromptPointResult PR = prompts.AcquirePoint(JPPO);//当鼠标未移动时，程序会在这里阻塞，直到鼠标移动，提取出当前鼠标位置，继续往下运行
            if (PR.Status != PromptStatus.OK)
            {
                return SamplerStatus.Cancel;
            }


            if (PR.Value.DistanceTo(((Line)Entity).EndPoint) < 0.000001f)//若当前鼠标位置离上一次绘制的位置很近，返回NoChange，不让系统去调用Update去刷新
                //此举是为了减少刷新频率，避免绘制时的闪烁
                //（需要注意的是Jig绘制刚开始和结束的瞬间， 即便Sampler返回的是NoChange，也会调用Update）
                return SamplerStatus.NoChange;

            m_AcquirePoint = PR.Value;//更新数据，返回OK,告诉系统，数据已整理好，需要刷新
            return SamplerStatus.OK;
        }

        protected override bool Update()//刷新（方法中应写对Base.Entity的更改,Sampler返回OK时调用）,(在开始绘制时即便Sampler返回NoChange也会调用,绘制的是圆时应避免给圆的半径赋值零,圆实体会退化成点)
        {
            ((Line)Entity).EndPoint = m_AcquirePoint;//利用绘制好的点去改变实体属性(EntityJig内部实现会把Entity成员绘制到模型空间)
            return true;
        }

        private Point3d m_AcquirePoint;//保存提取出来的鼠标位置
    }

    //DrawJig没有了Entity成员，需要自己定义绘制的实体的成员变量
    class MyDoubleLineJig : DrawJig
    {
        public MyDoubleLineJig(Point3d _basePt)
        {
            line_1 = new Line();
            line_2 = new Line();

            m_BasePt = _basePt;
            m_gap = 10.0f;
        }
        public static bool StartDraw()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Point3d startPt = Point3d.Origin;
            PromptPointOptions ppo = new PromptPointOptions("jafsf");//需自己实现（获取用户输入点）
            ppo.UseBasePoint = true;
            ppo.BasePoint = startPt;
            PromptPointResult ppr = ed.GetPoint(ppo);
            if (ppr.Status != PromptStatus.OK)
                return false;

            MyDoubleLineJig lineJig = new MyDoubleLineJig(startPt);
            PromptResult PR = doc.Editor.Drag(lineJig);
            if (PR.Status != PromptStatus.OK)
                return false;

            doc.Database.AddToModelSpace(lineJig.line_1);//需自己实现（将实体添加进模型空间）
            doc.Database.AddToModelSpace(lineJig.line_2);

            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions m_JPPO = new JigPromptPointOptions();
            m_JPPO.Message = "\ninput end point";
            m_JPPO.UserInputControls = (UserInputControls.Accept3dCoordinates | UserInputControls.NullResponseAccepted | UserInputControls.AnyBlankTerminatesInput);

            PromptPointResult PR = prompts.AcquirePoint(m_JPPO);

            if (PR.Status != PromptStatus.OK)
                return SamplerStatus.Cancel;

            //Point3d pt = PR.Value;
            if (PR.Value == m_AcquirePt)
                return SamplerStatus.NoChange;

            m_AcquirePt = PR.Value;
            return SamplerStatus.OK;
        }

        //与Entity最大不同的在这里（Update->WorldDraw）
        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            Vector3d lineV = m_AcquirePt - m_BasePt;
            Vector3d perpV = lineV.RotateBy(Math.PI / 2.0f, Vector3d.ZAxis);
            perpV = perpV.GetNormal() * m_gap;

            line_1.StartPoint = m_BasePt + perpV;
            line_2.StartPoint = m_BasePt - perpV;
            line_1.EndPoint = m_AcquirePt + perpV;
            line_2.EndPoint = m_AcquirePt - perpV;

            line_1.WorldDraw(draw);//需要调用WorldDraw将想要绘制的实体绘制到模型空间
            line_2.WorldDraw(draw);

            return true;
        }

        Line line_1;
        Line line_2;
        Point3d m_BasePt;
        Point3d m_AcquirePt;

        double m_gap;
    }




}
