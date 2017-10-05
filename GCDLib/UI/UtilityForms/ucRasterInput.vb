﻿Namespace UI.UtilityForms

    Public Class ucRasterInput
        Inherits ucInputBase

#Region "Methods"

        Private Sub RasterInputUC_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        End Sub

        Protected Overrides Sub Browse()

            Try
                naru.ui.Textbox.BrowseOpenRaster(txtPath, naru.ui.UIHelpers.WrapMessageWithNoun("Browse and Select a", Noun, "Raster"))

            Catch ex As Exception
                Core.ExceptionHelper.HandleException(ex, "Error browsing to raster.")
            End Try

        End Sub

        Public Overrides Function Validate() As Boolean

            If Not TypeOf SelectedItem Is Core.GISDataStructures.Raster Then
                System.Windows.Forms.MessageBox.Show(naru.ui.UIHelpers.WrapMessageWithNoun("Please select a", Noun, " to continue."), My.Resources.ApplicationNameLong, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information)
                Return False
            End If

            Return True

        End Function

#End Region

    End Class
End Namespace
