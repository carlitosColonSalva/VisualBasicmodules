module replace_all_hex_in_string_module

    'this module replaces all hex representations to the real hex character
    'the hex representation is like this:
    '"~h0x0000...~"



    '-----------------------------

    'new version


    Private Function contains_hex(ByVal some_string As String) As Boolean
        If some_string.ToLower.Contains("~h0x") Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' this module replaces all hex representations to the real hex character
    ''' the hex representation is like this:
    '''"~h0x0000...~"
    ''' </summary>
    ''' <param name="some_string">a string that contains a hex representation like this: ~h0x0000...~</param>
    Public Sub replace_all_hex_in_string(ByRef some_string As String)
        Dim new_string As String = ""
        Dim number_of_characters As Byte = 1
        While contains_hex(some_string)
            For i = 0 To some_string.Count - 1
                If some_string.Substring(i, 1) = "~" Then
                    For k = i + 1 To some_string.Count - 1
                        number_of_characters += 1
                        If some_string.Substring(k, 1) = "~" Then
                            Exit For
                        End If
                    Next
                    new_string = some_string.Substring(i, number_of_characters)
                    replace_an_hex(new_string)
                    some_string = new_string
                    number_of_characters = 1
                End If
                Exit For
            Next
        End While
    End Sub

    Private Sub replace_an_hex(ByRef received_string As String)
        Dim returned_string As String = received_string
        Dim hex_to_test As Char
        Dim valid_hex As Boolean = True
        If received_string.Substring(1, 3).ToLower = "h0x" Then
            'VERIFY VALID HEX CHARACTERS, 0 T0 9, A TO F

            For i = 4 To received_string.Length - 5
                hex_to_test = received_string.Substring(i).ToLower
                Select Case hex_to_test
                    Case "a" To "f", "0" To "9"
                    Case Else
                        valid_hex = False
                        Exit For
                End Select
            Next


            If valid_hex Then
                returned_string = parse_hex(received_string.Substring(4, received_string.Length - 5))
                '-5 to not include the prefix ~h0x and the suffix ~
                received_string = returned_string
            End If
        End If
    End Sub


 Function parse_hex(ByVal some_string as string) As String
        Dim new_string As String = ""
        For x = 0 To some_string.Length - 2 Step 2
            new_string &= ChrW(CInt("&H" & some_string.Substring(x, 2)))
        Next
        Return new_string
    End Function


'----------------------------------------------

'old version
'meant for only one hex character at a time
'Private Function contains_hex(ByVal some_string As String) As Boolean
'        If some_string.tolower.Contains("~h0x") Then
'            Return True
'        Else
'            Return False
'        End If
'    End Function
'
'    Public Sub replace_all_hex_in_string(ByVal some_string As String)
'        While contains_hex(some_string)
'            For i = 0 To some_string.Count - 1
'                If some_string.Substring(i, 1) = "~" Then
'                    replace_an_hex(some_string.Substring(i, 6))
'                End If
'                Exit For
'            Next
'        End While
'    End Sub
'
'
'
'    Private Sub replace_an_hex(ByRef received_string As String)
'        Dim returned_string As String = received_string
'        If received_string.Substring(1, 3) = "h0x" Then
'            If IsNumeric(received_string.Substring(4, 2)) Then
'                returned_string = ChrW(CInt("&H" & received_string.Substring(4, 2)))
'            End If
'        End If
'    End Sub



end module