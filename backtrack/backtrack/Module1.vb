Module Module1
    Enum directions
        up = 0
        down = 1
        left = 2
        right = 3
    End Enum
    Public visited(8, 8) As Boolean
    Public size As Byte = 0
    Public PrizeX As Integer = 1
    Public PrizeY As Integer = 1
    Public foundPrize As Boolean = False

    Sub Main()
        Console.Error.Write("enter size: ")
        size = CByte(Console.ReadLine())
        ReDim visited(size, size)
        For i = 0 To 5
            wall(random_by_unknown_quantity(size - 1), random_by_unknown_quantity(size - 1))
        Next

        While True
            setPrize()
            drawPrize()
            createWall()
            moveWhilePossible(0, 0)
            For i As Byte = 0 To size - 1
                For j As Byte = 0 To size - 1
                    visited(i, j) = False
                    Console.CursorLeft = i
                    Console.CursorTop = j
                    Console.Write(" ")
                Next
            Next
        End While
        Console.ReadKey()
    End Sub

    Function leftAvailable(x, y) As Boolean
        If x > 0 Then
            If Not visited(x - 1, y) Then Return True
        End If
        Return False

    End Function
    Function rightAvailable(x, y) As Boolean
        If x < size - 1 Then
            If Not visited(x + 1, y) Then Return True
        End If
        Return False
    End Function
    Function upAvailable(x, y) As Boolean
        If y > 0 Then
            If Not visited(x, y - 1) Then Return True
        End If
        Return False
    End Function
    Function downAvailable(x, y) As Boolean
        If y < size - 1 Then
            If Not visited(x, y + 1) Then Return True
        End If
        Return False
    End Function

    Function anyDirectionAvailable(x, y) As Boolean
        If upAvailable(x, y) Then Return True
        If downAvailable(x, y) Then Return True
        If leftAvailable(x, y) Then Return True
        If rightAvailable(x, y) Then Return True
        Return False
    End Function

    Function directionAvailable(x, y, direction) As Boolean
        Select Case direction
            Case directions.up
                Return upAvailable(x, y)
            Case directions.down
                Return downAvailable(x, y)
            Case directions.left
                Return leftAvailable(x, y)
            Case Else
                Return rightAvailable(x, y)

        End Select

    End Function

    Function randomDirection() As Byte
        Dim nRandom As New Random
        Dim nRand = nRandom.NextDouble
        Select Case nRand
            Case < 0.25
                Return directions.up
                Exit Select
            Case < 0.5
                Return directions.down
                Exit Select
            Case < 0.75
                Return directions.left
                Exit Select
            Case Else
                Return directions.right
        End Select
    End Function

    Sub moveWhilePossible(x, y)
        visited(x, y) = True
        If x = PrizeX And y = PrizeY Then
            foundPrize = True
        End If

        Console.CursorLeft = x
        Console.CursorTop = y
        Console.WriteLine("x")
        System.Threading.Thread.Sleep(300)
        Dim newDirection
        Dim newLocationX
        Dim newLocationY
        While anyDirectionAvailable(x, y)
            While Not foundPrize
                newDirection = randomDirection()
            If directionAvailable(x, y, newDirection) Then
                Select Case newDirection
                    Case directions.up
                        newLocationX = x
                        newLocationY = y - 1
                    Case directions.down
                        newLocationX = x
                        newLocationY = y + 1
                    Case directions.left
                        newLocationX = x - 1
                        newLocationY = y
                    Case Else 'right
                        newLocationX = x + 1
                        newLocationY = y
                End Select
                    moveWhilePossible(newLocationX, newLocationY)
                    If foundPrize Then Exit While
                    System.Threading.Thread.Sleep(300)
                Console.CursorLeft = x
                Console.CursorTop = y
                Console.Write("/")
                Console.CursorLeft = newLocationX
                Console.CursorTop = newLocationY
                Console.Write(" ")
                System.Threading.Thread.Sleep(300)
            End If
            End While
        End While
    End Sub

    Sub createWall()
        For i = 1 To 3
            wall(i, 2)
            wall(i, 4)
        Next
    End Sub
    Sub wall(x, y)
        visited(x, y) = True
        Console.CursorLeft = x
        Console.CursorTop = y
        Console.Write("0")
    End Sub

    Sub setPrize()
        Do
            PrizeX = random_by_unknown_quantity(size - 1)
            System.Threading.Thread.Sleep(249)
            PrizeY = random_by_unknown_quantity(size - 1)
        Loop While visited(PrizeX, PrizeY)
        foundPrize = False
    End Sub

    Sub drawPrize()
        Console.CursorLeft = PrizeX
        Console.CursorTop = PrizeY
        Console.Write("$")
    End Sub

End Module
