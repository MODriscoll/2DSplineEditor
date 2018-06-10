using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SplineComponentsTester;
using System.IO;
using System.Xml;

namespace SplineEditorTester
{
    public partial class frmEditor : Form
    {
        // Keeps track of total amount of tabs created
        private int m_TabsCreated = 0;

        public frmEditor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            nudCamX.Minimum = decimal.MinValue;
            nudCamX.Maximum = decimal.MaxValue;
            nudCamY.Minimum = decimal.MinValue;
            nudCamY.Maximum = decimal.MaxValue;

            // Create the initial tab
            SplineTab tab = new SplineTab(splineTabs.Size);

            // Set this tab to be the const tab
            tab.Name = "spline0";
            tab.Text = "Spline1";

            // Add this tab to the tab control
            splineTabs.TabPages.Add(tab);

            // Set the selected tab
            splineTabs.SelectTab(0);

            // Set the initial events for this tab
            KeyDown += tab.SplineKeyDown;
            tmrInterpolate.Tick += tab.SplineTick;

            tab.SplineCameraMove += CameraMove;

            tab.SplineKnotInserted += Knot_Insert_Remove;
            tab.SplineKnotRemoved += Knot_Insert_Remove;

            tab.SplineMouseMove += TabMouseMove;
            tab.MouseLeave += TabMouseExit;

            // Set starting mode based on splines default mode
            btnBezierMode.Enabled = !tab.BezierMode;
            btnHermiteMode.Enabled = tab.BezierMode;

            nudCamX.Value = (decimal)tab.Camera.x;
            nudCamY.Value = (decimal)tab.Camera.y;

            nudTimer.Maximum = tab.Segments();

            // Disable close button
            btnClose.Enabled = false;

            // Set one tab to be created
            m_TabsCreated = 1;

            // Start the timer
            tmrInterpolate.Start();
        }

        private void tmrInterpolate_Tick(object sender, EventArgs e)
        {
            // Get selected tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Assure tab exists
            if (tab != null)
            {
                // Set textboxs timer to spline interpolation time
                nudTimer.Value = (decimal)tab.InterpolationTime;
            }
        }

        private void splineTabs_Deselected(object sender, TabControlEventArgs e)
        {
            // Get the selected tab
            SplineTab tab = e.TabPage as SplineTab;

            // Assure the tab isn't null
            if (tab != null)
            {
                // Remove this tabs events
                KeyDown -= tab.SplineKeyDown;
                tmrInterpolate.Tick -= tab.SplineTick;

                tab.SplineCameraMove -= CameraMove;

                tab.SplineKnotInserted -= Knot_Insert_Remove;
                tab.SplineKnotRemoved -= Knot_Insert_Remove;

                tab.SplineMouseMove -= TabMouseMove;
                tab.MouseLeave -= TabMouseExit;
            }
        }

        private void splineTabs_Selected(object sender, TabControlEventArgs e)
        {
            // Get the selected tab
            SplineTab tab = e.TabPage as SplineTab;

            // Is there a tab?
            if (tab != null)
            {
                // Add tabs events
                KeyDown += tab.SplineKeyDown;
                tmrInterpolate.Tick += tab.SplineTick;

                tab.SplineCameraMove += CameraMove;

                tab.SplineKnotInserted += Knot_Insert_Remove;
                tab.SplineKnotRemoved += Knot_Insert_Remove;

                tab.SplineMouseMove += TabMouseMove;
                tab.MouseLeave += TabMouseExit;

                // Set editing buttons based tabs settings
                btnBezierMode.Enabled = !tab.BezierMode;
                btnHermiteMode.Enabled = tab.BezierMode;

                chkbInterpolate.Checked = tab.InterpolationActive;

                nudTimer.Maximum = tab.Segments();
                nudTimer.Value = (decimal)tab.InterpolationTime;

                nudCamX.Value = (decimal)tab.Camera.x;
                nudCamY.Value = (decimal)tab.Camera.y;
            }
        }

        private void btnBezierMode_Click(object sender, EventArgs e)
        {
            // Get current tab as a spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Assure this tab isn't null
            if (tab != null && btnBezierMode.Enabled)
            {
                // Set this tab to bezier mode
                tab.BezierMode = true;

                // Invalide the tabs graphics
                tab.Invalidate();

                // Switch enabled buttons
                btnBezierMode.Enabled = false;
                btnHermiteMode.Enabled = true;
            }
        }

        private void btnHermiteMode_Click(object sender, EventArgs e)
        {
            // Get current tab as a spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Assure this tab isn't null
            if (tab != null)
            {
                // Set this tab to hermite mode
                tab.BezierMode = false;

                // Invalide the tabs graphics
                tab.Invalidate();

                // Switch enabled buttons
                btnHermiteMode.Enabled = false;
                btnBezierMode.Enabled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            // Create the new tab
            SplineTab tab = new SplineTab(splineTabs.Size);

            // Set tabs name and text
            tab.Name = "spline" + m_TabsCreated.ToString();
            tab.Text = "Spline" + (++m_TabsCreated).ToString();

            // Add this tab to the tab pages
            splineTabs.TabPages.Add(tab);

            // Set this tab to the selected tab
            splineTabs.SelectTab(splineTabs.TabPages.Count - 1);  

            // Enable close button
            btnClose.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Get the current tab as a spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Create the dialog box
            SaveFileDialog sfd = new SaveFileDialog();

            // Set filter the dialog box
            sfd.Filter = "XML files (*.xml) | *.xml;";
            sfd.RestoreDirectory = true;
            sfd.FileName = tab.Text;

            // Show the dialog, is the result good?
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Create the new spline
                Spline spline = new Spline();

                // Get the knots from the current tab
                List<Knot> knots = tab.GetSpline;

                // Cycle through every knot
                foreach (Knot k in knots)
                {
                    // Add the knot to the spline
                    spline.m_Knots.Add(new Spline.Knot(k.Position, k.In, k.Out));
                }

                // Save the spline
                Spline.Save(spline, sfd.FileName);

                // Set the tabs name to files name
                tab.Text = System.IO.Path.GetFileNameWithoutExtension(sfd.FileName);
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            // Create the dialog box
            OpenFileDialog ofd = new OpenFileDialog();

            // Set filter the dialog box
            ofd.Filter = "XML files (*.xml) | *.xml;";
            ofd.RestoreDirectory = true;

            // Show the dialog, is the result good?
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Create the new spline tab
                SplineTab tab = new SplineTab(splineTabs.Size, ofd.FileName);

                // Set tabs name and text
                tab.Name = "spline" + m_TabsCreated++.ToString();
                tab.Text = Path.GetFileNameWithoutExtension(ofd.FileName);

                // Add this tab to the tab pages
                splineTabs.TabPages.Add(tab);

                // Set this tab to the selected tab
                splineTabs.SelectTab(splineTabs.TabPages.Count - 1);

                // Enable close button
                btnClose.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // Assure more than one tab exists
            if (splineTabs.TabPages.Count <= 1) { return; }

            // Get the selected tab index
            int Index = splineTabs.SelectedIndex;

            // Select the tab before the current one
            splineTabs.SelectTab(Index == 0 ? 1 : Index - 1);

            // Remove this splines from the tab page
            splineTabs.TabPages.RemoveAt(Index);

            // Is there only one tab page remaining?
            if (splineTabs.TabPages.Count == 1)
            {
                // Disable the close button
                btnClose.Enabled = false;
            }
        }

        private void chkbInterpolate_CheckedChanged(object sender, EventArgs e)
        {
            // Get current tab as a spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Assure the tab is a spline tab
            if (tab != null)
            {
                // Set interpolation based on check status
                tab.InterpolationActive = chkbInterpolate.Checked;
            }
        }

        private void Knot_Insert_Remove(object sender, SplineEventArgs e)
        {
            // Get the sender as a spline tab
            SplineTab tab = sender as SplineTab;

            // Assure tab isn't null
            if (tab != null)
            {
                // Set maximum value of timer to splines segments
                nudTimer.Maximum = tab.Segments();

                // Set timers value
                nudTimer.Value = (decimal)tab.InterpolationTime;
            }
        }

        private void CameraMove(object sender, SplineEventArgs e)
        {
            // Get the sender as a spline tab
            SplineTab tab = sender as SplineTab;

            // Assure tab isn't null
            if (tab != null)
            {
                // Set cameras position into textboxes
                nudCamX.Value = (decimal)tab.Camera.x;
                nudCamY.Value = (decimal)tab.Camera.y;
            }
        }

        private void nudTimer_ValueChanged(object sender, EventArgs e)
        {
            // Get current tab as spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Set current tabs interpolation time
            tab.InterpolationTime = (float)nudTimer.Value;
        }

        private void nudTimer_Enter(object sender, EventArgs e)
        {
            // Stop the timer
            tmrInterpolate.Stop();
        }

        private void nudTimer_Leave(object sender, EventArgs e)
        {
            // Start the timer
            tmrInterpolate.Start();
        }

        private void nudCamX_ValueChanged(object sender, EventArgs e)
        {
            // Get current tab as spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Get copy of camera position
            Vector2 pos = tab.Camera;

            // Set camera's new X point
            pos.x = (float)nudCamX.Value;

            // Set camera's position
            tab.Camera = pos;
        }

        private void nudCamY_ValueChanged(object sender, EventArgs e)
        {
            // Get current tab as spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Get copy of camera position
            Vector2 pos = tab.Camera;

            // Set camera's new Y point
            pos.y = (float)nudCamY.Value;

            // Set camera's position
            tab.Camera = pos;
        }

        private void TabMouseMove(object sender, SplineMouseEventArgs e)
        {
            // Set mouse's position into label
            lblMousePos.Text = "Mouse: " + e.Point.ToString();
        }

        private void TabMouseExit(object sender, EventArgs e)
        {
            // Clear label
            lblMousePos.Text = string.Empty;
        }


        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Focus title
            lblTitle.Focus();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Is key a number?
            if (!Char.IsDigit(e.KeyChar) && !Char.IsControl(e.KeyChar))
            {
                // Focus title
                lblTitle.Focus();
            }
        }

        private void splineTabs_DragEnter(object sender, DragEventArgs e)
        {
            // Set effect to allow for all drag effects
            e.Effect = DragDropEffects.Copy;
        }

        private void splineTabs_DragDrop(object sender, DragEventArgs e)
        {
            // Capture all the names of the drag
            string[] names = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            // Cycle through each name
            foreach (string n in names)
            {
                // Check if this file exists
                // Make sure file type is xml
                if (File.Exists(n) && Path.GetExtension(n) == ".xml") 
                {
                    // Create a new xml document
                    XmlDocument doc = new XmlDocument();

                    // Load the xml
                    doc.Load(n);

                    // Is the xml a spline xml?
                    if (doc.SelectSingleNode("Spline") != null)
                    {
                        // Create the new spline tab
                        SplineTab tab = new SplineTab(splineTabs.Size, n);

                        // Set tabs name and text
                        tab.Name = "spline" + m_TabsCreated++.ToString();
                        tab.Text = Path.GetFileNameWithoutExtension(n);

                        // Add this tab to the tab pages
                        splineTabs.TabPages.Add(tab);

                        // Set this tab to the selected tab
                        splineTabs.SelectTab(splineTabs.TabPages.Count - 1);

                        // Enable close button
                        btnClose.Enabled = true;
                    }
                }
            }
        }

        private void mnuNew_Click(object sender, EventArgs e)
        {
            // Add new tab
            btnNew.PerformClick();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            // Open spline
            btnOpen.PerformClick();
        }

        private void mnuSave_Click(object sender, EventArgs e)
        {
            // Save current tab
            btnSave.PerformClick();
        }

        private void tsiHelp_Click(object sender, EventArgs e)
        {
            // Show the help document
            Help.ShowHelp(this, "help.html");
        }

        private void tsiOptions_Click(object sender, EventArgs e)
        {
            // Get current tab as spline tab
            SplineTab tab = splineTabs.SelectedTab as SplineTab;

            // Create the new options form
            OptionsPrompt pr = new OptionsPrompt(tab.Radius, tab.Thickness);

            // Show the prompt
            pr.ShowDialog();

            // Were the option changes confirmed?
            if (pr.DialogResult == DialogResult.OK)
            {
                // Set the tabs variables
                tab.Radius = pr.Radius;
                tab.Thickness = pr.Thickness;

                // Invalide the tab
                tab.Invalidate();
            }

            // Dispose of the prompt
            pr.Dispose();
        }
    }
}
