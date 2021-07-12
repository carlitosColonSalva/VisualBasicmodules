Module binary_module

    Public Function get_bit_array_from_byte(ByVal byte_info As Byte) As Boolean()
        Dim numbers() As Int16 = {128, 64, 32, 16, 8, 4, 2, 1}
        Dim questions(8) As Boolean
        For i = 0 To numbers.Count - 1
            If byte_info >= numbers(i) Then
                questions(i) = True
                byte_info -= numbers(i)
            Else
                questions(i) = False
            End If
        Next
        Return questions
    End Function

    Public Function read_byte_from_file(ByVal filename As String) As Boolean()
        Dim questions(8) As Boolean
        Dim file_in As New IO.BinaryReader(IO.File.OpenRead(filename))

        Dim byte_in As Byte = file_in.ReadByte
        file_in.Close()

        Dim numbers() As Int16 = {128, 64, 32, 16, 8, 4, 2, 1}

        For i = 0 To numbers.Count - 1
            If byte_in >= numbers(i) Then
                questions(i) = True
                byte_in -= numbers(i)
            Else
                questions(i) = False
            End If
        Next
        Return questions
    End Function

    Public Function read_byte_array_from_file(ByVal filename As String) As Byte()
        Return IO.File.ReadAllBytes(filename)
    End Function

    Public Function get_byte_from_bit_array(ByVal bits As Boolean()) As Byte
        Dim questions() As Boolean = bits

        Dim total As Int16 = 0

        Dim divide_count As Integer = 128
        Dim total_out As Byte

        For i = 0 To 7
            If questions(i) = True Then
                total += divide_count
            End If
            divide_count /= 2
        Next
        divide_count = 0w

        total_out = total
        Return total_out
    End Function

    Public Sub write_byte_to_file(ByVal filename As String, ByVal bits As Boolean())

        Dim questions() As Boolean = bits

        Dim total As Int16 = 0

        Dim divide_count As Integer = 128
        Dim total_out As Byte

        For i = 0 To 7
            If questions(i) = True Then
                total += divide_count
            End If
            divide_count /= 2
        Next
        divide_count = 0

        total_out = total


        Dim FILEOUT As New IO.BinaryWriter(IO.File.OpenWrite(filename))

        FILEOUT.Write(total_out)
        FILEOUT.Close()
    End Sub

    Public Sub write_byte_array_to_file(ByVal filename As String, ByVal byte_array As Byte())
        Dim FILEOUT As New IO.BinaryWriter(IO.File.OpenWrite(filename))
        FILEOUT.Write(byte_array)
        FILEOUT.Close()
    End Sub


public sub invert_4_bytes_order(byref int_bytes() as byte)

dim temp = int_bytes(0)
int_bytes(0) = int_bytes(3)
int_bytes(3) = temp

        temp = int_bytes(1)
        int_bytes(1) = int_bytes(2)
        int_bytes(2) = temp
    End sub

public function get_4_bytes_in_Big_Endian_from_int32(byval some_int as int32) as byte()

        Dim bits As New Queue(Of Boolean)
        Dim bytes as new queue (of byte)
        Dim new_byte As Byte = 0
        Dim bitValue As Byte = 0
        Dim majorBitValue As UInt64 = 0

        For i = 3 To 0 Step -1 'each byte unit
            For k = 7 To 0 Step -1 'each bit 
                bitValue = 2 ^ k '128,64,32,...
                majorBitValue = 256 ^ i * (bitValue)
                If some_int >= majorBitValue Then
                    some_int -= majorBitValue
                    new_byte += bitValue
                End If
            Next k

            bytes.Enqueue(new_byte)
            new_byte = 0
        Next i

        Return bytes.ToArray
    End function 'get_4_bytes_in_Big_Endian_from_int32


public function get_4_bytes_in_Little_Endian_from_int32(byval some_int as int32) as byte()
dim BE_bytes = get_4_bytes_in_Big_Endian_from_int32(some_int)
dim LE_bytes = BE_bytes
invert_4_bytes_order(LE_bytes)
return LE_bytes
end function

public function get_2_bytes_in_Big_Endian_from_int32(byval some_int as int16) as byte()
dim bytes_4_BE = get_4_bytes_in_Big_Endian_from_int32(some_int)
Dim BE_Int16 = {bytes_4_BE(2), bytes_4_BE(3)}

return BE_Int16
end function


public function get_2_bytes_in_Little_Endian_from_int32(byval some_int as int16) as byte()
dim LE_4_bytes = get_4_bytes_in_Little_Endian_from_int32(some_int)
dim LE_2_bytes = {LE_4_bytes(0),LE_4_bytes(1)}
return LE_2_bytes
end function 'function get_2_bytes_in_Little_Endian_from_int32()

End Module 'binary_module