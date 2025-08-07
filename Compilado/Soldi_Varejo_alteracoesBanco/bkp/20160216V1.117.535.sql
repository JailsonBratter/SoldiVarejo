IF (SELECT COUNT(*) FROM Tipo_movimentacao WHERE Movimentacao ='DEVOLUCAO') =0
BEGIN
	INSERT INTO Tipo_movimentacao VALUES('DEVOLUCAO',0,0,1);
END