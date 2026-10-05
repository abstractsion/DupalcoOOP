Public Class LessonForm

    Public Sub LoadLesson(menuText As String)
        lblTitle.Text = menuText

        Dim n = menuText.Split("."c)(0)
        Dim fileName As String

        If IsNumeric(n) Then
            fileName = "Lesson" & n & ".txt"
        Else
            fileName = menuText.Replace(" ", "_") & ".txt"
        End If

        Dim path = IO.Path.Combine(Application.StartupPath, "..", "..", "Short", fileName)
        If IO.File.Exists(path) Then
            rtbNotes.Text = IO.File.ReadAllText(path)
        Else
            rtbNotes.Text = "Content coming soon."
        End If
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub LessonForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
