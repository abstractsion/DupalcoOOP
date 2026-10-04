Imports System.ComponentModel
Imports System.Drawing.Drawing2D

Public Enum P5PlateShape
    Slanted
    Jagged
    Burst
    Ribbon
    Ransom
End Enum

''' <summary>
''' A Persona-style title control. Pick a PlateShape in Properties:
''' Slanted, Jagged (torn edges), Burst (explosion), Ribbon (notched banner)
''' or Ransom (every letter on its own tilted tile).
''' Drag it from the Toolbox after building. Animation runs when the app runs (F5).
''' </summary>
<DefaultProperty("Text")>
Public Class P5Plate
    Inherits Control

    Private Class Tile
        Public Ch As String
        Public F As Font
        Public Idx As Integer
        Public X As Single
        Public Y As Single
        Public Dy As Single
        Public W As Single
        Public H As Single
        Public Ang As Single
    End Class

    Private ReadOnly tmr As New System.Windows.Forms.Timer() With {.Interval = 33}
    Private ReadOnly clock As New Stopwatch()
    Private ReadOnly _tiles As New List(Of Tile)
    Private _ransomW As Single = 0
    Private _ransomH As Single = 0

    Private Const M As Integer = 30            ' empty margin around the shape (room for tilt + shadow)
    Private Const IntroMs As Double = 800.0

    Private _shape As P5PlateShape = P5PlateShape.Slanted
    Private _image As Image
    Private _darken As Integer = 110
    Private _plateColor As Color = Color.White
    Private _outlineColor As Color = Color.Black
    Private _outlineW As Integer = 0
    Private _accent As Color = Color.FromArgb(30, 30, 255)
    Private _star As Boolean = False
    Private _slant As Integer = 14
    Private _tilt As Single = -3.0F
    Private _delay As Integer = 0
    Private _sway As Boolean = True
    Private _shine As Boolean = True
    Private _autoFit As Boolean = True

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or
                 ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.Black
        Font = New Font("Segoe UI", 28, FontStyle.Bold Or FontStyle.Italic)
        TabStop = False
        AddHandler tmr.Tick, AddressOf OnTick
        Text = "P5Plate"
    End Sub

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(320, 120)
        End Get
    End Property

    ' ---------------- properties ----------------

    <Browsable(True), EditorBrowsable(EditorBrowsableState.Always),
     DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
     Editor("System.ComponentModel.Design.MultilineStringEditor, System.Design", GetType(Drawing.Design.UITypeEditor))>
    Public Overrides Property Text As String
        Get
            Return MyBase.Text
        End Get
        Set(value As String)
            MyBase.Text = value
        End Set
    End Property

    <Category("Persona Plate"), Description("The look of the title: Slanted, Jagged, Burst, Ribbon or Ransom."), DefaultValue(P5PlateShape.Slanted)>
    Public Property PlateShape As P5PlateShape
        Get
            Return _shape
        End Get
        Set(value As P5PlateShape)
            _shape = value
            RebuildRansom()
            FitToText()
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Optional picture that fills the shape instead of a solid color (not used by Ransom).")>
    Public Property PlateImage As Image
        Get
            Return _image
        End Get
        Set(value As Image)
            _image = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("How dark to shade the picture (0-255) so the text stays readable."), DefaultValue(110)>
    Public Property ImageDarken As Integer
        Get
            Return _darken
        End Get
        Set(value As Integer)
            _darken = Math.Max(0, Math.Min(255, value))
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Color of the shape when no picture is set.")>
    Public Property PlateColor As Color
        Get
            Return _plateColor
        End Get
        Set(value As Color)
            _plateColor = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Color of the outline around the text.")>
    Public Property OutlineColor As Color
        Get
            Return _outlineColor
        End Get
        Set(value As Color)
            _outlineColor = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Thickness of the text outline in pixels. 0 = no outline."), DefaultValue(0)>
    Public Property OutlineWidth As Integer
        Get
            Return _outlineW
        End Get
        Set(value As Integer)
            _outlineW = Math.Max(0, value)
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Color of the offset shadow, the star badge, and some Ransom tiles.")>
    Public Property AccentColor As Color
        Get
            Return _accent
        End Get
        Set(value As Color)
            _accent = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Show a star badge on the top-left corner."), DefaultValue(False)>
    Public Property ShowStar As Boolean
        Get
            Return _star
        End Get
        Set(value As Boolean)
            _star = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("How slanted / notched the edges are, in pixels."), DefaultValue(14)>
    Public Property Slant As Integer
        Get
            Return _slant
        End Get
        Set(value As Integer)
            _slant = Math.Max(0, value)
            FitToText()
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Rotation in degrees. Negative = tilted up to the right."), DefaultValue(-3.0F)>
    Public Property Tilt As Single
        Get
            Return _tilt
        End Get
        Set(value As Single)
            _tilt = value
            Invalidate()
        End Set
    End Property

    <Category("Persona Plate"), Description("Milliseconds to wait before it appears. Use different values to make titles appear one after another."), DefaultValue(0)>
    Public Property StartDelay As Integer
        Get
            Return _delay
        End Get
        Set(value As Integer)
            _delay = Math.Max(0, value)
        End Set
    End Property

    <Category("Persona Plate"), Description("Slowly rock the shape (or bob the letters in Ransom)."), DefaultValue(True)>
    Public Property Sway As Boolean
        Get
            Return _sway
        End Get
        Set(value As Boolean)
            _sway = value
        End Set
    End Property

    <Category("Persona Plate"), Description("A bright streak sweeps across the shape every few seconds."), DefaultValue(True)>
    Public Property Shine As Boolean
        Get
            Return _shine
        End Get
        Set(value As Boolean)
            _shine = value
        End Set
    End Property

    <Category("Persona Plate"), Description("Resize the control automatically to fit the text."), DefaultValue(True)>
    Public Property AutoFit As Boolean
        Get
            Return _autoFit
        End Get
        Set(value As Boolean)
            _autoFit = value
            FitToText()
        End Set
    End Property

    ' ---------------- helpers ----------------

    Public Sub Replay()
        If DesignMode Then Return
        clock.Restart()
        tmr.Start()
    End Sub

    Private Function BuildTextPath() As GraphicsPath
        Dim gp As New GraphicsPath()
        Using sf As New StringFormat(StringFormat.GenericTypographic)
            sf.Alignment = StringAlignment.Center
            gp.AddString(If(Text, ""), Font.FontFamily, CInt(Font.Style), Font.SizeInPoints * 96.0F / 72.0F, New PointF(0, 0), sf)
        End Using
        Return gp
    End Function

    Private Sub RebuildRansom()
        For Each t In _tiles
            t.F.Dispose()
        Next
        _tiles.Clear()
        _ransomW = 0
        _ransomH = 0
        If _shape <> P5PlateShape.Ransom Then Return

        Dim txt = If(Text, "").Replace(vbCrLf, " ").Replace(vbLf, " ")
        Dim px = Font.SizeInPoints * 96.0F / 72.0F
        Dim fams = {Font.FontFamily.Name, "Georgia", "Impact", "Segoe UI Black", "Courier New"}
        Dim scales = {1.0F, 0.88F, 1.12F, 0.95F, 1.05F}
        Dim x As Single = 0
        Dim maxH As Single = 0

        Using bmp As New Bitmap(1, 1)
            Using g = Graphics.FromImage(bmp)
                For i = 0 To txt.Length - 1
                    Dim ch = txt(i)
                    If ch = " "c Then
                        x += px * 0.4F
                        Continue For
                    End If

                    Dim v = (i * 7 + 3) Mod 5
                    Dim f As Font
                    Try
                        f = New Font(fams(v), px * scales(v), FontStyle.Bold Or If(v Mod 2 = 0, FontStyle.Italic, FontStyle.Regular), GraphicsUnit.Pixel)
                    Catch
                        f = New Font(Font.FontFamily, px * scales(v), FontStyle.Bold, GraphicsUnit.Pixel)
                    End Try

                    Dim sz = g.MeasureString(ch.ToString(), f, PointF.Empty, StringFormat.GenericTypographic)
                    Dim pad = px * 0.2F
                    Dim t As New Tile With {
                        .Ch = ch.ToString(),
                        .F = f,
                        .Idx = i,
                        .W = sz.Width + pad * 2,
                        .H = sz.Height + pad * 0.8F,
                        .Ang = (((i * 53 + 17) Mod 13) - 6) * 0.9F,
                        .X = x,
                        .Dy = (((i * 31) Mod 9) - 4) * px * 0.04F
                    }
                    _tiles.Add(t)
                    x += t.W + px * 0.06F
                    maxH = Math.Max(maxH, t.H)
                Next
            End Using
        End Using

        _ransomW = x
        _ransomH = maxH + px * 0.5F
        For Each t In _tiles
            t.Y = (_ransomH - t.H) / 2.0F + t.Dy
        Next
    End Sub

    Private Sub FitToText()
        If Not _autoFit Then Return

        If _shape = P5PlateShape.Ransom Then
            Dim rs As New Size(CInt(Math.Ceiling(_ransomW)) + 2 * M, CInt(Math.Ceiling(_ransomH)) + 2 * M)
            If Size <> rs Then Size = rs
            Return
        End If

        Using gp = BuildTextPath()
            Dim b = gp.GetBounds()
            Dim pw As Integer
            Dim ph As Integer
            Select Case _shape
                Case P5PlateShape.Burst
                    pw = CInt(Math.Ceiling(b.Width * 1.3)) + 60
                    ph = CInt(Math.Ceiling(b.Height * 1.8)) + 30
                Case P5PlateShape.Ribbon
                    pw = CInt(Math.Ceiling(b.Width)) + 2 * (Math.Max(16, _slant + 8) + 24)
                    ph = CInt(Math.Ceiling(b.Height)) + 30
                Case Else
                    pw = CInt(Math.Ceiling(b.Width)) + 2 * (_slant + 24)
                    ph = CInt(Math.Ceiling(b.Height)) + 30
            End Select
            Dim ns As New Size(pw + 2 * M, ph + 2 * M)
            If Size <> ns Then Size = ns
        End Using
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        RebuildRansom()
        FitToText()
        Invalidate()
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
        RebuildRansom()
        FitToText()
        Invalidate()
        MyBase.OnFontChanged(e)
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not DesignMode Then
            clock.Restart()
            tmr.Start()
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            tmr.Dispose()
            For Each t In _tiles
                t.F.Dispose()
            Next
            _tiles.Clear()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        Dim introEnd = _delay + IntroMs + If(_shape = P5PlateShape.Ransom, _tiles.Count * 70.0, 0.0)
        If clock.ElapsedMilliseconds > introEnd AndAlso Not _sway AndAlso Not _shine Then
            tmr.Stop()
        End If
        Invalidate()
    End Sub

    Private Shared Sub DrawCover(g As Graphics, img As Image, dest As RectangleF)
        Dim scale = Math.Max(dest.Width / img.Width, dest.Height / img.Height)
        Dim sw = dest.Width / scale
        Dim sh = dest.Height / scale
        Dim src As New RectangleF((img.Width - sw) / 2.0F, (img.Height - sh) / 2.0F, sw, sh)
        g.DrawImage(img, dest, src, GraphicsUnit.Pixel)
    End Sub

    Private Sub DrawStar(g As Graphics, cx As Single, cy As Single, r As Single)
        Dim pts(9) As PointF
        For i = 0 To 9
            Dim rad = If(i Mod 2 = 0, r, r * 0.42F)
            Dim ang = (-90 + i * 36) * Math.PI / 180.0
            pts(i) = New PointF(cx + CSng(Math.Cos(ang)) * rad, cy + CSng(Math.Sin(ang)) * rad)
        Next
        Using b As New SolidBrush(_accent)
            g.FillPolygon(b, pts)
        End Using
        Using p As New Pen(Color.White, 2.0F)
            p.LineJoin = LineJoin.Round
            g.DrawPolygon(p, pts)
        End Using
    End Sub

    Private Function BuildShapePath(pr As RectangleF) As GraphicsPath
        Dim gp As New GraphicsPath()
        Dim s As Single = _slant

        Select Case _shape
            Case P5PlateShape.Jagged
                Dim rnd As New Random(7 + If(Text, "").Length)
                Dim pts As New List(Of PointF)
                Dim n = Math.Max(2, CInt(pr.Width / 16))
                For k = 0 To n
                    pts.Add(New PointF(pr.X + s + (pr.Width - s) * k / n, pr.Y + rnd.Next(0, 8)))
                Next
                For k = n To 0 Step -1
                    pts.Add(New PointF(pr.X + (pr.Width - s) * k / n, pr.Bottom - rnd.Next(0, 8)))
                Next
                gp.AddPolygon(pts.ToArray())

            Case P5PlateShape.Burst
                Dim pts(59) As PointF
                Dim cx = pr.X + pr.Width / 2.0F
                Dim cy = pr.Y + pr.Height / 2.0F
                Dim rx = pr.Width / 2.0F
                Dim ry = pr.Height / 2.0F
                For k = 0 To 59
                    Dim r = If(k Mod 2 = 0, 1.0F, 0.8F)
                    Dim ang = k * 2.0 * Math.PI / 60.0
                    pts(k) = New PointF(cx + CSng(Math.Cos(ang)) * rx * r, cy + CSng(Math.Sin(ang)) * ry * r)
                Next
                gp.AddPolygon(pts)

            Case P5PlateShape.Ribbon
                Dim nd = Math.Max(16.0F, s + 8.0F)
                gp.AddPolygon({New PointF(pr.X, pr.Y), New PointF(pr.Right, pr.Y),
                               New PointF(pr.Right - nd, pr.Y + pr.Height / 2.0F), New PointF(pr.Right, pr.Bottom),
                               New PointF(pr.X, pr.Bottom), New PointF(pr.X + nd, pr.Y + pr.Height / 2.0F)})

            Case Else
                gp.AddPolygon({New PointF(pr.X + s, pr.Y), New PointF(pr.Right, pr.Y),
                               New PointF(pr.Right - s, pr.Bottom), New PointF(pr.X, pr.Bottom)})
        End Select

        Return gp
    End Function

    ' ---------------- painting ----------------

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

        Dim animating = Not DesignMode AndAlso clock.IsRunning
        Dim ms As Double = If(animating, clock.ElapsedMilliseconds, 1000000000.0)
        If animating AndAlso ms < _delay Then Return        ' waiting for its turn

        If _shape = P5PlateShape.Ransom Then
            PaintRansom(g, ms, animating)
            Return
        End If

        Dim p = Math.Max(0.0, Math.Min(1.0, (ms - _delay) / IntroMs))
        Dim ease = 1.0 - Math.Pow(1.0 - p, 3)
        If ease < 0.001 Then Return

        Dim pr As New RectangleF(M, M, Math.Max(10, Width - 2 * M), Math.Max(10, Height - 2 * M))
        Dim sec = ms / 1000.0

        Dim swayDeg = If(animating AndAlso _sway AndAlso ease >= 1.0,
                         Math.Sin(sec * 2 * Math.PI / 4.0 + (Left + Top) * 0.01) * 0.8, 0.0)
        Dim cx = pr.X + pr.Width / 2.0F
        Dim cy = pr.Y + pr.Height / 2.0F

        g.TranslateTransform(cx, cy)
        g.RotateTransform(CSng(_tilt + swayDeg))
        g.TranslateTransform(-cx, -cy)

        ' the shape wipes in from the left
        Dim wipe As New RectangleF(pr.X - 12, pr.Y - 12, CSng((pr.Width + 30) * ease), pr.Height + 30)

        Using shape = BuildShapePath(pr)
            ' offset shadow
            g.SetClip(wipe)
            Using sp = CType(shape.Clone(), GraphicsPath)
                Using mtx As New Matrix()
                    mtx.Translate(7, 7)
                    sp.Transform(mtx)
                End Using
                Using b As New SolidBrush(Color.FromArgb(CInt(230 * Math.Min(1.0, ease * 2)), _accent))
                    g.FillPath(b, sp)
                End Using
            End Using

            g.SetClip(shape, CombineMode.Intersect)

            ' fill
            If _image IsNot Nothing Then
                DrawCover(g, _image, pr)
                Using b As New SolidBrush(Color.FromArgb(_darken, Color.Black))
                    g.FillRectangle(b, pr)
                End Using
            Else
                Using b As New SolidBrush(_plateColor)
                    g.FillRectangle(b, pr)
                End Using
            End If

            ' shine streak
            If animating AndAlso _shine AndAlso ease >= 1.0 Then
                Dim t = (ms Mod 4800.0) / 1200.0
                If t < 1.0 Then
                    Dim sx = pr.X - 80 + (pr.Width + 160) * CSng(t)
                    Dim band = {New PointF(sx + 30, pr.Y - 10), New PointF(sx + 70, pr.Y - 10),
                                New PointF(sx + 40, pr.Bottom + 10), New PointF(sx, pr.Bottom + 10)}
                    Using b As New SolidBrush(Color.FromArgb(110, Color.White))
                        g.FillPolygon(b, band)
                    End Using
                End If
            End If

            ' text: fades and slides in after the shape
            Dim ta = Math.Max(0.0, Math.Min(1.0, (ease - 0.45) / 0.55))
            If ta > 0.01 Then
                Using gp = BuildTextPath()
                    Dim bnd = gp.GetBounds()
                    Dim dx = cx - (bnd.X + bnd.Width / 2.0F) - CSng(18 * (1.0 - ta))
                    Dim dy = cy - (bnd.Y + bnd.Height / 2.0F)
                    Using mtx As New Matrix()
                        mtx.Translate(dx, dy)
                        gp.Transform(mtx)
                    End Using

                    Dim a = CInt(255 * ta)
                    If _outlineW > 0 Then
                        Using pen As New Pen(Color.FromArgb(a, _outlineColor), _outlineW * 2.0F)
                            pen.LineJoin = LineJoin.Round
                            g.DrawPath(pen, gp)
                        End Using
                    End If
                    Using b As New SolidBrush(Color.FromArgb(a, ForeColor))
                        g.FillPath(b, gp)
                    End Using
                End Using
            End If

            g.ResetClip()
        End Using

        If _star AndAlso ease > 0.3 Then DrawStar(g, pr.X + 8, pr.Y + 4, 17)

        g.ResetTransform()
    End Sub

    Private Sub PaintRansom(g As Graphics, ms As Double, animating As Boolean)
        If _tiles.Count = 0 Then Return

        Dim sec = ms / 1000.0
        Dim cx = Width / 2.0F
        Dim cy = Height / 2.0F
        g.TranslateTransform(cx, cy)
        g.RotateTransform(_tilt)
        g.TranslateTransform(-cx, -cy)

        For idx = 0 To _tiles.Count - 1
            Dim t = _tiles(idx)
            Dim tt = Math.Max(0.0, Math.Min(1.0, (ms - _delay - idx * 70.0) / 380.0))
            If tt <= 0.0 Then Continue For

            ' pop-in with a little overshoot
            Dim u = tt - 1.0
            Dim sc = CSng(1.0 + 2.70158 * u * u * u + 1.70158 * u * u)
            Dim bob = If(animating AndAlso _sway AndAlso tt >= 1.0, Math.Sin(sec * 2.2 + idx * 0.6) * 2.2, 0.0)

            Dim bg As Color
            Dim fg As Color
            Select Case t.Idx Mod 4
                Case 0
                    bg = Color.White : fg = Color.Black
                Case 1
                    bg = Color.Black : fg = Color.White
                Case 2
                    bg = _accent : fg = Color.White
                Case Else
                    bg = Color.FromArgb(225, 225, 255) : fg = _accent
            End Select

            Dim st = g.Save()
            g.TranslateTransform(M + t.X + t.W / 2.0F, M + t.Y + t.H / 2.0F + CSng(bob))
            g.RotateTransform(t.Ang)
            g.ScaleTransform(sc, sc)

            Dim r As New RectangleF(-t.W / 2.0F, -t.H / 2.0F, t.W, t.H)
            Using b As New SolidBrush(_accent)
                g.FillRectangle(b, r.X + 4, r.Y + 4, r.Width, r.Height)
            End Using
            Using b As New SolidBrush(bg)
                g.FillRectangle(b, r)
            End Using
            Using pen As New Pen(Color.Black, 1.5F)
                g.DrawRectangle(pen, r.X, r.Y, r.Width, r.Height)
            End Using
            Using sf As New StringFormat(StringFormat.GenericTypographic)
                sf.Alignment = StringAlignment.Center
                sf.LineAlignment = StringAlignment.Center
                Using b As New SolidBrush(fg)
                    g.DrawString(t.Ch, t.F, b, r, sf)
                End Using
            End Using

            g.Restore(st)
        Next

        g.ResetTransform()
    End Sub
End Class