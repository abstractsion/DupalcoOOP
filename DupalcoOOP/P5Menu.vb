Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' Persona-5 style animated menu.
'''   no dash = top-level item   (LESSONS)
'''   -       = inside top-level (- 1. Orientations)
'''   --      = inside that      (-- Topic 1)
''' </summary>
<DefaultEvent("ItemClicked")>
Public Class P5Menu
    Inherits Control

    Private Class Node
        Public Text As String
        Public Level As Integer
        Public Children As New List(Of Node)
        Public Parent As Node
        Public Expanded As Boolean
        Public H As Single = 0.0F   ' hover amount 0..1 (animated)
        Public A As Single = 1.0F   ' appear amount; negative = waiting to start
    End Class

    <Category("Persona Menu"), Description("Fires when an item with no sub-items is clicked. Gives you its text.")>
    Public Event ItemClicked(text As String)
    <Category("Persona Menu"), Description("Fires when a section opens or closes. True = a section is open.")>
    Public Event SectionToggled(isOpen As Boolean)

    Private Const DefaultText As String =
        "LESSONS" & vbLf &
        "- 1. Orientations" & vbLf &
        "- 2. Introduction to OOP" & vbLf & "-- Topic 1" & vbLf & "-- Topic 2" & vbLf & "-- Topic 3" & vbLf &
        "- 3. Getting Started with Microsoft Visual Basic .NET" & vbLf &
        "- 4. Planning Applications and Designing Interfaces" & vbLf &
        "- 5. Data Handling" & vbLf & "-- Topic 1" & vbLf & "-- Topic 2" & vbLf &
        "- 6. Coding With Variables Name Constants and Calculations" & vbLf & "-- Topic 1" & vbLf & "-- Topic 2" & vbLf &
        "- 7. Arrays" & vbLf & "-- Topic 1" & vbLf & "-- Topic 2" & vbLf &
        "- 8. Working With Controls and Properties" & vbLf &
        "- 9. Midterm Examinations" & vbLf &
        "- 10. Debugging and Tracing" & vbLf &
        "- 11. Working with .NET Framework and MDI" & vbLf &
        "- 12. Working with .NET Framework and MDI" & vbLf &
        "- 13. Database Connection" & vbLf &
        "- 14. Developing Data Driven Application" & vbLf &
        "- 15. Developing Data Driven Application" & vbLf &
        "- 16. Presentation" & vbLf &
        "- 17. Presentation" & vbLf &
        "- 18. Final Examination" & vbLf &
        "- 19. Animation" & vbLf & "-- Topic 1" & vbLf & "-- Topic 2" & vbLf &
        "- 20. Data Driven" & vbLf &
        "- 21. Exit" & vbLf &
        "HELP" & vbLf &
        "- About" & vbLf &
        "BSIT2E" & vbLf &
        "EXIT"

    Private ReadOnly roots As New List(Of Node)
    Private ReadOnly rows As New List(Of Node)
    Private _menuText As String = DefaultText
    Private _rowH As Integer = 32
    Private _accent As Color = Color.FromArgb(30, 30, 255)
    Private _idleBar As Boolean = True
    Private _stagger As Boolean = True
    Private hover As Integer = -1
    Private scrollY As Integer = 0

    ' animation
    Private ReadOnly tmr As New System.Windows.Forms.Timer() With {.Interval = 15}
    Private ReadOnly clock As Stopwatch = Stopwatch.StartNew()
    Private lastT As Single = 0.0F

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or
                 ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        Font = New Font("Segoe UI", 12, FontStyle.Bold Or FontStyle.Italic)
        Cursor = Cursors.Hand
        AddHandler tmr.Tick, AddressOf OnTick
        ParseText()
    End Sub

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(420, 300)
        End Get
    End Property

    <Category("Persona Menu"),
     Description("One item per line. No dash = top level. A dash (-) = inside the item above. Two dashes (--) = one level deeper."),
     Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", GetType(Drawing.Design.UITypeEditor))>
    Public Property MenuText As String
        Get
            Return _menuText
        End Get
        Set(value As String)
            _menuText = If(value, "")
            ParseText()
        End Set
    End Property

    <Category("Persona Menu"), Description("Height of each row in pixels."), DefaultValue(32)>
    Public Property RowHeight As Integer
        Get
            Return _rowH
        End Get
        Set(value As Integer)
            _rowH = Math.Max(16, value)
            Rebuild()
        End Set
    End Property

    <Category("Persona Menu"), Description("Color of the bars, shadows and glow.")>
    Public Property AccentColor As Color
        Get
            Return _accent
        End Get
        Set(value As Color)
            _accent = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Menu"), Description("Show a slanted bar behind items when not hovered."), DefaultValue(True)>
    Public Property ShowIdleBar As Boolean
        Get
            Return _idleBar
        End Get
        Set(value As Boolean)
            _idleBar = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Menu"), Description("Shift every other row sideways for a zigzag look."), DefaultValue(True)>
    Public Property Stagger As Boolean
        Get
            Return _stagger
        End Get
        Set(value As Boolean)
            _stagger = value
            Invalidate()
        End Set
    End Property

    ' ---------------- parsing / layout ----------------

    Private Sub ParseText()
        roots.Clear()
        Dim lastAt As New Dictionary(Of Integer, Node)
        Dim lines = _menuText.Replace(vbCrLf, vbLf).Split({vbLf(0)}, StringSplitOptions.RemoveEmptyEntries)

        For Each raw In lines
            Dim t = raw.Trim()
            If t.Length = 0 Then Continue For

            Dim lvl = 0
            While lvl < t.Length AndAlso t(lvl) = "-"c
                lvl += 1
            End While
            Dim text = t.Substring(lvl).Trim()
            If text.Length = 0 Then Continue For

            While lvl > 0 AndAlso Not lastAt.ContainsKey(lvl - 1)
                lvl -= 1
            End While

            Dim n As New Node With {.Text = text, .Level = lvl}
            If lvl = 0 Then
                roots.Add(n)
            Else
                n.Parent = lastAt(lvl - 1)
                n.Parent.Children.Add(n)
            End If

            lastAt(lvl) = n
            For Each k In lastAt.Keys.Where(Function(x) x > lvl).ToList()
                lastAt.Remove(k)
            Next
        Next
        Rebuild()
    End Sub

    Private Sub Rebuild()
        rows.Clear()
        AddRows(roots)
        ClampScroll()
        Invalidate()
    End Sub

    Private Sub AddRows(list As List(Of Node))
        For Each n In list
            rows.Add(n)
            If n.Expanded Then AddRows(n.Children)
        Next
    End Sub

    Private Sub ClampScroll()
        Dim maxScroll = Math.Max(0, rows.Count * _rowH - Height)
        scrollY = Math.Max(0, Math.Min(scrollY, maxScroll))
    End Sub

    ' ---------------- animation ----------------

    Private Sub StartAnim()
        If DesignMode Then Return
        If Not tmr.Enabled Then
            lastT = clock.ElapsedMilliseconds / 1000.0F
            tmr.Start()
        End If
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        Dim now = clock.ElapsedMilliseconds / 1000.0F
        Dim dt = Math.Min(0.05F, now - lastT)
        lastT = now

        Dim changed = False
        For i = 0 To rows.Count - 1
            Dim n = rows(i)
            Dim target = If(i = hover, 1.0F, 0.0F)
            If n.H <> target Then
                If n.H < target Then
                    n.H = Math.Min(target, n.H + dt / 0.14F)
                Else
                    n.H = Math.Max(target, n.H - dt / 0.14F)
                End If
                changed = True
            End If
            If n.A < 1.0F Then
                n.A = Math.Min(1.0F, n.A + dt / 0.28F)
                changed = True
            End If
        Next

        If changed Then Invalidate() Else tmr.Stop()
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not DesignMode Then
            ' top-level items slide in one after another when the app starts
            For k = 0 To roots.Count - 1
                roots(k).A = -0.2F * k
            Next
            StartAnim()
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then tmr.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Shared Function Mix(a As Color, b As Color, k As Single) As Color
        k = Math.Max(0.0F, Math.Min(1.0F, k))
        Dim rr = CInt(a.R) + (CInt(b.R) - CInt(a.R)) * k
        Dim gg = CInt(a.G) + (CInt(b.G) - CInt(a.G)) * k
        Dim bb = CInt(a.B) + (CInt(b.B) - CInt(a.B)) * k
        Return Color.FromArgb(CInt(rr), CInt(gg), CInt(bb))
    End Function

    ' ---------------- painting ----------------

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit

        Dim f0 As New Font(Font.FontFamily, Font.Size + 3, Font.Style)
        Dim f1 As New Font(Font.FontFamily, Font.Size, Font.Style)
        Dim f2 As New Font(Font.FontFamily, Math.Max(8, Font.Size - 1), FontStyle.Italic)
        Dim sf As New StringFormat With {
            .LineAlignment = StringAlignment.Center,
            .Trimming = StringTrimming.EllipsisCharacter,
            .FormatFlags = StringFormatFlags.NoWrap
        }

        For i = 0 To rows.Count - 1
            Dim n = rows(i)
            Dim y = i * _rowH - scrollY
            If y + _rowH < 0 OrElse y > Height Then Continue For

            Dim aRaw = Math.Max(0.0F, Math.Min(1.0F, n.A))
            If aRaw <= 0.0F Then Continue For
            Dim ea = 1.0F - CSng(Math.Pow(1.0F - aRaw, 3))          ' ease-out for sliding in
            Dim hRaw = Math.Max(0.0F, Math.Min(1.0F, n.H))
            Dim eh = 1.0F - (1.0F - hRaw) * (1.0F - hRaw)           ' ease-out for hover
            Dim alpha = CInt(255 * ea)

            Dim zig = If(_stagger, (i Mod 2) * 12, 0) * (1.0F - eh)
            Dim x = CInt(8 + n.Level * 28 + zig + 16 * eh - 40 * (1.0F - ea))
            Dim w = Width - x - 24
            Dim h = _rowH - 4
            Dim top = y + 2

            ' idle bar
            If _idleBar Then
                Dim bar = {New Point(x + 12, top), New Point(x + w, top),
                           New Point(x + w - 12, top + h), New Point(x, top + h)}
                Using b As New SolidBrush(Color.FromArgb(CInt(If(n.Level = 0, 170, 110) * ea), _accent))
                    g.FillPolygon(b, bar)
                End Using
                Using p As New Pen(Color.FromArgb(CInt(200 * ea), _accent), 1.5F)
                    g.DrawPolygon(p, bar)
                End Using
            End If

            ' hover: white bar wipes in from the left with a colored shadow
            If eh > 0.01F Then
                Dim ww = Math.Max(16, CInt(w * eh))
                Dim shadow = {New Point(x + 12 + 6, top + 5), New Point(x + ww + 6, top + 5),
                              New Point(x + ww - 12 + 6, top + h + 5), New Point(x + 6, top + h + 5)}
                Dim wbar = {New Point(x + 12, top), New Point(x + ww, top),
                            New Point(x + ww - 12, top + h), New Point(x, top + h)}
                Using b As New SolidBrush(Color.FromArgb(alpha, _accent))
                    g.FillPolygon(b, shadow)
                End Using
                Using b As New SolidBrush(Color.FromArgb(alpha, Color.White))
                    g.FillPolygon(b, wbar)
                End Using
            End If

            ' text
            Dim baseCol = If(n.Level >= 2, Color.FromArgb(190, 200, 255), Color.White)
            Dim tc = Mix(baseCol, Color.Black, eh)
            Dim fnt = If(n.Level = 0, f0, If(n.Level = 1, f1, f2))
            Dim rect As New RectangleF(x + 18, y, Math.Max(10, w - 50), _rowH)

            Dim shadowA = CInt(220 * (1.0F - eh) * ea)
            If shadowA > 4 Then
                Dim sRect As New RectangleF(rect.X + 2, rect.Y + 2, rect.Width, rect.Height)
                Using b As New SolidBrush(Color.FromArgb(shadowA, _accent))
                    g.DrawString(n.Text, fnt, b, sRect, sf)
                End Using
            End If
            Using b As New SolidBrush(Color.FromArgb(alpha, tc))
                g.DrawString(n.Text, fnt, b, rect, sf)
            End Using

            ' arrow
            If n.Children.Count > 0 Then
                Dim tx = x + w - 28
                Dim ty = y + _rowH \ 2
                Dim tri As Point()
                If n.Expanded Then
                    tri = {New Point(tx - 6, ty - 3), New Point(tx + 6, ty - 3), New Point(tx, ty + 6)}
                Else
                    tri = {New Point(tx - 3, ty - 6), New Point(tx - 3, ty + 6), New Point(tx + 6, ty)}
                End If
                Using b As New SolidBrush(Color.FromArgb(alpha, Mix(Color.White, Color.Black, eh)))
                    g.FillPolygon(b, tri)
                End Using
            End If
        Next

        f0.Dispose() : f1.Dispose() : f2.Dispose() : sf.Dispose()
    End Sub

    ' ---------------- mouse ----------------

    Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
        Dim idx = (e.Y + scrollY) \ _rowH
        If idx < 0 OrElse idx >= rows.Count Then idx = -1
        If idx <> hover Then
            hover = idx
            StartAnim()
            Invalidate()
        End If
        MyBase.OnMouseMove(e)
    End Sub

    Protected Overrides Sub OnMouseLeave(e As EventArgs)
        hover = -1
        StartAnim()
        Invalidate()
        MyBase.OnMouseLeave(e)
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        scrollY -= Math.Sign(e.Delta) * _rowH
        ClampScroll()
        Invalidate()
        MyBase.OnMouseWheel(e)
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        If hover >= 0 AndAlso hover < rows.Count Then
            Dim n = rows(hover)
            If n.Children.Count > 0 Then
                n.Expanded = Not n.Expanded
                If n.Expanded Then
                    ' close the other open items at the same level
                    Dim siblings = If(n.Parent Is Nothing, roots, n.Parent.Children)
                    For Each s In siblings
                        If s IsNot n Then s.Expanded = False
                    Next
                    ' children slide in one after another
                    For k = 0 To n.Children.Count - 1
                        n.Children(k).A = -0.12F * Math.Min(k, 8)
                    Next
                End If
                Rebuild()
                RaiseEvent SectionToggled(roots.Any(Function(r) r.Expanded))
                StartAnim()
            Else
                RaiseEvent ItemClicked(n.Text)
            End If
        End If
        MyBase.OnMouseClick(e)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        ClampScroll()
        MyBase.OnResize(e)
    End Sub
End Class