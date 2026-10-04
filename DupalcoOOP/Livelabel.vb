Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' An animated label: slides in with a fade, a slanted bar sweeps behind it,
''' then it gently floats and pulses between two colors.
''' Drag it from the Toolbox after building. Animation runs when the app runs (F5).
''' </summary>
<DefaultProperty("Text")>
Public Class LiveLabel
    Inherits Control

    Private ReadOnly tmr As New System.Windows.Forms.Timer() With {.Interval = 33}
    Private ReadOnly clock As New Stopwatch()

    Private _delay As Integer = 0
    Private _slide As Integer = 40
    Private _float As Integer = 3
    Private _pulse As Boolean = True
    Private _pulseColor As Color = Color.FromArgb(150, 170, 255)
    Private _accentBar As Boolean = True
    Private _accent As Color = Color.FromArgb(30, 30, 255)

    Private Const IntroMs As Double = 700.0

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.UserPaint Or ControlStyles.ResizeRedraw Or
                 ControlStyles.SupportsTransparentBackColor, True)
        BackColor = Color.Transparent
        ForeColor = Color.White
        Font = New Font("Segoe UI", 24, FontStyle.Bold Or FontStyle.Italic)
        TabStop = False
        AddHandler tmr.Tick, AddressOf OnTick
        Text = "LiveLabel"
    End Sub

    Protected Overrides ReadOnly Property DefaultSize As Size
        Get
            Return New Size(260, 60)
        End Get
    End Property

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

    <Category("Live Label"), Description("Milliseconds to wait before the intro animation starts. Use different values on each label to make them appear one after another."), DefaultValue(0)>
    Public Property StartDelay As Integer
        Get
            Return _delay
        End Get
        Set(value As Integer)
            _delay = Math.Max(0, value)
        End Set
    End Property

    <Category("Live Label"), Description("How many pixels the text slides in from the left."), DefaultValue(40)>
    Public Property SlideDistance As Integer
        Get
            Return _slide
        End Get
        Set(value As Integer)
            _slide = Math.Max(8, value)
            FitToText()
            Invalidate()
        End Set
    End Property

    <Category("Live Label"), Description("Pixels the text floats up and down. 0 = no floating."), DefaultValue(3)>
    Public Property FloatAmount As Integer
        Get
            Return _float
        End Get
        Set(value As Integer)
            _float = Math.Max(0, value)
            FitToText()
            Invalidate()
        End Set
    End Property

    <Category("Live Label"), Description("Slowly fade the text color back and forth to PulseColor."), DefaultValue(True)>
    Public Property Pulse As Boolean
        Get
            Return _pulse
        End Get
        Set(value As Boolean)
            _pulse = value
            Invalidate()
        End Set
    End Property

    <Category("Live Label"), Description("The color the text pulses toward.")>
    Public Property PulseColor As Color
        Get
            Return _pulseColor
        End Get
        Set(value As Color)
            _pulseColor = value
            Invalidate()
        End Set
    End Property

    <Category("Live Label"), Description("Draw a slanted bar that sweeps in behind the text."), DefaultValue(True)>
    Public Property AccentBar As Boolean
        Get
            Return _accentBar
        End Get
        Set(value As Boolean)
            _accentBar = value
            Invalidate()
        End Set
    End Property

    <Category("Live Label"), Description("Color of the sweeping bar.")>
    Public Property AccentColor As Color
        Get
            Return _accent
        End Get
        Set(value As Color)
            _accent = value
            Invalidate()
        End Set
    End Property

    ''' <summary>Play the intro animation again (call from code if you want).</summary>
    Public Sub Replay()
        If DesignMode Then Return
        clock.Restart()
        tmr.Start()
    End Sub

    Private Function Measure() As SizeF
        Using bmp As New Bitmap(1, 1)
            Using g = Graphics.FromImage(bmp)
                Return g.MeasureString(If(Text, ""), Font)
            End Using
        End Using
    End Function

    Private Sub FitToText()
        Dim sz = Measure()
        Dim padY = _float + 4
        Dim newSize As New Size(CInt(Math.Ceiling(sz.Width)) + _slide + 16,
                                CInt(Math.Ceiling(sz.Height)) + padY * 2)
        If Size <> newSize Then Size = newSize
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        FitToText()
        Invalidate()
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnFontChanged(e As EventArgs)
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
        If disposing Then tmr.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        ' once the intro is over and nothing else moves, stop the timer to save CPU
        If clock.ElapsedMilliseconds - _delay > IntroMs AndAlso _float = 0 AndAlso Not _pulse Then
            tmr.Stop()
        End If
        Invalidate()
    End Sub

    Private Shared Function Mix(a As Color, b As Color, k As Double) As Color
        k = Math.Max(0.0, Math.Min(1.0, k))
        Dim rr = CInt(a.R) + (CInt(b.R) - CInt(a.R)) * k
        Dim gg = CInt(a.G) + (CInt(b.G) - CInt(a.G)) * k
        Dim bb = CInt(a.B) + (CInt(b.B) - CInt(a.B)) * k
        Return Color.FromArgb(CInt(rr), CInt(gg), CInt(bb))
    End Function

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit

        Dim animating = Not DesignMode AndAlso clock.IsRunning
        Dim ms As Double = If(animating, clock.ElapsedMilliseconds, 1000000000.0)
        Dim p = Math.Max(0.0, Math.Min(1.0, (ms - _delay) / IntroMs))
        Dim ease = 1.0 - Math.Pow(1.0 - p, 3)
        Dim alpha = CInt(255 * ease)

        Dim phase = (Left + Top) * 0.013
        Dim sec = ms / 1000.0
        Dim bob = If(animating, Math.Sin(sec * 2 * Math.PI / 3.2 + phase) * _float * ease, 0.0)
        Dim k = If(animating AndAlso _pulse, (Math.Sin(sec * 2 * Math.PI / 2.5 + phase) + 1) / 2, 0.0)
        Dim col = Mix(ForeColor, _pulseColor, k)

        Dim sz = Measure()
        Dim x = CSng(_slide * ease)
        Dim y = CSng(_float + 4 + bob)

        If _accentBar AndAlso ease > 0.01 Then
            Dim bw = CSng((sz.Width + 16) * ease)
            Dim bx = x - 8
            Dim by = y + 4
            Dim bh = sz.Height - 8
            Dim bar = {New PointF(bx + 10, by), New PointF(bx + bw, by),
                       New PointF(bx + bw - 10, by + bh), New PointF(bx, by + bh)}
            Using b As New SolidBrush(Color.FromArgb(CInt(130 * ease), _accent))
                g.FillPolygon(b, bar)
            End Using
        End If

        Using b As New SolidBrush(Color.FromArgb(alpha, col))
            g.DrawString(Text, Font, b, x, y)
        End Using
    End Sub
End Class