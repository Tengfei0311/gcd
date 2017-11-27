using GCDCore.Project;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace GCDCore.UserInterface.BudgetSegregation
{
	partial class frmBudgetSegResults : System.Windows.Forms.Form
	{

		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}

		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBudgetSegResults));
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdHelp = new System.Windows.Forms.Button();
			this.tabMain = new System.Windows.Forms.TabControl();
			this.TabPage1 = new System.Windows.Forms.TabPage();
			this.ucSummary = new GCDCore.UserInterface.ChangeDetection.ucDoDSummary();
			this.cboSummaryClass = new System.Windows.Forms.ComboBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.TabPage2 = new System.Windows.Forms.TabPage();
			this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.ucBars = new GCDCore.UserInterface.ChangeDetection.ucChangeBars();
			this.Label5 = new System.Windows.Forms.Label();
			this.cboECDClass = new System.Windows.Forms.ComboBox();
			this.ucHistogram = new GCDCore.UserInterface.ChangeDetection.ucDoDHistogram();
			this.TabPage3 = new System.Windows.Forms.TabPage();
			this.grpBudgetSeg = new System.Windows.Forms.GroupBox();
			this.txtField = new System.Windows.Forms.TextBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.txtPolygonMask = new System.Windows.Forms.TextBox();
			this.cmsBasicRaster = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.AddToMapToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.Label3 = new System.Windows.Forms.Label();
			this.ucProperties = new GCDCore.UserInterface.ChangeDetection.ucDoDProperties();
			this.TabPage4 = new System.Windows.Forms.TabPage();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtName = new System.Windows.Forms.TextBox();
			this.cmdBrowse = new System.Windows.Forms.Button();
			this.tabMain.SuspendLayout();
			this.TabPage1.SuspendLayout();
			this.TabPage2.SuspendLayout();
			this.TableLayoutPanel1.SuspendLayout();
			this.TabPage3.SuspendLayout();
			this.grpBudgetSeg.SuspendLayout();
			this.cmsBasicRaster.SuspendLayout();
			this.SuspendLayout();
			//
			//cmdCancel
			//
			this.cmdCancel.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Location = new System.Drawing.Point(591, 475);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(75, 23);
			this.cmdCancel.TabIndex = 0;
			this.cmdCancel.Text = "Close";
			this.cmdCancel.UseVisualStyleBackColor = true;
			//
			//cmdHelp
			//
			this.cmdHelp.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left);
			this.cmdHelp.Location = new System.Drawing.Point(12, 475);
			this.cmdHelp.Name = "cmdHelp";
			this.cmdHelp.Size = new System.Drawing.Size(75, 23);
			this.cmdHelp.TabIndex = 2;
			this.cmdHelp.Text = "Help";
			this.cmdHelp.UseVisualStyleBackColor = true;
			//
			//tabMain
			//
			this.tabMain.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.tabMain.Controls.Add(this.TabPage1);
			this.tabMain.Controls.Add(this.TabPage2);
			this.tabMain.Controls.Add(this.TabPage3);
			this.tabMain.Controls.Add(this.TabPage4);
			this.tabMain.Location = new System.Drawing.Point(4, 36);
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(662, 433);
			this.tabMain.TabIndex = 3;
			//
			//TabPage1
			//
			this.TabPage1.Controls.Add(this.ucSummary);
			this.TabPage1.Controls.Add(this.cboSummaryClass);
			this.TabPage1.Controls.Add(this.Label2);
			this.TabPage1.Location = new System.Drawing.Point(4, 22);
			this.TabPage1.Name = "TabPage1";
			this.TabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage1.Size = new System.Drawing.Size(654, 407);
			this.TabPage1.TabIndex = 0;
			this.TabPage1.Text = "Tabular Results By Category";
			this.TabPage1.UseVisualStyleBackColor = true;
			//
			//ucSummary
			//
			this.ucSummary.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.ucSummary.Location = new System.Drawing.Point(8, 39);
			this.ucSummary.Name = "ucSummary";
			this.ucSummary.Size = new System.Drawing.Size(640, 362);
			this.ucSummary.TabIndex = 2;
			//
			//cboSummaryClass
			//
			this.cboSummaryClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboSummaryClass.FormattingEnabled = true;
			this.cboSummaryClass.Location = new System.Drawing.Point(64, 12);
			this.cboSummaryClass.Name = "cboSummaryClass";
			this.cboSummaryClass.Size = new System.Drawing.Size(293, 21);
			this.cboSummaryClass.TabIndex = 1;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Location = new System.Drawing.Point(10, 16);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(52, 13);
			this.Label2.TabIndex = 0;
			this.Label2.Text = "Category:";
			//
			//TabPage2
			//
			this.TabPage2.Controls.Add(this.TableLayoutPanel1);
			this.TabPage2.Location = new System.Drawing.Point(4, 22);
			this.TabPage2.Name = "TabPage2";
			this.TabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage2.Size = new System.Drawing.Size(654, 407);
			this.TabPage2.TabIndex = 1;
			this.TabPage2.Text = "Graphical Results By Category";
			this.TabPage2.UseVisualStyleBackColor = true;
			//
			//TableLayoutPanel1
			//
			this.TableLayoutPanel1.ColumnCount = 3;
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62f));
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75f));
			this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25f));
			this.TableLayoutPanel1.Controls.Add(this.ucBars, 2, 1);
			this.TableLayoutPanel1.Controls.Add(this.Label5, 0, 0);
			this.TableLayoutPanel1.Controls.Add(this.cboECDClass, 1, 0);
			this.TableLayoutPanel1.Controls.Add(this.ucHistogram, 0, 1);
			this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.TableLayoutPanel1.Name = "TableLayoutPanel1";
			this.TableLayoutPanel1.RowCount = 2;
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25f));
			this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.TableLayoutPanel1.Size = new System.Drawing.Size(648, 401);
			this.TableLayoutPanel1.TabIndex = 6;
			//
			//ucBars
			//
			this.ucBars.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucBars.Location = new System.Drawing.Point(504, 28);
			this.ucBars.Name = "ucBars";
			this.ucBars.Size = new System.Drawing.Size(141, 370);
			this.ucBars.TabIndex = 5;
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Label5.Location = new System.Drawing.Point(3, 0);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(56, 25);
			this.Label5.TabIndex = 2;
			this.Label5.Text = "Category:";
			//
			//cboECDClass
			//
			this.cboECDClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboECDClass.FormattingEnabled = true;
			this.cboECDClass.Location = new System.Drawing.Point(65, 3);
			this.cboECDClass.Name = "cboECDClass";
			this.cboECDClass.Size = new System.Drawing.Size(293, 21);
			this.cboECDClass.TabIndex = 3;
			//
			//ucHistogram
			//
			this.ucHistogram.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.TableLayoutPanel1.SetColumnSpan(this.ucHistogram, 2);
			this.ucHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ucHistogram.Location = new System.Drawing.Point(3, 28);
			this.ucHistogram.Name = "ucHistogram";
			this.ucHistogram.Size = new System.Drawing.Size(495, 370);
			this.ucHistogram.TabIndex = 4;
			//
			//TabPage3
			//
			this.TabPage3.Controls.Add(this.grpBudgetSeg);
			this.TabPage3.Controls.Add(this.ucProperties);
			this.TabPage3.Location = new System.Drawing.Point(4, 22);
			this.TabPage3.Name = "TabPage3";
			this.TabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage3.Size = new System.Drawing.Size(654, 407);
			this.TabPage3.TabIndex = 2;
			this.TabPage3.Text = "Analysis Inputs";
			this.TabPage3.UseVisualStyleBackColor = true;
			//
			//grpBudgetSeg
			//
			this.grpBudgetSeg.Controls.Add(this.txtField);
			this.grpBudgetSeg.Controls.Add(this.Label4);
			this.grpBudgetSeg.Controls.Add(this.txtPolygonMask);
			this.grpBudgetSeg.Controls.Add(this.Label3);
			this.grpBudgetSeg.Location = new System.Drawing.Point(8, 200);
			this.grpBudgetSeg.Name = "grpBudgetSeg";
			this.grpBudgetSeg.Size = new System.Drawing.Size(640, 100);
			this.grpBudgetSeg.TabIndex = 1;
			this.grpBudgetSeg.TabStop = false;
			this.grpBudgetSeg.Text = "Budget Segregation";
			//
			//txtField
			//
			this.txtField.Location = new System.Drawing.Point(99, 59);
			this.txtField.Name = "txtField";
			this.txtField.ReadOnly = true;
			this.txtField.Size = new System.Drawing.Size(204, 20);
			this.txtField.TabIndex = 3;
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(10, 59);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(32, 13);
			this.Label4.TabIndex = 2;
			this.Label4.Text = "Field:";
			//
			//txtPolygonMask
			//
			this.txtPolygonMask.ContextMenuStrip = this.cmsBasicRaster;
			this.txtPolygonMask.Location = new System.Drawing.Point(99, 24);
			this.txtPolygonMask.Name = "txtPolygonMask";
			this.txtPolygonMask.ReadOnly = true;
			this.txtPolygonMask.Size = new System.Drawing.Size(535, 20);
			this.txtPolygonMask.TabIndex = 1;
			//
			//cmsBasicRaster
			//
			this.cmsBasicRaster.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.AddToMapToolStripMenuItem1 });
			this.cmsBasicRaster.Name = "cmsBasicRaster";
			this.cmsBasicRaster.Size = new System.Drawing.Size(138, 26);
			//
			//AddToMapToolStripMenuItem1
			//
			this.AddToMapToolStripMenuItem1.Image = Properties.Resources.AddToMap;
			this.AddToMapToolStripMenuItem1.Name = "AddToMapToolStripMenuItem1";
			this.AddToMapToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
			this.AddToMapToolStripMenuItem1.Text = "Add to Map";
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(10, 24);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(76, 13);
			this.Label3.TabIndex = 0;
			this.Label3.Text = "Polygon mask:";
			//
			//ucProperties
			//
			this.ucProperties.Location = new System.Drawing.Point(6, 6);
			this.ucProperties.Name = "ucProperties";
			this.ucProperties.Size = new System.Drawing.Size(642, 206);
			this.ucProperties.TabIndex = 0;
			//
			//TabPage4
			//
			this.TabPage4.Location = new System.Drawing.Point(4, 22);
			this.TabPage4.Name = "TabPage4";
			this.TabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.TabPage4.Size = new System.Drawing.Size(654, 407);
			this.TabPage4.TabIndex = 3;
			this.TabPage4.Text = "Report";
			this.TabPage4.UseVisualStyleBackColor = true;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(13, 13);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(38, 13);
			this.Label1.TabIndex = 4;
			this.Label1.Text = "Name:";
			//
			//txtName
			//
			this.txtName.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.txtName.Location = new System.Drawing.Point(78, 10);
			this.txtName.Name = "txtName";
			this.txtName.ReadOnly = true;
			this.txtName.Size = new System.Drawing.Size(559, 20);
			this.txtName.TabIndex = 5;
			//
			//cmdBrowse
			//
			this.cmdBrowse.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdBrowse.Image = Properties.Resources.BrowseFolder;
			this.cmdBrowse.Location = new System.Drawing.Point(643, 9);
			this.cmdBrowse.Name = "cmdBrowse";
			this.cmdBrowse.Size = new System.Drawing.Size(23, 23);
			this.cmdBrowse.TabIndex = 6;
			this.cmdBrowse.UseVisualStyleBackColor = true;
			//
			//frmBudgetSegResults
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 510);
			this.Controls.Add(this.cmdBrowse);
			this.Controls.Add(this.txtName);
			this.Controls.Add(this.Label1);
			this.Controls.Add(this.tabMain);
			this.Controls.Add(this.cmdHelp);
			this.Controls.Add(this.cmdCancel);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.Name = "frmBudgetSegResults";
			this.Text = "Budget Segregation Results";
			this.tabMain.ResumeLayout(false);
			this.TabPage1.ResumeLayout(false);
			this.TabPage1.PerformLayout();
			this.TabPage2.ResumeLayout(false);
			this.TableLayoutPanel1.ResumeLayout(false);
			this.TableLayoutPanel1.PerformLayout();
			this.TabPage3.ResumeLayout(false);
			this.grpBudgetSeg.ResumeLayout(false);
			this.grpBudgetSeg.PerformLayout();
			this.cmsBasicRaster.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		internal System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button withEventsField_cmdHelp;
		internal System.Windows.Forms.Button cmdHelp {
			get { return withEventsField_cmdHelp; }
			set {
				if (withEventsField_cmdHelp != null) {
					withEventsField_cmdHelp.Click -= cmdHelp_Click;
				}
				withEventsField_cmdHelp = value;
				if (withEventsField_cmdHelp != null) {
					withEventsField_cmdHelp.Click += cmdHelp_Click;
				}
			}
		}
		internal System.Windows.Forms.TabControl tabMain;
		internal System.Windows.Forms.TabPage TabPage1;
		internal System.Windows.Forms.TabPage TabPage2;
		internal System.Windows.Forms.Label Label1;
		internal System.Windows.Forms.TextBox txtName;
		internal ChangeDetection.ucDoDSummary ucSummary;
		private System.Windows.Forms.ComboBox withEventsField_cboSummaryClass;
		internal System.Windows.Forms.ComboBox cboSummaryClass {
			get { return withEventsField_cboSummaryClass; }
			set {
				if (withEventsField_cboSummaryClass != null) {
					withEventsField_cboSummaryClass.SelectedIndexChanged -= cboSummaryClass_SelectedIndexChanged;
				}
				withEventsField_cboSummaryClass = value;
				if (withEventsField_cboSummaryClass != null) {
					withEventsField_cboSummaryClass.SelectedIndexChanged += cboSummaryClass_SelectedIndexChanged;
				}
			}
		}
		internal System.Windows.Forms.Label Label2;
		internal System.Windows.Forms.TabPage TabPage3;
		internal System.Windows.Forms.TabPage TabPage4;
		internal System.Windows.Forms.GroupBox grpBudgetSeg;
		internal System.Windows.Forms.TextBox txtField;
		internal System.Windows.Forms.Label Label4;
		internal System.Windows.Forms.TextBox txtPolygonMask;
		internal System.Windows.Forms.Label Label3;
		internal ChangeDetection.ucDoDProperties ucProperties;
		private System.Windows.Forms.ComboBox withEventsField_cboECDClass;
		internal System.Windows.Forms.ComboBox cboECDClass {
			get { return withEventsField_cboECDClass; }
			set {
				if (withEventsField_cboECDClass != null) {
					withEventsField_cboECDClass.SelectedIndexChanged -= cboECDClass_SelectedIndexChanged;
				}
				withEventsField_cboECDClass = value;
				if (withEventsField_cboECDClass != null) {
					withEventsField_cboECDClass.SelectedIndexChanged += cboECDClass_SelectedIndexChanged;
				}
			}
		}
		internal System.Windows.Forms.Label Label5;
		internal ChangeDetection.ucDoDHistogram ucHistogram;
		internal ChangeDetection.ucChangeBars ucBars;
		internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
		private System.Windows.Forms.Button withEventsField_cmdBrowse;
		internal System.Windows.Forms.Button cmdBrowse {
			get { return withEventsField_cmdBrowse; }
			set {
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click -= cmdBrowse_Click;
				}
				withEventsField_cmdBrowse = value;
				if (withEventsField_cmdBrowse != null) {
					withEventsField_cmdBrowse.Click += cmdBrowse_Click;
				}
			}
		}
		internal System.Windows.Forms.ContextMenuStrip cmsBasicRaster;
		private System.Windows.Forms.ToolStripMenuItem withEventsField_AddToMapToolStripMenuItem1;
		internal System.Windows.Forms.ToolStripMenuItem AddToMapToolStripMenuItem1 {
			get { return withEventsField_AddToMapToolStripMenuItem1; }
			set {
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click -= AddToMapToolStripMenuItem1_Click;
				}
				withEventsField_AddToMapToolStripMenuItem1 = value;
				if (withEventsField_AddToMapToolStripMenuItem1 != null) {
					withEventsField_AddToMapToolStripMenuItem1.Click += AddToMapToolStripMenuItem1_Click;
				}
			}
		}
	}
}