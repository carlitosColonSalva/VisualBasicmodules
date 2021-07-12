Module file_is_correct_module
    Public Function file_is_correct(ByVal file As String, Optional ByVal app_running_on_console As Boolean = False) As Boolean
        If Not IO.File.Exists(file) Then
            If (app_running_on_console) Then Console.WriteLine("specified file not found")
            Return False

            'check file size
        ElseIf My.Computer.FileSystem.GetFileInfo(file).Length = 0 Then
            If (app_running_on_console) Then Console.WriteLine("file specified is empty")
            Return False
        Else
            Return True
        End If
    End Function
End Module