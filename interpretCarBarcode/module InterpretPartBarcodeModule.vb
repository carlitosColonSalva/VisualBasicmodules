module InterpretPartBarcodeModule
    Dim manualEntryOfPart = False

    Public Function processBarcodes(ByVal argument As String) As String
        Dim allParts = ""

        manualEntryOfPart = False
        allParts = showResult(argument)

        manualEntryOfPart = True


        Console.WriteLine()
        If allParts = "" Then
            allParts = showResult(argument)
        Else
            allParts &= "," & showResult(argument)
        End If


        Dim partsLines As New List(Of String)
        partsLines.AddRange(allParts.Split(",").Distinct.ToArray)
        Dim partsToline = ""

        For i = 0 To partsLines.Count - 1
            If partsLines(i) <> "" Then
                Console.WriteLine(partsLines(i))
                If i <> 0 Then
                    partsToline &= "," & partsLines(i)
                Else
                    partsToline &= partsLines(i)
                End If
            End If
        Next


        partsToline = partsToline.Trim(",").Replace(",", "','")

        partsToline = "'" & partsToline & "'"


        Return partsToline
    End Function 'process()


    Private Function partsListToLine(parts As String()) As String
        Dim line = ""
        For i = 0 To parts.Count - 1
            If parts(i) <> "" And
                parts(i).Length <> 3 Then

                If parts(i).Contains(vbCr) Then

                    For j = 0 To parts(i).Split(vbCr).Count - 1
                        line &= "'" & parts(i).Split(vbCr)(j) & "'"
                        If i <> parts.Count - 1 Then
                            line &= ", "
                        End If
                    Next
                Else
                    line &= "'" & parts(i) & "'"
                    If i <> parts.Count - 1 Then
                        line &= ", "
                    End If
                End If

            End If
        Next

        If line.Length >= 2 Then
            If Right(line, 2) = ", " Then
                line = line.TrimEnd(" ")
            End If
        End If
        line = line.Trim(",")
        Return line
    End Function

    ''' <summary>
    ''' expects parts divided by comma 
    ''' and each part must be inside single quotes
    ''' </summary>
    ''' <param name="parts"></param>
    ''' <returns></returns>
    Public Function queryGetPartsExist(ByRef parts As String) As String
        Dim query = "select keycode from product with (nolock) where keycode in (" &
            parts &
            ")"
        Return query
    End Function

    

    Private Function genericBarcodeToPart(ByVal Barcode As String) As String
        If Barcode.Length < 4 Then
            Return ""
        End If
'part number can't be less than 4 characters
        Return Barcode
    End Function


    Private Function mazdaBarcodeToPart(ByVal Barcode As String) As String
        Dim part = ""

        If Barcode.Length > 15 Then
            Return ""
        End If

        If manualEntryOfPart Then
            If Barcode.Length < 6 Then
                Return ""
            End If

            Barcode = Barcode.Trim(" ")

            'must remove dash if contains
            'or compare that are in correct place: 4-2-4

            If Barcode.Contains("-") Then
                If Barcode.Length >= 5 Then
                    If fragment(Barcode, 4, 1) <> "-" Then
                        Return ""
                    Else
                        Return "MAZ" & Barcode
                    End If
                End If
            Else
                'convert (add "-" every 4,2,4.. characters)
                For i = 0 To Barcode.Length - 1
                    If fragment(Barcode, i, 1) <> "-" Then
                        part &= fragment(Barcode, i, 1)
                        Select Case i
                            Case 3, 5, 9
                                part &= "-"
                            Case Else
                        End Select
                    End If
                Next

                'remove last character if is "-"
                If Right(part, 1) = "-" Then
                    part = fragment(part, 0, part.Length - 1)
                End If
            End If
            Return $"MAZ{part}" 'after manually entered

            'probably should not have "-" if manually entered


        Else 'scanned barcode
            'this only applies if scanned
            'verify if contains dash, that is the last
            If Barcode.Contains("-") Then
                If Right(Barcode, 1) <> "-" Then
                    Return "" 'because barcode only contains dash at end  as check digit
                End If
            End If

            'remove last character
            Barcode = fragment(Barcode, 0, Barcode.Length - 1)
            'remove filler spaces
            Barcode = Barcode.TrimEnd(" ")
        End If

        removeCharactersAfterSpace(Barcode)

        'convert (add "-" every 4,2,4.. characters)
        For i = 0 To Barcode.Length - 1
            If fragment(Barcode, i, 1) <> "-" Then
                part &= fragment(Barcode, i, 1)
                Select Case i
                    Case 3, 5, 9
                        part &= "-"
                    Case Else
                End Select
            End If
        Next

        'remove last character if is "-"
        If Right(part, 1) = "-" Then
            part = fragment(part, 0, part.Length - 1)
        End If

        Return "MAZ" & part

    End Function

    Private Function hyundaiKiaConvertBarcodeToPart(ByVal Barcode As String) As String
        Dim part = ""
        Dim extraPart = ""
        If manualEntryOfPart Then
            Select Case Barcode.Length
                Case 8 To 14
                    If Barcode.Contains(" ") Then
                        Return ""
                    End If
                    If Not Barcode.Contains("-") Then
                        addDashEvery5Characters(Barcode)
                    End If
                    part = Barcode
                Case Else 'barcode lenth < 7 and > 14
                    Return ""
            End Select
        Else 'scanned
            Barcode = Barcode.TrimEnd(" ")
            Select Case Barcode.Length

                Case >= 8
                    'good
                Case Else 'bad part number
                    Return ""
            End Select

            Dim characterDivder = getCharacterDivider(Barcode)
            Select Case characterDivder
                Case "-"
                    If Barcode.Contains(" ") Then
                        'part is first half of space
                        Barcode = Barcode.Split(" ")(0)
                    Else
                        'done
                    End If
                    'don't need to add dash
                    part = Barcode
                Case " "
                    '**** must get everything before last space ***'
                    'group of spaces found?
                    '2 possibilites
                    'part could be first half of first space
                    part = Barcode.Split(" ")(0)
                    addDashEvery5Characters(part)
                    'or part could be first half of second space
                    If contains2_OrMoreSpaces(Barcode) Then
                        extraPart = Barcode.Split(" ")(0) & Barcode.Split("")(1)
                        addDashEvery5Characters(extraPart)
                    Else
                        extraPart = Barcode.Replace(" ", "")
                        addDashEvery5Characters(extraPart)
                    End If
                Case Else
                    If Barcode.Contains(" ") Then
                        'part is first half of space
                        Barcode = Barcode.Split(" ")(0)
                    Else
                        'good
                    End If
                    'must add dash(es)
                    addDashEvery5Characters(Barcode)
                    part = Barcode
            End Select
        End If
        '---------------

        removeLastCharacterIfIsDash(part)
        removeLastCharacterIfIsDash(extraPart)

        Return "HYU" & part & vbCr &
                   "HYU" & part.Replace("-", "") & vbCr & 'no dash
                   "HYU" & extraPart & vbCr &
                   "HYU" & extraPart.Replace("-", "") & vbCr & 'no dash
                   "KIA" & part & vbCr &
                   "KIA" & part.Replace("-", "") & 'no dash
                   "KIA" & extraPart & vbCr &
                   "KIA" & extraPart.Replace("-", "")  'no dash
    End Function

    Private Function suzukiConvertBarcodeToPart(ByVal Barcode As String) As String
        Dim part = ""

        If Barcode.Length < 8 Or
                Barcode.Length > 16 Then
            Return ""
        End If




        If manualEntryOfPart Then
            If Not Barcode.Contains("-") Then 'have to add dashes
                part = fragment(Barcode, 0, 5) &
                    "-"
                If Barcode.Length <= 10 Then
                    part &= fragment(Barcode, 5)
                Else
                    part &= fragment(Barcode, 5, 5) &
                        "-" & fragment(Barcode, 10)
                End If
                Return ""
            Else 'Barcode.Contains -
                Return "SUZ" & Barcode
            End If
        Else 'scanned

            Select Case Barcode.Length
                Case 11, 16

                Case Else
                    Return ""
            End Select

            If Barcode.Contains("-") Then
                'confirm contains the 2 dashes
                If fragment(Barcode, 5, 1) <> "-" Then
                    Return ""
                End If


                If Barcode.Length >= 12 Then
                    If fragment(Barcode, 11, 1) <> "-" Then
                        Return ""
                    End If
                End If
            Else 'dont contain -
                Return ""
            End If


        End If




        'create part
        Dim partEnd = 0

        Dim foundPartEnd = False
        Dim character = "0"


        For i = Barcode.Length - 1 To 0 Step -1
            character = fragment(Barcode, i, 1)
            If character <> "0" And
                        character <> "-" Then
                partEnd = i + 1
                foundPartEnd = True
                Exit For
            End If
        Next

        If Not foundPartEnd Then MsgBox("ERROR")
        part = fragment(Barcode, 0, partEnd)
        'remove last character if is "-"
        If fragment(part, part.Length - 1, 1) = "-" Then
            part = fragment(part, 0, part.Length - 2)
        End If

        Return "SUZ" & part
    End Function

    Private Function chryslerConvertBarcodeToPart(ByVal Barcode As String, Optional linecode As String = "MOP") As String

        If Barcode.Length < 7 Or
                Barcode.Length > 15 Then
            Return ""
        End If


        Dim part = ""

        Select Case Barcode.Length
            Case 9
                Return ""
            Case 8, 10
                If Barcode.Contains("-") Then
                    Return ""
                End If
                part = Barcode
                Return linecode & part
            Case Else
                part = Barcode
                Return linecode & part

        End Select


        If manualEntryOfPart Then
            Select Case Barcode.Length
                Case > 10
                    Return ""
                Case 8, 10
                    Return linecode & Barcode
                Case Else
                    Return ""
            End Select
        Else 'scanned
            If fragment(Barcode, 0, 7).Contains("-") Then
                Return ""
            End If



            Select Case Barcode.Length
                Case > 10
                    'check ending
                    If Not IsNumeric(Right(Barcode, 3)) Then
                        Return ""
                    Else
                        If fragment(Barcode, 10).Contains("-") Then
                            Return linecode & Barcode.Split("-")(0)
                        Else
                            Return linecode &
                              fragment(Barcode, 0, Barcode.Length - 3)
                        End If
                    End If

                Case 8, 10
                    Return linecode &
                        Barcode

                Case Else
                    Return ""
            End Select
        End If

        part = fragment(Barcode, 0, 10)

        Return linecode & part
    End Function

    Private Function chryslerWithZeroConvertBarcodeToPart(ByVal Barcode As String) As String
        If manualEntryOfPart Then
            Return ""
        Else
            Select Case Barcode.Length
                Case 8, 10
                    If Left(Barcode, 1) = "0" Then
                        Return "MOP" & fragment(Barcode, 1)
                    End If
                Case Else

            End Select
        End If
        Return ""
    End Function


    Private Function mitsubishiConvertBarcodeToPart(ByVal Barcode As String) As String

        If Barcode.Length < 6 Then
            Return ""
        End If

        If Barcode.Contains("-") Then
            Return ""
        End If

        If manualEntryOfPart Then
            If Barcode.Contains(" ") Then
                Return ""
            Else
                Return "MIT" & Barcode
            End If
        Else 'scanned
            If Barcode.Contains(" ") Then
                If Not IsNumeric(Right(Barcode, 3)) Then
                    Return ""
                End If
                Barcode = Barcode.Split(" ")(0)
            Else 'Barcode DONT Contains SPACE 
                If Not IsNumeric(Right(Barcode, 4)) Then
                    Return ""
                End If
                Barcode = fragment(Barcode, 0, Barcode.Length - 4)
            End If
        End If



        Return "MIT" & Barcode
    End Function

    Private Function hondaConvertBarcodeToPart(ByVal Barcode As String) As String
        Dim part = ""

        'verify last character is not -
        If Barcode.Contains("-") Then
            If Barcode.Length > 3 Then
                If fragment(Barcode, Barcode.Length - 2, 1) = "-" Then
                    Return ""
                End If
            Else
                Return ""
            End If
        End If


        If manualEntryOfPart Then
            If Barcode.Contains("/") Then
                Return ""
            End If
            If Not Barcode.Contains("-") Then
                Select Case Barcode.Length
                    Case 8 To 10
                        Return "HON" &
                            Left(Barcode, 5) & "-" &
                         fragment(Barcode, 5)
                    Case 11 To 13
                        Dim PREFIX = "HON" &
                            Left(Barcode, 5) & "-"
                        Return PREFIX &
                          fragment(Barcode, 5, 3) & "-" &
                          fragment(Barcode, 8) &
                            vbCr &
                            PREFIX & fragment(Barcode, 5)

                    Case Else
                        Return ""
                End Select
            Else 'Barcode.Contains - (MANUAL)
                If fragment(Barcode, 5, 1) <> "-" Then
                    Return ""
                End If
                Return "HON" & Barcode
            End If
        Else 'scanned
            If Barcode.Contains("/") Then
                If fragment(Barcode, Barcode.Length - 2, 1) <> "/" Then 'last character not /
                    Barcode = Barcode.Replace("/", "-")
                Else
                    Return ""
                End If
            End If



            'new part
            Select Case Barcode.Length
                Case 10
                    If Barcode.Contains("-") Then
                        Return ""
                    End If
                Case 11
                    If Not Barcode.Contains("-") Then
                        part = fragment(Barcode, 0, 5) & "-" &
                      fragment(Barcode, 5, 3) & "-" &
                     fragment(Barcode, 8)
                    Else
                        If fragment(Barcode, 5, 1) <> "-" Then
                            Return ""
                        Else
                            part = Barcode
                        End If
                    End If

                Case 12
                    If Barcode.Contains("-") Then
                        Return ""
                    Else
                        Barcode = fragment(Barcode, 0, 5) & "-" &
                    fragment(Barcode, 5, 3) & "-" &
                      fragment(Barcode, 8)
                    End If
                Case 13
                    If Barcode.Contains("-") Then
                        If fragment(Barcode, 5, 1) <> "-" Or
                   fragment(Barcode, 9, 1) <> "-" Then
                            Return ""
                        Else
                            part = Barcode
                        End If
                    Else
                        part = fragment(Barcode, 0, 5) & "-" &
                      fragment(Barcode, 5, 3) & "-" &
                     fragment(Barcode, 8)
                    End If


                Case 14, 15
                    If Not Barcode.Contains("-") Then
                        Return ""
                    Else
                        If fragment(Barcode, 5, 1) <> "-" Or
                      fragment(Barcode, 9, 1) <> "-" Then
                            Return ""
                        Else
                            part = Barcode
                        End If

                    End If
                Case Else
                    Return ""
            End Select
        End If
        Return "HON" & part
    End Function

    Private Function partsFromBarcodeToList(barcode As String) As String()

        Dim suzukiPart = ""
        Dim hyudaiKiaPart = ""
        Dim mazdaPart = ""
        Dim chryslerPart = ""
        Dim mitsubishiPart = ""
        Dim hondaPart = ""
        Dim genericPart = ""
        Dim crownPart = ""
        Dim chryslerPartWithOutZero = ""

        mazdaPart = mazdaBarcodeToPart(barcode)

        suzukiPart = suzukiConvertBarcodeToPart(barcode)

        chryslerPart = chryslerConvertBarcodeToPart(barcode)
        crownPart = chryslerConvertBarcodeToPart(barcode, "CRO")
        chryslerPartWithOutZero = chryslerWithZeroConvertBarcodeToPart(barcode)
        mitsubishiPart = mitsubishiConvertBarcodeToPart(barcode)
        hyudaiKiaPart = hyundaiKiaConvertBarcodeToPart(barcode)

        hondaPart = hondaConvertBarcodeToPart(barcode)

        remPart = RemConvertBarcodeTopart(barcode)

        genericPart = genericBarcodeToPart(barcode)

        Return {mazdaPart, suzukiPart, hondaPart, chryslerPart, chryslerPartWithOutZero, crownPart, mitsubishiPart, hyudaiKiaPart, genericPart, remPart}
    End Function

    Public Function showResult(ByVal argument As String) As String
        Dim parts As String = ""
        For Each bar In partsFromBarcodeToList(argument)
            If bar.Length > 3 Then
                If bar.Contains(vbCr) Then
                    For Each b In bar.Split(vbCr)
                        If b.Length <> 0 And
                                b.Length <> 3 Then
                            ' Console.WriteLine(b)
                            parts &= b & ","
                        End If
                    Next
                Else 'bar not Contains(vbCr)
                    ' Console.WriteLine(bar)
                    parts &= bar & ","
                End If
            End If
        Next

        parts = parts.TrimEnd(",")
        Return parts
    End Function

    Public Function getPartsThatMustBeInsertedInStock(ByVal partsExist As String, ByVal partsFound As String) As String
        Dim partsThatMustBeInserted As New List(Of String)
        If partsFound.Split(vbCr).Count < partsExist.Split(vbCr).Count Then
            'a part must be added to stock

            Dim currPfound = False
            For Each p As String In partsExist
                currPfound = False
                For Each f As String In partsFound.Split(vbCr)
                    If p.Replace("'", "") = f.Split(vbTab)(0) Then
                        currPfound = True
                        Exit For
                    End If
                Next
                If Not currPfound Then
                    partsThatMustBeInserted.Add(p.Replace("'", ""))
                End If
            Next

        End If
        Return String.Join(vbCr, partsThatMustBeInserted.toarray)
    End Function

    Private Sub removeCharactersAfterSpace(ByRef info As String)
        If info.Length = 0 Then Exit Sub
        If Not info.Contains(" ") Then Exit Sub
        Dim newInfo As String = ""
        For i = 0 To info.Length - 1
            If info.Substring(i, 1) = " " Then Exit For
            newInfo &= info.Substring(i, 1)
        Next
        info = newInfo
    End Sub

    Function getNonSpecialCharacters(someString As String) As String
        If someString = "" Then Return ""
        Dim newString = "" 'value to return
        Dim evaluatedCharacter As Char = "a"
        For i = 0 To someString.Count - 1 'each character
            evaluatedCharacter = someString.Substring(i, 1)

            Select Case Asc(evaluatedCharacter)
                Case 48 To 57, 65 To 90, 97 To 122 'normal characteer
                    '0-9 A-Z a-z
                    newString &= evaluatedCharacter
                Case Else
                    'special character
            End Select

        Next
        Return newString
    End Function 'getNonSpecialCharacters

    Function getCharacterDivider(someString As String) As String
        '1234567890123
        '12345-78901-3
        '12345 78901 3
        '12345-78901-3    0001
        '12345 78901 3    0001
        'will contain no more than 2 spaces or 2 dashes
        'valid for the part

        If someString.Length < 6 Then Return ""

        Select Case someString.Substring(5, 1) '6th character
            Case " ", "-"
                Return someString.Substring(5, 1)
            Case Else
                Return ""
        End Select


    End Function

    Sub addDashEvery5Characters(ByRef barcode As String)
        Dim newBarcode = ""
        For i = 0 To barcode.Length - 1
            newBarcode &= fragment(barcode, i, 1)
            Select Case i
                Case 4, 9, 14
                    newBarcode &= "-"
                Case Else
            End Select
        Next
        barcode = newBarcode
    End Sub

    Sub removeLastCharacterIfIsDash(ByRef someString As String)
        If someString.Length = 0 Then Exit Sub
        If Right(someString, 1) = "-" Then
            someString = fragment(someString, 0, someString.Length - 1)
        End If

    End Sub

    Function contains2_OrMoreSpaces(someString As String) As Boolean
        If Not someString.Contains(" ") Then Return False
        If someString.Split(" ").Count > 2 Then
            Return True
        Else
            Return False
        End If
    End Function
End module 'InterpretPartBarcodeModule