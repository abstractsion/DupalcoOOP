Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub P5Menu1_ItemClicked(text As String) Handles P5Menu1.ItemClicked
        If text = "EXIT" OrElse text.StartsWith("21.") Then
            Application.Exit()
        Else
            MessageBox.Show("You clicked: " & text)
        End If
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub P5Menu1_SectionToggled(isOpen As Boolean) Handles P5Menu1.SectionToggled
        For Each c As Control In Me.Controls
            If TypeOf c Is Label OrElse TypeOf c Is LiveLabel Then
                c.Visible = Not isOpen
                If Not isOpen AndAlso TypeOf c Is LiveLabel Then DirectCast(c, LiveLabel).Replay()
            End If
        Next
    End Sub
End Class