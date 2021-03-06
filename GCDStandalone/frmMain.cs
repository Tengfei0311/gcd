﻿using System;
using System.Linq;
using System.Windows.Forms;
using GCDCore.Project;
using System.Text.RegularExpressions;
using System.Deployment.Application;
using System.Drawing;
using System.Diagnostics;

namespace GCDStandalone
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.Text = GCDCore.Properties.Resources.ApplicationNameLong;

            try
            {
                ProjectManager.Init(GCDCore.Properties.Settings.Default.AutomaticPyramids);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex, "Error setting up application files.");
            }

            // ensure this is empty in case there's no auto open project
            tssProjectPath.Text = string.Empty;

            //ucProjectExplorer1.ProjectTreeNodeSelectionChange += UpdateMenusAndToolstrips;
            UpdateMenusAndToolstrips(sender, e);

            // User setting for loading the last project
            bool bLoadLastProject = GCDCore.Properties.Settings.Default.LoadLastProjectAtStartUp;

#if DEBUG
            // Developers always try to last the load project
            bLoadLastProject = true;
#endif

            // Attempt to read project path from application arguments
            //string[] args = Environment.GetCommandLineArgs();
            string[] activationData = new string[] { };
            if (AppDomain.CurrentDomain.SetupInformation.ActivationArguments != null)
                activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
            //activationData = new string[] { @"file:///D:/Testing/GCD/James/Shotover/Shotover.gcd" };

            // First, try to get a path from the parameters
            if (activationData != null && activationData.Length >= 1)
            {
                string lastGCD = GCDCore.Properties.Settings.Default.LastUsedProjectFolder;
                if (!string.IsNullOrEmpty(lastGCD) && System.IO.Directory.Exists(lastGCD))
                {
                    Uri uri = new Uri(activationData[0]);
                    string fileNamePassedIn = uri.LocalPath.ToString();
                    try
                    {
                        OpenGCDProject(fileNamePassedIn);
                        bLoadLastProject = false;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        // Something went wrong with the last opened project. Ensure it doesn't try to get opened again.
                        GCDCore.Properties.Settings.Default.LastUsedProjectFolder = string.Empty;
                        GCDCore.Properties.Settings.Default.Save();
                        closeGCDProjectToolStripMenuItem_Click(null, null);
                    }

                }
            }
            // If the above fails or/and we want to load the last project do that instead
            if (bLoadLastProject)
            {
                string lastGCD = GCDCore.Properties.Settings.Default.LastUsedProjectFolder;
                if (!string.IsNullOrEmpty(lastGCD) && System.IO.Directory.Exists(lastGCD))
                {
                    string[] gcdFiles = System.IO.Directory.GetFiles(lastGCD, "*.gcd", System.IO.SearchOption.TopDirectoryOnly);
                    if (gcdFiles.Length == 1)
                    {
                        try
                        {
                            OpenGCDProject(gcdFiles[0]);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            // Something went wrong with the last opened project. Ensure it doesn't try to get opened again.
                            GCDCore.Properties.Settings.Default.LastUsedProjectFolder = string.Empty;
                            GCDCore.Properties.Settings.Default.Save();
                            closeGCDProjectToolStripMenuItem_Click(null, null);
                        }
                    }
                }
            }

            // Most recently used GCD projects
            RefreshMRUItems();

            ProjectManager.OnProgressChange += OnProgressChange;
            tssProgress.Visible = false;
            tspProgress.Visible = false;

        }

        private void RefreshMRUItems()
        {
            tsmiRecentGCDProjects.DropDownItems.Clear();
            AppendMRUItem(GCDCore.Properties.Settings.Default.MRU1);
            AppendMRUItem(GCDCore.Properties.Settings.Default.MRU2);
            AppendMRUItem(GCDCore.Properties.Settings.Default.MRU3);
            AppendMRUItem(GCDCore.Properties.Settings.Default.MRU4);
            AppendMRUItem(GCDCore.Properties.Settings.Default.MRU5);
            tsmiRecentGCDProjects.Enabled = tsmiRecentGCDProjects.DropDownItems.Count > 0;
        }

        private void AppendMRUItem(string gcdProjectFilePath)
        {
            // Abort if the GCD project file doesn't exist on this system.
            if (string.IsNullOrEmpty(gcdProjectFilePath) || !System.IO.File.Exists(gcdProjectFilePath))
                return;

            // Abort if the file is already in the list
            foreach (ToolStripMenuItem tsmiExisting in tsmiRecentGCDProjects.DropDownItems)
            {
                if (string.Compare(tsmiExisting.Tag.ToString(), gcdProjectFilePath, true) == 0)
                {
                    return;
                }
            }

            ToolStripMenuItem tsmi = new ToolStripMenuItem(string.Format("{0}. {1}", tsmiRecentGCDProjects.DropDownItems.Count + 1, gcdProjectFilePath), null, MRUItem_Click);
            tsmi.Tag = gcdProjectFilePath;
            tsmiRecentGCDProjects.DropDownItems.Add(tsmi);
        }

        private void MRUItem_Click(object sender, EventArgs e)
        {
            Match theMatch = Regex.Match(((ToolStripMenuItem)sender).Text, "([0-9]). (.*)");
            if (theMatch.Groups.Count == 3)
            {
                string path = theMatch.Groups[2].Value;
                if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
                {
                    // Opening the project will reorder the MRU list and refresh the MRU itself.
                    OpenGCDProject(path);
                }
            }
        }

        private void ProjectProperties_Click(object sender, EventArgs e)
        {
            try
            {
                bool bEditMode = string.Compare(((ToolStripItem)sender).Name, "projectPropertiesToolStripMenuItem", true) == 0 ||
                        string.Compare(((ToolStripItem)sender).Name, "tsiProjectProperties", true) == 0;

                GCDCore.UserInterface.Project.frmProjectProperties frm = new GCDCore.UserInterface.Project.frmProjectProperties(!bEditMode);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    OpenGCDProject(ProjectManager.Project.ProjectFile.FullName);
                }

                UpdateMenusAndToolstrips(sender, e);
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        private void openGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.DefaultExt = "xml";
            f.Filter = "GCD Project Files (*.gcd)|*.gcd";
            f.Title = "Open Existing GCD Project";
            f.CheckFileExists = true;
            //
            // PGB 2 May 2011 - Use the last browsed folder for project files. Note that
            // this is stored in a user setting and does not rely on the FileDialog to 
            // remember this value because the FileDialog may have been used for other purposes.
            if (!string.IsNullOrEmpty(GCDCore.Properties.Settings.Default.LastUsedProjectFolder) && System.IO.Directory.Exists(GCDCore.Properties.Settings.Default.LastUsedProjectFolder))
            {
                f.InitialDirectory = GCDCore.Properties.Settings.Default.LastUsedProjectFolder;

                // Try and find the last used project in the folder
                string[] fis = System.IO.Directory.GetFiles(GCDCore.Properties.Settings.Default.LastUsedProjectFolder, "*.gcd", System.IO.SearchOption.TopDirectoryOnly);
                if (fis.Count<string>() > 0)
                {
                    f.FileName = System.IO.Path.GetFileName(fis[0]);
                }
            }

            if (f.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    OpenGCDProject(f.FileName);
                }
                catch (Exception ex)
                {
                    GCDCore.GCDException.HandleException(ex);
                }
            }

            UpdateMenusAndToolstrips(sender, e);
        }

        private void OpenGCDProject(string gcdProject)
        {
            // Set the project file path first (which will attempt to read the XML file and throw an error if anything goes wrong)
            // Then set the settings if the read was successful.
            if (!ProjectManager.OpenProject(new System.IO.FileInfo(gcdProject)))
                return;

            try
            {
                // Insert the new file at the start of the MRU and bump all other items;
                GCDCore.Properties.Settings.Default.MRU5 = GCDCore.Properties.Settings.Default.MRU4;
                GCDCore.Properties.Settings.Default.MRU4 = GCDCore.Properties.Settings.Default.MRU3;
                GCDCore.Properties.Settings.Default.MRU3 = GCDCore.Properties.Settings.Default.MRU2;
                GCDCore.Properties.Settings.Default.MRU2 = GCDCore.Properties.Settings.Default.MRU1;
                GCDCore.Properties.Settings.Default.MRU1 = gcdProject;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error reading GCD most recent used (MRU) items {0}", ex.Message));
                // Something went wrong loading MRU list. Clear them all to start fresh
                GCDCore.Properties.Settings.Default.MRU1 = string.Empty;
                GCDCore.Properties.Settings.Default.MRU2 = string.Empty;
                GCDCore.Properties.Settings.Default.MRU3 = string.Empty;
                GCDCore.Properties.Settings.Default.MRU4 = string.Empty;
                GCDCore.Properties.Settings.Default.MRU5 = string.Empty;
            }

            GCDCore.Properties.Settings.Default.Save();
            RefreshMRUItems();

            // Now update the tool status strip
            tssProjectPath.Text = ProjectManager.Project is GCDProject ? ProjectManager.Project.ProjectFile.FullName : string.Empty;

            try
            {
                //GCDCore.Project.ProjectManager.Validate();
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating GCD project", ex);
            }

            ucProjectExplorer1.LoadTree();
            UpdateMenusAndToolstrips(null, null);
        }

        private void browseGCDProjectFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ProjectManager.Project is GCDProject)
            {
                System.Diagnostics.Process.Start(ProjectManager.Project.ProjectFile.DirectoryName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UpdateMenusAndToolstrips(object sender, EventArgs e)
        {
            UpdateMenuItemStatus(toolStrip1.Items);
            UpdateMenuItemStatus(menuStrip1.Items);

            // Control whether uploader is visible using developer preview mode.
            // Add the following to the user.config file to turn this feature on
            //<setting name="DeveloperPreview" serializeAs="String">
            //<value>True</value>
            //</setting>

            bool bShowUpload = GCDCore.Properties.Settings.Default.DeveloperPreview;

#if DEBUG
            bShowUpload = true;
#endif

            uploadThisProjectToGCDOnlineToolStripMenuItem.Visible = bShowUpload;
        }

        private void UpdateMenuItemStatus(ToolStripItemCollection aMenu)
        {
            foreach (ToolStripItem subMenu in aMenu)
            {
                // Skip over separators etc
                if (!(subMenu is ToolStripMenuItem || subMenu is ToolStripButton))
                    continue;

                if ((subMenu is ToolStripMenuItem && ((ToolStripMenuItem)subMenu).HasDropDownItems)) // if subMenu has children
                {
                    switch (subMenu.Name)
                    {
                        // Skip top level menus
                        case "analysisToolStripMenuItem":
                        case "dataPreparationToolStripMenuItem":
                        case "customizeToolStripMenuItem":
                        case "helpToolStripMenuItem1":
                            continue;

                        default:
                            // Drill into sub menus except for the most recently used items
                            if (string.Compare(subMenu.Name, "tsmiRecentGCDProjects", true) != 0)
                            {
                                UpdateMenuItemStatus(((ToolStripMenuItem)subMenu).DropDownItems); // Call recursive Method.
                            }
                            break;
                    }
                }
                else
                {
                    switch (subMenu.Name)
                    {
                        // Skip specific menu items here
                        case "newGCDProjectToolStripMenuItem":
                        case "openGCDProjectToolStripMenuItem":
                        case "exitToolStripMenuItem":
                        case "customizeToolStripMenuItem":
                            break; // do nothing. Always enabled.

                        // Skip specific tool strip items here
                        case "tsiNewProject":
                        case "tsiOpenProject":
                            break;

                        default:
                            subMenu.Enabled = ProjectManager.Project is GCDProject;
                            break;
                    }
                }
            }
        }

        private void onlineGCDHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDCore.Properties.Resources.HelpBaseURL);
        }

        private void gCDWebSiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDCore.Properties.Resources.GCDWebSiteURL);
        }

        private void aboutGCDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDCore.UserInterface.About.frmAbout frm = new GCDCore.UserInterface.About.frmAbout();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDCore.UserInterface.Options.frmOptions frm = new GCDCore.UserInterface.Options.frmOptions();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        private void fISLibraryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                GCDCore.UserInterface.FISLibrary.frmFISLibrary frm = new GCDCore.UserInterface.FISLibrary.frmFISLibrary();
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        private void closeGCDProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectManager.CloseCurrentProject();
                ucProjectExplorer1.LoadTree();
                UpdateMenusAndToolstrips(sender, e);

                // Now update the tool status strip
                tssProjectPath.Text = string.Empty;
            }
            catch (Exception ex)
            {
                GCDCore.GCDException.HandleException(ex);
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.O:
                        openGCDProjectToolStripMenuItem_Click(null, null);
                        break;

                    case Keys.N:
                        ProjectProperties_Click(newGCDProjectToolStripMenuItem, null);
                        break;
                }
            }

            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Enter:
                        // show project properties if a project is open
                        if (ProjectManager.Project is GCDProject)
                            ProjectProperties_Click(projectPropertiesToolStripMenuItem, null);
                        break;
                }
            }
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCheckInfo info = null;
            Cursor.Current = Cursors.WaitCursor;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment AD = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = AD.CheckForDetailedUpdate();
                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time.\n\nPlease check your network connection, or try again later. Error: "
                        + dde.Message, GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message,
                        GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    bool doUpdate = true;

                    if (info.IsUpdateRequired)
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " + "version to version "
                            + info.MinimumRequiredVersion.ToString() + ". The application will now install the update and restart.", "Update Available",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        if (dr != DialogResult.Yes)
                        {
                            doUpdate = false;
                        }
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            AD.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.", GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show(string.Format("Cannot install the latest version of the application.\n\nPlease check your network connection, or try again later.\n\n{0}", dde.Message),
                                GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("There are no updates available. The {0} software is up to date.",
                        GCDCore.Properties.Resources.ApplicationNameLong), GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("The application is not deployed over the internet and therefore cannot be updated automatically.",
                    GCDCore.Properties.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            Cursor.Current = Cursors.Default;
        }

        private void uploadThisProjectToGCDOnlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GCDCore.UserInterface.Tools.frmOnlineManager frm = new GCDCore.UserInterface.Tools.frmOnlineManager();
            frm.ShowDialog();
        }

        private void topographicAnalysisToolkitTATToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDCore.Properties.Resources.TATWebSite);
        }

        private void crossSectionViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(GCDCore.Properties.Resources.CrossSectionViewerWebSite);
        }

        public void OnProgressChange(object sender, GCDConsoleLib.OpStatus progress)
        {
            bool bShow = progress.State != GCDConsoleLib.OpStatus.States.Complete;

            tssProgress.Visible = bShow;
            tspProgress.Visible = bShow;

            if (tssProgress.Text != progress.Message)
                tssProgress.Text = progress.Message;
            tspProgress.Value = progress.Progress;

            statusStrip1.Refresh();

            using (Graphics gr = tspProgress.ProgressBar.CreateGraphics())
            {
                string theString = progress.Progress.ToString() + "%";
                float top = tspProgress.Height / 2 - (gr.MeasureString(theString, SystemFonts.DefaultFont).Height / 2.0F);
                float left = tspProgress.Width / 2 - (gr.MeasureString(theString, SystemFonts.DefaultFont).Width / 2.0F);

                PointF thePoint = new PointF(left, top);
                gr.DrawString(theString, SystemFonts.DefaultFont, Brushes.Black, thePoint);
            }


        }
    }
}