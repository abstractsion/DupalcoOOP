Partial Public Class OopLessonTopics

    Private topic As String
    Private bank As New BankAccount()

    ' Form1 calls this with the topic name from the menu
    Public Sub LoadTopic(t As String)
        topic = t
        lblTitle.Text = t

        Dim path = IO.Path.Combine(Application.StartupPath, "..", "..", "Short", NotesFile(t))
        If IO.File.Exists(path) Then
            rtbNotes.Text = IO.File.ReadAllText(path)
        Else
            rtbNotes.Text = "Notes file not found:" & vbCrLf & path
        End If

        txtInput.Visible = False
        cboChoice.Visible = False
        cboChoice.Items.Clear()
        txtInput.Clear()
        txtOutput.Text = "Click EXECUTE to run the demo."
        txtInput.MaxLength = 32767
        txtOutput.BackColor = Color.FromArgb(10, 10, 40)
        txtOutput.ForeColor = Color.White

        Select Case t
            Case "Classes and Objects"
                lblPrompt.Text = "Type a student name:"
                txtInput.Visible = True
            Case "Encapsulation"
                lblPrompt.Text = "Deposit amount (try 500, then -50):"
                txtInput.Visible = True
            Case "Inheritance"
                lblPrompt.Text = "Pick an animal:"
                cboChoice.Items.AddRange(New Object() {"Animal", "Dog", "Cat"})
                cboChoice.Visible = True
            Case "Polymorphism"
                lblPrompt.Text = "Pick an animal:"
                cboChoice.Items.AddRange(New Object() {"Dog", "Cat", "Animal"})
                cboChoice.Visible = True
            Case "Interfaces"
                lblPrompt.Text = "Pick something to print:"
                cboChoice.Items.AddRange(New Object() {"Document", "Photo", "Report"})
                cboChoice.Visible = True
            Case "Computer Programming and Translators"
                lblPrompt.Text = "Type a word (max 8 letters):"
                txtInput.MaxLength = 8
                txtInput.Visible = True
            Case "What a Program is Made Of"
                lblPrompt.Text = "Type a declaration, like  Dim age As Integer"
                txtInput.Visible = True
            Case "Exploring the IDE"
                lblPrompt.Text = "Pick an IDE window:"
                cboChoice.Items.AddRange(New Object() {"Toolbox", "Solution Explorer", "Properties Window", "Form Designer", "Error List", "Output Window"})
                cboChoice.Visible = True
            Case "Console Application"
                lblPrompt.Text = "Type your name:"
                txtInput.Visible = True
                txtOutput.BackColor = Color.Black
                txtOutput.ForeColor = Color.LightGray
            Case "Data Types And Arithmetic Operations",
     "Data Handling",
     "Variable Names",
     "Logical Operators",
     "Array Example",
     "Month Listbox",
     "Control Sample",
     "Text Properties Manipulator",
     "Excessive Controls"

                SetupNewTopic(t)
        End Select

        If cboChoice.Visible Then cboChoice.SelectedIndex = 0
    End Sub

    Private Sub btnExecute_Click(sender As Object, e As EventArgs) Handles btnExecute.Click

        Select Case topic

            Case "Data Types And Arithmetic Operations",
             "Data Handling",
             "Variable Names",
             "Logical Operators",
             "Array Example",
             "Month Listbox",
             "Control Sample",
             "Text Properties Manipulator",
             "Excessive Controls"

                RunNewTopic()
                Return

            Case "Classes and Objects" : RunClasses()
            Case "Encapsulation" : RunEncapsulation()
            Case "Inheritance" : RunInheritance()
            Case "Polymorphism" : RunPolymorphism()
            Case "Interfaces" : RunInterfaces()
            Case "Computer Programming and Translators" : RunTranslators()
            Case "What a Program is Made Of" : RunProgramParts()
            Case "Exploring the IDE" : RunIDE()
            Case "Console Application" : RunConsole()

        End Select

    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    ' ---------- demos ----------

    Private Sub RunClasses()
        Dim nm = txtInput.Text.Trim()
        If nm = "" Then nm = "Ana"

        Dim s1 As New Student()
        s1.Name = nm
        s1.Grade = 85

        Dim s2 As New Student()
        s2.Name = "Ben"
        s2.Grade = 80

        s1.Improve(5)

        Dim lines As New List(Of String)
        lines.Add("Made 2 objects from the Student class.")
        lines.Add("")
        lines.Add(s1.Name & " : " & s1.Grade & "   (after Improve(5))")
        lines.Add(s2.Name & " : " & s2.Grade & "   (unchanged)")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunEncapsulation()
        Dim amt As Decimal
        If Not Decimal.TryParse(txtInput.Text, amt) Then
            txtOutput.Text = "Type a number first, like 500 or -50."
            Return
        End If

        Dim lines As New List(Of String)
        If bank.Deposit(amt) Then
            lines.Add("Deposit accepted: " & amt)
        Else
            lines.Add("Deposit REJECTED: the amount must be more than 0.")
        End If
        lines.Add("Balance (from GetBalance): " & bank.GetBalance())
        lines.Add("")
        lines.Add("Writing bank.balance = 999 directly would be an error,")
        lines.Add("because balance is Private.")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunInheritance()
        Dim a As Animal = MakeAnimal(cboChoice.Text)

        Dim lines As New List(Of String)
        lines.Add("Object type: " & a.GetType().Name)
        lines.Add("Eat():   " & a.Eat() & "   (inherited from Animal)")
        lines.Add("Speak(): " & a.Speak())
        If TypeOf a Is Dog Then
            lines.Add("Fetch(): " & DirectCast(a, Dog).Fetch() & "   (only Dog has this)")
        End If
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunPolymorphism()
        Dim a As Animal = MakeAnimal(cboChoice.Text)
        Dim helper As New MathHelper()

        Dim lines As New List(Of String)
        lines.Add("Dim a As Animal = New " & a.GetType().Name & "()")
        lines.Add("a.Speak()  ->  " & a.Speak())
        lines.Add("")
        lines.Add("Same call on every animal (overriding):")
        For Each x As Animal In New Animal() {New Dog(), New Cat(), New Animal()}
            lines.Add("  " & x.GetType().Name & ".Speak()  ->  " & x.Speak())
        Next
        lines.Add("")
        lines.Add("Same name, different parameters (overloading):")
        lines.Add("  Add(2, 3)      ->  " & helper.Add(2, 3))
        lines.Add("  Add(2.5, 1.5)  ->  " & helper.Add(2.5, 1.5))
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunInterfaces()
        Dim p As IPrintable
        Select Case cboChoice.Text
            Case "Photo" : p = New Photo()
            Case "Report" : p = New Report()
            Case Else : p = New Document()
        End Select

        Dim lines As New List(Of String)
        lines.Add("Dim p As IPrintable = New " & p.GetType().Name & "()")
        lines.Add("p.Print()  ->  " & p.Print())
        lines.Add("")
        lines.Add("Everything that implements IPrintable prints the same way:")
        Dim items As IPrintable() = {New Document(), New Photo(), New Report()}
        For Each item In items
            lines.Add("  " & item.Print())
        Next
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunTranslators()
        Dim w = txtInput.Text.Trim()
        If w = "" Then
            txtOutput.Text = "Type a word first (up to 8 letters)."
            Return
        End If
        If w.Length > 8 Then w = w.Substring(0, 8)

        Dim lines As New List(Of String)
        lines.Add("Source code (words): " & w)
        lines.Add("")
        lines.Add("Machine language (1's and 0's):")
        For Each ch As Char In w
            lines.Add("  " & ch & "  =  " & Convert.ToString(AscW(ch) And 255, 2).PadLeft(8, "0"c))
        Next
        lines.Add("")
        lines.Add("A translator (compiler) does this work for you.")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunProgramParts()
        Dim s = txtInput.Text.Trim()
        If s = "" Then s = "Dim age As Integer"

        Dim lines As New List(Of String)
        lines.Add("Statement: " & s)
        lines.Add("")

        Dim m = System.Text.RegularExpressions.Regex.Match(s, "^Dim\s+(\S+)\s+As\s+(\S+)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        If Not m.Success Then
            lines.Add("SYNTAX ERROR: it does not follow the pattern")
            lines.Add("    Dim <name> As <DataType>")
            lines.Add("The program would not compile.")
            txtOutput.Text = String.Join(vbCrLf, lines)
            Return
        End If

        Dim nm = m.Groups(1).Value
        Dim tp = m.Groups(2).Value
        Dim reserved = New String() {"dim", "as", "if", "then", "else", "end", "const", "print", "let", "for", "next", "while", "do", "loop", "select", "case", "sub", "function", "new", "class", "integer", "string", "decimal", "double", "single", "boolean", "long", "short"}
        Dim types = New String() {"integer", "string", "decimal", "double", "single", "boolean", "long", "short", "date"}

        If Not System.Text.RegularExpressions.Regex.IsMatch(nm, "^[A-Za-z][A-Za-z0-9_]*$") Then
            lines.Add("NAMING ERROR: '" & nm & "' is not a valid name.")
            lines.Add("A name must start with a letter and use only")
            lines.Add("letters, digits and underscore.")
        ElseIf Array.IndexOf(reserved, nm.ToLower()) >= 0 Then
            lines.Add("NAMING ERROR: '" & nm & "' is a reserved word.")
            lines.Add("Pick a different name.")
        ElseIf Array.IndexOf(types, tp.ToLower()) < 0 Then
            lines.Add("SYNTAX ERROR: '" & tp & "' is not a data type.")
            lines.Add("Try Integer, String, Decimal, Double or Boolean.")
        Else
            lines.Add("Keyword  : Dim")
            lines.Add("Variable : " & nm)
            lines.Add("Keyword  : As")
            lines.Add("Keyword  : " & tp & "  (the data type)")
            lines.Add("")
            lines.Add("No syntax errors. The program would compile.")
        End If
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunIDE()
        Dim what As String, how As String
        Select Case cboChoice.Text
            Case "Toolbox"
                what = "Holds the controls (Button, Label, TextBox...) you drag onto a form."
                how = "View > Toolbox"
            Case "Solution Explorer"
                what = "Shows the files in your project. Open, add or delete them here."
                how = "View > Solution Explorer"
            Case "Properties Window"
                what = "Shows the settings of the selected control so you can change them."
                how = "View > Properties Window (or press F4)"
            Case "Form Designer"
                what = "The picture of your form. Drag controls onto it and place them."
                how = "Double-click a form in Solution Explorer"
            Case "Error List"
                what = "Lists the errors and warnings found when you build."
                how = "View > Error List"
            Case Else
                what = "Shows status messages from building and running."
                how = "View > Output (or press Ctrl+Alt+O)"
        End Select

        Dim lines As New List(Of String)
        lines.Add(cboChoice.Text)
        lines.Add("")
        lines.Add("What it does:")
        lines.Add("  " & what)
        lines.Add("")
        lines.Add("How to open it:")
        lines.Add("  " & how)
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Sub RunConsole()
        Dim nm = txtInput.Text.Trim()
        If nm = "" Then nm = "Ana"

        Dim lines As New List(Of String)
        lines.Add("Enter your name: " & nm)
        lines.Add("Hello, " & nm & "!")
        lines.Add("")
        lines.Add("Press any key to continue . . .")
        txtOutput.Text = String.Join(vbCrLf, lines)
    End Sub

    Private Function MakeAnimal(kind As String) As Animal
        Select Case kind
            Case "Dog" : Return New Dog()
            Case "Cat" : Return New Cat()
            Case Else : Return New Animal()
        End Select
    End Function

End Class

' ---------- classes used by the demos ----------

Public Class Student
    Public Name As String
    Public Grade As Integer

    Public Sub Improve(points As Integer)
        Grade += points
    End Sub
End Class

Public Class BankAccount
    Private balance As Decimal

    Public Function Deposit(amount As Decimal) As Boolean
        If amount > 0 Then
            balance += amount
            Return True
        End If
        Return False
    End Function

    Public Function GetBalance() As Decimal
        Return balance
    End Function
End Class

Public Class Animal
    Public Function Eat() As String
        Return "eating..."
    End Function

    Public Overridable Function Speak() As String
        Return "Animal speaks"
    End Function
End Class

Public Class Dog
    Inherits Animal

    Public Overrides Function Speak() As String
        Return "Dog barks"
    End Function

    Public Function Fetch() As String
        Return "fetching the ball"
    End Function
End Class

Public Class Cat
    Inherits Animal

    Public Overrides Function Speak() As String
        Return "Cat meows"
    End Function
End Class

Public Class MathHelper
    Public Function Add(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    Public Function Add(a As Double, b As Double) As Double
        Return a + b
    End Function
End Class

Public Interface IPrintable
    Function Print() As String
End Interface

Public Class Document
    Implements IPrintable

    Public Function Print() As String Implements IPrintable.Print
        Return "Printing Document..."
    End Function
End Class

Public Class Photo
    Implements IPrintable

    Public Function Print() As String Implements IPrintable.Print
        Return "Printing Photo..."
    End Function
End Class

Public Class Report
    Implements IPrintable

    Public Function Print() As String Implements IPrintable.Print
        Return "Printing Report..."
    End Function
End Class