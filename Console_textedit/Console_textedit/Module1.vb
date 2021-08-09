Module Module1
    Function Main() As Integer
        Dim arguments = Environment.GetCommandLineArgs
        Select Case arguments.Count
            Case 1 'norArguments

                Console.WriteLine()
                Dim consoler_ As New Consoler
                With consoler_
                    .error_writeline("didn't receive arguments or piped arguments from another executable")
                    .error_writeline("enter as parameter /?, help or --help for more help")
                    If Not .IsInputRedirected And
                       Not .IsOutputRedirected And
                       Not .IsErrorRedirected Then
                        .error_writeline("press any key to close the program.")
                        .ReadKey()
                    Else
                        Threading.Thread.Sleep(2000)
                    End If
                    Return 1
                End With

            Case 2 '1 argument
                Select Case arguments(1).ToLower
                    Case "/?", "help", "--help"
                        help_explanation_of_program()
                        Return 0
                    Case Else
                        error_found("need more arguments")
                        Return 1
                End Select
            Case Else '2 or more arguments

                If Not correct_arguments_quantity() Then
                    error_found("arguments quantity wrong")
                    Return 1
                    End
                End If

                Dim line_ As String = ""
                Dim all_lines = read_from_console_effectively()
                For h = 0 To all_lines.Count - 1
                    line_ = all_lines(h)
                    For i = 1 To arguments.Count - 2 Step 2

                        'check if argument is tab

                        check_tab(arguments(i + 1))
                        check_quote(arguments(i + 1))
                        'check if argument is QUOTES

                        replace_all_hex_in_string(arguments(i + 1))


                        'CHECK IF IS - OR / AT START
                        Select Case arguments(i).Substring(0, 1)
                            Case "-", "/"
                                Exit Select
                            Case Else
                                Console.Error.WriteLine(
                                    "INCORRECT ARGUMENT CHARACTER " &
                                    arguments(i).Substring(0, 1) &
                                    vbCrLf & "expected - OR / at start of argument"
                                    )
                                Environment.Exit(1)
                        End Select

                        'FIRST CHARACTER IS CHECKED BEFORE
                        Select Case arguments(i).Substring(1).ToLower
                            Case "r", "replace", "rl", "replaceline"


                                If Not arguments.Count >= i + 3 Then 'argument quantity wrong
                                    Console.WriteLine()
                                    error_found("replace or replaceline AND with parameters, are wrong", line_)
                                    Return 1
                                End If

                                'replace argument if it is a tab

                                'don't need to check for arguments(1)
                                'because it was already checked at start of for loop
                                check_tab(arguments(i + 3))


                                'replace argument if it is quotes

                                'don't need to check for arguments(1)
                                'because it was already checked at start of for loop
                                check_quote(arguments(i + 3))


                                replace_all_hex_in_string(arguments(i + 3))

                                If Not edit_lines(line_, arguments(i), arguments(i + 1), arguments(i + 3)) Then
                                    Return 1
                                End If
                            Case Else
                                If Not edit_lines(line_, arguments(i), arguments(i + 1)) Then
                                    Return 1
                                End If

                        End Select
                    Next

                    If h = all_lines.Count - 1 Then
                        If line_ <> "" Then Console.Write(line_)
                    Else
                        Console.WriteLine(line_)
                    End If

                Next
        End Select
        Return 0
    End Function

    Public Function read_from_console_effectively() As String()
        Dim lines As New Queue(Of String)
        Dim achar = 0
        Dim aword As String = ""
        While True
            achar = Console.Read()
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
                If achar > 255 Then
                    achar = 32
                    Console.Error.Write($"character exception on line: {aword}")
                End If
                aword &= {Chr(achar)}
            End If
        End While
        Return lines.ToArray
    End Function

    Sub error_found(ByVal message As String, Optional ByVal line_with_error As String = "")
        Dim an_error = "(*error*)"
        Console.Error.WriteLine()
        Console.Error.WriteLine(an_error & " " & message)
        Console.Error.WriteLine(line_with_error)
    End Sub

    Function correct_arguments_quantity() As Boolean
        Dim arguments_quantity = Environment.GetCommandLineArgs.Count

        While arguments_quantity > 1
            arguments_quantity -= 2
        End While

        If arguments_quantity = 1 Then
            Return True
        Else
            Return False
        End If
    End Function

    Sub help_explanation_of_program()
        Dim help = My.Resources.help_file
        help = help.Replace("this_exe.exe", Environment.GetCommandLineArgs(0)) 'get new executable name
        Console.WriteLine(help)
    End Sub
End Module
