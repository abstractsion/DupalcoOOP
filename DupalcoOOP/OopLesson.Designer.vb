<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OopLessonTopics
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.rtbNotes = New System.Windows.Forms.RichTextBox()
        Me.lblPrompt = New System.Windows.Forms.Label()
        Me.txtInput = New System.Windows.Forms.TextBox()
        Me.cboChoice = New System.Windows.Forms.ComboBox()
        Me.btnExecute = New System.Windows.Forms.Button()
        Me.txtOutput = New System.Windows.Forms.TextBox()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.Transparent
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 28.125!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.White
        Me.lblTitle.Location = New System.Drawing.Point(50, 30)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(272, 100)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        '
        'rtbNotes
        '
        Me.rtbNotes.BackColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.rtbNotes.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbNotes.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbNotes.ForeColor = System.Drawing.Color.White
        Me.rtbNotes.Location = New System.Drawing.Point(49, 133)
        Me.rtbNotes.Name = "rtbNotes"
        Me.rtbNotes.ReadOnly = True
        Me.rtbNotes.Size = New System.Drawing.Size(560, 500)
        Me.rtbNotes.TabIndex = 2
        Me.rtbNotes.Text = ""
        '
        'lblPrompt
        '
        Me.lblPrompt.AutoSize = True
        Me.lblPrompt.BackColor = System.Drawing.Color.Transparent
        Me.lblPrompt.Font = New System.Drawing.Font("Segoe UI", 12.0!)
        Me.lblPrompt.ForeColor = System.Drawing.Color.White
        Me.lblPrompt.Location = New System.Drawing.Point(660, 110)
        Me.lblPrompt.Name = "lblPrompt"
        Me.lblPrompt.Size = New System.Drawing.Size(112, 45)
        Me.lblPrompt.TabIndex = 3
        Me.lblPrompt.Text = "Label1"
        '
        'txtInput
        '
        Me.txtInput.Location = New System.Drawing.Point(660, 140)
        Me.txtInput.Name = "txtInput"
        Me.txtInput.Size = New System.Drawing.Size(480, 31)
        Me.txtInput.TabIndex = 4
        '
        'cboChoice
        '
        Me.cboChoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboChoice.FormattingEnabled = True
        Me.cboChoice.Location = New System.Drawing.Point(660, 140)
        Me.cboChoice.Name = "cboChoice"
        Me.cboChoice.Size = New System.Drawing.Size(480, 33)
        Me.cboChoice.TabIndex = 5
        '
        'btnExecute
        '
        Me.btnExecute.Location = New System.Drawing.Point(660, 550)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New System.Drawing.Size(220, 50)
        Me.btnExecute.TabIndex = 7
        Me.btnExecute.Text = "EXECUTE"
        Me.btnExecute.UseVisualStyleBackColor = True
        '
        'txtOutput
        '
        Me.txtOutput.BackColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(10, Byte), Integer), CType(CType(40, Byte), Integer))
        Me.txtOutput.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!)
        Me.txtOutput.ForeColor = System.Drawing.Color.White
        Me.txtOutput.Location = New System.Drawing.Point(660, 190)
        Me.txtOutput.Multiline = True
        Me.txtOutput.Name = "txtOutput"
        Me.txtOutput.ReadOnly = True
        Me.txtOutput.Size = New System.Drawing.Size(480, 340)
        Me.txtOutput.TabIndex = 6
        '
        'btnBack
        '
        Me.btnBack.Location = New System.Drawing.Point(920, 550)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(220, 50)
        Me.btnBack.TabIndex = 8
        Me.btnBack.Text = "CLOSE"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'OopLesson
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(12.0!, 25.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DupalcoOOP.My.Resources.Resources._41e3d16b2aaf512abf1fcceb5099656a
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(1639, 903)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnExecute)
        Me.Controls.Add(Me.txtOutput)
        Me.Controls.Add(Me.cboChoice)
        Me.Controls.Add(Me.txtInput)
        Me.Controls.Add(Me.lblPrompt)
        Me.Controls.Add(Me.rtbNotes)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "OopLesson"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OopLesson"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents rtbNotes As RichTextBox
    Friend WithEvents lblPrompt As Label
    Friend WithEvents txtInput As TextBox
    Friend WithEvents cboChoice As ComboBox
    Friend WithEvents btnExecute As Button
    Friend WithEvents txtOutput As TextBox
    Friend WithEvents btnBack As Button
End Class
