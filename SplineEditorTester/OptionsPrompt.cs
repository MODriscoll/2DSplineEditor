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

namespace SplineEditorTester
{
    public partial class OptionsPrompt : Form
    {
        public float Radius { get; private set; }
        public float Thickness { get; private set; }

        public OptionsPrompt(float radius, float thickness)
        {
            InitializeComponent();

            // Set the radius and thickness
            Radius = MathF.Clamp((float)nudRadius.Minimum, (float)nudRadius.Maximum, radius);
            Thickness = MathF.Clamp((float)nudThickness.Minimum, (float)nudThickness.Maximum, thickness);

            // Set the input boxes values
            nudRadius.Value = (decimal)Radius;
            nudThickness.Value = (decimal)Thickness;

            // Set result to none by default
            DialogResult = DialogResult.None;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Save the values
            Radius = (float)nudRadius.Value;
            Thickness = (float)nudThickness.Value;

            // Set result to OK
            DialogResult = DialogResult.OK;

            // Close this form
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Set result to cancel
            DialogResult = DialogResult.Cancel;

            // Close this form
            Close();
        }
    }
}
