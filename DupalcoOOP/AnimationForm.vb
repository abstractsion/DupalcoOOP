Public Class AnimationForm
    Inherits Form

    ' Positions: 0 = Left Bank, 1 = In Boat, 2 = Right Bank
    Private priestPos As Integer() = {0, 0, 0}
    Private devilPos As Integer() = {0, 0, 0}
    Private boatSide As Integer = 0 ' 0 = Left, 2 = Right

    ' Smooth Animation variables for the boat gliding
    Private boatCurrentX As Single = 290.0F
    Private boatTargetX As Single = 290.0F
    Private isMoving As Boolean = False

    Private crossings As Integer = 0
    Private gameSeconds As Integer = 0
    Private WithEvents gameTimer As New Timer()
    Private WithEvents animTimer As New Timer()
    Private gameStarted As Boolean = False

    ' UI Buttons
    Private WithEvents btnRow As Button
    Private WithEvents btnReset As Button
    Private WithEvents btnClose As Button

    Public Sub New()
        MyBase.New()
        Me.Text = "3 Priests & 3 Devils - Retro Arcade Pixel Edition"
        Me.Size = New Size(960, 720)
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.DoubleBuffered = True

        SetupUI()

        gameTimer.Interval = 1000
        gameTimer.Start()

        animTimer.Interval = 15
        animTimer.Start()
    End Sub

    Private Sub SetupUI()
        btnRow = CreatePixelButton("ROW BOAT", New Point(330, 605), Color.FromArgb(200, 20, 60))
        btnReset = CreatePixelButton("RESET", New Point(485, 605), Color.FromArgb(40, 40, 90))
        btnClose = CreatePixelButton("CLOSE", New Point(640, 605), Color.FromArgb(50, 50, 60))
        AddHandler btnClose.Click, Sub(s, e) Me.Close()
    End Sub

    Private Function CreatePixelButton(text As String, loc As Point, bg As Color) As Button
        Dim btn As New Button()
        btn.Text = text
        btn.Location = loc
        btn.Size = New Size(140, 45)
        btn.BackColor = bg
        btn.ForeColor = Color.White
        ' Use a retro monospaced arcade font
        btn.Font = New Font("Courier New", 11, FontStyle.Bold)
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 3
        btn.FlatAppearance.BorderColor = Color.White
        Me.Controls.Add(btn)
        Return btn
    End Function

    Private Sub gameTimer_Tick(sender As Object, e As EventArgs) Handles gameTimer.Tick
        If gameStarted And Not isMoving Then
            gameSeconds += 1
            Me.Invalidate()
        End If
    End Sub

    Private Sub animTimer_Tick(sender As Object, e As EventArgs) Handles animTimer.Tick
        If Math.Abs(boatCurrentX - boatTargetX) > 1.0F Then
            boatCurrentX += (boatTargetX - boatCurrentX) * 0.18F
            isMoving = True
            Me.Invalidate()
        Else
            If isMoving Then
                isMoving = False
                CheckRules()
            End If
        End If
    End Sub

    ' Retro Pixel Rendering Pipeline (Forces Nearest Neighbor for crisp pixels)
    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g = e.Graphics

        ' CRITICAL FOR PIXEL ART: Disable smoothing, enforce nearest-neighbor
        g.SmoothingMode = Drawing2D.SmoothingMode.None
        g.InterpolationMode = Drawing2D.InterpolationMode.NearestNeighbor
        g.PixelOffsetMode = Drawing2D.PixelOffsetMode.Half
        g.TextRenderingHint = Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit

        DrawPixelBackground(g)
        DrawPixelScenery(g)
        DrawPixelRiverAndSea(g)
        DrawPixelBoat(g)
        DrawCharacters(g)
        DrawHUD(g)
    End Sub

    Private Sub DrawPixelBackground(g As Graphics)
        ' Retro Arcade Header Background
        g.FillRectangle(New SolidBrush(Color.FromArgb(12, 10, 25)), 0, 0, 960, 150)

        ' Retro Arcade Border Frame Line
        Using penFrame As New Pen(Color.FromArgb(220, 20, 60), 4)
            g.DrawRectangle(penFrame, 4, 4, 952, 708)
            g.DrawLine(penFrame, 0, 150, 960, 150)
        End Using

        ' Arcade Title (Monospace Pixel Style)
        Using titleFont = New Font("Courier New", 20, FontStyle.Bold)
            g.DrawString("3 PRIESTS & 3 DEVILS [ARCADE]", titleFont, Brushes.Black, 242, 18)
            g.DrawString("3 PRIESTS & 3 DEVILS [ARCADE]", titleFont, Brushes.Yellow, 240, 16)
        End Using
        Using subFont = New Font("Courier New", 9, FontStyle.Bold)
            g.DrawString("TRANSIT ALL SAFELY. BOAT CAPACITY: 2. WATCH THE DEVIL RATIO!", subFont, Brushes.Cyan, 205, 55)
        End Using

        ' Instruction Retro Box
        g.FillRectangle(New SolidBrush(Color.FromArgb(25, 25, 50)), 180, 85, 600, 45)
        g.DrawRectangle(Pens.White, 180, 85, 600, 45)
        Using bannerFont = New Font("Courier New", 9, FontStyle.Bold)
            g.DrawString("CLICK CHARACTERS TO BOARD, THEN CLICK [ ROW BOAT ]", bannerFont, Brushes.LightGreen, 205, 99)
        End Using
    End Sub

    Private Sub DrawPixelScenery(g As Graphics)
        ' 8-Bit Dark Night Sky
        g.FillRectangle(New SolidBrush(Color.FromArgb(15, 18, 35)), 0, 150, 960, 280)

        ' Retro Blocky Pixel Stars in the Sky
        Using starBrush As New SolidBrush(Color.FromArgb(70, 80, 120))
            For i = 60 To 900 Step 110
                g.FillRectangle(starBrush, i, 180, 8, 8)
                g.FillRectangle(Brushes.White, i + 2, 182, 4, 4)
            Next
        End Using

        ' Blocky 8-Bit Mountain Silhouettes
        Dim mLeft() As Point = {New Point(0, 430), New Point(180, 230), New Point(360, 430)}
        Dim mRight() As Point = {New Point(600, 430), New Point(780, 210), New Point(960, 430)}
        g.FillPolygon(New SolidBrush(Color.FromArgb(30, 35, 60)), mLeft)
        g.FillPolygon(New SolidBrush(Color.FromArgb(20, 25, 45)), mRight)
    End Sub

    Private Sub DrawPixelRiverAndSea(g As Graphics)
        ' Pixel Art Grass Banks
        g.FillRectangle(New SolidBrush(Color.FromArgb(34, 110, 50)), 0, 430, 270, 150)
        g.FillRectangle(New SolidBrush(Color.FromArgb(20, 70, 35)), 0, 565, 270, 15)

        g.FillRectangle(New SolidBrush(Color.FromArgb(34, 110, 50)), 690, 430, 270, 150)
        g.FillRectangle(New SolidBrush(Color.FromArgb(20, 70, 35)), 690, 565, 270, 15)

        ' Retro Blue Pixel River
        g.FillRectangle(New SolidBrush(Color.FromArgb(15, 60, 150)), 270, 430, 420, 150)

        ' Blocky Pixel Waves
        Using wavePen As New Pen(Color.FromArgb(80, 150, 255), 3)
            For w = 290 To 630 Step 50
                g.DrawRectangle(wavePen, w, 475, 20, 4)
                g.DrawRectangle(wavePen, w + 25, 520, 20, 4)
            Next
        End Using

        ' Bottom Control Deck
        g.FillRectangle(New SolidBrush(Color.FromArgb(10, 10, 15)), 0, 580, 960, 140)
        Using redPen As New Pen(Color.FromArgb(220, 20, 60), 4)
            g.DrawLine(redPen, 0, 580, 960, 580)
        End Using
    End Sub

    Private Sub DrawPixelBoat(g As Graphics)
        Dim bx As Integer = CInt(boatCurrentX)
        ' Retro Pixel Boat
        Dim boatPoints() As Point = {New Point(bx, 500), New Point(bx + 160, 500), New Point(bx + 135, 545), New Point(bx + 25, 545)}
        g.FillPolygon(New SolidBrush(Color.FromArgb(110, 60, 30)), boatPoints)
        g.DrawPolygon(Pens.SaddleBrown, boatPoints)
        g.DrawLine(Pens.Black, bx + 40, 500, bx + 120, 500)
    End Sub

    Private Sub DrawCharacters(g As Graphics)
        For i As Integer = 0 To 2
            ' Draw Devil Sprite (Pixel Crisp via Nearest Neighbor)
            Dim dPt = GetCoords(i, True)
            If My.Resources.sprite_devil IsNot Nothing Then
                g.DrawImage(My.Resources.sprite_devil, dPt.X, dPt.Y, 70, 85)
            Else
                g.FillRectangle(Brushes.Crimson, dPt.X, dPt.Y, 50, 70)
            End If

            ' Draw Priest Sprite (Pixel Crisp via Nearest Neighbor)
            Dim pPt = GetCoords(i, False)
            If My.Resources.sprite_priest IsNot Nothing Then
                g.DrawImage(My.Resources.sprite_priest, pPt.X, pPt.Y, 70, 85)
            Else
                g.FillRectangle(New SolidBrush(Color.FromArgb(20, 40, 120)), pPt.X, pPt.Y, 50, 70)
            End If
        Next
    End Sub

    Private Function GetCoords(index As Integer, isDevil As Boolean) As Point
        Dim pos As Integer = If(isDevil, devilPos(index), priestPos(index))

        If pos = 0 Then ' Left Bank (Two rows: Devils top, Priests bottom)
            Dim x As Integer = 20 + (index * 82)
            Dim y As Integer = If(isDevil, 395, 485)
            Return New Point(x, y)
        ElseIf pos = 2 Then ' Right Bank
            Dim x As Integer = 710 + (index * 82)
            Dim y As Integer = If(isDevil, 395, 485)
            Return New Point(x, y)
        Else ' In Boat (pos = 1)
            Dim passengerSlot As Integer = 0
            Dim currentCount As Integer = 0
            For d = 0 To 2
                If devilPos(d) = 1 Then
                    If isDevil AndAlso d = index Then passengerSlot = currentCount
                    currentCount += 1
                End If
            Next
            For p = 0 To 2
                If priestPos(p) = 1 Then
                    If Not isDevil AndAlso p = index Then passengerSlot = currentCount
                    currentCount += 1
                End If
            Next

            Dim boatBaseX As Integer = CInt(boatCurrentX)
            Dim slotX As Integer = boatBaseX + 15 + (passengerSlot * 80)
            Return New Point(slotX, 440)
        End If
    End Function

    Private Sub DrawHUD(g As Graphics)
        Using statFont = New Font("Courier New", 12, FontStyle.Bold)
            Dim timeStr As String = $"TIME: {gameSeconds / 60:00}:{gameSeconds Mod 60:00}"
            g.DrawString($"CROSSINGS: {crossings}", statFont, Brushes.Cyan, 40, 615)
            g.DrawString(timeStr, statFont, Brushes.Gold, 200, 615)
        End Using
    End Sub

    Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
        MyBase.OnMouseDown(e)
        If isMoving Then Return
        gameStarted = True

        For i As Integer = 0 To 2
            Dim dPt = GetCoords(i, True)
            If New Rectangle(dPt.X, dPt.Y, 70, 85).Contains(e.Location) Then
                TogglePosition(devilPos(i), i, True)
                Exit For
            End If

            Dim pPt = GetCoords(i, False)
            If New Rectangle(pPt.X, pPt.Y, 70, 85).Contains(e.Location) Then
                TogglePosition(priestPos(i), i, False)
                Exit For
            End If
        Next
        Me.Invalidate()
    End Sub

    Private Sub TogglePosition(ByRef currentPos As Integer, index As Integer, isDevil As Boolean)
        Dim activeBank = If(boatCurrentX < 450, 0, 2)

        If currentPos = activeBank Then
            Dim boatCount As Integer = 0
            For i As Integer = 0 To 2
                If priestPos(i) = 1 Then boatCount += 1
                If devilPos(i) = 1 Then boatCount += 1
            Next
            If boatCount < 2 Then
                If isDevil Then devilPos(index) = 1 Else priestPos(index) = 1
            End If
        ElseIf currentPos = 1 Then
            If isDevil Then devilPos(index) = activeBank Else priestPos(index) = activeBank
        End If
    End Sub

    Private Sub btnRow_Click(sender As Object, e As EventArgs) Handles btnRow.Click
        If isMoving Then Return

        Dim boatCount As Integer = 0
        For i As Integer = 0 To 2
            If priestPos(i) = 1 Then boatCount += 1
            If devilPos(i) = 1 Then boatCount += 1
        Next

        If boatCount = 0 Then
            MessageBox.Show("THE BOAT NEEDS AT LEAST ONE PASSENGER!", "EMPTY BOAT", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If boatSide = 0 Then
            boatSide = 2
            boatTargetX = 510.0F
        Else
            boatSide = 0
            boatTargetX = 290.0F
        End If

        crossings += 1
        isMoving = True
    End Sub

    Private Sub CheckRules()
        For i As Integer = 0 To 2
            If priestPos(i) = 1 Then priestPos(i) = boatSide
            If devilPos(i) = 1 Then devilPos(i) = boatSide
        Next
        Me.Invalidate()

        Dim lP = 0, lD = 0
        For i As Integer = 0 To 2
            If priestPos(i) = 0 Then lP += 1
            If devilPos(i) = 0 Then lD += 1
        Next
        If lP > 0 AndAlso lD > lP Then
            MessageBox.Show("GAME OVER! DEVILS OUTNUMBERED PRIESTS ON LEFT BANK.", "GAME OVER", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ResetGame()
            Return
        End If

        Dim rP = 0, rD = 0
        For i As Integer = 0 To 2
            If priestPos(i) = 2 Then rP += 1
            If devilPos(i) = 2 Then rD += 1
        Next
        If rP > 0 AndAlso rD > rP Then
            MessageBox.Show("GAME OVER! DEVILS OUTNUMBERED PRIESTS ON RIGHT BANK.", "GAME OVER", MessageBoxButtons.OK, MessageBoxIcon.Error)
            ResetGame()
            Return
        End If

        If rP = 3 AndAlso rD = 3 Then
            MessageBox.Show($"VICTORY! COMPLETED IN {crossings} CROSSINGS AND {gameSeconds} SECONDS!", "YOU WIN!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ResetGame()
        End If
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        ResetGame()
    End Sub

    Private Sub ResetGame()
        priestPos = New Integer() {0, 0, 0}
        devilPos = New Integer() {0, 0, 0}
        boatSide = 0
        boatCurrentX = 290.0F
        boatTargetX = 290.0F
        isMoving = False
        crossings = 0
        gameSeconds = 0
        gameStarted = False
        Me.Invalidate()
    End Sub
End Class