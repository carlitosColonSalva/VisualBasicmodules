Module Module1

    Sub Main()
        Dim arguments = Environment.GetCommandLineArgs
        Select Case arguments.Count
            Case 1 'norarguments
                Console.WriteLine("didn't receive arguments or piped arguments from another executable")
                help_explanation_of_program()
            Case 2
                Select Case arguments(1).ToLower
                    Case "/?", "help", "--help"
                        help_explanation_of_program()
                    Case Else
                        Console.WriteLine("need more arguments")
                End Select
            Case 3
                If Not IsNumeric(arguments(1)) Then
                    Console.WriteLine("number of fields wrong")
                    End
                End If

                Dim fields_count As Integer = arguments(1)

                Dim delimiter As String = ""
                If arguments(2).ToLower = "/t" Then
                    delimiter = vbTab
                Else
                    delimiter = arguments(2)
                End If

                Dim fields_wanted As New Queue(Of String)
                Dim fields_line As String = ""
                Dim all_lines = read_from_console_effectively()

                If fields_count = 0 Then
                    For i = 0 To all_lines.Count - 1
                        If i = 0 Then
                            Console.Write(all_lines(i))
                        Else
                            Console.Write(delimiter & all_lines(i))
                        End If
                    Next
                Else
                    For i = 0 To all_lines.Count - fields_count Step fields_count
                    If i > all_lines.Count - 1 Then End
                    For j = i To i + fields_count - 1
                        fields_wanted.Enqueue(all_lines(j))
                    Next
                    For k = 0 To fields_wanted.Count - 1
                        If k = 0 Then
                            fields_line = fields_wanted(k)
                        Else
                            fields_line = fields_line & delimiter & fields_wanted(k)
                        End If
                    Next
                    Console.WriteLine(fields_line)
                    fields_wanted.Clear()
                    fields_line = ""
                Next

                End If 'fields_count = 0

            Case Else
                '
        End Select
    End Sub
    Sub help_explanation_of_program()
        Dim help = My.Resources.help_file
        help = help.Replace("this_exe.exe", Environment.GetCommandLineArgs(0)) 'get new executable name
        Console.WriteLine(help)
    End Sub
End Module
