Module console_Main_module

    Private Function process(ByVal arguments As String()) As Boolean
        If Not IsNumeric(arguments(3) Or
           Not IsNumeric(arguments(4))) Then
            Console.Error.WriteLine("from or to format are not numeric")
            Return False
        End If

        Try
            startChangingByteSequence(arguments(1),
                                  arguments(2),
                             CInt(arguments(3)),
                             CInt(arguments(4)))
        Catch this As Exception
            Console.Error.WriteLine("error changing bytes sequence")
            Console.Error.WriteLine(this.Message)
            Console.Error.WriteLine()
            Console.Error.WriteLine(this.ToString)
            Return -1
        End Try
        Return True
    End Function 'process()

    Function processArguments() As Integer
        Dim arguments = Environment.GetCommandLineArgs
        Select Case arguments.Count
            Case 1 'no arguments 
                Console.Error.WriteLine("didn't receive arguments or piped arguments from another executable")
				Console.Error.WriteLine("type /? OR help OR --help for mor info")
                Return -1
            Case 2
                Select Case arguments(1).ToLower
                    Case "/?", "help", "--help"
                        help_explanation_of_program()
                        Return 0
                    Case Else
                        Console.Error.WriteLine("need more arguments")
						showArguments()
                        Return -1
                End Select
            Case 3, 4
                Console.Error.WriteLine("need more arguments")
                showArguments()
                Return -1
            Case 5
                If Not process(arguments) Then Return -1 Else Return 0
            Case Else
				Console.Error.WriteLine("too many arguments")
				showArguments()
                Return -1
        End Select
        Return 0
    End Function 'console_Main()

	public sub showArguments()
		Console.Error.WriteLine("arguments that got")
		for each arg in Environment.GetCommandLineArgs()
			Console.Error.WriteLine(arg)
		next
	end sub

	   public sub help_explanation_of_program()
        Dim help = My.Resources.help_file
        help = help.Replace("this_exe.exe", Environment.GetCommandLineArgs(0)) 'get new executable name
        Dim help_parts As String() = help.Split(vbCrLf)
        Dim lines_count As Integer = 0

        For i = 0 To help_parts.Count - 1
            Console.WriteLine(help_parts(i))
            If lines_count = 5 Then
                lines_count = 0
                If Not Console.IsInputRedirected And
                 Not Console.IsOutputRedirected Then
                    Console.WriteLine()
                    Console.Write("more..")
                    Console.ReadKey()
                    Console.CursorLeft = Console.CursorLeft = 0
                    'clear press key line
                    For j = 1 To 7
                        Console.Write(" ")
                    Next
                    Console.CursorLeft = Console.CursorLeft = 0
					Console.CursorTop -= 1
                End If
            End If
			lines_count += 1
        Next

        If Not Console.IsInputRedirected And
        Not Console.IsOutputRedirected Then
            Console.Write("help done..")
            Console.ReadKey()
            'clear press key line
            Console.CursorLeft = Console.CursorLeft = 0
            For j = 1 To 12
                Console.Write(" ")
            Next
        End If
    End Sub

End Module 'console_Main_module