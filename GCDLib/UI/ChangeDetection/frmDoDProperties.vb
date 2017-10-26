﻿Imports System.Windows.Forms
Imports GCDLib.Core

Namespace UI.ChangeDetection

    Public Class frmDoDProperties

        'Private m_pArcMap As ESRI.ArcGIS.ArcMapUI.IMxApplication
        Private m_bUserEditedName As Boolean
        Private m_frmSpatialCoherence As frmCoherenceProperties

        ' These are the results of the analysis. They are not populated until
        ' the user clicks OK and the change detection completes successfully.
        Private m_rDoD As ProjectDS.DoDsRow
        Private m_DoDResult As Core.ChangeDetection.DoDResult

        ' Initial DEM Surveys to select. (User right clicked on a pair of DEMs
        ' in the project explorer and chose to add a new DoD for the same DEMs.
        Private m_nInitialNewDEMSurveyID As Integer
        Private m_nInitialOldDEMSurveyID As Integer

        ''' <summary>
        ''' Retrieve the GCD project dataset DoD row generated by the change detection
        ''' </summary>
        ''' <returns>GCD project dataset DoD row generated by the change detection</returns>
        ''' <remarks>Returns nothing if not calculated.</remarks>
        Public ReadOnly Property DoDRow As ProjectDS.DoDsRow
            Get
                Return m_rDoD
            End Get
        End Property

        Public ReadOnly Property DoDResults As Core.ChangeDetection.DoDResult
            Get
                Return m_DoDResult
            End Get
        End Property

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            'm_pArcMap = pArcMap
            m_bUserEditedName = False

            m_frmSpatialCoherence = New frmCoherenceProperties
            m_rDoD = Nothing
            m_DoDResult = Nothing

            m_nInitialNewDEMSurveyID = 0
            m_nInitialOldDEMSurveyID = 0

        End Sub

        Public Sub New(nNewDEMSurveyID As Integer, nOldDEMSurveyID As Integer)

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            'm_pArcMap = pArcMap
            m_bUserEditedName = False

            m_frmSpatialCoherence = New frmCoherenceProperties
            m_rDoD = Nothing
            m_DoDResult = Nothing

            m_nInitialNewDEMSurveyID = nNewDEMSurveyID
            m_nInitialOldDEMSurveyID = nOldDEMSurveyID

        End Sub

        Private Sub DoDPropertiesForm_Load(sender As Object, e As System.EventArgs) Handles Me.Load

            Try
                EnableDisableControls()
                For Each rDEMSurvey As ProjectDS.DEMSurveyRow In Core.GCDProject.ProjectManagerBase.CurrentProject.GetDEMSurveyRows
                    Dim nIndex As Integer

                    nIndex = cboNewDEM.Items.Add(New naru.db.NamedObject(rDEMSurvey.DEMSurveyID, rDEMSurvey.Name))
                    If rDEMSurvey.DEMSurveyID = m_nInitialNewDEMSurveyID Then
                        cboNewDEM.SelectedIndex = nIndex
                    End If

                    nIndex = cboOldDEM.Items.Add(New naru.db.NamedObject(rDEMSurvey.DEMSurveyID, rDEMSurvey.Name))
                    If rDEMSurvey.DEMSurveyID = m_nInitialOldDEMSurveyID Then
                        cboOldDEM.SelectedIndex = nIndex
                    End If
                Next

                Dim sUnits As String = Core.GCDProject.ProjectManagerBase.CurrentProject.DisplayUnits
                If Not String.IsNullOrEmpty(sUnits) Then
                    lblMinLodThreshold.Text = lblMinLodThreshold.Text.Replace("()", "(" & sUnits & ")")
                End If

                If cboNewDEM.Items.Count > 0 AndAlso cboNewDEM.SelectedIndex < 0 Then
                    cboNewDEM.SelectedIndex = 0
                End If

                If cboOldDEM.Items.Count > 0 AndAlso cboOldDEM.SelectedIndex < 0 Then
                    If cboOldDEM.Items.Count > 1 Then
                        cboOldDEM.SelectedIndex = 1
                    Else
                        cboOldDEM.SelectedIndex = 0
                    End If
                End If

                ' Load AOIs
                For Each rAOI As ProjectDS.AOIsRow In Core.GCDProject.ProjectManagerBase.CurrentProject.GetAOIsRows
                    lstAOI.Items.Add(New naru.db.NamedObject(rAOI.AOIID, rAOI.Name))
                Next

                UpdateAnalysisName()

            Catch ex As Exception
                Core.ExceptionHelper.HandleException(ex)
            End Try

        End Sub

        Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

            If Not ValidateForm() Then
                Me.DialogResult = System.Windows.Forms.DialogResult.None
                Exit Sub
            End If

            Try
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

                Dim gNewDEM As New GCDConsoleLib.Raster(Core.GCDProject.ProjectManagerBase.GetAbsolutePath(GetDEMRow(cboNewDEM).Source))
                Dim gOldDEM As New GCDConsoleLib.Raster(Core.GCDProject.ProjectManagerBase.GetAbsolutePath(GetDEMRow(cboOldDEM).Source))

                Dim gAOI As GCDConsoleLib.Vector = Nothing
                Dim rAOI As ProjectDS.AOIsRow = GetAOIRow()
                If TypeOf rAOI Is ProjectDS.AOIsRow Then
                    gAOI = New GCDConsoleLib.Vector(Core.GCDProject.ProjectManagerBase.GetAbsolutePath(rAOI.Source))
                End If

                Dim cdEngine As Core.ChangeDetection.ChangeDetectionEngineBase = Nothing
                If rdoMinLOD.Checked Then
                    cdEngine = New Core.ChangeDetection.ChangeDetectionEngineMinLOD(txtName.Text, txtOutputFolder.Text, gNewDEM, gOldDEM, gAOI, valMinLodThreshold.Value, My.Settings.ChartWidth, My.Settings.ChartHeight)
                Else
                    ' Propagated or probabilistic. Use the error surfaces
                    Dim gNewError As New GCDConsoleLib.Raster(GCDProject.ProjectManagerBase.GetAbsolutePath(GetErrorRow(cboNewDEM, cboNewError).Source))
                    Dim gOldError As New GCDConsoleLib.Raster(GCDProject.ProjectManagerBase.GetAbsolutePath(GetErrorRow(cboOldDEM, cboOldError).Source))

                    If rdoPropagated.Checked Then
                        cdEngine = New Core.ChangeDetection.ChangeDetectionEnginePropProb(txtName.Text, txtOutputFolder.Text, gNewDEM, gOldDEM, gNewError, gOldError, gAOI, My.Settings.ChartHeight, My.Settings.ChartWidth)
                    Else
                        Dim spatCoherence As Core.ChangeDetection.CoherenceProperties = Nothing
                        If chkBayesian.Checked Then
                            spatCoherence = New Core.ChangeDetection.CoherenceProperties(m_frmSpatialCoherence.FilterSize, m_frmSpatialCoherence.PercentLess, m_frmSpatialCoherence.PercentGreater)
                        End If

                        cdEngine = New Core.ChangeDetection.ChangeDetectionEngineProbabilistic(txtName.Text, txtOutputFolder.Text, gNewDEM, gOldDEM, gNewError, gOldError, gAOI, valConfidence.Value, My.Settings.ChartHeight, My.Settings.ChartWidth, spatCoherence)
                    End If
                End If

                Dim sRawDoDPath As String = String.Empty
                Dim sThreshDodPath As String = String.Empty
                Dim sRawHistPath As String = String.Empty
                Dim sThreshHistPath As String = String.Empty
                Dim sSummaryXMLPath As String = String.Empty
                Dim sPropagatedError As String = String.Empty
                Dim sProbabilityRaster As String = String.Empty
                Dim sSpatialCoErosionRaster As String = String.Empty
                Dim sSpatialCoDepositionRaster As String = String.Empty
                Dim sConditionalRaster As String = String.Empty
                Dim sPosterior As String = String.Empty

                Cursor.Current = Cursors.WaitCursor
                m_DoDResult = cdEngine.Calculate(sRawDoDPath, sThreshDodPath, sRawHistPath, sThreshHistPath, sSummaryXMLPath)
                Cursor.Current = Cursors.WaitCursor
                '
                ' Temporary fix. The change detection routine seems to be messing up the projection on 
                ' the resultant raster. So assume that the output project is identical to the
                ' input project and simply use the geoprocessing routine to define it.
                Try
                    If IO.File.Exists(sThreshDodPath) Then
                        Throw New NotImplementedException
                        'GP.DataManagement.DefineProjection(sThreshDodPath, pSR)
                    End If

                    If IO.File.Exists(sRawDoDPath) Then
                        Throw New NotImplementedException
                        ' GP.DataManagement.DefineProjection(sRawDoDPath, pSR)
                    End If

                    If Not rdoMinLOD.Checked Then

                        Dim sPropErrPath As String = IO.Path.Combine(txtOutputFolder.Text, "PropErr.tif")

                        If IO.File.Exists(sPropErrPath) Then
                            Throw New NotImplementedException
                            ' GP.DataManagement.DefineProjection(sPropErrPath, pSR)
                        End If

                        If rdoProbabilistic.Checked Then

                            Dim sPriorProbPath As String = IO.Path.Combine(txtOutputFolder.Text, "PriorProb.tif")

                            If IO.File.Exists(sPriorProbPath) Then
                                Throw New NotImplementedException
                                ' GP.DataManagement.DefineProjection(sPriorProbPath, pSR)
                            End If

                        End If
                    End If

                Catch ex As Exception
                    'Do nothing
                End Try
                '
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                ' Pyramid generation
                '
                ' Raw DoD
                If RasterPyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw) Then
                    RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDRaw, sRawDoDPath)
                End If

                ' Thresholded DoD
                If RasterPyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded) Then
                    RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.DoDThresholded, sThreshDodPath)
                End If

                If TypeOf m_DoDResult Is Core.ChangeDetection.DoDResultProbabilisitic Then
                    Dim dodProb As Core.ChangeDetection.DoDResultProbabilisitic = DirectCast(m_DoDResult, Core.ChangeDetection.DoDResultProbabilisitic)

                    ' Probability Raster pyramids
                    If RasterPyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters) Then

                        If Not String.IsNullOrEmpty(dodProb.ProbabilityRaster) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, dodProb.ProbabilityRaster)
                        End If

                        If Not String.IsNullOrEmpty(dodProb.SpatialCoErosionRaster) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, dodProb.SpatialCoErosionRaster)
                        End If

                        If Not String.IsNullOrEmpty(dodProb.SpatialCoDepositionRaster) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, dodProb.SpatialCoDepositionRaster)
                        End If

                        If Not String.IsNullOrEmpty(dodProb.ConditionalRaster) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, dodProb.ConditionalRaster)
                        End If

                        If Not String.IsNullOrEmpty(dodProb.PosteriorRaster) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.ProbabilityRasters, dodProb.PosteriorRaster)
                        End If
                    End If

                    ' Make relative paths for storing in the project.
                    sProbabilityRaster = GCDProject.ProjectManagerBase.GetRelativePath(dodProb.ProbabilityRaster)
                    sSpatialCoErosionRaster = GCDProject.ProjectManagerBase.GetRelativePath(dodProb.SpatialCoErosionRaster)
                    sSpatialCoDepositionRaster = GCDProject.ProjectManagerBase.GetRelativePath(dodProb.SpatialCoDepositionRaster)
                    sConditionalRaster = GCDProject.ProjectManagerBase.GetRelativePath(dodProb.ConditionalRaster)
                    sPosterior = GCDProject.ProjectManagerBase.GetRelativePath(dodProb.PosteriorRaster)
                End If

                If TypeOf m_DoDResult Is Core.ChangeDetection.DoDResultPropagated Then
                    sPropagatedError = DirectCast(m_DoDResult, Core.ChangeDetection.DoDResultPropagated).PropagatedErrorRaster

                    ' Propagated Raster pyrmads
                    If Not String.IsNullOrEmpty(sPropagatedError) Then
                        If RasterPyramidManager.AutomaticallyBuildPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError) Then
                            RasterPyramidManager.PerformRasterPyramids(RasterPyramidManager.PyramidRasterTypes.PropagatedError, sPropagatedError)
                        End If

                        ' Make the path relative
                        sPropagatedError = GCDProject.ProjectManagerBase.GetRelativePath(sPropagatedError)
                    End If
                End If

                Dim fThreshold As Double = 0
                If rdoMinLOD.Checked Then
                    fThreshold = valMinLodThreshold.Value
                ElseIf rdoProbabilistic.Checked Then
                    fThreshold = valConfidence.Value
                End If

                Dim sRelativeOutputFolder As String = GCDProject.ProjectManagerBase.GetRelativePath(txtOutputFolder.Text)

                ' Now save the DoD to the project dataset.
                m_rDoD = GCDProject.ProjectManagerBase.ds.DoDs.AddDoDsRow(txtName.Text, sRelativeOutputFolder, cboNewDEM.Text, cboOldDEM.Text,
                                                             GCDProject.ProjectManagerBase.CurrentProject, cboNewError.Text, cboOldError.Text _
                                                             , rdoMinLOD.Checked, rdoPropagated.Checked, rdoProbabilistic.Checked _
                                                             , fThreshold, chkBayesian.Checked, m_frmSpatialCoherence.FilterSize _
                                                             , m_frmSpatialCoherence.PercentLess, m_frmSpatialCoherence.PercentGreater _
                                                             , GCDProject.ProjectManagerBase.GetRelativePath(sRawDoDPath) _
                                                             , GCDProject.ProjectManagerBase.GetRelativePath(sThreshDodPath) _
                                                             , GCDProject.ProjectManagerBase.GetRelativePath(sRawHistPath) _
                                                             , GCDProject.ProjectManagerBase.GetRelativePath(sThreshHistPath) _
                                                             , GCDProject.ProjectManagerBase.GetRelativePath(sSummaryXMLPath) _
                                                             , m_DoDResult.ChangeStats.CellArea _
                                                             , m_DoDResult.ChangeStats.AreaErosion_Raw, m_DoDResult.ChangeStats.AreaDeposition_Raw _
                                                             , m_DoDResult.ChangeStats.AreaErosion_Thresholded, m_DoDResult.ChangeStats.AreaDeposition_Thresholded _
                                                             , m_DoDResult.ChangeStats.VolumeErosion_Raw, m_DoDResult.ChangeStats.VolumeDeposition_Raw _
                                                             , m_DoDResult.ChangeStats.VolumeErosion_Thresholded, m_DoDResult.ChangeStats.VolumeDeposition_Thresholded _
                                                             , m_DoDResult.ChangeStats.VolumeErosion_Error, m_DoDResult.ChangeStats.VolumeDeposition_Error, sPropagatedError _
                                                             , sProbabilityRaster, sConditionalRaster, sPosterior, sSpatialCoDepositionRaster, sSpatialCoErosionRaster)

                GCDProject.ProjectManagerBase.save()
                Cursor.Current = Cursors.Default

                ' Try and add the DoD to the map
                If My.Settings.AddOutputLayersToMap Then
                    Try
                        ' TODO 
                        Throw New Exception("not implemented")
                        'GCDProject.ProjectManagerUI.ArcMapManager.AddDoD(m_rDoD)
                    Catch ex As Exception
                        ' Do nothing. Failing to add the to map is not serious.
                    End Try
                End If

            Catch ex As Exception
                If ex.Message.ToLower.Contains("all values are zero") Then
                    MessageBox.Show("The change detection was cancelled because all values in the resultant DoD raster were zero. This is likely because the new and old rasters represent the same data.", My.Resources.ApplicationNameLong, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    ExceptionHelper.HandleException(ex)
                End If
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End Sub

        Private Function ValidateForm() As Boolean

            If String.IsNullOrEmpty(txtName.Text) Then
                MsgBox("Please enter a name for the analysis.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            Else
                If IO.Directory.Exists(txtOutputFolder.Text) Then
                    MsgBox("An analysis folder with the same output path already exists. Please change the analysis name so that a different output folder will be used.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            If TypeOf cboNewDEM.SelectedItem Is naru.db.NamedObject Then
                If TypeOf cboOldDEM.SelectedItem Is naru.db.NamedObject Then
                    If DirectCast(cboNewDEM.SelectedItem, naru.db.NamedObject).ID = DirectCast(cboOldDEM.SelectedItem, naru.db.NamedObject).ID Then
                        MsgBox("Please choose two different DEM Surveys.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                        Return False
                    End If
                Else
                    MsgBox("Please select an Old DEM Survey.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            Else
                MsgBox("Please select a New DEM Survey.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                Return False
            End If

            If Not rdoMinLOD.Checked Then
                If Not TypeOf cboNewError.SelectedItem Is naru.db.NamedObject Then
                    MsgBox("Please select a new error surface.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If

                If Not TypeOf cboOldError.SelectedItem Is naru.db.NamedObject Then
                    MsgBox("Please select an old error surface.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
                    Return False
                End If
            End If

            'If rdoAOI.Checked Then
            '    If Not TypeOf cboAOI.SelectedItem Is GISCode.ListItem Then
            '        MsgBox("Please select an AOI or choose to analyse the common area of the DEM Surveys.", MsgBoxStyle.Information, My.Resources.ApplicationNameLong)
            '        Return False
            '    End If
            'End If

            Return True

        End Function

        Private Sub rdoProbabilistic_CheckedChanged(sender As Object, e As System.EventArgs) Handles _
        rdoMinLOD.CheckedChanged,
        rdoPropagated.CheckedChanged,
        rdoProbabilistic.CheckedChanged

            EnableDisableControls()
            UpdateAnalysisName()

        End Sub

        Private Sub EnableDisableControls()

            lblNewError.Enabled = Not rdoMinLOD.Checked
            cboNewError.Enabled = Not rdoMinLOD.Checked
            lblOldError.Enabled = Not rdoMinLOD.Checked
            cboOldError.Enabled = Not rdoMinLOD.Checked

            valMinLodThreshold.Enabled = rdoMinLOD.Checked
            lblMinLodThreshold.Enabled = rdoMinLOD.Checked

            lblConfidence.Enabled = rdoProbabilistic.Checked
            valConfidence.Enabled = rdoProbabilistic.Checked
            chkBayesian.Enabled = rdoProbabilistic.Checked
            cmdBayesianProperties.Enabled = rdoProbabilistic.Checked AndAlso chkBayesian.Checked

        End Sub

#Region "DEM Selection Changed"

        Private Sub cboNewDEM_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboNewDEM.SelectedIndexChanged

            cboNewError.Items.Clear()
            Dim rDEM As ProjectDS.DEMSurveyRow = GetDEMRow(cboNewDEM)
            If TypeOf rDEM Is ProjectDS.DEMSurveyRow Then
                For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
                    cboNewError.Items.Add(New naru.db.NamedObject(rError.ErrorSurfaceID, rError.Name))
                Next
            End If

            If cboNewError.Items.Count = 1 Then
                cboNewError.SelectedIndex = 0
            End If
            UpdateAnalysisName()

        End Sub

        Private Sub cboOldDEM_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cboOldDEM.SelectedIndexChanged

            cboOldError.Items.Clear()
            Dim rDEM As ProjectDS.DEMSurveyRow = GetDEMRow(cboOldDEM)
            If TypeOf rDEM Is ProjectDS.DEMSurveyRow Then
                For Each rError As ProjectDS.ErrorSurfaceRow In rDEM.GetErrorSurfaceRows
                    cboOldError.Items.Add(New naru.db.NamedObject(rError.ErrorSurfaceID, rError.Name))
                Next
            End If
            If cboOldError.Items.Count = 1 Then
                cboOldError.SelectedIndex = 0
            End If

            UpdateAnalysisName()
        End Sub

        Private Sub UpdateAnalysisName()

            If m_bUserEditedName Then
                Exit Sub
            End If

            Dim sAnalysisName As String = naru.os.File.RemoveDangerousCharacters(cboNewDEM.Text)
            If Not String.IsNullOrEmpty(sAnalysisName) Then
                sAnalysisName &= "_"
            End If

            If Not String.IsNullOrEmpty(cboOldDEM.Text) Then
                sAnalysisName &= naru.os.File.RemoveDangerousCharacters(cboOldDEM.Text)
            End If

            If rdoMinLOD.Checked Then
                sAnalysisName &= " MinLoD " & valMinLodThreshold.Value.ToString("#0.00")
            ElseIf rdoPropagated.Checked Then
                sAnalysisName &= " Prop"
            Else
                sAnalysisName &= " Prob " & valConfidence.Value.ToString("#0.00")
            End If

            txtName.Text = sAnalysisName.Trim()

        End Sub

#End Region

        Private Function GetDEMRow(cbo As ComboBox) As ProjectDS.DEMSurveyRow

            Dim rResult As ProjectDS.DEMSurveyRow = Nothing
            Dim lItem As naru.db.NamedObject = cbo.SelectedItem
            If TypeOf lItem Is naru.db.NamedObject Then
                rResult = GCDProject.ProjectManagerBase.ds.DEMSurvey.FindByDEMSurveyID(lItem.ID)
            End If

            Return rResult

        End Function

        Private Function GetErrorRow(cboDEM As ComboBox, cboError As ComboBox) As ProjectDS.ErrorSurfaceRow

            Dim rResult As ProjectDS.ErrorSurfaceRow = Nothing
            Dim lItem As naru.db.NamedObject = cboError.SelectedItem
            If TypeOf lItem Is naru.db.NamedObject Then
                For Each rError As ProjectDS.ErrorSurfaceRow In GetDEMRow(cboDEM).GetErrorSurfaceRows
                    If rError.ErrorSurfaceID = lItem.ID Then
                        rResult = rError
                        Exit For
                    End If
                Next
            End If

            Return rResult

        End Function

        Private Function GetAOIRow() As ProjectDS.AOIsRow

            Dim rResult As ProjectDS.AOIsRow = Nothing
            'Dim lItem As GISCode.ListItem = cboAOI.SelectedItem
            'If TypeOf lItem Is GISCode.ListItem Then
            '    For Each rAOI As ProjectDS.AOIsRow In GCD.GCDProject.ProjectManagerBase.CurrentProject.GetAOIsRows
            '        If rAOI.AOIID = lItem.ID Then
            '            rResult = rAOI
            '            Exit For
            '        End If
            '    Next
            'End If

            Return rResult

        End Function

        Private Sub rdoCommonArea_CheckedChanged(sender As Object, e As System.EventArgs)

            EnableDisableControls()
        End Sub

        Private Sub txtName_KeyDown(sender As Object, e As System.Windows.Forms.KeyEventArgs) Handles txtName.KeyDown
            m_bUserEditedName = True
        End Sub

        Private Sub txtName_TextChanged(sender As Object, e As System.EventArgs) Handles txtName.TextChanged

            Try
                If String.IsNullOrEmpty(txtName.Text) Then
                    txtOutputFolder.Text = String.Empty
                Else
                    txtOutputFolder.Text = GCDProject.ProjectManagerBase.OutputManager.GetDoDOutputFolder(txtName.Text)
                End If
            Catch ex As Exception

            End Try

        End Sub

        Private Sub cmdBayesianProperties_Click(sender As System.Object, e As System.EventArgs) Handles cmdBayesianProperties.Click

            m_frmSpatialCoherence.ShowDialog()

        End Sub

        Private Sub chkBayesian_CheckedChanged(sender As Object, e As System.EventArgs) Handles chkBayesian.CheckedChanged
            EnableDisableControls()
        End Sub

        Private Sub valConfidence_ValueChanged(sender As Object, e As System.EventArgs) Handles _
        valConfidence.ValueChanged,
        valMinLodThreshold.ValueChanged

            UpdateAnalysisName()
        End Sub

        Private Sub cmdHelp_Click(sender As System.Object, e As System.EventArgs) Handles cmdHelp.Click
            Process.Start(My.Resources.HelpBaseURL & "gcd-command-reference/gcd-project-explorer/j-change-detection-context-menu/i-add-change-detection")
        End Sub
    End Class

End Namespace