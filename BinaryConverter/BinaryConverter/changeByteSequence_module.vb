Module changeByteSequence_module
    Private byteSequenceA As Int16() = {0, 1, 2, 3}
    Private byteSequenceB As Int16() = {1, 0, 3, 2}
    Private byteSequenceC As Int16() = {2, 3, 0, 1}
    Private byteSequenceD As Int16() = {3, 2, 1, 0}

    Public Sub startChangingByteSequence(inFile As String, outFile As String, fromFormat As Int16, toFormat As Int16)

        Dim fromSequence As Int16 = fromFormat
        Dim toSequence As Int16 = toFormat

        Dim FILEOUT As New IO.BinaryWriter(IO.File.OpenWrite(outFile))

        Dim allBytes = read_byte_array_from_file(inFile)
        For i = 0 To allBytes.Count - 1 Step 4
            FILEOUT.Write(getBytes({allBytes(i),
                                    allBytes(i + 1),
                                    allBytes(i + 2),
                                    allBytes(i + 3)},
                                    fromSequence,
                                    toSequence))
        Next

        FILEOUT.Close()

    End Sub


    Private Function getBytes(someBytes As Byte(), fromFormat As Int16, toFormat As Int16) As Byte()
        Dim organized As Byte() = {0, 0, 0, 0}

        Select Case fromFormat
            Case 0
                organized = changeBytes(someBytes, byteSequenceA)
            Case 1
                organized = changeBytes(someBytes, byteSequenceB)
            Case 2
                organized = changeBytes(someBytes, byteSequenceC)
            Case 3
                organized = changeBytes(someBytes, byteSequenceD)
            Case Else

        End Select

        Select Case toFormat
            Case 0
                Return changeBytes(organized, byteSequenceA)
            Case 1
                Return changeBytes(organized, byteSequenceB)
            Case 2
                Return changeBytes(organized, byteSequenceC)
            Case 3
                Return changeBytes(organized, byteSequenceD)
            Case Else
                Return changeBytes(organized, byteSequenceA)
        End Select
    End Function

    Private Function changeBytes(someBytes As Byte(), sequence As Int16()) As Byte()
        Return {someBytes(sequence(0)),
                someBytes(sequence(1)),
                someBytes(sequence(2)),
                someBytes(sequence(3))}
    End Function

End Module
