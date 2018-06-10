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
    // First draft
    public class SplineTab : TabPage
    {
        // Segment wrapper
        public class Segment
        {
            public Knot Start { get; private set; }
            public Knot End { get; private set; }

            public Vector2 P1 { get { return Start.Position; } }
            public Vector2 P2 { get { return Start.After; } }
            public Vector2 P3 { get { return End.Before; } }
            public Vector2 P4 { get { return End.Position; } }

            public Vector2 T1 { get { return Start.Out; } }
            public Vector2 T2 { get { return End.In; } }

            public Segment(Knot a_k1, Knot a_k2)
            {
                Start = a_k1;
                End = a_k2;
            }

            public bool IsStart()
            {
                return Start.Previous == null;
            }

            public bool IsEnd()
            {
                return End.Next == null;
            }

            public bool IsOnly()
            {
                return IsStart() && IsEnd();
            }
        }

        // Variables and properties
        #region Variables

        // Knots of the spline being created
        private List<Knot> m_Knots;

        // Camera for the tab
        private Vector2 m_Camera;

        // Selected knot
        private Knot m_Selected;

        // Interpolation time
        private float m_InterTime;

        // Position of interpolation time
        private Vector2 m_InterPos;

        // Get knots ( spline ) property
        public List<Knot> GetSpline { get { return m_Knots; } }

        // Get camera property
        public Vector2 Camera { get { return m_Camera; } set { m_Camera = value; Invalidate(); } }

        // Get selected knot property
        public Knot Selected { get { return m_Selected; } }

        // If this spline is in hermite or bezier mode
        public bool BezierMode { get; set; }
        
        // If this spline is interpolating itself
        public bool InterpolationActive { get; set; }

        // Radius of controls
        public float Radius { get; set; }

        // Thickness of the lines
        public float Thickness { get; set; }

        // The current time of interpolation
        public float InterpolationTime
        {
            get
            {
                // Return the current interpolation time
                return m_InterTime;
            }

            set
            {
                // Assure the given time is in range
                m_InterTime = MathF.Clamp(0f, (float)Segments(), value);

                // Interpolate
                m_InterPos = Interpolate(m_InterTime);

                // Invalide the graphics
                Invalidate();
            }
        }       

        #endregion

        // All the events used for knots
        #region Event Handlers

        public event SplinePaintEventHandler SplinePaintTools;
        public event SplinePaintEventHandler SplinePaintCurve;
        public event SplineMouseEventHandler SplineMouseClick;
        public event SplineMouseEventHandler SplineMouseMove;
        public event SplineEventHandler SplineCameraMove;

        public event SplineEventHandler SplineKnotInserted;
        public event SplineEventHandler SplineKnotRemoved;

        #endregion

        // SplineTab requires initial size
        public SplineTab(SizeF size)
        {
            // Create the knots list, set initial capacity to two
            m_Knots = new List<Knot>(2);

            // Set the size
            Size = new Size((int)size.Width, (int)size.Height);

            // Set cameras initial position
            m_Camera = new Vector2(0f, size.Height);

            // Set no knot to being selected
            m_Selected = null;

            // Set bezier mode to true by default
            BezierMode = true;

            // Set back colour to gray
            BackColor = Color.Gray;

            // Create the initial points
            Knot Start = new Knot(new Vector2(size.Width * 0.1f, size.Height * 0.1f));
            Knot End = new Knot(new Vector2(size.Width - Start.Position.x, size.Height - Start.Position.y));

            // Set the knots as neighbours
            Start.Next = End;
            End.Previous = Start;

            // Set the knots initial tangents
            Start.Out = (End.Position - Start.Position) * 0.5f;
            End.In = (End.Position - Start.Position) * 0.5f;

            // Sets the knots events
            SplineMouseClick += Start.MouseClick;
            SplineMouseMove += Start.MouseMove;
            SplinePaintCurve += Start.PaintCurve;
            SplinePaintTools += Start.PaintTools;

            SplineMouseClick += End.MouseClick;
            SplineMouseMove += End.MouseMove;
            SplinePaintCurve += End.PaintCurve;
            SplinePaintTools += End.PaintTools;

            // Add the knots to the list
            m_Knots.Add(Start);
            m_Knots.Add(End);

            // Reset interpolation variables
            InterpolationActive = false;
            InterpolationTime = 0f;

            // Set default draw variables
            Radius = 10f;
            Thickness = 5f;     
        }

        // SplineTab that loads in existing spline
        public SplineTab(SizeF size, string path)
        {
            // Load the spline 
            Spline spline = Spline.Load(path);

            // Create the knots list, set initial capacity to splines knots
            m_Knots = new List<Knot>(spline.m_Knots.Count);

            // Set the size
            Size = new Size((int)size.Width, (int)size.Height);

            // Set cameras initial position
            m_Camera = new Vector2(0f, size.Height);

            // Set no knot to being selected
            m_Selected = null;

            // Set bezier mode to true by default
            BezierMode = true;

            // Set back colour to gray
            BackColor = Color.Gray;

            // Cycle through each knot
            foreach (Spline.Knot knot in spline.m_Knots)
            {
                // Create the knot
                Knot n = new Knot(knot.Point);

                // Set the tangents
                n.In = knot.In;
                n.Out = knot.Out;

                // Add the knots events
                SplineMouseClick += n.MouseClick;
                SplineMouseMove += n.MouseMove;
                SplinePaintCurve += n.PaintCurve;
                SplinePaintTools += n.PaintTools;

                // Add the knot
                m_Knots.Add(n);
            }

            // Cycle through the knots again
            for (int i = 0; i < m_Knots.Count - 1; ++i)
            {
                // Link these knots together
                m_Knots[i].Next = m_Knots[i + 1];
                m_Knots[i + 1].Previous = m_Knots[i];
            }

            // Reset interpolation variables
            InterpolationActive = false;
            InterpolationTime = 0f;

            // Set default draw variables
            Radius = 10f;
            Thickness = 5f;
        }

        ~SplineTab()
        {
            // Cycle through each knot
            foreach (Knot k in m_Knots)
            {
                // Remove the knots callbacks
                SplineMouseClick -= k.MouseClick;
                SplineMouseMove -= k.MouseMove;
                SplinePaintCurve -= k.PaintCurve;
                SplinePaintTools -= k.PaintTools;
            }
        }

        // Manipulating the current spline
        #region Spline Functions

        // Time will be clamped
        // Time = 0 <= time <= m_Knots.Count - 1
        public Vector2 Interpolate(float time)
        {
            // Clamp the time
            time = MathF.Clamp(0f, (float)Segments(), time);

            // Get time floored
            float Fl = MathF.Floor(time);

            // Assure floored value stays within range
            Fl = Fl >= (float)Segments() ? 0f : Fl;

            // Get the segment being interpolated
            Knot St = m_Knots[(int)Fl];
            Knot En = m_Knots[(int)Fl + 1];

            // Interpolate spline and return the calculate value
            return Vector2.Hermite(St.Position, En.Position, St.Out, En.In, time - Fl);
        }

        public int Knots()
        {
            return m_Knots.Count;
        }

        public int Segments()
        {
            return m_Knots.Count - 1;
        }

        private bool AddKnot(Vector2 point)
        {
            // Cycle through every knot
            for (int i = 0; i < m_Knots.Count - 1; ++i)
            {
                // Get the segment
                Segment seg = new Segment(m_Knots[i], m_Knots[i + 1]);

                // Get a value of time using the point and segment
                float t = Time(seg, point);

                // Get the displacement between the given point and timed point
                Vector2 dis = Vector2.Hermite(seg.P1, seg.P4, seg.T1, seg.T2, t) - point;

                // Is this point on the segment and within thickness of lines?
                if (Valid(t) && dis.Magnitude() < Thickness)
                {
                    // Split this segment
                    Knot n = Split(seg, t);

                    // Set this knots next and previous
                    n.Previous = seg.Start;
                    n.Next = seg.End;

                    // Reset the old segments next and previous
                    seg.Start.Next = n;
                    seg.End.Previous = n;

                    // Align the knots
                    Align(seg.Start);
                    Align(n);
                    Align(seg.End);

                    // Add the new knot into the spline
                    m_Knots.Insert(i + 1, n);

                    // Get knots events and attach to delegates
                    SplineMouseClick += n.MouseClick;
                    SplineMouseMove += n.MouseMove;
                    SplinePaintCurve += n.PaintCurve;
                    SplinePaintTools += n.PaintTools;

                    // Return a new knot has been inserted
                    return true;
                }
            }

            // Return no knot was inserted
            return false;
        }

        private bool RemoveKnot(Vector2 point)
        {
            // Cycle through each knot
            for (int i = 1; i < m_Knots.Count - 1; ++i)
            {
                // Get the displacement between this knot and the point
                float Dis = (m_Knots[i].Position - point).Magnitude();

                // Is this point within thickness of lines?
                if (Dis < Thickness)
                {
                    // Create the segment for the previous curve and next curve
                    Segment p = new Segment(m_Knots[i - 1], m_Knots[i]);
                    Segment n = new Segment(m_Knots[i], m_Knots[i + 1]);

                    // Couple the segments together and capture the new segment
                    Segment c = Couple(ref p, ref n);

                    // Set the new segments next and previous
                    c.Start.Next = c.End;
                    c.End.Previous = c.Start;

                    // Align the knots
                    Align(c.Start);
                    Align(c.End);

                    // Remove the old knot from the spline
                    SplineMouseClick -= m_Knots[i].MouseClick;
                    SplineMouseMove -= m_Knots[i].MouseMove;
                    SplinePaintCurve -= m_Knots[i].PaintCurve;
                    SplinePaintTools -= m_Knots[i].PaintTools;

                    m_Knots.RemoveAt(i);

                    // Return an old knot has been removed
                    return true;
                }
            }

            // Return no knot was removed
            return false;
        }

        #endregion

        // Events relating to timer ticks
        #region Timer Events

        public void SplineTick(object sender, EventArgs e)
        {
            // Is interpolation active?
            if (InterpolationActive)
            {
                // Increment time
                m_InterTime += ((Timer)sender).Interval / 1000f;

                // Is interpolation time greater than max time?
                if (m_InterTime > Segments())
                {
                    // Reset the time
                    m_InterTime -= Segments();
                }

                // Set the time
                InterpolationTime = m_InterTime;
            }
        }

        #endregion

        // Events relating to keys
        #region Key Events

        public void SplineKeyDown(object sender, KeyEventArgs e)
        {
            // Bool to capture if bound key was pressed
            bool va = false;

            // Was up key pressed?
            if (e.KeyCode == Keys.W)
            {
                // Move camera forward
                m_Camera.y += 5f;

                // Set flag
                va = true;
            }
            // Was down key pressed?
            if (e.KeyCode == Keys.S)
            {
                // Move camera backwards
                m_Camera.y -= 5f;

                // Set flag
                va = true;
            }
            // Was left key pressed?
            if (e.KeyCode == Keys.A)
            {
                // Move camera sidewards
                m_Camera.x -= 5f;

                // Set flag
                va = true;
            }
            // Was right key pressed
            if (e.KeyCode == Keys.D)
            {
                // Move camera sidewards
                m_Camera.x += 5f;

                // Set flag
                va = true;
            }

            // Was valid key pressed?
            if (va)
            {
                // Create the new event arguments
                SplineEventArgs se = new SplineEventArgs(BezierMode, 1, 1, 1);

                // Call on camera move events
                SplineCameraMove(this, se);

                // Get the mouse position
                Point mp = PointToClient(Cursor.Position);

                // Is mouse captured?
                if ((mp.X >= 0) && (mp.X <= Size.Width) && (mp.Y >= 0) && (mp.Y <= Size.Height))
                {
                    // Create the mouse event args
                    MouseEventArgs me = new MouseEventArgs(MouseButtons.None, 0, mp.X, mp.Y, 0);

                    // Calculate the position of the mouse
                    Vector2 po = new Vector2(m_Camera.x + me.X, m_Camera.y - me.Y);

                    // Call mouse move event
                    SplineMouseMove(this, new SplineMouseEventArgs(me, po, BezierMode, Radius, Thickness, Thickness));
                }

                // Invalide the graphics
                Invalidate();
            }
        }

        #endregion

        // Events relating to the mouse
        #region Mouse Events

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            // Calculate the position of the click
            Vector2 po = new Vector2(m_Camera.x + e.X, m_Camera.y - e.Y);

            // Create the new mouse event
            SplineMouseEventArgs me = new SplineMouseEventArgs(e, po, BezierMode, Radius, Thickness, Thickness);

            // Set the selected knot
            me.Selected = m_Selected;

            // Was left button clicked?
            if (e.Button == MouseButtons.Left)
            {
                // Call knots move function
                SplineMouseClick(this, me);

                // Set the selected knot
                m_Selected = me.Selected;

                // Was no knot selected?
                if (m_Selected == null)
                {
                    // Was the knot successfully added?
                    if (AddKnot(po))
                    {
                        // Call the on knot added event
                        SplineKnotInserted(this, new SplineEventArgs(BezierMode, Radius, Thickness, Thickness));

                        // Invalidate the graphics
                        Invalidate();
                    }
                    else
                    {
                        // Set mouse to down
                        Capture = true;
                    }
                }
            }
            // Was the remove button clicked?
            else if (e.Button == MouseButtons.Right)
            {
                // Was the knot successfully removed?
                if (RemoveKnot(po))
                {
                    // Reset interpolation time
                    InterpolationTime = 0f;

                    // Call the on knot removed event
                    SplineKnotRemoved(this, new SplineEventArgs(BezierMode, Radius, Thickness, Thickness));

                    // Invalidate the graphics
                    Invalidate();
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            // Calculate the position of the click
            Vector2 po = new Vector2(m_Camera.x + e.X, m_Camera.y - e.Y);

            // Create the new mouse event
            SplineMouseEventArgs me = new SplineMouseEventArgs(e, po, BezierMode, Radius, Thickness, Thickness);

            // Set the selected knot
            me.Selected = m_Selected;

            // Call knots move function
            SplineMouseMove(this, me);

            // Set the selected knot
            m_Selected = me.Selected;

            // Is there a selected spline?
            if (m_Selected != null)
            {
                // Invalide the graphics
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            // Set mouse to no longer being down
            Capture = false;

            // Is there a selected knot?
            if (m_Selected != null)
            {
                // Set the selected knots selected control to none
                m_Selected.SelControl = SelectedControl.None;

                // Set selected knot to null
                m_Selected = null;

                // Refresh the tab
                Invalidate();
            }
        }

        #endregion

        // Events relating to drawing
        #region Draw Events

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            // Get the graphics from the event
            Graphics g = pevent.Graphics;

            // Get the back colour brush
            SolidBrush Br = new SolidBrush(BackColor);

            // Draw the background of the map
            g.FillRectangle(Br, DisplayRectangle);

            // Dispose the brush
            Br.Dispose();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Create the new event
            SplinePaintEventArgs se = new SplinePaintEventArgs(e, BezierMode, Radius, Thickness, Thickness);

            // Is interpolation active?
            if (InterpolationActive)
            {
                // Get a yellow coloured brush
                Brush yBrush = new SolidBrush(Color.Yellow);

                // Draw a circle to represent the current interpolation
                e.Graphics.FillEllipse(yBrush, (m_InterPos.x - m_Camera.x) - Radius, (-(m_InterPos.y - m_Camera.y)) - Radius, Radius * 2f, Radius * 2f);

                // Dispose of the brush
                yBrush.Dispose();
            }

            // Draw each knot
            SplinePaintCurve(this, se);

            // Draw the knots tools
            SplinePaintTools(this, se);
        }

        #endregion

        // Math related to the splines
        #region Spline Math

        public float Length(int accuracy)
        {
            // Get starting knot and following knot
            List<Knot>.Enumerator Cur = m_Knots.GetEnumerator();
            List<Knot>.Enumerator Next = Cur;

            // Set length to zero
            float Len = 0f;

            // Cycle through every knot
            while (Next.MoveNext())
            {
                // Add this curves length to total length
                Len += Length(new Segment(Cur.Current, Next.Current), accuracy);

                // Increment the iterator
                Cur = Next;
            }

            // Return the length
            return Len;
        }

        private Vector2 Derivative(Segment s, float t)
        {
            // Get variables needed
            float t2 = t * t;

            float lp = 1f - t;
            float sp = lp * lp;

            // Get the displacements between the control points
            Vector2 d1 = (s.P2 - s.P1) * 2f;
            Vector2 d2 = (s.P3 - s.P2) * 2f;
            Vector2 d3 = (s.P4 - s.P3) * 2f;

            // Sum all of the derivatives together
            Vector2 Sum = new Vector2();

            Sum += sp * d1;
            Sum += 2f * lp * t * d2;
            Sum += t2 * d3;

            // Return the derivative
            return Sum;
        }

        private List<Vector2> Derivatives(Segment s)
        {
            // Create the derivative array
            List<Vector2> Ders = new List<Vector2>(3);

            // Calculate each displacement and add it to the vector
            Ders[0] = (s.P2 - s.P1) * 3f;
            Ders[1] = (s.P3 - s.P2) * 3;
            Ders[2] = (s.P4 - s.P3) * 3;

            // Return the derivatives
            return Ders;
        }

        private List<Vector2> Flatten(Segment s, int segments)
        {
            // Create the vector to store points
            List<Vector2> Points = new List<Vector2>(segments + 1);

            // Get the interval between each segment
            float itv = 1f / (float)segments;

            // Set loop index to zero
            int c = 0;

            // Cycle through the amount of segments
            for (float i = 0f; c <= segments; i += itv, ++c)
            {
                // Calculate and add this point
                Points[c] = Vector2.Hermite(s.P1, s.P4, s.T1, s.T2, i);
            }

            // Return the points
            return Points;
        }

        private float Length(Segment s, int accuracy)
        {
            // Flatten this curve
            List<Vector2> Segs = Flatten(s, accuracy);

            // Set length to zero
            float Len = 0f;

            // Cycle through the segments
            for (int i = 0; i < Segs.Count - 1; ++i)
            {
                // Get length from the displacement between the points
                Len += (Segs[i + 1] - Segs[i]).Magnitude();
            }

            // Return the length
            return Len;
        }

        private List<Vector2> Roots(Segment s)
        {
            // Get the derivatives of this segment
            List<Vector2> Ders = Derivatives(s);

            // Calculate the co-efficients
            Vector2 a = Ders[0] - (2f * Ders[1]) + Ders[2];
            Vector2 b = 2f * (Ders[1] - Ders[0]);
            Vector2 c = Ders[0];

            // Get the initail boundary
            Vector2 Lower = s.P1;
            Vector2 Upper = s.P4;

            // Re-arrange the boundary so points are in the right spot
            if (Lower.x > Upper.x)
            {
                float temp = Lower.x;
                Lower.x = Upper.x;
                Upper.x = temp;
            }

            if (Lower.y > Upper.y)
            {
                float temp = Lower.y;
                Lower.y = Upper.y;
                Upper.y = temp;
            }

            // Calculate the square root of the quadratic formula for both axes
            float XSqe = (b.x * b.x) - (4f * a.x * c.x);
            float YSqe = (b.y * b.y) - (4f * a.y * c.y);

            // Assure the square root is positive
            if (!MathF.Signbit(XSqe))
            {
                // Calculate the plus quadratic formula
                float Time = (-b.x + MathF.Sqrt(XSqe)) / (2f * a.x);

                // Is this time valid?
                if (Valid(Time))
                {
                    // Calculate the point using the time
                    float Val = Bezier(s.P1.x, s.P2.x, s.P3.x, s.P4.x, Time);

                    // Is point less than the current lowest?
                    if (Val < Lower.x) { Lower.x = Val; }
                }

                // Calculate the minus quadratic formula
                Time = (-b.x - MathF.Sqrt(XSqe)) / (2f * a.x);

                // Is this time valid
                if (Valid(Time))
                {
                    // Calculate the point using the time
                    float Val = Bezier(s.P1.x, s.P2.x, s.P3.x, s.P4.x, Time);

                    // Is point greater than the current highest?
                    if (Val > Upper.x) { Upper.x = Val; }
                }
            }

            // Assure the square root is positive
            if (!MathF.Signbit(YSqe))
            {
                // Calculate the plus quadratic formula
                float Time = (-b.y + MathF.Sqrt(YSqe)) / (2f * a.y);

                // Is this time valid?
                if (Valid(Time))
                {
                    // Calculate the point using the time
                    float Val = Bezier(s.P1.y, s.P2.y, s.P3.y, s.P4.y, Time);

                    // Is point less than the current lowest?
                    if (Val < Lower.y) { Lower.y = Val; }
                }

                // Calculate the minus quadratic formula
                Time = (-b.y - MathF.Sqrt(YSqe)) / (2f * a.y);

                // Is this time valid
                if (Valid(Time))
                {
                    // Calculate the point using the time
                    float Val = Bezier(s.P1.y, s.P2.y, s.P3.y, s.P4.y, Time);

                    // Is point greater than the current highest?
                    if (Val > Upper.y) { Upper.y = Val; }
                }
            }

            // Create the list
            List<Vector2> Roo = new List<Vector2>(2);

            // Add the roots to the list
            Roo.Add(Lower);
            Roo.Add(Upper);

            // Return the roots
            return Roo;
        }

        private Knot Split(Segment s, float t)
        {
            // Get the bezier points from the segment as an array
            List<Vector2> Points = new List<Vector2>(4);
            
            Points.Add(s.P1);
            Points.Add(s.P2);
            Points.Add(s.P3);
            Points.Add(s.P4);

            // Create buffers to capture the split curve
            List<Vector2> Left = new List<Vector2>();
            List<Vector2> Right = new List<Vector2>();

            Left.AddRange(Enumerable.Repeat(new Vector2(0f, 0f), 4));
            Right.AddRange(Enumerable.Repeat(new Vector2(0f, 0f), 4));

            // Get the split control points
            Split(Points, ref Left, ref Right, t);

            // Create the new knot
            Knot Between = new Knot(Left[3]);

            // Set the values of the between knot
            Between.Before = Left[2];
            Between.After = Right[1];

            // Alter the values of the original curve
            s.Start.After = Left[1];
            s.End.Before = Right[2];

            // Return the between knot
            return Between;
        }

        private void Split(List<Vector2> points, ref List<Vector2> left, ref List<Vector2> right, float t)
        {
            // Is this the last point?
            if (points.Count == 1)
            {
                // Set final control points for the curves
                left[0] = points[0];
                right[0] = points[0];

                // Reverse the first curve
                left.Reverse();

                // End this function
                return;
            }
            else
            {
                // Get size of points subtracted by one
                int Rec = points.Count - 1;

                // Create a new buffer
                List<Vector2> Buf = new List<Vector2>(Rec);

                // Set the lerped into split curves
                left[Rec] = points[0];
                right[Rec] = points[points.Count - 1];

                // Cycle through current points
                for (int i = 0; i < Rec; ++i)
                {
                    // Lerp and capture the points value
                    Buf.Add(Vector2.Lerp(points[i], points[i + 1], t));
                }

                // Continue splitting the curve
                Split(Buf, ref left, ref right, t);
            }
        }

        private Segment Couple(ref Segment s1, ref Segment s2)
        {
            // https://math.stackexchange.com/questions/870672/merge-two-or-more-cubic-b%C3%A9zier-curves-for-optimization

            // Are these curves not together?
            if (s1.End != s2.Start)
            {
                // Throw an execption stating coupling was a failure
                throw new Exception("Can not merge segments that are not connected");
            }

            // Calculate the time value to couple the segments
            float Spl = (s2.P2 - s2.P1).Magnitude() / (s1.P4 - s1.P3).Magnitude();

            // Create the new segment
            Segment seg = new Segment(s1.Start, s2.End);

            // Calculate the new before and after points for the new segment
            seg.Start.After = (1f + Spl) * seg.P2 - (Spl * seg.P1);
            seg.End.Before = ((1f + Spl) * seg.P3 - seg.P4) / Spl;

            // Return the new segment
            return seg;
        }

        private float Time(Segment s, Vector2 point)
        {
            // Calculate the determinant for each displacement

            //float D32_1 = (point.x * ((s.P4.y - s.P3.y)));
            //float D32_2 = (point.y * ((s.P4.x - s.P3.x)));
            //float D32_3 = ((s.P4.x * s.P3.y) - (s.P3.x * s.P4.y));
            //float D32 = 3f * (D32_1 - D32_2 + D32_3);

            float D31_1 = (point.x * ((s.P4.y - s.P2.y)));
            float D31_2 = (point.y * ((s.P4.x - s.P2.x)));
            float D31_3 = ((s.P4.x * s.P2.y) - (s.P2.x * s.P4.y));
            float D31 = 3f * (D31_1 - D31_2 + D31_3);

            float D30_1 = (point.x * ((s.P4.y - s.P1.y)));
            float D30_2 = (point.y * ((s.P4.x - s.P1.x)));
            float D30_3 = ((s.P4.x * s.P1.y) - (s.P1.x * s.P4.y));
            float D30 = D30_1 - D30_2 + D30_3;

            float D21_1 = (point.x * ((s.P3.y - s.P2.y)));
            float D21_2 = (point.y * ((s.P3.x - s.P2.x)));
            float D21_3 = ((s.P3.x * s.P2.y) - (s.P2.x * s.P3.y));
            float D21 = 9f * (D21_1 - D21_2 + D21_3);

            float D20_1 = (point.x * ((s.P3.y - s.P1.y)));
            float D20_2 = (point.y * ((s.P3.x - s.P1.x)));
            float D20_3 = ((s.P3.x * s.P1.y) - (s.P1.x * s.P3.y));
            float D20 = 3f * (D20_1 - D20_2 + D20_3);

            float D10_1 = (point.x * ((s.P2.y - s.P1.y)));
            float D10_2 = (point.y * ((s.P2.x - s.P1.x)));
            float D10_3 = ((s.P2.x * s.P1.y) - (s.P1.x * s.P2.y));
            float D10 = 3f * (D10_1 - D10_2 + D10_3);

            // Calculate the determine for the constants

            float DC1_1 = (s.P1.x * (s.P2.y - s.P4.y));
            float DC1_2 = (s.P1.y * (s.P2.x - s.P4.x));
            float DC1_3 = ((s.P2.x * s.P4.y) - (s.P4.x * s.P2.y));

            float DC2_1 = (s.P1.x * (s.P3.y - s.P4.y));
            float DC2_2 = (s.P1.y * (s.P3.x - s.P4.x));
            float DC2_3 = ((s.P3.x * s.P4.y) - (s.P4.x * s.P3.y));

            float DC3_1 = (s.P2.x * (s.P3.y - s.P4.y));
            float DC3_2 = (s.P2.y * (s.P3.x - s.P4.x));
            float DC3_3 = ((s.P3.x * s.P4.y) - (s.P4.x * s.P3.y));

            float DC1 = DC1_1 - DC1_2 + DC1_3;
            float DC2 = DC2_1 - DC2_2 + DC2_3;
            float DC3 = 3f * (DC3_1 - DC3_2 + DC3_3);

            // Scale the constants determinants

            float SC1 = DC1 / DC3;
            float SC2 = -(DC2 / DC3);

            // Calculate values for the inversion

            float LA = (SC1 * D31) + (SC2 * (D30 + D21)) + D20;
            float LB = (SC1 * D30) + (SC2 * D20) + D10;

            // Calculate and return the time

            return LB / (LB - LA);
        }

        private float Bezier(float p1, float p2, float p3, float p4, float t)
        {
            // Get the time values
            float ts = t * t;
            float tc = ts * t;

            // Get the polynomial values
            float lp = 1f - t;
            float sp = lp * lp;
            float cp = sp * lp;

            // Calculate and return the value along the curve
            return (p1 * cp) + (3f * p2 * sp * t) + (3f * p3 * lp * ts) + (p4 * tc);
        }

        private float Hermite(float p1, float p2, float t1, float t2, float t)
        {
            // Get the time values
            float ts = t * t;
            float tc = ts * t;

            // Get the basics values
            float h00 = 2f * tc - 3f * ts + 1;
            float h01 = -2f * tc + 3f * ts;
            float h10 = tc - 2f * ts + t;
            float h11 = tc - ts;

            // Calculate and return the value along the curve
            return h00 * p1 + h10 * t1 + h01 * p2 + h11 * t2;
        }

        private void Align(Knot n)
        {
            // Is hermite mode active?
            if (!BezierMode)
            {
                // Is this knot between others?
                if (n.Previous != null && n.Next != null)
                {
                    // Get displacement between new knots tangents
                    Vector2 tdis = n.In - n.Out;

                    // Half the displacement
                    tdis *= 0.5f;

                    // Is in tangent greater than out tangent
                    if (n.In.Magnitude() > n.Out.Magnitude())
                    {
                        // Subtract from in
                        n.In = n.In - tdis;
                    }
                    else
                    {
                        // Add to in
                        n.In = n.In + tdis;
                    }

                    // Set out to equal in
                    n.Out = n.In;
                }
            }
        }

        private bool Valid(float value)
        {
            // Return if this value is valid
            return (0f <= value) && (value <= 1f);
        }

        #endregion
    }
}

