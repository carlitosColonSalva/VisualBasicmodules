Public Class Consoler
    Public Sub error_writeline(Optional message As String = "")
        If message = "" Then
            Console.Error.WriteLine()
        Else
            Console.Error.WriteLine(message)
        End If
    End Sub

    Public Function IsInputRedirected() As Boolean
        Return Console.IsInputRedirected
    End Function

    Public Function IsOutputRedirected() As Boolean
        Return Console.IsOutputRedirected
    End Function

    Public Function IsErrorRedirected() As Boolean
        Return Console.IsErrorRedirected
    End Function

    Public Sub ReadKey()
        Console.ReadKey()
    End Sub


End Class
