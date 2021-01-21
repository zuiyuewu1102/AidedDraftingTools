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
        private Point2d[] m_pts = new Point2d[4];
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
        private Point2d[] m_pts = new Point2d[8];
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
        private Point2d[] m_pts = new Point2d[6];
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
}
