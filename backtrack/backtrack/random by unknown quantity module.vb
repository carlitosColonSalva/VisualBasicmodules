module random_by_unknown_quantity_module

function random_by_unknown_quantity(byval quantity as integer) as integer
	if quantity = 0 or quantity = 1 then return 0
	
	dim maxPossible as double = 1.0
	dim piece as double = maxPossible / quantity
        Dim countPieces As Integer = 0
        Dim pieceAcumulator As Double = 0
        Dim RandomGen as new Random
        Dim newRand = RandomGen.NextDouble
        While pieceAcumulator < newRand
            countPieces += 1
            pieceAcumulator += piece
        End While
        Return countPieces

end function 'random_by_unknown_quantity

end module 'random_by_unknown_quantity_module