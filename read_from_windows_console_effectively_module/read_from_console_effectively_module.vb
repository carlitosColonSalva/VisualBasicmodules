Module read_from_console_effectively_module
Public Function read_from_console_effectively() As String()
        Dim lines As New Queue(Of String)
        Dim achar = 0
        Dim aword As String = ""
        Dim my_byte As Integer = 0
        While True
            my_byte = Console.Read()
            If my_byte > 255 Then
                Console.Write("special character found " & my_byte)
            Else
                achar = my_byte
            End If

            If achar = -1 Then
                lines.Enqueue(aword)
                Exit While
            End If
            '            Console.Write(Chr(achar))
            If achar = 13 Then
                Console.Read() 'remove char 10
                lines.Enqueue(aword)
                aword = ""
            ElseIf achar = 10 Then
                lines.Enqueue(aword)
                aword = ""
            Else
                aword = aword & Chr(achar)
            End If
        End While
        Return lines.ToArray
    End Function

End Module