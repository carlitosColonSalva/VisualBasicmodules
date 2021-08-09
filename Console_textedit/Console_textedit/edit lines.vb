module edit_lines_module

    Sub check_tab(ByRef some_info As String)
        If some_info.ToLower.Contains("/t") Then
            some_info = some_info.Replace("/t", vbTab)
            some_info = some_info.Replace("/T", vbTab)
        End If
    End Sub


    Sub check_quote(ByRef some_info As String)
        If some_info.ToLower.Contains("/q") Then
            some_info = some_info.Replace("/q", Chr(34))
            some_info = some_info.Replace("/Q", Chr(34))
        End If
    End Sub


    Public Function edit_lines(ByRef line_ As String, ByVal command As String, ByVal info_to_change As String, Optional ByVal new_info As String = "") As Boolean

        Try
            If command = "" Then
                Console.Error.WriteLine("an instruction was wrongly typed")
                Return False
            End If

            Select Case command.Substring(0, 1)
                Case "/", "-"
                Case Else
                    Console.Error.WriteLine("an instruction was wrongly typed" & vbCrLf & "expected - OR / at start of argument")
                    Return False
                    Exit Select
            End Select

			If command.Length = 1 Then
                Console.Error.WriteLine("command " & Chr(34) & command & Chr(34) & " was too short..")
                Return False
            End If

            check_tab(info_to_change)
            Select Case command.Substring(1).ToLower
                Case "p", "prepend"
                    line_ = info_to_change & line_
                    Exit Select
                Case "a", "append"
                    line_ = line_ & info_to_change
                    Exit Select
                Case "d", "duplicate"
                    line_ = line_ & info_to_change & line_
                    Exit Select
                Case "rm", "remove"
                    line_ = line_.Replace(info_to_change, "")
                    Exit Select
                Case "r", "replace"
                    check_tab(new_info)
                    line_ = line_.Replace(info_to_change, new_info)

                    Exit Select

                Case "rl", "replaceline"
                    check_tab(new_info)
                    If line_.Contains(info_to_change) Then
                        line_ = new_info
                    End If
                    Exit Select

                Case "w", "with"
                    'ignore
                    Exit Select
                Case "s", "substring"
                    Dim indexes As String() = info_to_change.Split(",")
                    Dim characters_to_skip As Integer

                    If indexes(0).Contains("-") Then
                        indexes(0) = indexes(0).Substring(1) 'remove dash
                        characters_to_skip = line_.Count - CInt(indexes(0))
                    Else
                        characters_to_skip = CInt(indexes(0))
                    End If

                    If indexes.Count = 1 Then
                        If characters_to_skip >= line_.Count Then
                            Console.Error.WriteLine("skipped more characters than length of string " & vbCrLf & line_)
                            Return False
                            End
                        Else
                            line_ = line_.Substring(characters_to_skip)
                        End If
                    Else
                        Dim characters_to_get As Integer

                        If indexes(1).Contains("-") Then
                            indexes(1) = indexes(1).Substring(1) 'remove dash
                            characters_to_get = line_.Count - CInt(indexes(1))
                        Else
                            characters_to_get = CInt(indexes(1))
                        End If

                        'verify errors on substring, if no error get substring
                        If characters_to_skip >= line_.Count Then
                            Console.Error.WriteLine("skipped more characters than length of string " & vbCrLf & line_)
                            Return False
                        ElseIf characters_to_get > line_.Count Then
                            Console.Error.WriteLine("tried to get more characters than length of string " & vbCrLf & line_)
                            Return False
                        ElseIf characters_to_skip + characters_to_get > line_.Count Then
                            Console.Error.WriteLine("tried to skip and get more characters than length of string " & vbCrLf & line_)
                            Return False
                        Else
                            line_ = line_.Substring(characters_to_skip, characters_to_get)
                        End If
                    End If
                    Exit Select
                Case Else
                    Console.Error.WriteLine("an instruction was wrongly typed")
                    Return False
            End Select
        Catch this_error As System.Exception
            Console.Error.WriteLine("error editing line" &
          vbCrLf & line_ & vbCrLf & this_error.Message)
            Return False
        End Try
        Return True
    End Function
End module