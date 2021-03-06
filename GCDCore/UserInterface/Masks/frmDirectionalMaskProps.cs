﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GCDCore.Project.Masks;
using GCDCore.Project;

namespace GCDCore.UserInterface.Masks
{
    public partial class frmDirectionalMaskProps : Form, IProjectItemForm
    {
        private const string DirectionFieldInfo = "The lowest integer should be at the top of the reach and the highest integer the most downstream." +
            " All features must have a valid value (i.e. null values are not permitted), but gaps in the numbering are permitted.";

        public DirectionalMask Mask { get; internal set; }

        public GCDProjectItem GCDProjectItem { get { return Mask; } }


        public frmDirectionalMaskProps(DirectionalMask mask = null)
        {
            InitializeComponent();
            Mask = mask;
        }

        private void frmDirectionalMaskProps_Load(object sender, EventArgs e)
        {
            // subscribe to the event when the user changes the input ShapeFile
            ucPolygon.PathChanged += InputShapeFileChanged;
            UpdateControls(sender, e);

            if (Mask == null)
            {
                cmdOK.Text = Properties.Resources.CreateButtonText;
                ucPolygon.InitializeBrowseNew("Directional Mask", GCDConsoleLib.GDalGeometryType.SimpleTypes.Polygon);
            }
            else
            {
                cmdOK.Text = Properties.Resources.UpdateButtonText;

                txtName.Text = Mask.Name;
                txtPath.Text = ProjectManager.Project.GetRelativePath(Mask.Vector.GISFileInfo);
                ucPolygon.InitializeExisting("Directional Mask", Mask.Vector);
                ucPolygon.AddToMap += cmdAddToMap_Click;

                cboField.Text = Mask._Field;
                cboField.Enabled = false;

                if (!string.IsNullOrEmpty(Mask.LabelField))
                {
                    chkLabel.Checked = true;
                    cboLabel.Text = Mask.LabelField;
                }

                cboDirection.Text = Mask.DirectionField;
                cboDirection.Enabled = false;
                rdoDescending.Checked = !Mask.Ascending;

                if (!string.IsNullOrEmpty(Mask.DistanceField))
                {
                    cboDistance.Text = Mask.DistanceField;
                }

                lblPath.Visible = false;
                txtPath.Visible = false;
                Height -= (grpFeatureClass.Top - txtPath.Top);
            }

            grpFeatureClass.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            MinimumSize = new Size(this.Width, this.Height);

            tTip.SetToolTip(txtName, "The name that will be used to refer to this mask throughout the GCD.");
            tTip.SetToolTip(txtPath, "The relative file path within this GCD project where this mask will be stored.");
            tTip.SetToolTip(cboField, "The string attribute field that will be used to identify regions within the feature class.");
            tTip.SetToolTip(chkLabel, "Check this box to use a different string field than the main mask field for display labels when viewing the mask items.");
            tTip.SetToolTip(cboLabel, "The optional string attribute field that contains the display labels for each mask item.");
            tTip.SetToolTip(cboDirection, "The integer attribute field that defines the order in which mask regions are arranged. Must be consecutive, non-zero integers, but not necessarily starting at zero or one.");
            tTip.SetToolTip(chkDistance, "Check this box to include a floating point field that defines the distance downstream of each mask feature.");
            tTip.SetToolTip(cboDistance, "The optional floating point attribute field that defines the distance downstream of each mask polygon.");
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
            {
                DialogResult = DialogResult.None;
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;

                if (Mask == null)
                {
                    FileInfo fiMask = ProjectManager.Project.GetAbsolutePath(txtPath.Text);
                    fiMask.Directory.Create();

                    ucPolygon.SelectedItem.Copy(fiMask);

                    string lablField = chkLabel.Checked ? cboLabel.Text : string.Empty;
                    string distField = chkDistance.Checked ? cboDistance.Text : string.Empty;

                    Mask = new DirectionalMask(txtName.Text, fiMask, cboField.Text, lablField, cboDirection.Text, rdoAscending.Checked, distField);
                    ProjectManager.Project.Masks.Add(Mask);
                    ProjectManager.AddNewProjectItemToMap(Mask);
                }
                else
                {
                    Mask.Name = txtName.Text;
                    Mask.LabelField = chkLabel.Checked ? cboLabel.Text : string.Empty;
                    Mask.DistanceField = chkDistance.Checked ? cboDistance.Text : string.Empty;
                    Mask.Ascending = rdoAscending.Checked;
                }

                ProjectManager.Project.Save();
                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error creating regular mask.");
            }
        }

        private bool ValidateForm()
        {
            if (!MaskValidation.ValidateMaskName(txtName, Mask))
                return false;

            if (Mask == null)
            {
                if (!MaskValidation.ValidateShapeFile(ucPolygon) || !MaskValidation.ValidateShapefileProjection(ucPolygon))
                    return false;
            }

            if (!MaskValidation.ValidateField(cboField))
                return false;

            if (chkLabel.Checked)
            {
                if (cboLabel.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select a text field in the ShapeFile that provides mask labels or uncheck the label checkbox.", "Missing Label Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboLabel.Select();
                    return false;
                }

                if (string.Compare(cboLabel.Text, cboField.Text, true) == 0)
                {
                    MessageBox.Show("You cannot select the same ShapeFile field for both the mask field and label." +
                        " Please choose different fields or uncheck the label checkbox to proceed with just mask field values as the mask definition.",
                        "Duplicate Field Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (chkDistance.Checked)
            {
                if (cboDistance.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select a floating point field in the ShapeFile that provides distance values or uncheck the distance checkbox.", "Missing Distance Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboDistance.Select();
                    return false;
                }

                if (Mask == null)
                {
                    if (ucPolygon.SelectedItem.Features.Values.Any(x => x.IsNull(cboDistance.Text)))
                    {
                        MessageBox.Show(string.Format("One or more features in the ShapeFile have null or invalid values in the {0} field. A valid distance field must possess valid floating point values for all features.", cboDistance.Text), "Invalid Distance Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                }
            }

            if (cboDirection.SelectedIndex < 0)
            {
                MessageBox.Show("Directional masks require an integer direction field that specifies how the features are ordered from top to bottom of the reach. " +
                  DirectionFieldInfo, "Missing Direction Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            if (Mask == null)
            {
                if (ucPolygon.SelectedItem.Features.Values.Any(x => x.IsNull(cboDirection.Text)))
                {
                    MessageBox.Show(string.Format("One or more features in the ShapeFile have null or invalid values in the {0} field. {1}",
                        cboDistance.Text, DirectionFieldInfo), "Invalid Direction Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!ValidateDirectionFieldValues())
                return false;

            return true;
        }

        private bool ValidateDirectionFieldValues()
        {
            List<int> existingValues = new List<int>();
            List<string> duplicateValues = new List<string>();

            foreach (GCDConsoleLib.VectorFeature feat in ucPolygon.SelectedItem.Features.Values)
            {
                int fldValue = feat.GetFieldAsInt(cboDirection.Text);
                if (existingValues.Contains(fldValue))
                {
                    if (!duplicateValues.Contains(fldValue.ToString()))
                        duplicateValues.Add(fldValue.ToString());
                }
                else
                {
                    existingValues.Add(fldValue);
                }
            }

            if (duplicateValues.Count > 0)
            {

                MessageBox.Show(string.Format("There are multiple occurances of the values {0} in the {1} direction field. {2}",
                    string.Join(",", duplicateValues.ToArray<string>()), cboDirection.Text, DirectionFieldInfo),
                    "Duplicate Direction Values", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }

            return true;
        }

        private void UpdateControls(object sender, EventArgs e)
        {
            cboLabel.Enabled = chkLabel.Checked;
            cboDistance.Enabled = chkDistance.Checked;
        }

        private void InputShapeFileChanged(object sender, naru.ui.PathEventArgs e)
        {
            cboField.DataSource = null;

            if (ucPolygon.SelectedItem == null)
            {
                return;
            }

            Cursor = Cursors.WaitCursor;
            GCDConsoleLib.Vector shapeFile = ucPolygon.SelectedItem;

            // Use the ShapeFile file name if the user hasn't specified one yet
            if (string.IsNullOrEmpty(txtName.Text))
            {
                txtName.Text = naru.os.File.RemoveDangerousCharacters(System.IO.Path.GetFileNameWithoutExtension(shapeFile.GISFileInfo.FullName));
            }

            cboField.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            cboLabel.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.StringField)).ToList<GCDConsoleLib.VectorField>();
            cboDirection.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.IntField)).ToList<GCDConsoleLib.VectorField>();
            cboDistance.DataSource = shapeFile.Fields.Values.Where(x => x.Type.Equals(GCDConsoleLib.GDalFieldType.RealField)).ToList<GCDConsoleLib.VectorField>();
            Cursor = Cursors.Default;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtPath.Text = string.Empty;
            else
                txtPath.Text = ProjectManager.Project.GetRelativePath(ProjectManager.Project.MaskPath(txtName.Text));
        }

        private void cmdAddToMap_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.OnAddVectorToMap(Mask);
            }
            catch (Exception ex)
            {
                GCDException.HandleException(ex, "Error adding directional mask to the map.");
            }
        }

        private void cmdHelp_Click(object sender, EventArgs e)
        {
            OnlineHelp.Show(Name);
        }
    }
}
