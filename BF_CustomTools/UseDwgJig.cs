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
    public static class LinshiFangfa
    {
        public static Point2d PolarPoint(Point2d basePt, double angle, double dis)
        {
            double x = basePt[0] + dis * Math.Cos(angle);
            double y = basePt[1] + dis * Math.Sin(angle);
            Point2d point = new Point2d(x, y);
            return point;
        }
        public static double Rad2Ang(double angle)
        {
            double rad = angle * Math.PI / 180.0;
            return rad;
        }
        
    }
    class JiaCengBanJig : EntityJig
    {
        public double m_t;
        public Point3d m_peakPt;
        private readonly Point2d[] m_pts = new Point2d[4];
        public JiaCengBanJig(Point3d spt,Point3d ept,double t):base(new Polyline())
        {
            m_t = t;
            m_pts[0] = new Point2d(spt.X, spt.Y);
            m_pts[1] = new Point2d(ept.X, ept.Y);
            ((Polyline)Entity).AddVertexAt(0, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(1, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(2, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(3, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).Closed = true;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions optJig = new JigPromptPointOptions("\n移动鼠标确定翻转方向");
            PromptPointResult resJig = prompts.AcquirePoint(optJig);
            Point3d curPt = resJig.Value;

            if (resJig.Status == PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }
            if (m_peakPt != curPt)
            {
                m_peakPt = curPt;
                Vector2d vec1 = m_pts[1] - m_pts[0];
                Vector2d vec2 = new Point2d(m_peakPt.X, m_peakPt.Y) - m_pts[0];
                if(vec1.Angle > Math.PI)
                {
                    if (vec2.Angle >= vec1.Angle || vec2.Angle <= vec1.Angle - Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                    }
                }
                else
                {
                    if (vec2.Angle >= vec1.Angle && vec2.Angle <= vec1.Angle + Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                    }
                }
                
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }
        protected override bool Update()
        {
            ((Polyline)Entity).SetPointAt(0, m_pts[0]);
            ((Polyline)Entity).SetPointAt(1, m_pts[1]);
            ((Polyline)Entity).SetPointAt(2, m_pts[2]);
            ((Polyline)Entity).SetPointAt(3, m_pts[3]);
            ((Polyline)Entity).Normal = Vector3d.ZAxis;
            ((Polyline)Entity).Elevation = 0.0;
            ((Polyline)Entity).Closed = true;
            return true;
        }
        public Entity GetEntity()
        {
            return Entity;
        }
    }

    class BoLiJig : EntityJig
    {
        public double m_t;
        public Point3d m_peakPt;
        private readonly Point2d[] m_pts = new Point2d[8];
        public BoLiJig(Point3d spt, Point3d ept, double t) : base(new Polyline())
        {
            m_t = t;
            Vector2d v1 = new Point2d(ept.X, ept.Y) - new Point2d(spt.X, spt.Y);
            m_pts[0] = LinshiFangfa.PolarPoint(new Point2d(spt.X, spt.Y), v1.Angle, 1);
            m_pts[1] = LinshiFangfa.PolarPoint(new Point2d(ept.X, ept.Y), v1.Angle + Math.PI, 1);
            ((Polyline)Entity).AddVertexAt(0, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(1, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(2, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(3, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(4, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(5, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(6, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(7, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).Closed = true;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions optJig = new JigPromptPointOptions("\n移动鼠标确定翻转方向或[中线偏移(C)]");
            optJig.Keywords.Add("C");
            PromptPointResult resJig = prompts.AcquirePoint(optJig);
            Point3d curPt = resJig.Value;

            if (resJig.Status == PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }
            else if(resJig.Status == PromptStatus.Keyword)
            {
                Vector2d vec1 = m_pts[1] - m_pts[0];
                m_pts[0] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t / 2);
                m_pts[1] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t / 2);
                m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(45.0), Math.Sqrt(2));
                m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                m_pts[4] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                m_pts[5] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                m_pts[6] = LinshiFangfa.PolarPoint(m_pts[5], vec1.Angle + LinshiFangfa.Rad2Ang(225.0), Math.Sqrt(2));
                m_pts[7] = LinshiFangfa.PolarPoint(m_pts[6], vec1.Angle + LinshiFangfa.Rad2Ang(270.0), m_t - 2);
                bool isUpdate = Update();
                if(isUpdate) return SamplerStatus.OK;
            }
            if (m_peakPt != curPt)
            {
                m_peakPt = curPt;
                Vector2d vec1 = m_pts[1] - m_pts[0];
                Vector2d vec2 = new Point2d(m_peakPt.X, m_peakPt.Y) - m_pts[0];
                if (vec1.Angle > Math.PI)
                {
                    if (vec2.Angle >= vec1.Angle || vec2.Angle <= vec1.Angle - Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(45.0), Math.Sqrt(2));
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[6] = LinshiFangfa.PolarPoint(m_pts[5], vec1.Angle + LinshiFangfa.Rad2Ang(225.0), Math.Sqrt(2));
                        m_pts[7] = LinshiFangfa.PolarPoint(m_pts[6], vec1.Angle + LinshiFangfa.Rad2Ang(270.0), m_t - 2);
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(45.0), Math.Sqrt(2));
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[6] = LinshiFangfa.PolarPoint(m_pts[5], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(2));
                        m_pts[7] = LinshiFangfa.PolarPoint(m_pts[6], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                    }
                }
                else
                {
                    if (vec2.Angle >= vec1.Angle && vec2.Angle <= vec1.Angle + Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(45.0), Math.Sqrt(2));
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[6] = LinshiFangfa.PolarPoint(m_pts[5], vec1.Angle + LinshiFangfa.Rad2Ang(225.0), Math.Sqrt(2));
                        m_pts[7] = LinshiFangfa.PolarPoint(m_pts[6], vec1.Angle + LinshiFangfa.Rad2Ang(270.0), m_t - 2);
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(45.0), Math.Sqrt(2));
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[0], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t);
                        m_pts[6] = LinshiFangfa.PolarPoint(m_pts[5], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(2));
                        m_pts[7] = LinshiFangfa.PolarPoint(m_pts[6], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                    }
                }
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }
        protected override bool Update()
        {
            ((Polyline)Entity).SetPointAt(0, m_pts[0]);
            ((Polyline)Entity).SetPointAt(1, m_pts[1]);
            ((Polyline)Entity).SetPointAt(2, m_pts[2]);
            ((Polyline)Entity).SetPointAt(3, m_pts[3]);
            ((Polyline)Entity).SetPointAt(4, m_pts[4]);
            ((Polyline)Entity).SetPointAt(5, m_pts[5]);
            ((Polyline)Entity).SetPointAt(6, m_pts[6]);
            ((Polyline)Entity).SetPointAt(7, m_pts[7]);
            ((Polyline)Entity).Normal = Vector3d.ZAxis;
            ((Polyline)Entity).Elevation = 0.0;
            ((Polyline)Entity).Closed = true;
            return true;
        }
        public Entity GetEntity()
        {
            return Entity;
        }
    }

    class ShiTouBanJig : EntityJig
    {
        public double m_t;
        public Point3d m_peakPt;
        private readonly Point2d[] m_pts = new Point2d[6];
        public ShiTouBanJig(Point3d spt, Point3d ept, double t) : base(new Polyline())
        {
            m_t = t;
            m_pts[0] = new Point2d(spt.X, spt.Y);
            m_pts[1] = new Point2d(ept.X, ept.Y);
            ((Polyline)Entity).AddVertexAt(0, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(1, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(2, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(3, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(4, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).AddVertexAt(5, Point2d.Origin, 0.0, 0.0, 0.0);
            ((Polyline)Entity).Closed = true;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            JigPromptPointOptions optJig = new JigPromptPointOptions("\n移动鼠标确定翻转方向");
            PromptPointResult resJig = prompts.AcquirePoint(optJig);
            Point3d curPt = resJig.Value;

            if (resJig.Status == PromptStatus.Cancel)
            {
                return SamplerStatus.Cancel;
            }
            if (m_peakPt != curPt)
            {
                m_peakPt = curPt;
                Vector2d vec1 = m_pts[1] - m_pts[0];
                Vector2d vec2 = new Point2d(m_peakPt.X, m_peakPt.Y) - m_pts[0];
                if (vec1.Angle > Math.PI)
                {
                    if (vec2.Angle >= vec1.Angle || vec2.Angle <= vec1.Angle - Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[3], vec1.Angle + Math.PI, vec1.Length - 4);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[4], vec1.Angle + LinshiFangfa.Rad2Ang(225.0), Math.Sqrt(8));
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle - LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[3], vec1.Angle + Math.PI, vec1.Length - 4);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[4], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                    }
                }
                else
                {
                    if (vec2.Angle >= vec1.Angle && vec2.Angle <= vec1.Angle + Math.PI)
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle + LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[3], vec1.Angle + Math.PI, vec1.Length - 4);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[4], vec1.Angle + LinshiFangfa.Rad2Ang(225.0), Math.Sqrt(8));
                    }
                    else
                    {
                        m_pts[2] = LinshiFangfa.PolarPoint(m_pts[1], vec1.Angle - LinshiFangfa.Rad2Ang(90.0), m_t - 2);
                        m_pts[3] = LinshiFangfa.PolarPoint(m_pts[2], vec1.Angle - LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                        m_pts[4] = LinshiFangfa.PolarPoint(m_pts[3], vec1.Angle + Math.PI, vec1.Length - 4);
                        m_pts[5] = LinshiFangfa.PolarPoint(m_pts[4], vec1.Angle + LinshiFangfa.Rad2Ang(135.0), Math.Sqrt(8));
                    }
                }
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }
        protected override bool Update()
        {
            ((Polyline)Entity).SetPointAt(0, m_pts[0]);
            ((Polyline)Entity).SetPointAt(1, m_pts[1]);
            ((Polyline)Entity).SetPointAt(2, m_pts[2]);
            ((Polyline)Entity).SetPointAt(3, m_pts[3]);
            ((Polyline)Entity).SetPointAt(4, m_pts[4]);
            ((Polyline)Entity).SetPointAt(5, m_pts[5]);            
            ((Polyline)Entity).Normal = Vector3d.ZAxis;
            ((Polyline)Entity).Elevation = 0.0;
            ((Polyline)Entity).Closed = true;
            return true;
        }
        public Entity GetEntity()
        {
            return Entity;
        }        
    }

    class PiGeBanJig : DrawJig
    {
        public Polyline m_pl1, m_pl2;
        private Point3d m_peakPt;
        private readonly double m_t;
        public Point3d m_spt, m_ept;
        public Point2d[] pts1 = new Point2d[4];
        public Point2d[] pts2 = new Point2d[8];

        public PiGeBanJig(Point3d spt,Point3d ept,double t,Polyline pl1,Polyline pl2)
        {
            m_spt = spt;
            m_ept = ept;
            m_t = t;
            m_pl1 = pl1;
            m_pl2 = pl2;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            //Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            //Matrix3d mt = ed.CurrentUserCoordinateSystem;
            JigPromptPointOptions jppo = new JigPromptPointOptions("\n移动鼠标确定翻转方向")
            {
                //光标类型
                Cursor = CursorType.Crosshair,
                //拖拽限制
                UserInputControls = UserInputControls.Accept3dCoordinates
                | UserInputControls.NoZeroResponseAccepted
                | UserInputControls.NoNegativeResponseAccepted,
                //拖拽基点必须是WCS点
                BasePoint = new Point3d(m_ept.X, m_ept.Y, 0),
                UseBasePoint = true
            };
            PromptPointResult ppr = prompts.AcquirePoint(jppo);
            Point3d tempPt = ppr.Value;
            //拖拽取消
            if (ppr.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
            if(m_peakPt != tempPt)
            {
                m_peakPt = tempPt;
                //将WCS点转化未UCS点
                //m_peakPt = m_peakPt.TransformBy(mt.Inverse());
                Vector2d vec1 = new Point2d(m_ept.X,m_ept.Y) - new Point2d(m_spt.X,m_spt.Y);
                Vector2d vec2 = new Point2d(m_peakPt.X, m_peakPt.Y) - new Point2d(m_spt.X, m_spt.Y);
                pts2[2] = new Point2d(m_spt.X, m_spt.Y);
                pts2[1] = LinshiFangfa.PolarPoint(pts2[2], vec1.Angle, 20.0);
                pts2[5] = new Point2d(m_ept.X, m_ept.Y);
                pts2[6] = LinshiFangfa.PolarPoint(pts2[5], vec1.Angle, -20.0);

                if (vec1.Angle > Math.PI)
                {
                    if (vec2.Angle >= vec1.Angle || vec2.Angle <= vec1.Angle - Math.PI)
                    {
                        pts1[0] = LinshiFangfa.PolarPoint(new Point2d(m_spt.X, m_spt.Y), vec1.Angle + Math.PI / 4, Math.Sqrt(2));
                        pts1[1] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle, vec1.Length - 2);
                        pts1[2] = LinshiFangfa.PolarPoint(pts1[1], vec1.Angle + Math.PI / 2, m_t);
                        pts1[3] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle + Math.PI / 2, m_t);

                        pts2[0] = LinshiFangfa.PolarPoint(pts2[1], vec1.Angle + Math.PI / 2, 1.0);
                        pts2[3] = LinshiFangfa.PolarPoint(pts2[2], vec1.Angle + Math.PI / 2, m_t + 2.0);
                        pts2[4] = LinshiFangfa.PolarPoint(pts2[5], vec1.Angle + Math.PI / 2, m_t + 2.0);
                        pts2[7] = LinshiFangfa.PolarPoint(pts2[6], vec1.Angle + Math.PI / 2, 1.0);
                    }
                    else
                    {
                        pts1[0] = LinshiFangfa.PolarPoint(new Point2d(m_spt.X, m_spt.Y), vec1.Angle - Math.PI / 4, Math.Sqrt(2));
                        pts1[1] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle, vec1.Length - 2);
                        pts1[2] = LinshiFangfa.PolarPoint(pts1[1], vec1.Angle - Math.PI / 2, m_t);
                        pts1[3] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle - Math.PI / 2, m_t);

                        pts2[0] = LinshiFangfa.PolarPoint(pts2[1], vec1.Angle - Math.PI / 2, 1.0);
                        pts2[3] = LinshiFangfa.PolarPoint(pts2[2], vec1.Angle - Math.PI / 2, m_t + 2.0);
                        pts2[4] = LinshiFangfa.PolarPoint(pts2[5], vec1.Angle - Math.PI / 2, m_t + 2.0);
                        pts2[7] = LinshiFangfa.PolarPoint(pts2[6], vec1.Angle - Math.PI / 2, 1.0);
                    }
                }
                else
                {
                    if (vec2.Angle >= vec1.Angle && vec2.Angle <= vec1.Angle + Math.PI)
                    {
                        pts1[0] = LinshiFangfa.PolarPoint(new Point2d(m_spt.X, m_spt.Y), vec1.Angle + Math.PI / 4, Math.Sqrt(2));
                        pts1[1] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle, vec1.Length - 2);
                        pts1[2] = LinshiFangfa.PolarPoint(pts1[1], vec1.Angle + Math.PI / 2, m_t);
                        pts1[3] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle + Math.PI / 2, m_t);

                        pts2[0] = LinshiFangfa.PolarPoint(pts2[1], vec1.Angle + Math.PI / 2, 1.0);
                        pts2[3] = LinshiFangfa.PolarPoint(pts2[2], vec1.Angle + Math.PI / 2, m_t + 2.0);
                        pts2[4] = LinshiFangfa.PolarPoint(pts2[5], vec1.Angle + Math.PI / 2, m_t + 2.0);
                        pts2[7] = LinshiFangfa.PolarPoint(pts2[6], vec1.Angle + Math.PI / 2, 1.0);
                    }
                    else
                    {
                        pts1[0] = LinshiFangfa.PolarPoint(new Point2d(m_spt.X, m_spt.Y), vec1.Angle - Math.PI / 4, Math.Sqrt(2));
                        pts1[1] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle, vec1.Length - 2);
                        pts1[2] = LinshiFangfa.PolarPoint(pts1[1], vec1.Angle - Math.PI / 2, m_t);
                        pts1[3] = LinshiFangfa.PolarPoint(pts1[0], vec1.Angle - Math.PI / 2, m_t);

                        pts2[0] = LinshiFangfa.PolarPoint(pts2[1], vec1.Angle - Math.PI / 2, 1.0);
                        pts2[3] = LinshiFangfa.PolarPoint(pts2[2], vec1.Angle - Math.PI / 2, m_t + 2.0);
                        pts2[4] = LinshiFangfa.PolarPoint(pts2[5], vec1.Angle - Math.PI / 2, m_t + 2.0);
                        pts2[7] = LinshiFangfa.PolarPoint(pts2[6], vec1.Angle - Math.PI / 2, 1.0);
                    }
                }
                m_pl1.Normal = Vector3d.ZAxis;
                m_pl1.Elevation = 0.0;
                m_pl1.SetPointAt(0, pts1[0]);
                m_pl1.SetPointAt(1, pts1[1]);
                m_pl1.SetPointAt(2, pts1[2]);
                m_pl1.SetPointAt(3, pts1[3]);

                m_pl2.Normal = Vector3d.ZAxis;
                m_pl2.Elevation = 0.0;
                m_pl2.SetPointAt(0, pts2[0]);
                m_pl2.SetPointAt(1, pts2[1]);
                m_pl2.SetPointAt(2, pts2[2]);
                m_pl2.SetPointAt(3, pts2[3]);
                m_pl2.SetPointAt(4, pts2[4]);
                m_pl2.SetPointAt(5, pts2[5]);
                m_pl2.SetPointAt(6, pts2[6]);
                m_pl2.SetPointAt(7, pts2[7]);
                return SamplerStatus.OK;
            }
            return SamplerStatus.OK;
        }

        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            draw.Geometry.Draw(m_pl1);
            draw.Geometry.Draw(m_pl2);
            return true;
        }
    }

    class FanMenJig : DrawJig
    {
        public Polyline m_pl1, m_pl2, m_pl3, m_pl4, m_pl5;
        public BlockReference m_bRef;
        private Point3d m_pt1, m_pt2;
        //private Database db = Application.DocumentManager.MdiActiveDocument.Database;
        public FanMenJig(Polyline pl1,Polyline pl2,Polyline pl3,Polyline pl4,Polyline pl5,Point3d spt,BlockReference bRef)
        {
            m_pl1 = pl1;
            m_pl2 = pl2;
            m_pl3 = pl3;
            m_pl4 = pl4;
            m_pl5 = pl5;
            m_bRef = bRef;
            m_pt1 = spt;
            
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            //定义一个拖拽交互类
            JigPromptPointOptions jppo = new JigPromptPointOptions("\n请指定门洞的对角点")
            {
                //光标类型
                Cursor = CursorType.Crosshair,
                //拖拽限制
                UserInputControls =
                UserInputControls.Accept3dCoordinates
                | UserInputControls.NoZeroResponseAccepted
                | UserInputControls.NoNegativeResponseAccepted,
                //拖拽基点必须是WCS点
                BasePoint = m_pt1.TransformBy(mt),
                UseBasePoint = true
            };
            //用AcquirePoint函数获得拖拽得到的即时点
            PromptPointResult ppr = prompts.AcquirePoint(jppo);
            Point3d tempPt = ppr.Value;
            //拖拽取消
            if (ppr.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
            //拖拽
            if (m_pt2 != tempPt)
            {
                m_pt2 = tempPt;
                //将WCS点转化为UCS点
                Point3d ucsPt2 = m_pt2.TransformBy(mt.Inverse());
                //转换成最小点和最大点
                Point3d minPt = new Point3d(Math.Min(m_pt1.X, ucsPt2.X), Math.Min(m_pt1.Y, ucsPt2.Y), 0);
                Point3d maxPt = new Point3d(Math.Max(m_pt1.X, ucsPt2.X), Math.Max(m_pt1.Y, ucsPt2.Y), 0);
                //更新m_pl1参数
                m_pl1.Normal = Vector3d.ZAxis;
                m_pl1.Elevation = 0.0;
                m_pl1.SetPointAt(0, new Point2d(minPt.X + 2, minPt.Y + 4));
                m_pl1.SetPointAt(1, new Point2d(minPt.X + 7, minPt.Y + 4));
                m_pl1.SetPointAt(2, new Point2d(minPt.X + 7, maxPt.Y - 2));
                m_pl1.SetPointAt(3, new Point2d(minPt.X + 2, maxPt.Y - 2));
                //更新m_pl2参数
                m_pl2.Normal = Vector3d.ZAxis;
                m_pl2.Elevation = 0.0;
                m_pl2.SetPointAt(0, new Point2d(maxPt.X - 2, minPt.Y + 4));
                m_pl2.SetPointAt(1, new Point2d(maxPt.X - 7, minPt.Y + 4));
                m_pl2.SetPointAt(2, new Point2d(maxPt.X - 7, maxPt.Y - 2));
                m_pl2.SetPointAt(3, new Point2d(maxPt.X - 2, maxPt.Y - 2));
                //更新m_pl3参数
                m_pl3.Normal = Vector3d.ZAxis;
                m_pl3.Elevation = 0.0;
                m_pl3.SetPointAt(0, new Point2d(minPt.X + 7, minPt.Y + 4));
                m_pl3.SetPointAt(1, new Point2d(minPt.X + 7, minPt.Y + 9));
                m_pl3.SetPointAt(2, new Point2d(maxPt.X - 7, minPt.Y + 9));
                m_pl3.SetPointAt(3, new Point2d(maxPt.X - 7, minPt.Y + 4));
                //更新m_pl4参数
                m_pl4.Normal = Vector3d.ZAxis;
                m_pl4.Elevation = 0.0;
                m_pl4.SetPointAt(0, new Point2d(minPt.X + 7, maxPt.Y - 7));
                m_pl4.SetPointAt(1, new Point2d(minPt.X + 7, maxPt.Y - 2));
                m_pl4.SetPointAt(2, new Point2d(maxPt.X - 7, maxPt.Y - 2));
                m_pl4.SetPointAt(3, new Point2d(maxPt.X - 7, maxPt.Y - 7));
                //更新m_pl5参数
                m_pl5.Normal = Vector3d.ZAxis;
                m_pl5.Elevation = 0.0;
                m_pl5.SetPointAt(0, new Point2d(minPt.X + 2, maxPt.Y - 2));
                m_pl5.SetPointAt(1, new Point2d((maxPt.X - minPt.X) / 2 + minPt.X, minPt.Y + 4));
                m_pl5.SetPointAt(2, new Point2d(maxPt.X - 2, maxPt.Y - 2));
                //更新bRef参数
                m_bRef.Position = new Point3d((maxPt.X - minPt.X) / 2 + minPt.X, maxPt.Y - 29, 0);
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }

        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            draw.Geometry.Draw(m_pl1);
            draw.Geometry.Draw(m_pl2);
            draw.Geometry.Draw(m_pl3);
            draw.Geometry.Draw(m_pl4);
            draw.Geometry.Draw(m_pl5);
            draw.Geometry.Draw(m_bRef);
            return true; 
        }
    }

    class HoleonWallJig : DrawJig
    {
        public Polyline m_pl1, m_pl2;
        private Point3d m_pt1, m_pt2;
        public HoleonWallJig(Polyline pl1,Polyline pl2,Point3d spt)
        {
            m_pl1 = pl1;
            m_pl2 = pl2;
            m_pt1 = spt;
        }
        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            //定义一个拖拽交互类
            JigPromptPointOptions jppo = new JigPromptPointOptions("\n请指定墙洞的对角点")
            {
                //光标类型
                Cursor = CursorType.Crosshair,
                //拖拽限制
                UserInputControls =
                UserInputControls.Accept3dCoordinates
                | UserInputControls.NoZeroResponseAccepted
                | UserInputControls.NoNegativeResponseAccepted,
                //拖拽基点必须是WCS点
                BasePoint = m_pt1.TransformBy(mt),
                UseBasePoint = true
            };
            //用AcquirePoint函数获得拖拽得到的即时点
            PromptPointResult ppr = prompts.AcquirePoint(jppo);
            Point3d tempPt = ppr.Value;
            //拖拽取消
            if (ppr.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
            //拖拽
            if (m_pt2 != tempPt)
            {
                m_pt2 = tempPt;
                //将WCS点转化为UCS点
                Point3d ucsPt2 = m_pt2.TransformBy(mt.Inverse());
                //转换成最小点和最大点
                Point3d minPt = new Point3d(Math.Min(m_pt1.X, ucsPt2.X), Math.Min(m_pt1.Y, ucsPt2.Y), 0);
                Point3d maxPt = new Point3d(Math.Max(m_pt1.X, ucsPt2.X), Math.Max(m_pt1.Y, ucsPt2.Y), 0);
                //更新m_pl1参数
                m_pl1.Normal = Vector3d.ZAxis;
                m_pl1.Elevation = 0.0;
                m_pl1.SetPointAt(0, new Point2d(minPt.X, minPt.Y));
                m_pl1.SetPointAt(1, new Point2d(maxPt.X, minPt.Y));
                m_pl1.SetPointAt(2, new Point2d(maxPt.X, maxPt.Y));
                m_pl1.SetPointAt(3, new Point2d(minPt.X, maxPt.Y));
                //更新m_pl2参数
                m_pl2.Normal = Vector3d.ZAxis;
                m_pl2.Elevation = 0.0;
                m_pl2.SetPointAt(0, new Point2d(minPt.X, minPt.Y));
                m_pl2.SetPointAt(1, new Point2d(minPt.X + 80, maxPt.Y - 80));
                m_pl2.SetPointAt(2, new Point2d(maxPt.X, maxPt.Y));
                return SamplerStatus.OK;
            }
            else
                return SamplerStatus.NoChange;
        }

        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            draw.Geometry.Draw(m_pl1);
            draw.Geometry.Draw(m_pl2);
            return true;
        }
    }

    class LiXingCai : DrawJig
    {
        private Point3d m_pt;
        public BlockReference m_bRef;

        public LiXingCai(BlockReference bRef,Point3d pt)
        {
            m_pt = pt;
            m_bRef = bRef;
        }

        protected override bool WorldDraw(Autodesk.AutoCAD.GraphicsInterface.WorldDraw draw)
        {
            draw.Geometry.Draw(m_bRef);
            return true;
        }

        protected override SamplerStatus Sampler(JigPrompts prompts)
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Matrix3d mt = ed.CurrentUserCoordinateSystem;
            //定义一个拖拽交互类
            JigPromptPointOptions jppo = new JigPromptPointOptions("\n请指定插入点");
            
            //用AcquirePoint函数获得拖拽得到的即时点
            PromptPointResult ppr = prompts.AcquirePoint(jppo);
            Point3d tempPt = ppr.Value;
            //拖拽取消
            if (ppr.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
            //拖拽
            //将WCS点转化为UCS点
            Point3d ucsPt2 = tempPt.TransformBy(mt.Inverse());
            //更新bRef参数
            m_bRef.Position = ucsPt2;
            return SamplerStatus.OK;

            //if (m_pt != tempPt)
            //{
            //    m_pt = tempPt;
            //    //将WCS点转化为UCS点
            //    Point3d ucsPt2 = m_pt.TransformBy(mt.Inverse());
            //    //更新bRef参数
            //    m_bRef.Position = ucsPt2;
            //    return SamplerStatus.OK;
            //}
            //else
            //    return SamplerStatus.NoChange;
        }
    }
}
