Module Module1


    Function character_to_base64(ByVal a_byte As Byte) As Byte()
        Dim a_character As Byte() = {a_byte}
        Dim transformed_word As New System.Security.Cryptography.ToBase64Transform
        Return transformed_word.TransformFinalBlock(a_character, 0, 1)
    End Function

    Function base64_to_character(ByVal a_base64_character As Byte()) As Byte
        Dim normal_character() As Byte = {0, 0, 0, 0}
        Dim returned_word As New Security.Cryptography.FromBase64Transform
        Dim result = returned_word.TransformBlock(a_base64_character, 0, 4, normal_character, 0)
        Return normal_character(0)
    End Function

    Sub Main()

        Dim mykey = Console.ReadKey.KeyChar
        Dim encoded = character_to_base64(Asc(mykey))

        Console.WriteLine()
        For i = 0 To encoded.Count - 1
            Console.Write(Chr(encoded(i)))
        Next
        Console.WriteLine()

        Dim neww = base64_to_character(encoded)


        Console.WriteLine(Chr(neww))
        Console.ReadLine()
    End Sub

End Module
