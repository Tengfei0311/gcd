﻿namespace GCDCore.UserInterface.ChangeDetection.MultiEpoch
{
    partial class frmMultiEpoch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMultiEpoch));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdMoveUp = new System.Windows.Forms.Button();
            this.cmdMoveDown = new System.Windows.Forms.Button();
            this.grdDEMs = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdEpochs = new System.Windows.Forms.DataGridView();
            this.chkPrevious = new System.Windows.Forms.CheckBox();
            this.chkEarliest = new System.Windows.Forms.CheckBox();
            this.chkNewest = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.ucThresholding1 = new GCDCore.UserInterface.ChangeDetection.ucThresholding();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.colIsActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colNewDEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldDEM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colActive = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colDEMSurvey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colErrorSurface = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdDEMs)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEpochs)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(494, 213);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(413, 213);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "Run Batch";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(8, 213);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cmdMoveUp);
            this.groupBox1.Controls.Add(this.cmdMoveDown);
            this.groupBox1.Controls.Add(this.grdDEMs);
            this.groupBox1.Location = new System.Drawing.Point(8, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(561, 171);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DEM Surveys in Chronological Order";
            // 
            // cmdMoveUp
            // 
            this.cmdMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdMoveUp.Image = global::GCDCore.Properties.Resources.up;
            this.cmdMoveUp.Location = new System.Drawing.Point(532, 19);
            this.cmdMoveUp.Name = "cmdMoveUp";
            this.cmdMoveUp.Size = new System.Drawing.Size(23, 23);
            this.cmdMoveUp.TabIndex = 2;
            this.cmdMoveUp.UseVisualStyleBackColor = true;
            this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
            // 
            // cmdMoveDown
            // 
            this.cmdMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdMoveDown.Image = global::GCDCore.Properties.Resources.down;
            this.cmdMoveDown.Location = new System.Drawing.Point(532, 45);
            this.cmdMoveDown.Name = "cmdMoveDown";
            this.cmdMoveDown.Size = new System.Drawing.Size(23, 23);
            this.cmdMoveDown.TabIndex = 1;
            this.cmdMoveDown.UseVisualStyleBackColor = true;
            this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
            // 
            // grdDEMs
            // 
            this.grdDEMs.AllowUserToAddRows = false;
            this.grdDEMs.AllowUserToDeleteRows = false;
            this.grdDEMs.AllowUserToResizeRows = false;
            this.grdDEMs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdDEMs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDEMs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colActive,
            this.colDEMSurvey,
            this.colErrorSurface});
            this.grdDEMs.Location = new System.Drawing.Point(6, 19);
            this.grdDEMs.MultiSelect = false;
            this.grdDEMs.Name = "grdDEMs";
            this.grdDEMs.RowHeadersVisible = false;
            this.grdDEMs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdDEMs.Size = new System.Drawing.Size(520, 146);
            this.grdDEMs.TabIndex = 0;
            this.grdDEMs.SelectionChanged += new System.EventHandler(this.grdDEMs_SelectionChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.grdEpochs);
            this.groupBox2.Controls.Add(this.chkPrevious);
            this.groupBox2.Controls.Add(this.chkEarliest);
            this.groupBox2.Controls.Add(this.chkNewest);
            this.groupBox2.Location = new System.Drawing.Point(8, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(560, 204);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DEM Survey Epoch Queue";
            // 
            // grdEpochs
            // 
            this.grdEpochs.AllowUserToAddRows = false;
            this.grdEpochs.AllowUserToDeleteRows = false;
            this.grdEpochs.AllowUserToResizeRows = false;
            this.grdEpochs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdEpochs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdEpochs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIsActive,
            this.colNewDEM,
            this.colOldDEM});
            this.grdEpochs.Location = new System.Drawing.Point(14, 83);
            this.grdEpochs.MultiSelect = false;
            this.grdEpochs.Name = "grdEpochs";
            this.grdEpochs.RowHeadersVisible = false;
            this.grdEpochs.Size = new System.Drawing.Size(540, 115);
            this.grdEpochs.TabIndex = 3;
            // 
            // chkPrevious
            // 
            this.chkPrevious.AutoSize = true;
            this.chkPrevious.Checked = true;
            this.chkPrevious.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrevious.Location = new System.Drawing.Point(14, 60);
            this.chkPrevious.Name = "chkPrevious";
            this.chkPrevious.Size = new System.Drawing.Size(187, 17);
            this.chkPrevious.TabIndex = 2;
            this.chkPrevious.Text = "All DEMs minus the previous DEM";
            this.chkPrevious.UseVisualStyleBackColor = true;
            // 
            // chkEarliest
            // 
            this.chkEarliest.AutoSize = true;
            this.chkEarliest.Checked = true;
            this.chkEarliest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEarliest.Location = new System.Drawing.Point(14, 39);
            this.chkEarliest.Name = "chkEarliest";
            this.chkEarliest.Size = new System.Drawing.Size(180, 17);
            this.chkEarliest.TabIndex = 1;
            this.chkEarliest.Text = "All DEMs minus the earliest DEM";
            this.chkEarliest.UseVisualStyleBackColor = true;
            // 
            // chkNewest
            // 
            this.chkNewest.AutoSize = true;
            this.chkNewest.Checked = true;
            this.chkNewest.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNewest.Location = new System.Drawing.Point(14, 20);
            this.chkNewest.Name = "chkNewest";
            this.chkNewest.Size = new System.Drawing.Size(191, 17);
            this.chkNewest.TabIndex = 0;
            this.chkNewest.Text = "Newest DEM minus all other DEMs";
            this.chkNewest.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pnlTop, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.pnlBottom, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 612);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.groupBox1);
            this.pnlTop.Controls.Add(this.ucThresholding1);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(578, 361);
            this.pnlTop.TabIndex = 0;
            // 
            // ucThresholding1
            // 
            this.ucThresholding1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucThresholding1.Location = new System.Drawing.Point(8, 186);
            this.ucThresholding1.Name = "ucThresholding1";
            this.ucThresholding1.Size = new System.Drawing.Size(560, 169);
            this.ucThresholding1.TabIndex = 4;
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.groupBox2);
            this.pnlBottom.Controls.Add(this.cmdHelp);
            this.pnlBottom.Controls.Add(this.cmdCancel);
            this.pnlBottom.Controls.Add(this.cmdOK);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 370);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(578, 239);
            this.pnlBottom.TabIndex = 1;
            // 
            // colIsActive
            // 
            this.colIsActive.DataPropertyName = "IsActive";
            this.colIsActive.HeaderText = "Queued";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.Width = 50;
            // 
            // colNewDEM
            // 
            this.colNewDEM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNewDEM.DataPropertyName = "NewDEMName";
            this.colNewDEM.HeaderText = "New DEM Survey";
            this.colNewDEM.Name = "colNewDEM";
            this.colNewDEM.ReadOnly = true;
            // 
            // colOldDEM
            // 
            this.colOldDEM.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colOldDEM.DataPropertyName = "OldDEMName";
            this.colOldDEM.HeaderText = "Old DEM Survey";
            this.colOldDEM.Name = "colOldDEM";
            this.colOldDEM.ReadOnly = true;
            // 
            // colActive
            // 
            this.colActive.DataPropertyName = "IsActive";
            this.colActive.HeaderText = "Active";
            this.colActive.Name = "colActive";
            this.colActive.Width = 50;
            // 
            // colDEMSurvey
            // 
            this.colDEMSurvey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDEMSurvey.DataPropertyName = "DEMName";
            this.colDEMSurvey.HeaderText = "DEM Survey";
            this.colDEMSurvey.Name = "colDEMSurvey";
            this.colDEMSurvey.ReadOnly = true;
            this.colDEMSurvey.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colDEMSurvey.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colErrorSurface
            // 
            this.colErrorSurface.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colErrorSurface.DataPropertyName = "ErrorName";
            this.colErrorSurface.HeaderText = "Error Surface";
            this.colErrorSurface.Name = "colErrorSurface";
            this.colErrorSurface.ReadOnly = true;
            // 
            // frmMultiEpoch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 612);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMultiEpoch";
            this.Text = "Batch Change Detection - Multiple Epochs";
            this.Load += new System.EventHandler(this.frmInterComp_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdDEMs)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdEpochs)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdMoveUp;
        private System.Windows.Forms.Button cmdMoveDown;
        private System.Windows.Forms.DataGridView grdDEMs;
        private ucThresholding ucThresholding1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView grdEpochs;
        private System.Windows.Forms.CheckBox chkPrevious;
        private System.Windows.Forms.CheckBox chkEarliest;
        private System.Windows.Forms.CheckBox chkNewest;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDEMSurvey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colErrorSurface;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNewDEM;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldDEM;
    }
}