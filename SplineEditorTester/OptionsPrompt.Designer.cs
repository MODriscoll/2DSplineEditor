namespace SplineEditorTester
{
    partial class OptionsPrompt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.nudRadius = new System.Windows.Forms.NumericUpDown();
            this.lblRadius = new System.Windows.Forms.Label();
            this.lblThickness = new System.Windows.Forms.Label();
            this.nudThickness = new System.Windows.Forms.NumericUpDown();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(242, 42);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Options";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudRadius
            // 
            this.nudRadius.DecimalPlaces = 5;
            this.nudRadius.Location = new System.Drawing.Point(96, 114);
            this.nudRadius.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudRadius.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudRadius.Name = "nudRadius";
            this.nudRadius.Size = new System.Drawing.Size(67, 20);
            this.nudRadius.TabIndex = 1;
            this.nudRadius.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // lblRadius
            // 
            this.lblRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRadius.Location = new System.Drawing.Point(19, 67);
            this.lblRadius.Name = "lblRadius";
            this.lblRadius.Size = new System.Drawing.Size(235, 32);
            this.lblRadius.TabIndex = 2;
            this.lblRadius.Text = "Control Radius";
            this.lblRadius.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblThickness
            // 
            this.lblThickness.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThickness.Location = new System.Drawing.Point(14, 148);
            this.lblThickness.Name = "lblThickness";
            this.lblThickness.Size = new System.Drawing.Size(235, 32);
            this.lblThickness.TabIndex = 4;
            this.lblThickness.Text = "Line Thickness";
            this.lblThickness.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudThickness
            // 
            this.nudThickness.DecimalPlaces = 5;
            this.nudThickness.Location = new System.Drawing.Point(96, 199);
            this.nudThickness.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.nudThickness.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudThickness.Name = "nudThickness";
            this.nudThickness.Size = new System.Drawing.Size(67, 20);
            this.nudThickness.TabIndex = 3;
            this.nudThickness.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(19, 234);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(105, 46);
            this.btnConfirm.TabIndex = 5;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(144, 234);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(105, 46);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OptionsPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 292);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lblThickness);
            this.Controls.Add(this.nudThickness);
            this.Controls.Add(this.lblRadius);
            this.Controls.Add(this.nudRadius);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OptionsPrompt";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.nudRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudThickness)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.NumericUpDown nudRadius;
        private System.Windows.Forms.Label lblRadius;
        private System.Windows.Forms.Label lblThickness;
        private System.Windows.Forms.NumericUpDown nudThickness;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Button btnCancel;
    }
}