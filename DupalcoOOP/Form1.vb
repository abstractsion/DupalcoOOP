Public Class Form1

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.ClientSize = New Size(1200, 700)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.DoubleBuffered = True

        ' Put the SAME existing artwork into the animated backdrop
        If P5Backdrop1.BackgroundArt Is Nothing Then P5Backdrop1.BackgroundArt = Me.BackgroundImage
        Me.BackgroundImage = Nothing

        ' Move every other control onto the backdrop so the moving art shows behind them
        For Each c In Me.Controls.OfType(Of Control)().Where(Function(x) x IsNot P5Backdrop1).ToList()
            c.Parent = P5Backdrop1
        Next
    End Sub

    Private Sub P5Menu1_ItemClicked(text As String) Handles P5Menu1.ItemClicked

        If text = "HELP" OrElse text = "BSIT2E" OrElse text = "About" Then Return

        If text = "EXIT" Then
            Application.Exit()
            Return
        End If

        Select Case text
            Case "Classes and Objects", "Encapsulation", "Inheritance", "Polymorphism",
         "Interfaces", "Computer Programming and Translators",
         "What a Program is Made of", "Exploring the IDE", "Console Application",
         "Data Types And Arithmetic Operations", "Data Handling",
         "Variable Names", "Logical Operators",
         "Array Example", "Month Listbox",
         "Control Sample", "Text Properties Manipulator",
         "Excessive Controls"

                Dim o As New OopLessonTopics()
                o.LoadTopic(text)
                o.ShowDialog(Me)
                Return
        End Select

        Dim f As New LessonForm()
        f.LoadLesson(text)
        f.ShowDialog(Me)

    End Sub

    ' Hide the title plates while a menu section is open so the list stays readable
    Private Sub P5Menu1_SectionToggled(isOpen As Boolean) Handles P5Menu1.SectionToggled
        For Each c As Control In P5Backdrop1.Controls
            If c Is P5Menu1 Then Continue For
            c.Visible = Not isOpen
            If Not isOpen AndAlso TypeOf c Is P5Plate Then DirectCast(c, P5Plate).Replay()
        Next
    End Sub

    Private Sub P5Plate1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub P5Plate2_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub P5Plate4_Click(sender As Object, e As EventArgs) Handles P5Plate4.Click

    End Sub

    Private Sub P5Plate2_Click_1(sender As Object, e As EventArgs) Handles P5Plate2.Click

    End Sub

    Private Sub P5Backdrop1_Paint(sender As Object, e As PaintEventArgs) Handles P5Backdrop1.Paint

    End Sub
End Class