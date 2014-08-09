Imports System.IO

Public Class Form1

    Private activity As String = ""

    Private Sub error_handler(ByVal ex As Exception)
        MsgBox(ex.ToString)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try

        
            If FolderBrowserDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
                TextBox1.Text = FolderBrowserDialog1.SelectedPath
            End If
        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If TextBox1.Text.Length < 1 Then
                MsgBox("Please select a valid base folder to operate on.", MsgBoxStyle.Information, "Input Required")
            Else
                Label2.Text = "0"
                Label1.Text = ""
                BackgroundWorker1.RunWorkerAsync(TextBox1.Text)
            End If


        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            folderwalker(e.Argument.ToString)
        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub folderwalker(ByVal dir As String)
        Try

            Dim dinfo As DirectoryInfo = New DirectoryInfo(dir)
            If dinfo.Name.StartsWith("_") Then
                activity = "Removed: " & dinfo.FullName
                BackgroundWorker1.ReportProgress(1)
                My.Computer.FileSystem.DeleteDirectory(dinfo.FullName, FileIO.DeleteDirectoryOption.DeleteAllContents)
            Else
                For Each subdir As DirectoryInfo In dinfo.GetDirectories
                    folderwalker(subdir.FullName)
                Next
                activity = "Ignored: " & dinfo.FullName
                BackgroundWorker1.ReportProgress(0)
            End If
            dinfo = Nothing
        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Try
            Label1.Text = activity
            Label2.Text = Integer.Parse(Label2.Text) + Integer.Parse(e.ProgressPercentage)
        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Try
            MsgBox("Operation Completed")
        Catch ex As Exception
            error_handler(ex)
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = "FPSubweb Cleanup " & My.Application.Info.Version.Major & My.Application.Info.Version.Minor & My.Application.Info.Version.Build & "." & My.Application.Info.Version.Revision
    End Sub
End Class
