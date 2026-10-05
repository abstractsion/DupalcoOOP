Partial Public Class OopLessonTopics

    Private baseFont As Font
    Private probeCount As Integer

    Private ReadOnly newTopics() As String = {
        "Data Types And Arithmetic Operations", "Data Handling",
        "Variable Names", "Logical Operators",
        "Array Example", "Month Listbox",
        "Control Sample", "Text Properties Manipulator", "Excessive Controls"}

    Private ReadOnly months() As String = {"January", "February", "March", "April", "May", "June",
                                           "July", "August", "September", "October", "November", "December"}

    ' ---------- helpers called from OopLesson.vb ----------

    Private Function NotesFile(t As String) As String
        If Array.IndexOf(newTopics, t) >= 0 Then Return "Topic_" & t.Replace(" ", "") & ".txt"
        Return "Oop_" & t.Replace(" ", "") & ".txt"
    End Function

    Private Sub SetupNewTopic(t As String)
        If baseFont Is Nothing Then baseFont = txtOutput.Font

        Select Case t
            Case "Data Types And Arithmetic Operations"
                lblPrompt.Text = "Type two numbers separated by a comma (try 10, 4  or  7.5, 2):"
                txtInput.Text = "10, 4"
                txtInput.Visible = True
            Case "Data Handling"
                lblPrompt.Text = "Type your name and age, like  Ana, 20"
                txtInput.Text = "Ana, 20"
                txtInput.Visible = True
            Case "Variable Names"
                lblPrompt.Text = "Type a variable name to test (try 2ndNumber, first name, Dim):"
                txtInput.Text = "totalPrice"
                txtInput.Visible = True
            Case "Logical Operators"
                lblPrompt.Text = "Pick a logical operator:"
                cboChoice.Items.AddRange(New Object() {"And", "Or", "Not", "Xor", "AndAlso", "OrElse"})
                cboChoice.Visible = True
            Case "Array Example"
                lblPrompt.Text = "Type numbers separated by commas:"
                txtInput.Text = "5, 3, 9, 1, 7"
                txtInput.Visible = True
            Case "Month Listbox"
                lblPrompt.Text = "Pick a month:"
                cboChoice.Items.AddRange(months)
                cboChoice.Visible = True
            Case "Control Sample"
                lblPrompt.Text = "Pick a control:"
                cboChoice.Items.AddRange(New Object() {"Button", "Label", "TextBox", "CheckBox", "RadioButton", "ComboBox", "NumericUpDown"})
                cboChoice.Visible = True
            Case "Text Properties Manipulator"
                lblPrompt.Text = "Pick a change to apply to the output text:"
                cboChoice.Items.AddRange(New Object() {"Bold", "Italic", "Bold + Italic", "Bigger (16 pt)", "Yellow text", "Monospace font", "Reset"})
                cboChoice.Visible = True
            Case "Excessive Controls"
                lblPrompt.Text = "How many controls should we create?"
                cboChoice.Items.AddRange(New Object() {"10", "100", "500", "1000", "2000"})
                cboChoice.Visible = True
        End Select
    End Sub

    Private Sub RunNewTopic()
        Select Case topic
            Case "Data Types And Arithmetic Operations" : RunArithmetic()
            Case "Data Handling" : RunDataHandling()
            Case "Variable Names" : RunVarNames()
            Case "Logical Operators" : RunLogical()
            Case "Array Example" : RunArray()
            Case "Month Listbox" : RunMonth()
            Case "Control Sample" : RunControlSample()
            Case "Text Properties Manipulator" : RunTextProps()
            Case "Excessive Controls" : RunExcessive()
        End Select
    End Sub

    ' ---------- Lesson 5 ----------

    Private Sub RunArithmetic()
        Dim parts = txtInput.Text.Split(","c)
        Dim da, db As Double
        If parts.Length <> 2 OrElse Not Double.TryParse(parts(0).Trim(), da) OrElse Not Double.TryParse(parts(1).Trim(), db) Then
            txtOutput.Text = "Type two numbers separated by a comma, like  10, 4"
            Return
        End If

        Dim lines As New List(Of String)
        Dim ia, ib As Integer
        If Integer.TryParse(parts(0).Trim(), ia) AndAlso Integer.TryParse(parts(1).Trim(), ib) Then
            lines.Add("Both are whole numbers -> Integer")
            lines.Add("")
            lines.Add($"{ia} + {ib} = {ia + ib}")
            lines.Add($"{ia} - {ib} = {ia - ib}")
            lines.Add($"{ia} * {ib} = {ia * ib}")
            If ib = 0 Then
                lines.Add($"{ia} / 0 = Infinity  (\ and Mod would crash)")
            Else
                lines.Add($"{ia} / {ib} = {ia / ib}   (/ gives a Double)")
                lines.Add($"{ia} \ {ib} = {ia \ ib}   (integer divide)")
                lines.Add($"{ia} Mod {ib} = {ia Mod ib}   (remainder)")
            End If
        Else
            lines.Add("A number has a decimal part -> Double")
            lines.Add("(an Integer cannot store decimals)")
            lines.Add("")
            lines.Add($"{da} + {db} = {da + db}")
            lines.Add($"{da} - {db} = {da - db}")
            lines.Add($"{da} * {db} = {da * db}")
            If db = 0 Then
                lines.Add($"{da} / 0 = Infinity")
            Else
                lines.Add($"{da} / {db} = {da / db}")
            End If
        End If

        Dim ma, mb As Decimal
        If Decimal.TryParse(parts(0).Trim(), ma) AndAlso Decimal.TryParse(parts(1).Trim(), mb) Then
            lines.Add("")
            lines.Add($"As Decimal (exact, good for money): {ma + mb}")
        End If
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunDataHandling()
        Dim parts = txtInput.Text.Split(","c)
        Dim age As Integer
        If parts.Length <> 2 OrElse Not Integer.TryParse(parts(1).Trim(), age) Then
            txtOutput.Text = "Type a name, a comma, then a whole-number age." & vbCrLf & "Example:  Ana, 20"
            Return
        End If
        Dim nm = parts(0).Trim()
        Dim isAdult As Boolean = age >= 18

        Dim lines As New List(Of String)
        lines.Add("INPUT  (a TextBox always gives a String):")
        lines.Add("  " & txtInput.Text)
        lines.Add("")
        lines.Add("STORED in variables:")
        lines.Add($"  name  As {nm.GetType().Name} = {nm}")
        lines.Add($"  age   As {age.GetType().Name} = {age}")
        lines.Add("")
        lines.Add("PROCESSED:")
        lines.Add($"  age + 1 = {age + 1}   (next year)")
        lines.Add($"  age >= 18 = {isAdult}   ({isAdult.GetType().Name})")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    ' ---------- Lesson 6 ----------

    Private Sub RunVarNames()
        Dim s = txtInput.Text.Trim()
        If s = "" Then
            txtOutput.Text = "Type a variable name first."
            Return
        End If

        Dim reservedWords = New String() {"dim", "as", "if", "then", "else", "end", "const", "for", "next", "while",
            "do", "loop", "select", "case", "sub", "function", "new", "class", "integer", "string", "decimal",
            "double", "single", "boolean", "long", "short", "date", "true", "false", "return", "me", "public", "private"}

        Dim problems As New List(Of String)
        If Not (Char.IsLetter(s(0)) OrElse s(0) = "_"c) Then problems.Add("It must start with a letter or underscore.")
        If s.Contains(" ") Then problems.Add("It contains a space.")
        If System.Text.RegularExpressions.Regex.IsMatch(s, "[^A-Za-z0-9_ ]") Then problems.Add("It has a symbol. Only letters, digits and _ are allowed.")
        If Array.IndexOf(reservedWords, s.ToLower()) >= 0 Then problems.Add("It is a reserved word.")

        Dim lines As New List(Of String)
        If problems.Count = 0 Then
            lines.Add($"'{s}' is a VALID name.")
            lines.Add("")
            If Char.IsUpper(s(0)) Then lines.Add("Tip: variables usually start lowercase (camelCase).")
            If s.Length = 1 Then lines.Add("Tip: use a meaningful name, not one letter.")
            lines.Add("Examples:")
            lines.Add("  Dim totalPrice As Decimal")
            lines.Add("  Const TAX_RATE As Double = 0.12")
            lines.Add("  txtName, lblResult, btnSave   (controls)")
        Else
            lines.Add($"'{s}' is NOT a valid name:")
            For Each p As String In problems
                lines.Add("  - " & p)
            Next
        End If
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Function Probe(v As Boolean) As Boolean
        probeCount += 1
        Return v
    End Function

    Private Sub RunLogical()
        Dim op = cboChoice.Text
        Dim lines As New List(Of String)
        Dim tf = New Boolean() {True, False}

        If op = "Not" Then
            lines.Add("A        Not A")
            For Each a As Boolean In tf
                lines.Add(a.ToString().PadRight(9) & (Not a).ToString())
            Next
            lines.Add("")
            lines.Add("Not flips True to False and False to True.")
            txtOutput.Text = String.Join(vbCrLf, lines)
            Return
        End If

        lines.Add("A        B        A " & op & " B")
        For Each a As Boolean In tf
            For Each b As Boolean In tf
                Dim r As Boolean
                Dim note As String = ""
                probeCount = 0
                Select Case op
                    Case "And" : r = a And b
                    Case "Or" : r = a Or b
                    Case "Xor" : r = a Xor b
                    Case "AndAlso" : r = a AndAlso Probe(b)
                    Case Else : r = a OrElse Probe(b)
                End Select
                If op = "AndAlso" OrElse op = "OrElse" Then
                    note = If(probeCount > 0, "  B checked", "  B skipped")
                End If
                lines.Add(a.ToString().PadRight(9) & b.ToString().PadRight(9) & r.ToString() & note)
            Next
        Next

        lines.Add("")
        Select Case op
            Case "And" : lines.Add("True only if BOTH are True.")
            Case "Or" : lines.Add("True if AT LEAST ONE is True.")
            Case "Xor" : lines.Add("True only if they are DIFFERENT.")
            Case "AndAlso" : lines.Add("If A is False, B is never checked.")
            Case Else : lines.Add("If A is True, B is never checked.")
        End Select
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    ' ---------- Lesson 7 ----------

    Private Sub RunArray()
        Dim parts() As String = txtInput.Text.Split(","c)
        Dim nums(parts.Length - 1) As Integer
        For i As Integer = 0 To parts.Length - 1
            If Not Integer.TryParse(parts(i).Trim(), nums(i)) Then
                txtOutput.Text = "'" & parts(i).Trim() & "' is not a whole number." & vbCrLf & "Example:  5, 3, 9, 1, 7"
                Return
            End If
        Next

        Dim lines As New List(Of String)
        lines.Add($"Length = {nums.Length}   (indexes 0 to {nums.Length - 1})")
        For i As Integer = 0 To nums.Length - 1
            lines.Add($"  nums({i}) = {nums(i)}")
        Next

        Dim sum As Integer = 0
        Dim max As Integer = nums(0)
        Dim min As Integer = nums(0)
        For Each n As Integer In nums
            sum += n
            If n > max Then max = n
            If n < min Then min = n
        Next
        lines.Add("")
        lines.Add($"Sum = {sum}   Average = {sum / nums.Length:0.00}")
        lines.Add($"Max = {max}   Min = {min}")
        Array.Sort(nums)
        lines.Add("Sorted: " & String.Join(", ", nums))
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunMonth()
        Dim i As Integer = cboChoice.SelectedIndex
        Dim lines As New List(Of String)
        lines.Add("Dim months() As String = {""January"", ..., ""December""}")
        lines.Add("")
        lines.Add($"SelectedIndex : {i}")
        lines.Add($"months({i}) = {months(i)}")
        lines.Add($"Month number  : {i + 1}   (index + 1)")
        lines.Add($"Days in 2026  : {Date.DaysInMonth(2026, i + 1)}")
        lines.Add($"Quarter       : Q{i \ 3 + 1}")
        lines.Add($"Items in list : {cboChoice.Items.Count}")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    ' ---------- Lesson 8 ----------

    Private Sub RunControlSample()
        Dim lines As New List(Of String)
        Select Case cboChoice.Text
            Case "Button"
                lines.Add("BUTTON")
                lines.Add("Properties: Text, Enabled, BackColor, Visible")
                lines.Add("Event: Click")
                lines.Add("")
                lines.Add("btnSave.Text = ""Save""")
                lines.Add("btnSave.Enabled = False")
            Case "Label"
                lines.Add("LABEL")
                lines.Add("Properties: Text, Font, ForeColor, AutoSize")
                lines.Add("Shows text. The user cannot edit it.")
                lines.Add("")
                lines.Add("lblResult.Text = ""Total: "" & total")
            Case "TextBox"
                lines.Add("TEXTBOX")
                lines.Add("Properties: Text, MaxLength, Multiline, ReadOnly")
                lines.Add("Event: TextChanged")
                lines.Add("")
                lines.Add("Dim name As String = txtName.Text")
                lines.Add("txtName.Clear()")
            Case "CheckBox"
                lines.Add("CHECKBOX")
                lines.Add("Properties: Text, Checked (True/False)")
                lines.Add("Event: CheckedChanged")
                lines.Add("")
                lines.Add("If chkNews.Checked Then")
                lines.Add("    lblResult.Text = ""Subscribed""")
                lines.Add("End If")
            Case "RadioButton"
                lines.Add("RADIOBUTTON")
                lines.Add("Properties: Text, Checked")
                lines.Add("Only ONE in the same container can be checked.")
                lines.Add("")
                lines.Add("If rbAdd.Checked Then")
                lines.Add("    result = a + b")
                lines.Add("End If")
            Case "ComboBox"
                lines.Add("COMBOBOX")
                lines.Add("Properties: Items, SelectedIndex, SelectedItem, Text")
                lines.Add("Event: SelectedIndexChanged")
                lines.Add("")
                lines.Add("cboColor.Items.Add(""Red"")")
                lines.Add("Dim c = cboColor.SelectedItem")
            Case Else
                lines.Add("NUMERICUPDOWN")
                lines.Add("Properties: Minimum, Maximum, Value, Increment")
                lines.Add("Event: ValueChanged")
                lines.Add("")
                lines.Add("nudQty.Maximum = 100")
                lines.Add("Dim qty As Integer = CInt(nudQty.Value)")
        End Select
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunTextProps()
        txtOutput.Font = baseFont
        txtOutput.ForeColor = Color.White

        Dim code As String
        Select Case cboChoice.Text
            Case "Bold"
                txtOutput.Font = New Font(baseFont, FontStyle.Bold)
                code = "txtOutput.Font = New Font(txtOutput.Font, FontStyle.Bold)"
            Case "Italic"
                txtOutput.Font = New Font(baseFont, FontStyle.Italic)
                code = "txtOutput.Font = New Font(txtOutput.Font, FontStyle.Italic)"
            Case "Bold + Italic"
                txtOutput.Font = New Font(baseFont, FontStyle.Bold Or FontStyle.Italic)
                code = "New Font(f, FontStyle.Bold Or FontStyle.Italic)"
            Case "Bigger (16 pt)"
                txtOutput.Font = New Font(baseFont.FontFamily, 16)
                code = "txtOutput.Font = New Font(f.FontFamily, 16)"
            Case "Yellow text"
                txtOutput.ForeColor = Color.Yellow
                code = "txtOutput.ForeColor = Color.Yellow"
            Case "Monospace font"
                txtOutput.Font = New Font("Consolas", baseFont.Size)
                code = "txtOutput.Font = New Font(""Consolas"", 9)"
            Case Else
                code = "(back to the original Font and ForeColor)"
        End Select

        txtOutput.Text = "This sentence is the Text property." & vbCrLf & vbCrLf &
                         "You just ran:" & vbCrLf & code & vbCrLf & vbCrLf &
                         "Length = " & txtOutput.Text.Length & "  ToUpper = THIS SENTENCE..."
    End Sub

    Private Sub RunExcessive()
        Dim n As Integer = CInt(cboChoice.Text)
        Me.Cursor = Cursors.WaitCursor

        Dim sw = System.Diagnostics.Stopwatch.StartNew()
        Dim holder As New Panel()
        For i As Integer = 1 To n
            holder.Controls.Add(New Button With {.Text = "Btn " & i})
        Next
        For Each c As Control In holder.Controls
            Dim h = c.Handle        ' forces Windows to create the real control
        Next
        sw.Stop()
        Dim msButtons = sw.ElapsedMilliseconds
        holder.Dispose()

        sw.Restart()
        Dim lb As New ListBox()
        For i As Integer = 1 To n
            lb.Items.Add("Item " & i)
        Next
        sw.Stop()
        Dim msItems = sw.ElapsedMilliseconds
        lb.Dispose()

        Me.Cursor = Cursors.Default

        Dim lines As New List(Of String)
        lines.Add($"{n} Buttons (each a real window): {msButtons} ms")
        lines.Add($"{n} ListBox items (just strings): {msItems} ms")
        lines.Add("")
        lines.Add("Every control uses a Windows handle.")
        lines.Add("A process gets about 10,000 of them,")
        lines.Add("and the form gets slower as you add more.")
        lines.Add("")
        lines.Add("Use a ListBox / ListView / DataGridView")
        lines.Add("instead of hundreds of separate controls.")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub



    Private Sub OopLesson_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
