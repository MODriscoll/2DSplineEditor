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
    public class SplineGrid : Control
    {
        // Position of the gird ( camera )
        private PointF camera;

        // Size of a grid cell
        private PointF cellsize;
        [ 
        Category("Cells"),
        Description("Size of the cells"),
        DefaultValue(true)
        ]

        public PointF CellSize
        {
            get
            {
                return cellsize;
            }

            set
            {
                cellsize = value;
                Invalidate();
            }
        }

        public SplineGrid()
        {
            DoubleBuffered = true;

            camera = new PointF(0f, 0f);
            cellsize = new PointF(100f, 20f);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);

            // Get graphics from event
            Graphics g = pevent.Graphics;

            // Get a grey coloured brush
            Brush gBrush = new SolidBrush(Color.FromArgb(81, 81, 81));

            // Draw the far point of the background
            g.FillRectangle(gBrush, DisplayRectangle);

            // Dispose the grey brush
            gBrush.Dispose();

            // Get the size of the window ( control )
            SizeF CSize = this.Size;

            // Calculate the amount of grid cells that can fit in the window
            Point Grid = new Point((int)(CSize.Width / CellSize.X) + 1, (int)(CSize.Height / CellSize.Y) + 1);

            // Get a yellow coloured pen for the X and Y axis
            // Set the thickness relative to the size of the axis
            Pen yPenX = new Pen(Color.FromArgb(239, 242, 96), CellSize.X * 0.05f);
            Pen yPenY = new Pen(Color.FromArgb(239, 242, 96), CellSize.Y * 0.05f);

            // Cycle through the rows
            for (int i = 0; i < Grid.X; ++i)
            {
                // Cycle through the columns
                for (int j = 0; j < Grid.Y; ++j)
                {
                    // Calculate the point to draw from ( top-left )
                    PointF TL = new PointF(camera.X + (CellSize.X * i), camera.Y + (CellSize.Y * j));

                    // Calculate the point to draw to ( bottom-right)
                    PointF BR = new PointF(camera.X + (CellSize.X * (i + 1)), camera.Y + (CellSize.Y * (j + 1)));

                    // Draw this grid cell
                    g.DrawLine(yPenX, TL.X, TL.Y, BR.X, TL.Y);
                    g.DrawLine(yPenY, BR.X, TL.Y, BR.X, BR.Y);
                    g.DrawLine(yPenX, BR.X, BR.Y, TL.X, BR.Y);
                    g.DrawLine(yPenY, TL.X, BR.Y, TL.X, TL.Y);
                }
            }

            // Dispose of the pens
            yPenX.Dispose();
            yPenY.Dispose();
        }
    }
}
