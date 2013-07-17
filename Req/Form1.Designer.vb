<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Me.BGRequis = New System.ComponentModel.BackgroundWorker
        Me.BGEKKO = New System.ComponentModel.BackgroundWorker
        Me.BGEKPO = New System.ComponentModel.BackgroundWorker
        Me.BGMasterData = New System.ComponentModel.BackgroundWorker
        Me.BGSourceList = New System.ComponentModel.BackgroundWorker
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.lblNextUpload = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.PB1 = New System.Windows.Forms.ProgressBar
        Me.txtStatus = New System.Windows.Forms.ListBox
        Me.BGEKKOPO = New System.ComponentModel.BackgroundWorker
        Me.BGOTD = New System.ComponentModel.BackgroundWorker
        Me.bgManufacter = New System.ComponentModel.BackgroundWorker
        Me.SuspendLayout()
        '
        'BGRequis
        '
        Me.BGRequis.WorkerReportsProgress = True
        '
        'BGEKKO
        '
        Me.BGEKKO.WorkerReportsProgress = True
        '
        'BGEKPO
        '
        Me.BGEKPO.WorkerReportsProgress = True
        '
        'BGMasterData
        '
        Me.BGMasterData.WorkerReportsProgress = True
        '
        'BGSourceList
        '
        Me.BGSourceList.WorkerReportsProgress = True
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'lblNextUpload
        '
        Me.lblNextUpload.AutoSize = True
        Me.lblNextUpload.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNextUpload.ForeColor = System.Drawing.Color.Red
        Me.lblNextUpload.Location = New System.Drawing.Point(201, 215)
        Me.lblNextUpload.Name = "lblNextUpload"
        Me.lblNextUpload.Size = New System.Drawing.Size(101, 31)
        Me.lblNextUpload.TabIndex = 11
        Me.lblNextUpload.Text = "Label2"
        Me.lblNextUpload.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Navy
        Me.Label6.Location = New System.Drawing.Point(12, 215)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(183, 31)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "Next Upload:"
        '
        'PB1
        '
        Me.PB1.Location = New System.Drawing.Point(12, 250)
        Me.PB1.Maximum = 3600
        Me.PB1.Name = "PB1"
        Me.PB1.Size = New System.Drawing.Size(452, 12)
        Me.PB1.Step = 1
        Me.PB1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.PB1.TabIndex = 12
        '
        'txtStatus
        '
        Me.txtStatus.FormattingEnabled = True
        Me.txtStatus.Location = New System.Drawing.Point(12, 12)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(452, 199)
        Me.txtStatus.TabIndex = 13
        '
        'BGEKKOPO
        '
        Me.BGEKKOPO.WorkerReportsProgress = True
        '
        'BGOTD
        '
        Me.BGOTD.WorkerReportsProgress = True
        '
        'bgManufacter
        '
        Me.bgManufacter.WorkerReportsProgress = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(481, 287)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.PB1)
        Me.Controls.Add(Me.lblNextUpload)
        Me.Controls.Add(Me.Label6)
        Me.Name = "Form1"
        Me.Text = "Req Info"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BGRequis As System.ComponentModel.BackgroundWorker
    Friend WithEvents BGEKKO As System.ComponentModel.BackgroundWorker
    Friend WithEvents BGEKPO As System.ComponentModel.BackgroundWorker
    Friend WithEvents BGMasterData As System.ComponentModel.BackgroundWorker
    Friend WithEvents BGSourceList As System.ComponentModel.BackgroundWorker
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents lblNextUpload As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PB1 As System.Windows.Forms.ProgressBar
    Friend WithEvents txtStatus As System.Windows.Forms.ListBox
    Friend WithEvents BGEKKOPO As System.ComponentModel.BackgroundWorker
    Friend WithEvents BGOTD As System.ComponentModel.BackgroundWorker
    Friend WithEvents bgManufacter As System.ComponentModel.BackgroundWorker

End Class
