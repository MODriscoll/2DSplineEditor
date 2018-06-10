namespace SplineEditorTester
{
    partial class frmEditor
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
            this.components = new System.ComponentModel.Container();
            this.splineTabs = new System.Windows.Forms.TabControl();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkbInterpolate = new System.Windows.Forms.CheckBox();
            this.tmrInterpolate = new System.Windows.Forms.Timer(this.components);
            this.btnBezierMode = new System.Windows.Forms.Button();
            this.btnHermiteMode = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCamX = new System.Windows.Forms.Label();
            this.lblCamY = new System.Windows.Forms.Label();
            this.nudCamX = new System.Windows.Forms.NumericUpDown();
            this.nudCamY = new System.Windows.Forms.NumericUpDown();
            this.nudTimer = new System.Windows.Forms.NumericUpDown();
            this.lblMousePos = new System.Windows.Forms.Label();
            this.mnsMain = new System.Windows.Forms.MenuStrip();
            this.tsiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiHelp = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).BeginInit();
            this.mnsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // splineTabs
            // 
            this.splineTabs.AllowDrop = true;
            this.splineTabs.ItemSize = new System.Drawing.Size(60, 20);
            this.splineTabs.Location = new System.Drawing.Point(25, 25);
            this.splineTabs.Name = "splineTabs";
            this.splineTabs.SelectedIndex = 0;
            this.splineTabs.Size = new System.Drawing.Size(700, 500);
            this.splineTabs.TabIndex = 0;
            this.splineTabs.Selected += new System.Windows.Forms.TabControlEventHandler(this.splineTabs_Selected);
            this.splineTabs.Deselected += new System.Windows.Forms.TabControlEventHandler(this.splineTabs_Deselected);
            this.splineTabs.DragDrop += new System.Windows.Forms.DragEventHandler(this.splineTabs_DragDrop);
            this.splineTabs.DragEnter += new System.Windows.Forms.DragEventHandler(this.splineTabs_DragEnter);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(731, 25);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(241, 47);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Spline Editor";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkbInterpolate
            // 
            this.chkbInterpolate.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkbInterpolate.Location = new System.Drawing.Point(738, 109);
            this.chkbInterpolate.Name = "chkbInterpolate";
            this.chkbInterpolate.Size = new System.Drawing.Size(148, 41);
            this.chkbInterpolate.TabIndex = 2;
            this.chkbInterpolate.Text = "Interpolate";
            this.chkbInterpolate.UseVisualStyleBackColor = true;
            this.chkbInterpolate.CheckedChanged += new System.EventHandler(this.chkbInterpolate_CheckedChanged);
            // 
            // tmrInterpolate
            // 
            this.tmrInterpolate.Interval = 20;
            this.tmrInterpolate.Tick += new System.EventHandler(this.tmrInterpolate_Tick);
            // 
            // btnBezierMode
            // 
            this.btnBezierMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBezierMode.Location = new System.Drawing.Point(738, 181);
            this.btnBezierMode.Name = "btnBezierMode";
            this.btnBezierMode.Size = new System.Drawing.Size(110, 40);
            this.btnBezierMode.TabIndex = 4;
            this.btnBezierMode.Text = "Bezier";
            this.btnBezierMode.UseVisualStyleBackColor = true;
            this.btnBezierMode.Click += new System.EventHandler(this.btnBezierMode_Click);
            // 
            // btnHermiteMode
            // 
            this.btnHermiteMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHermiteMode.Location = new System.Drawing.Point(862, 181);
            this.btnHermiteMode.Name = "btnHermiteMode";
            this.btnHermiteMode.Size = new System.Drawing.Size(110, 40);
            this.btnHermiteMode.TabIndex = 5;
            this.btnHermiteMode.Text = "Hermite";
            this.btnHermiteMode.UseVisualStyleBackColor = true;
            this.btnHermiteMode.Click += new System.EventHandler(this.btnHermiteMode_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpen.Location = new System.Drawing.Point(738, 485);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(110, 40);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnNew
            // 
            this.btnNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.Location = new System.Drawing.Point(738, 439);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(110, 40);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(862, 439);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(110, 40);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(862, 485);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 40);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCamX
            // 
            this.lblCamX.AutoSize = true;
            this.lblCamX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCamX.Location = new System.Drawing.Point(739, 272);
            this.lblCamX.Name = "lblCamX";
            this.lblCamX.Size = new System.Drawing.Size(61, 20);
            this.lblCamX.TabIndex = 11;
            this.lblCamX.Text = "Cam X:";
            this.lblCamX.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCamY
            // 
            this.lblCamY.AutoSize = true;
            this.lblCamY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCamY.Location = new System.Drawing.Point(739, 315);
            this.lblCamY.Name = "lblCamY";
            this.lblCamY.Size = new System.Drawing.Size(61, 20);
            this.lblCamY.TabIndex = 12;
            this.lblCamY.Text = "Cam Y:";
            this.lblCamY.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudCamX
            // 
            this.nudCamX.DecimalPlaces = 10;
            this.nudCamX.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCamX.Location = new System.Drawing.Point(806, 270);
            this.nudCamX.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudCamX.Name = "nudCamX";
            this.nudCamX.Size = new System.Drawing.Size(165, 26);
            this.nudCamX.TabIndex = 13;
            this.nudCamX.ValueChanged += new System.EventHandler(this.nudCamX_ValueChanged);
            // 
            // nudCamY
            // 
            this.nudCamY.DecimalPlaces = 10;
            this.nudCamY.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCamY.Location = new System.Drawing.Point(806, 313);
            this.nudCamY.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudCamY.Name = "nudCamY";
            this.nudCamY.Size = new System.Drawing.Size(165, 26);
            this.nudCamY.TabIndex = 14;
            this.nudCamY.ValueChanged += new System.EventHandler(this.nudCamY_ValueChanged);
            // 
            // nudTimer
            // 
            this.nudTimer.DecimalPlaces = 4;
            this.nudTimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTimer.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudTimer.Location = new System.Drawing.Point(876, 114);
            this.nudTimer.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudTimer.Name = "nudTimer";
            this.nudTimer.Size = new System.Drawing.Size(95, 31);
            this.nudTimer.TabIndex = 15;
            this.nudTimer.ValueChanged += new System.EventHandler(this.nudTimer_ValueChanged);
            this.nudTimer.Enter += new System.EventHandler(this.nudTimer_Enter);
            this.nudTimer.Leave += new System.EventHandler(this.nudTimer_Leave);
            // 
            // lblMousePos
            // 
            this.lblMousePos.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMousePos.Location = new System.Drawing.Point(738, 370);
            this.lblMousePos.Name = "lblMousePos";
            this.lblMousePos.Size = new System.Drawing.Size(233, 44);
            this.lblMousePos.TabIndex = 16;
            this.lblMousePos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mnsMain
            // 
            this.mnsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiFile,
            this.tsiOptions,
            this.tsiHelp});
            this.mnsMain.Location = new System.Drawing.Point(0, 0);
            this.mnsMain.Name = "mnsMain";
            this.mnsMain.Size = new System.Drawing.Size(984, 24);
            this.mnsMain.TabIndex = 17;
            // 
            // tsiFile
            // 
            this.tsiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.mnuSave});
            this.tsiFile.Name = "tsiFile";
            this.tsiFile.Size = new System.Drawing.Size(37, 20);
            this.tsiFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.Size = new System.Drawing.Size(103, 22);
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
            // 
            // mnuOpen
            // 
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.Size = new System.Drawing.Size(103, 22);
            this.mnuOpen.Text = "&Open";
            this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(103, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.mnuSave_Click);
            // 
            // tsiOptions
            // 
            this.tsiOptions.Name = "tsiOptions";
            this.tsiOptions.Size = new System.Drawing.Size(61, 20);
            this.tsiOptions.Text = "&Options";
            this.tsiOptions.Click += new System.EventHandler(this.tsiOptions_Click);
            // 
            // tsiHelp
            // 
            this.tsiHelp.Name = "tsiHelp";
            this.tsiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsiHelp.Text = "&Help";
            this.tsiHelp.Click += new System.EventHandler(this.tsiHelp_Click);
            // 
            // frmEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.lblMousePos);
            this.Controls.Add(this.nudTimer);
            this.Controls.Add(this.nudCamY);
            this.Controls.Add(this.nudCamX);
            this.Controls.Add(this.lblCamY);
            this.Controls.Add(this.lblCamX);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnHermiteMode);
            this.Controls.Add(this.btnBezierMode);
            this.Controls.Add(this.chkbInterpolate);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.splineTabs);
            this.Controls.Add(this.mnsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.mnsMain;
            this.MaximizeBox = false;
            this.Name = "frmEditor";
            this.Text = "Spline Editor Tester";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            ((System.ComponentModel.ISupportInitialize)(this.nudCamX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCamY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTimer)).EndInit();
            this.mnsMain.ResumeLayout(false);
            this.mnsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl splineTabs;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkbInterpolate;
        private System.Windows.Forms.Timer tmrInterpolate;
        private System.Windows.Forms.Button btnBezierMode;
        private System.Windows.Forms.Button btnHermiteMode;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCamX;
        private System.Windows.Forms.Label lblCamY;
        private System.Windows.Forms.NumericUpDown nudCamX;
        private System.Windows.Forms.NumericUpDown nudCamY;
        private System.Windows.Forms.NumericUpDown nudTimer;
        private System.Windows.Forms.Label lblMousePos;
        private System.Windows.Forms.MenuStrip mnsMain;
        private System.Windows.Forms.ToolStripMenuItem tsiFile;
        private System.Windows.Forms.ToolStripMenuItem mnuNew;
        private System.Windows.Forms.ToolStripMenuItem mnuOpen;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem tsiOptions;
        private System.Windows.Forms.ToolStripMenuItem tsiHelp;
    }
}

