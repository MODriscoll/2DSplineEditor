using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace SplineComponentsTester
{
    public class SplineEventArgs : EventArgs
    {
        public bool Bezier { get; private set; }
        public bool Hermite { get; private set; }

        public float Range { get; private set; }
        public float Radius { get; private set; }
        public float Thickness { get; private set; }

        public SplineEventArgs(bool bezier, float radius, float thickness, float range)
        {
            Bezier = bezier;
            Hermite = !bezier;

            Radius = radius;
            Thickness = thickness;

            Range = range;
        }
    }

    public class SplineMouseEventArgs : SplineEventArgs
    {
        public Vector2 Point { get; private set; }

        public MouseEventArgs MouseEvent { get; private set; }

        public Knot Selected { get; set; }

       public SplineMouseEventArgs(MouseEventArgs mevent, Vector2 point, bool bezier, float radius, float thickness, float range)
            : base(bezier, radius, thickness, range)
        {
            MouseEvent = mevent;

            Point = point;

            Selected = null;
        }
    }

    public class SplinePaintEventArgs : SplineEventArgs
    {
        public PaintEventArgs PaintEvent { get; private set; }

        public SplinePaintEventArgs(PaintEventArgs pevent, bool bezier, float radius, float thickness, float range)
            : base(bezier, radius, thickness, range)
        {
            PaintEvent = pevent;
        }

        ~SplinePaintEventArgs()
        {
            PaintEvent.Dispose();
        }
    }

    public delegate void SplineEventHandler(object sender, SplineEventArgs e);
    public delegate void SplineMouseEventHandler(object sender, SplineMouseEventArgs e);
    public delegate void SplinePaintEventHandler(object sender, SplinePaintEventArgs e);
}
