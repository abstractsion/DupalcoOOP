<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.P5Menu1 = New DupalcoOOP.P5Menu()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'P5Menu1
        '
        Me.P5Menu1.AccentColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.P5Menu1.BackColor = System.Drawing.Color.Transparent
        Me.P5Menu1.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.P5Menu1.Font = New System.Drawing.Font("Segoe UI", 11.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle))
        Me.P5Menu1.Location = New System.Drawing.Point(-10, 12)
        Me.P5Menu1.MenuText = resources.GetString("P5Menu1.MenuText")
        Me.P5Menu1.Name = "P5Menu1"
        Me.P5Menu1.Size = New System.Drawing.Size(380, 460)
        Me.P5Menu1.TabIndex = 0
        Me.P5Menu1.Text = "P5Menu1"
        Me.P5Menu1.UseWaitCursor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Bauhaus 93", 24.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(12, 282)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(445, 36)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Object Oriented Programming"
        Me.Label2.UseWaitCursor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 14.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(150, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(135, 447)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(168, 25)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "Dupalco, Justin C."
        Me.Label3.UseWaitCursor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 24.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(170, 237)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(133, 45)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "PF-101 "
        Me.Label1.UseWaitCursor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Bauhaus 93", 24.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.White
        Me.Label4.Location = New System.Drawing.Point(158, 402)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(112, 36)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "SBIT2E"
        Me.Label4.UseWaitCursor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Persona 5 Menu Font Prototype", 48.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Label5.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Label5.Location = New System.Drawing.Point(98, 208)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(263, 64)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "PF-101 "
        Me.Label5.UseWaitCursor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImage = Global.DupalcoOOP.My.Resources.Resources._41e3d16b2aaf512abf1fcceb5099656a
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(686, 464)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.P5Menu1)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.UseWaitCursor = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents P5Menu1 As P5Menu
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
End Class
