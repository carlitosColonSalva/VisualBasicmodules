Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim MYCONNECTION As New SqlClient.SqlConnection("Server=;Database=;User Id=;Password=;")
        MYCONNECTION.Open()
        Dim queryString As String = "SELECT top 5 document, goods FROM some_database.dbo.PHEADS;"
        Dim COMMAND As New SqlClient.SqlCommand(queryString, MYCONNECTION)
        Dim READER As SqlClient.SqlDataReader = COMMAND.ExecuteReader

        While READER.Read


            MsgBox(READER.GetValue(0) & " " & READER.GetValue(1) & ", ")

            Console.WriteLine(READER.GetValue(0) & " " & READER.GetValue(1) & ", ")

            Console.WriteLine()
        End While

        queryString = "select top 5 A1, PREFIX, KEYCODE from some_database.dbo.codes where prefix = 'WHL'"
        COMMAND = New SqlClient.SqlCommand(queryString, MYCONNECTION)
        READER.Close()
        READER = COMMAND.ExecuteReader
        While READER.Read
            For I = 0 To READER.FieldCount - 1

                Console.WriteLine(READER.GetValue(I))
            Next
            Console.WriteLine("ENDS")
        End While
        MYCONNECTION.Close()
    End Sub

End Class
