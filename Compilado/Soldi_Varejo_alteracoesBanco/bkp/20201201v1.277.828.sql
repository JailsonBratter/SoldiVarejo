GO

 IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Pedido_Itens') 
            AND  UPPER(COLUMN_NAME) = UPPER('Agrupamento'))
begin
	alter table Pedido_Itens alter column Agrupamento VARCHAR(50)
end
else
begin
	alter table Pedido_Itens add Agrupamento VARCHAR(50)
end 

 go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Pedido_Venda_Producao]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Pedido_Venda_Producao]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Producao]
	@Filial         As Varchar(20),
	@DataDe         As Varchar(8),
	@DataAte        As Varchar(8),
	@Agrupamento    As Varchar(20),
	@horaInicio     As Varchar(5),
	@horaFim        As Varchar(5),
	@PLU            As Varchar(17) = '',
	@REF_Fornecedor As Varchar(20) = '',
	@Descricao      As Varchar(60) = '',
	@Cliente        As Varchar(40) = '',
	@Vendedor       AS VARCHAR(30) = ''

AS
Begin
   -- exec  sp_Rel_Pedido_Venda_Producao   @Filial='MATRIZ', @datade = '20201125',  @dataate = '20201125',  @horaInicio = '00:00',  @horaFim = '23:59', @Agrupamento = '', @Plu = '',  @Ref_Fornecedor = '',  @Descricao = '',  @cliente = '',   @Vendedor = 'TODOS'
	Declare @String    As nVarchar(3000)
	Declare @String2   As nVarchar(1024)
	Declare @Where     As nVarchar(1024)

	SET @Where = ' WHERE PEDIDO.FILIAL='+ CHAR(39) +@Filial+ CHAR(39) +' AND Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) AND ISNULL(Pedido_Itens.Produzir, 0) = 1'

	SET @Where = @Where + ' AND REPLACE(CONVERT(VARCHAR, Pedido_Itens.data_hora_produzir, 102), ' + CHAR(39) + '.' + CHAR(39) + ', ' + CHAR(39) + CHAR(39) +')  BETWEEN '  + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)         

	SET @Where = @Where +' AND SUBSTRING(CONVERT(VARCHAR, Pedido_Itens.data_hora_produzir, 8), 1, 5)  BETWEEN ' + CHAR(39) + @horaInicio + CHAR(39) + ' AND ' + CHAR(39) + @horaFim + CHAR(39)

	IF @Agrupamento <> ''
		SET @Where = @Where + ' AND ISNULL(Pedido_Itens.Agrupamento, ' + CHAR(39) + CHAR(39) + ') = ' + CHAR(39) + @Agrupamento + CHAR(39)

	IF @PLU <> ''
		SET @Where = @Where + ' AND Pedido_Itens.PLU = ' + CHAR(39) + @PLU + CHAR(39)

	IF @Descricao <> ''
		SET @Where = @Where + ' AND Mercadoria.Descricao LIKE' + CHAR(39) + '%' + @Descricao + '%' + CHAR(39)

	IF @REF_Fornecedor <> ''
		SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE' + CHAR(39) + @REF_Fornecedor + '%' + CHAR(39)

	IF @Cliente <> ''
	BEGIN
		SET @Where = @Where + ' AND (Cliente.Nome_Cliente LIKE ' + CHAR(39) + '%' + @Cliente + '%' + CHAR(39)
		SET @Where = @Where + '        OR Replace(Replace(Replace(Cliente.CNPJ, ' + CHAR(39) + '.' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '/' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '-' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ') LIKE ' + CHAR(39) + '%' + @Cliente + '%'+ CHAR(39) + ')'
	END

	IF @Vendedor <> 'TODOS'
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)                 
	

	SET @String = 'SELECT'

	SET @String = @String + ' ROW_NUMBER() OVER (ORDER BY Pedido_Itens.data_hora_produzir) Seq, '

	SET @String = @String + ' Pedido.pedido,'

	SET @String = @String + ' isnull(Pedido_Itens.Agrupamento,' + CHAR(39) + CHAR(39) +') as Agrupamento,'

	SET @String = @String + ' Pedido_Itens.Qtde,'

	SET @String = @String + ' Pedido_Itens.PLU,'

	SET @String = @String + ' Mercadoria.descricao,'

	SET @String = @String + ' SUBSTRING(CONVERT(VARCHAR,Pedido_Itens.data_hora_produzir,21), 1, 16) as data_hora,'

	SET @String = @String + ' Pedido_Itens.Obs'

	SET @String = @String + ' INTO #lixo'

	SET @String = @String + ' FROM'

	SET @String = @String + ' Pedido'

	SET @String = @String + ' INNER JOIN Pedido_itens ON Pedido.pedido = Pedido_Itens.pedido'

	SET @String = @String + ' INNER JOIN mercadoria ON Mercadoria.plu = Pedido_Itens.plu'

	SET @String = @String + @Where

	SET @String = @String + ' SELECT pedido, agrupamento, qtde, plu, descricao, data_hora + ' + char(39) + ' Seq.' + char(39) + ' + REPLICATE(' + CHAR(39) + '0' + CHAR(39) + ', 3 - LEN(CONVERT(VARCHAR, Seq))) + CONVERT(VARCHAR, Seq) AS [Data Hora Seq.Prod.]  FROM #lixo'

	SET @String = @String + ' UNION ALL'

	SET @String = @String + ' SELECT ' + CHAR(39) + CHAR(39) + ', '+CHAR(39) + CHAR(39) + ', 0, ' + CHAR(39) + CHAR(39) + ',' + char(39) +'Obs: ' + char(39) + ' + obs,  data_hora + ' + char(39) + ' Seq.' + char(39) + ' + REPLICATE(' + CHAR(39) + '0' + CHAR(39) + ', 3 - LEN(CONVERT(VARCHAR, Seq))) + CONVERT(VARCHAR, Seq) from #lixo where isnull(obs,' + CHAR(39) + CHAR(39) + ') <> ' + CHAR(39) + CHAR(39)

	SET @String = @String + ' ORDER BY 6 ASC, pedido DESC'
		
	EXEC (@String)

 end 


go

insert into Versoes_Atualizadas select 'Versão:1.277.828', getdate();