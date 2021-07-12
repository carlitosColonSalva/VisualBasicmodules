Module Module1
    Dim goingDown As Boolean = True
    Sub Main()
        FileSystem.ChDir("test")
        showByLine("FIAT 2018 10 REVERTIR.csv")
        If Environment.GetCommandLineArgs.Count = 1 Then
            Console.Error.WriteLine("no file")
            Environment.Exit(1)
        End If

        Dim file = Environment.GetCommandLineArgs(1)
        If Not file_is_correct(file) Then
            Console.Error.WriteLine("problem with file")
            Environment.Exit(1)
        End If

        'showFirstLines(file)

        ' Dim keysResource = My.Resources.keys.ToString
        ' keyNextLetter = keysResource.Split(vbCr)

        'Dim fileEditor = IO.File.Open(Environment.GetCommandLineArgs(1), IO.FileMode.Open)

        'While True
        '    editFile(chooseFile)
        'End While


        System.Threading.Thread.Sleep(20000)

    End Sub

    Function chooseFile() As String
        Console.Clear()
        Dim previousFile = "E"
        Dim nextFile = "R"
        Dim fileIndex As Byte = 0
        Dim chooseFileButton = "T"

        Dim key As Char
        Dim fileList As String()
        With My.Computer.FileSystem
            fileList = .GetFiles(.CurrentDirectory).ToArray
        End With

        'show list files
        For Each f In fileList
            f = My.Computer.FileSystem.GetName(f)
            Console.WriteLine(f)
        Next
        Console.CursorTop = 0


        While True
            Console.CursorLeft = 0
            key = Console.ReadKey.KeyChar
            'clean appeareance
            Console.CursorLeft -= 1
            Console.Write(" ")
            Console.CursorLeft -= 1
            key = key.ToString.ToUpper

            Console.Clear()
            'show list files
            For Each f In fileList
                f = My.Computer.FileSystem.GetName(f)
                Console.WriteLine(f)
            Next


            Select Case key
                Case previousFile
                    If fileIndex > 0 Then
                        fileIndex -= 1
                        Console.CursorTop = fileIndex
                    End If
                Case nextFile
                    If fileIndex < fileList.Count - 1 Then
                        fileIndex += 1
                        Console.CursorTop = fileIndex
                    End If
                Case chooseFileButton
                    Return My.Computer.FileSystem.GetName(fileList(fileIndex))
                Case Else
                    Console.CursorTop = fileIndex
                    Console.CursorLeft = 0
            End Select
        End While

        Return ""
    End Function
    Sub editFile(file As String)
        Dim fileEditor = IO.File.Open(file, IO.FileMode.Open)
        Dim key As Char
        Dim keyNextLetter = "="
        Dim keyLastLetter = "-"
        Dim keyByteUp = "W"
        Dim keyByteDown = "S"
        Console.Clear()
        Console.WriteLine(file)
        System.Threading.Thread.Sleep(2000)
        Console.Clear()

        Console.Write(Chr((fileEditor.ReadByte)))
        fileEditor.Position -= 1
        Console.CursorLeft -= 1

        While True
            Console.CursorLeft = 0
            key = Console.ReadKey.KeyChar
            'clean appeareance
            Console.CursorLeft -= 1
            Console.Write(" ")
            Console.CursorLeft -= 1
            key = key.ToString.ToUpper


            Dim letter = ""
            Console.Write(Chr((fileEditor.ReadByte)))
            If Console.CursorLeft > 0 Then Console.CursorLeft -= 1
            fileEditor.Position -= 1

            Select Case key
                Case keyLastLetter
                    If fileEditor.Position > 0 Then
                        fileEditor.Position -= 1
                        letter = Chr((fileEditor.ReadByte))
                        Console.Write(letter)
                        fileEditor.Position -= 1
                        If Asc(letter) = 13 Or Asc(letter) = 10 Then
                            Console.CursorTop -= 1
                        End If
                    End If
                Case keyNextLetter
                    If fileEditor.Position < fileEditor.Length - 1 Then
                        fileEditor.Position += 1
                        letter = Chr((fileEditor.ReadByte))
                        Console.Write(letter)
                        fileEditor.Position -= 1
                    End If
                Case keyByteDown 'change letter down
                    letter = Chr((fileEditor.ReadByte))
                    fileEditor.Position -= 1
                    If Asc(letter) > 0 Then
                        fileEditor.WriteByte(Asc(letter) - 1)
                        Console.Write(Chr(Asc(letter) - 1))
                        Console.CursorLeft = 0
                        fileEditor.Position -= 1
                    Else
                        Console.Write("too low")
                        System.Threading.Thread.Sleep(250)
                        Console.CursorLeft = 0
                        For i = 1 To 10
                            Console.Write(" ")
                        Next
                        Console.CursorLeft = 0
                    End If
                Case keyByteUp 'change letter up
                    letter = Chr((fileEditor.ReadByte))
                    fileEditor.Position -= 1
                    If Asc(letter) < 255 Then

                        fileEditor.WriteByte(Asc(letter) + 1)
                        Console.Write(Chr(Asc(letter) + 1))
                        Console.CursorLeft = 0
                        fileEditor.Position -= 1
                    Else
                        Console.Write("too high")
                        System.Threading.Thread.Sleep(250)
                        Console.CursorLeft = 0
                        For i = 1 To 10
                            Console.Write(" ")
                        Next
                        Console.CursorLeft = 0
                    End If
                Case "Z"
                    fileEditor.Close()
                    Console.WriteLine("done")
                    System.Threading.Thread.Sleep(500)
                    Exit While

                Case Else

            End Select
        End While
    End Sub

    Sub showFirstLines(file As String)
        Dim fileEditor = IO.File.Open(file, IO.FileMode.Open)
        Dim letter As Char = "0"
        Dim byteRead As Integer = 0
        fileEditor.Position = fileEditor.Length - 2
        Dim finished = False
        Do
            If finished Then Exit Do
            byteRead = fileEditor.ReadByte
            fileEditor.Position -= 1
            If fileEditor.Position = 0 Then
                finished = True
            Else
                fileEditor.Position -= 1
            End If


            If byteRead = -1 Then
                Exit Do
            ElseIf byteRead = 13 Then
                Console.Write(Chr(Asc(letter)))
                System.Threading.Thread.Sleep(100)
            Else
                letter = Chr(byteRead)
                Console.Write(Chr(Asc(letter)))
            End If
        Loop While byteRead <> -1
        Console.WriteLine("end")
        System.Threading.Thread.Sleep(1000)
        fileEditor.Close()
    End Sub

    Public ReadOnly Property goingUp As Boolean
        Get
            Return Not goingDown
        End Get
    End Property


    Sub showByLine(file As String)
        Dim fileEditor = IO.File.Open(file, IO.FileMode.Open)
        Dim key As Char
        Dim keyLineUp = "Q"
        Dim keyLineDown = "A"
        Dim byteRead As Integer = 0
        Dim letter As Char = "A"
        Dim finished = False

        Dim lettersReverse As New Stack(Of Char)

        While True
            finished = False
            Console.CursorLeft = 0
            key = Console.ReadKey.KeyChar
            'clean appereance
            Console.CursorLeft -= 1
            Console.Write(" ")
            Console.CursorLeft -= 1
            key = key.ToString.ToUpper


            Select Case key
                Case keyLineDown
                    goingDown = True
                Case keyLineUp
                    goingDown = False
            End Select
            Console.Clear()
            Do

                If finished Then Exit Do

                byteRead = fileEditor.ReadByte
                fileEditor.Position -= 1
                If goingDown Then
                    If fileEditor.Position < fileEditor.Length - 2 Then
                        fileEditor.Position += 1
                    Else
                        finished = True
                    End If
                Else
                    If fileEditor.Position > 0 Then
                        fileEditor.Position -= 1
                    Else
                        finished = True
                    End If

                End If

                If byteRead = -1 Then
                    Exit Do
                ElseIf byteRead = 13 Then
                    '  Console.Write(Chr(Asc(letter)))
                    System.Threading.Thread.Sleep(100)
                    Exit Do
                Else
                    letter = Chr(byteRead)
                    If goingDown Then
                        Console.Write(Chr(Asc(letter)))
                    Else
                        lettersReverse.Push(letter)
                    End If
                End If
            Loop While byteRead <> -1
            If Not goingDown Then
                While lettersReverse.Count > 0
                    Console.Write(lettersReverse.Pop)
                End While
            End If
        End While
        Console.WriteLine("end")

    End Sub


End Module
