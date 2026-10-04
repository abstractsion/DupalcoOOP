Imports System.ComponentModel
Imports System.Drawing.Drawing2D

''' <summary>
''' A panel that draws your artwork (undistorted) with a very slow pan/zoom, plus subtle
''' animated extras behind the controls placed on it: drifting particles, thin diagonal
''' lines and occasional diagonal slashes. Controls with a transparent BackColor that sit
''' on this panel (P5Menu, P5Plate, LiveLabel) show the moving art behind them.
''' </summary>
Public Class P5Backdrop
    Inherits Panel

    Private Class Particle
        Public X As Single
        Public Y As Single
        Public Vx As Single
        Public Vy As Single
        Public Size As Single
        Public Phase As Single
        Public Tint As Boolean
    End Class

    Private ReadOnly tmr As New System.Windows.Forms.Timer() With {.Interval = 33}
    Private ReadOnly clock As Stopwatch = Stopwatch.StartNew()
    Private ReadOnly rnd As New Random(11)
    Private ReadOnly parts As New List(Of Particle)

    Private _art As Image
    Private _motion As Boolean = True
    Private _particleCount As Integer = 36
    Private _slashEveryMs As Integer = 8000
    Private _accent As Color = Color.FromArgb(60, 80, 255)

    Private slashAt As Double = -100000.0
    Private nextAuto As Double = 2500.0
    Private lastT As Double = 0.0

    Public Sub New()
        SetStyle(ControlStyles.AllPaintingInWmPaint Or ControlStyles.OptimizedDoubleBuffer Or
                 ControlStyles.ResizeRedraw, True)
        BackColor = Color.Black
        AddHandler tmr.Tick, AddressOf OnTick
        BuildParticles()
    End Sub

    ' ---------------- properties ----------------

    <Category("Persona Backdrop"), Description("The artwork. Shown without distortion (it fills the panel and may crop slightly at the edges).")>
    Public Property BackgroundArt As Image
        Get
            Return _art
        End Get
        Set(value As Image)
            _art = value
            Invalidate(True)
        End Set
    End Property

    <Category("Persona Backdrop"), Description("Turn the slow pan, particles, lines and slashes on or off."), DefaultValue(True)>
    Public Property Motion As Boolean
        Get
            Return _motion
        End Get
        Set(value As Boolean)
            _motion = value
            If IsHandleCreated AndAlso Not DesignMode Then
                If _motion Then
                    lastT = clock.ElapsedMilliseconds
                    tmr.Start()
                Else
                    tmr.Stop()
                End If
            End If
            Invalidate(True)
        End Set
    End Property

    <Category("Persona Backdrop"), Description("Number of small drifting dots."), DefaultValue(36)>
    Public Property ParticleCount As Integer
        Get
            Return _particleCount
        End Get
        Set(value As Integer)
            _particleCount = Math.Max(0, Math.Min(200, value))
            BuildParticles()
        End Set
    End Property

    <Category("Persona Backdrop"), Description("Milliseconds between automatic diagonal slashes. 0 = never."), DefaultValue(8000)>
    Public Property SlashInterval As Integer
        Get
            Return _slashEveryMs
        End Get
        Set(value As Integer)
            _slashEveryMs = Math.Max(0, value)
        End Set
    End Property

    <Category("Persona Backdrop"), Description("Color of the lines, slashes and some particles.")>
    Public Property AccentColor As Color
        Get
            Return _accent
        End Get
        Set(value As Color)
            _accent = value
            Invalidate(True)
        End Set
    End Property

    ' ---------------- public ----------------

    ''' <summary>Play a quick diagonal slash across the screen (P5Menu calls this when you click).</summary>
    Public Sub Slash()
        If tmr.Enabled Then slashAt = clock.ElapsedMilliseconds
    End Sub

    ' ---------------- internals ----------------

    Private Sub BuildParticles()
        parts.Clear()
        For i = 1 To _particleCount
            parts.Add(New Particle With {
                .X = CSng(rnd.NextDouble()),
                .Y = CSng(rnd.NextDouble()),
                .Vx = 0.004F + CSng(rnd.NextDouble()) * 0.012F,
                .Vy = -(0.01F + CSng(rnd.NextDouble()) * 0.03F),
                .Size = 1.5F + CSng(rnd.NextDouble()) * 2.5F,
                .Phase = CSng(rnd.NextDouble() * 6.28),
                .Tint = rnd.NextDouble() < 0.35
            })
        Next
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        MyBase.OnHandleCreated(e)
        If Not DesignMode AndAlso _motion Then
            lastT = clock.ElapsedMilliseconds
            tmr.Start()
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then tmr.Dispose()
        MyBase.Dispose(disposing)
    End Sub

    Private Sub OnTick(sender As Object, e As EventArgs)
        Dim now As Double = clock.ElapsedMilliseconds
        Dim dt = Math.Min(0.1, (now - lastT) / 1000.0)
        lastT = now

        For Each p In parts
            p.X += p.Vx * CSng(dt)
            p.Y += p.Vy * CSng(dt)
            If p.X > 1.05F Then p.X = -0.05F
            If p.Y < -0.05F Then p.Y = 1.05F
        Next

        If _slashEveryMs > 0 AndAlso now >= nextAuto Then
            slashAt = now
            nextAuto = now + _slashEveryMs
        End If

        Invalidate(True)    ' repaint this panel and the transparent controls on it
    End Sub

    Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
        Dim g = e.Graphics
        g.Clear(Color.Black)
        If Width <= 0 OrElse Height <= 0 Then Return

        Dim animating = Not DesignMode AndAlso _motion AndAlso tmr.Enabled
        Dim sec As Double = clock.ElapsedMilliseconds / 1000.0

        ' ----- artwork: very slow pan + gentle breathing zoom, never distorted -----
        If _art IsNot Nothing Then
            Dim cover = Math.Max(Width / CSng(_art.Width), Height / CSng(_art.Height))
            Dim sc = cover
            Dim offX As Single = 0.0F
            Dim offY As Single = 0.0F
            If animating Then
                sc = cover * CSng(1.045 + 0.012 * Math.Sin(sec * 2 * Math.PI / 13.0))
                Dim dw0 = _art.Width * sc
                Dim dh0 = _art.Height * sc
                offX = CSng(Math.Sin(sec * 2 * Math.PI / 23.0) * (dw0 - Width) / 2.0 * 0.8)
                offY = CSng(Math.Cos(sec * 2 * Math.PI / 17.0) * (dh0 - Height) / 2.0 * 0.8)
            End If
            Dim dw = _art.Width * sc
            Dim dh = _art.Height * sc
            g.InterpolationMode = InterpolationMode.Bilinear
            g.DrawImage(_art, (Width - dw) / 2.0F + offX, (Height - dh) / 2.0F + offY, dw, dh)
        End If

        If Not animating Then Return
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' ----- thin diagonal lines drifting across -----
        Dim span = Width + Height
        For k = 0 To 2
            Dim speed = 55.0 + 25.0 * k
            Dim x0 = CSng(((sec * speed + k * span / 3.0) Mod span) - Height * 0.6)
            Dim col = If(k Mod 2 = 0, Color.FromArgb(26, Color.White), Color.FromArgb(55, _accent))
            Using pen As New Pen(col, If(k = 1, 2.0F, 1.2F))
                g.DrawLine(pen, x0, Height, x0 + Height * 0.6F, 0)
            End Using
        Next

        ' ----- drifting dots -----
        For Each p In parts
            Dim a = CInt(70 + 60 * Math.Sin(sec * 1.5 + p.Phase))
            Dim c = If(p.Tint, Color.FromArgb(a, _accent), Color.FromArgb(a, Color.White))
            Using b As New SolidBrush(c)
                g.FillEllipse(b, p.X * Width, p.Y * Height, p.Size, p.Size)
            End Using
        Next

        ' ----- diagonal slash sweeping behind the controls -----
        Dim st = clock.ElapsedMilliseconds - slashAt
        If st >= 0 AndAlso st < 520 Then
            Dim pr = st / 520.0
            Dim eo = 1.0 - Math.Pow(1.0 - pr, 3)
            Dim cx = CSng(-250 + (Width + 500) * eo)
            Dim sk = Height * 0.35F
            Dim topX = cx + sk
            Dim botX = cx - sk

            Dim wide = {New PointF(topX - 75, 0), New PointF(topX + 75, 0),
                        New PointF(botX + 75, Height), New PointF(botX - 75, Height)}
            Using b As New SolidBrush(Color.FromArgb(CInt(110 * (1.0 - pr)), _accent))
                g.FillPolygon(b, wide)
            End Using

            Dim thin = {New PointF(topX + 15, 0), New PointF(topX + 50, 0),
                        New PointF(botX + 50, Height), New PointF(botX + 15, Height)}
            Using b As New SolidBrush(Color.FromArgb(CInt(150 * (1.0 - pr)), Color.White))
                g.FillPolygon(b, thin)
            End Using
        End If
    End Sub
End Class