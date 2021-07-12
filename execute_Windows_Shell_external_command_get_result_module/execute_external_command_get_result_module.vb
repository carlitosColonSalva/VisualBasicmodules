module execute_external_command_get_result_module
    Function execute_external_command_get_result(ByVal executable_name As String, ByVal argument As String) As String
        Dim myprocess As New Process
        With myprocess.StartInfo
            .FileName = executable_name
            .Arguments = argument
            .CreateNoWindow = True
            .UseShellExecute = False
            .RedirectStandardOutput = True
            myprocess.Start()
        End With

        Dim output_result As String = myprocess.StandardOutput.ReadToEnd


        Return output_result
    End Function
End module