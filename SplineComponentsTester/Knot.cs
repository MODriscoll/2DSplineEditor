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
    public enum SelectedControl
    {
        None = 0,
        In = 1,
        Out = 2,
        Point = 4
    }

    public class Knot
    {
        // Variables and properties
        #region Variables

        // Point of this knot
        private Vector2 m_Point;

        // The in and out tangent of this knot
        private Vector2 m_In, m_Out;

        // The front and after control points of this knot
        private Vector2 m_Front, m_After;

        // The knot before and after this knot
        private Knot m_Prev, m_Next;

        // If this knot is selected
        private bool m_Selected;
       
        // Point property
        public Vector2 Position
        {
            get
            {
                // Return the point of this knot
                return m_Point;
            }
            
            set
            {
                // Set the point of this knot
                m_Point = value;

                // Call on change position event

            }
        }

        // In tangent property
        public Vector2 In
        {
            get
            {
                // Return the in tangent of this knot
                return m_In;
            }

            set
            {
                // Set the tangent of this knot
                m_In = value;

                // Convert the velocity to a position and set it into the after control point
                m_Front = m_Point - m_In / 3f;

                // Call on change tangent event

            }
        }

        // Before control point property
        public Vector2 Before
        {
            get
            {
                // Return the front control point of this knot
                return m_Front;
            }

            set
            {
                // Set the front control point
                m_Front = value;

                // Convert the position to a velocity and set it into the in tangent
                m_In = 3f * (m_Point - m_Front);

                // Call on change tangent event

            }
        }

        // Out tangent property
        public Vector2 Out
        {
            get
            {
                // Return the out tangent of this knot
                return m_Out;
            }

            set
            {
                // Set the tangent of this knot
                m_Out = value;

                // Convert the velocity to a position and set it into the after control point
                m_After = m_Point + m_Out / 3f;

                // Call on change tangent event

            }
        }

        // After control point property
        public Vector2 After
        {
            get
            {
                // Return the after control point of this knot
                return m_After;
            }

            set
            {
                // Set the after control point
                m_After = value;

                // Convert the position to a velocity and set it into the out tangent
                m_Out = 3f * (m_After - m_Point);

                // Call on change tangent event

            }
        }

        // Previous knot property
        public Knot Previous
        {
            get
            {
                // Return the knot before this one
                return m_Prev;
            }

            set
            {
                // Set the knot before this one
                m_Prev = value;

                // Call on change curve event

            }
        }

        // Next knot property
        public Knot Next
        {
            get
            {
                // Return the knot after this one
                return m_Next;
            }

            set
            {
                // Set the knot after this one
                m_Next = value;

                // Call on change curve event

            }
        }

        // Is Selected property
        public bool Selected
        {
            get
            {
                // Return if this knot is selected
                return m_Selected;
            }

            set
            {
                // Set if this knot is selected
                m_Selected = value;
            }
        }

        // What part of this knot has been selected
        public SelectedControl SelControl { get; set; }

        #endregion

        public Knot(Vector2 a_Point)
        {
            // Set the position of this knot
            m_Point = a_Point;

            // Default variables
            m_Prev = null;
            m_Next = null;

            m_Selected = false;

            SelControl = SelectedControl.None;
        }

        // Functions related to the mouse
        #region Mouse Functions

        public void MouseClick(object sender, SplineMouseEventArgs e)
        {
            // Get sender as a spline tab
            SplineTab tb = sender as SplineTab;

            // Assure sender was a spline tab
            if (tb == null) { throw new Exception("Sender for knots must be spline tab"); }

            // Check if point was clicked first
            if ((e.Point - m_Point).Magnitude() <= e.Radius)
            {
                // Set this knot to being selected
                e.Selected = this;

                // Set the selected control to the point
                SelControl = SelectedControl.Point;

                // End this function
                return;
            }

            // Create booleans to capture result
            bool hasFront = false;
            bool hasAfter = false;

            // Is bezier mode active?
            if (e.Bezier)
            {
                // Create bound rectangle
                Vector2 BU = new Vector2(e.Radius, e.Radius);

                // Calculate fronts bounding box
                Vector2 FLL = m_Front - BU;
                Vector2 FUR = m_Front + BU;

                // Calculate afters bounding box
                Vector2 ALL = m_After - BU;
                Vector2 AUR = m_After + BU;

                // Set if mouse is contained in either
                hasFront = (e.Point.x >= FLL.x && e.Point.x <= FUR.x) && (e.Point.y >= FLL.y && e.Point.y <= FUR.y);
                hasAfter = (e.Point.x >= ALL.x && e.Point.x <= AUR.x) && (e.Point.y >= ALL.y && e.Point.y <= AUR.y);
            }
            else
            {
                // Set if mouse is contained in either
                hasFront = (e.Point - m_Front).Magnitude() <= e.Radius;
                hasAfter = (e.Point - m_After).Magnitude() <= e.Radius;
            }

            // Was mouse contained by the in tangent?
            if (hasFront)
            {
                // Set this knot to being selected
                e.Selected = this;

                // Set the selected control to the in tangent
                SelControl = SelectedControl.In;

                // End this function
                return;
            }

            // Was the mouse contained by the out tangent?
            if (hasAfter)
            {
                // Set this knot to being selected
                e.Selected = this;

                // Set the selected control to the out tangent
                SelControl = SelectedControl.Out;

                // End this function
                return;
            }

            // Set no control to being selected
            SelControl = SelectedControl.None;
        }

        public void MouseMove(object sender, SplineMouseEventArgs e)
        {
            // Get sender as a spline tab
            SplineTab tb = sender as SplineTab;

            // Assure sender was a spline tab
            if (tb == null) { throw new Exception("Sender for knots must be spline tab"); }

            // Is bezier being constructed?
            if (e.Bezier)
            {
                // Move bezier tools
                MouseMove_Bezier(e.Point, e);
            }
            else
            {
                // Move hermite tools
                MouseMove_Hermite(e.Point, e);
            }
        }

        private void MouseMove_Bezier(Vector2 p, SplineMouseEventArgs e)
        {
            // Switch based on knots selection
            switch (SelControl)
            {
                case SelectedControl.Point:
                    {
                        // Set the points position
                        Position = p;

                        // Re-calculate tangents
                        m_In = 3f * (m_Point - m_Front);
                        m_Out = 3f * (m_After - m_Point);

                        // Break from this case
                        break;
                    }
                case SelectedControl.In:
                    {
                        // Set the points in tangent
                        Before = p;

                        // Break from this case
                        break;
                    }
                case SelectedControl.Out:
                    {
                        // Set the points out tangent
                        After = p;

                        // Break from this case
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        
        private void MouseMove_Hermite(Vector2 p, SplineMouseEventArgs e)
        {
            // Switch based on knots selection
            switch (SelControl)
            {
                case SelectedControl.Point:
                    {
                        // Set the points position
                        Position = p;

                        // Re-calculate control points
                        m_Front = m_Point - m_In / 3f;
                        m_After = m_Point + m_Out / 3f;

                        // Break from this case
                        break;
                    }
                case SelectedControl.In:
                    {
                        // Keep tangents equal
                        Before = p;
                        After = m_Point + (m_Point - m_Front);

                        // Break from this case
                        break;
                    }
                case SelectedControl.Out:
                    {
                        // Keep tangents equal
                        After = p;
                        Before = m_Point - (m_After - m_Point);

                        // Break from this case
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }

        #endregion

        // Functions related to drawing
        #region Paint Functions

        public void PaintCurve(object sender, SplinePaintEventArgs e)
        {
            // Get sender as a spline tab
            SplineTab tb = sender as SplineTab;

            // Assure sender was a spline tab
            if (tb == null) { throw new Exception("Sender for knots must be spline tab"); }

            // Get the graphics from the event
            Graphics g = e.PaintEvent.Graphics;

            // Create a red brush
            Brush rBrush = new SolidBrush(SelControl == SelectedControl.None ? Color.Red : e.Bezier ? Color.Blue : Color.Green);

            // Get the camera as a vector
            Vector2 cam = tb.Camera;

            // Draw a circle to represent this knot
            g.FillEllipse(rBrush, (m_Point.x - cam.x) - e.Radius, (-(m_Point.y - cam.y)) - e.Radius, e.Radius * 2f, e.Radius * 2f);

            // Create a white pen
            Pen wPen = new Pen(Color.White, e.Thickness);

            // Is this knot not the last knot?
            if (m_Next != null)
            {
                // Draw the curve
                g.DrawBezier(wPen, GetPos(m_Point, cam), GetPos(m_After, cam), GetPos(Next.Before, cam), GetPos(Next.Position, cam));
            }

            // Dispose of pens and brushes
            rBrush.Dispose();
            wPen.Dispose();
        }

        public void PaintTools(object sender, SplinePaintEventArgs e)
        {
            // Get sender as a spline tab
            SplineTab tb = sender as SplineTab;

            // Assure sender was a spline tab
            if (tb == null) { throw new Exception("Sender for knots must be spline tab"); }

            // Is bezier being drawn?
            if (e.Bezier)
            {
                // Draw bezier tools
                PaintTools_Bezier(tb.Camera, e);
            }
            else
            {
                // Draw hermite tools
                PaintTools_Hermite(tb.Camera, e);
            }
        }

        private void PaintTools_Bezier(Vector2 c, SplinePaintEventArgs e)
        {
            // Get the graphics from the event
            Graphics g = e.PaintEvent.Graphics;

            // Get a blue brush and pen
            Brush bBrush = new SolidBrush(Color.Blue);
            Pen bPen = new Pen(Color.Blue, e.Radius / 2f);

            // Create the common size
            SizeF BS = new SizeF(e.Radius * 2f, e.Radius * 2f);

            // Does this knot have a previous?
            if (m_Prev != null)
            {
                // Get the front point as a rectangle
                RectangleF Fr = new RectangleF(GetPos(m_Front, c), BS);

                Fr.X -= e.Radius;
                Fr.Y -= e.Radius;

                // Draw the front control point
                g.FillRectangle(bBrush, Fr);

                // Draw a line from front point to the knots point
                g.DrawLine(bPen, GetPos(m_Front, c), GetPos(m_Point, c));

                // Draw a line from the nexts after point to this knots front point
                g.DrawLine(bPen, GetPos(Previous.After, c), GetPos(m_Front, c));
            }

            // Does this knot have a next?
            if (m_Next != null)
            {
                // Get the after point as a rectangle
                RectangleF Af = new RectangleF(GetPos(m_After, c), BS);

                Af.X -= e.Radius;
                Af.Y -= e.Radius;

                // Draw the after control point
                g.FillRectangle(bBrush, Af);

                // Draw a line from the knots point to the after point
                g.DrawLine(bPen, GetPos(m_Point, c), GetPos(m_After, c));
            }

            // Dispose of pens and brushes
            bPen.Dispose();
            bBrush.Dispose();
        }

        private void PaintTools_Hermite(Vector2 c, SplinePaintEventArgs e)
        {
            // Get the graphics from the event
            Graphics g = e.PaintEvent.Graphics;

            // Get a green brush and pen
            Brush gBrush = new SolidBrush(Color.Green);
            Pen gPen = new Pen(Color.Green, e.Radius / 2f);

            // Does this knot have a previous?
            if (m_Prev != null)
            {
                // Draw a circle to represent the in tangent
                g.FillEllipse(gBrush, (m_Front.x - c.x) - e.Radius, (-(m_Front.y - c.y)) - e.Radius, e.Radius * 2f, e.Radius * 2f);

                // Draw a line from the in tangent to the knots point
                g.DrawLine(gPen, GetPos(m_Front, c), GetPos(m_Point, c));
            }

            // Does this knot have a next?
            if (m_Next != null)
            {
                // Draw a circle to represent the out tangent
                g.FillEllipse(gBrush, (m_After.x - c.x) - e.Radius, (-(m_After.y - c.y)) - e.Radius, e.Radius * 2f, e.Radius * 2f);

                // Draw a line from the out tangent to the knots point
                g.DrawLine(gPen, GetPos(m_Point, c), GetPos(m_After, c));
            }

            // Dispose of pens and brushes
            gPen.Dispose();
            gBrush.Dispose();
        }

        // Helper function for getting relative positions from cameras position
        public static Vector2 GetPos(Vector2 point, Vector2 cam)
        {
            return new Vector2(point.x - cam.x, -(point.y - cam.y));
        }

        #endregion
    }
}
