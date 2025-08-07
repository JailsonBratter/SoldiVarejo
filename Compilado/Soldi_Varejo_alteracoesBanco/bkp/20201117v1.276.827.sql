

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Pedido_Venda_Analitico]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Pedido_Venda_Analitico]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Analitico]
            @Filial					As Varchar(20),
            @DataDe                 As Varchar(8),
            @DataAte				As Varchar(8),
			@tipo					As Varchar(20),
			@horaInicio				As Varchar(5),
			@horaFim				As Varchar(5),
			@PLU					As Varchar(17) = '',
			@REF_Fornecedor			As Varchar(20) = '',
            @Descricao				As Varchar(60) = '',
			@Cliente				As Varchar(40) = '',
			@Simples    			As VarChar(3) = '',
			@Vendedor				as Varchar(30) = ''			
AS
   -- exec  sp_Rel_Pedido_Venda_Analitico   @Filial='MATRIZ', @tipo = 'Cadastro',  @datade = '20190601',  @dataate = '20201117',  @horaInicio = '00:00',  @horaFim = '23:59',  @Plu = '',  @Ref_Fornecedor = '',  @Descricao = '',  @cliente = '',  @simples = 'TODOS',  @Vendedor = 'TODOS' 
   
	Declare @String               As nVarchar(3000)
	Declare @String2			  As nVarchar(1024)
	Declare @Where                As nVarchar(1024)

	SET @Where = ' WHERE PEDIDO.FILIAL='+ CHAR(39) +@Filial+ CHAR(39) +' AND Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
	SET @Where = @Where + 'AND Pedido.Data_'+@tipo+' BETWEEN '  + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)	
	IF @TIPO = 'Cadastro'
		Set @Where = @Where +' and hora_cadastro '

	ELSE 
		Set @Where = @Where +' and hora_cadastro '

	SET @Where = @Where + 'Between '+ char(39) +@horaInicio +char(39) +' AND ' + CHAR(39) + @horaFim + CHAR(39)

	--//Filtro PLU
	IF @PLU <> ''
		SET @Where = @Where + ' AND Pedido_Itens.PLU = ' + CHAR(39) + @PLU + CHAR(39)
	--** Filtro Descricao	
	IF @Descricao <> ''
		SET @Where = @Where + ' AND Mercadoria.Descricao LIKE' + CHAR(39) + '%' + @Descricao + '%' + CHAR(39)
	--** Filtro Ref_Fornecedor
	IF @REF_Fornecedor <> ''
		SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE' + CHAR(39) + @REF_Fornecedor + '%' + CHAR(39)
	--** Filtro Cliente
	IF @Cliente <> ''
		BEGIN
			SET @Where = @Where + ' AND (Cliente.Nome_Cliente LIKE ' + CHAR(39) + '%' + @Cliente + '%' + CHAR(39)
			SET @Where = @Where + '	OR Replace(Replace(Replace(Cliente.CNPJ, ' + CHAR(39) + '.' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '/' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '-' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ') LIKE ' + CHAR(39) + '%' + @Cliente + '%'+ CHAR(39) + ')'
		END
	--//Filtro Vendedor
	IF @Vendedor <> 'TODOS'
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)		
	-- ** Filtro Pedido Simples
	IF @Simples <> 'TODOS' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' [Data Cadastro]= CONVERT(VARCHAR, Pedido.Data_cadastro, 103), '
		SET @String = @String + ' [Hora Cadastro]= Pedido.hora_cadastro,'
		SET @String = @String + ' [Data Entrega]= CONVERT(VARCHAR, Pedido.Data_entrega, 103), '
		SET @String = @String + ' [Hora Entrega]= Pedido.hora,'
		SET @String = @String + ' Pedido.Pedido,'
		SET @String = @String + ' Cliente.Nome_Cliente, ISNULL(Pedido.Funcionario, ' + CHAR(39) + CHAR(39) + ') AS Vendedor, MERCADORIA.PLU, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String = @String + ' Qtde= CONVERT(NUMERIC(12,2), ISNULL(SUM(PEDIDO_ITENS.QTDE * Pedido_Itens.Embalagem), 0)), '
		SET @String = @String + ' Vlr = CONVERT(DECIMAL(12,2), ISNULL(SUM(PEDIDO_ITENS.TOTAL),0))'
		SET @String = @String + ' FROM Pedido '
		SET @String2 = ' INNER JOIN Pedido_ITENS ON PEDIDO_ITENS.Pedido = PEDIDO.Pedido and pedido.tipo=pedido_itens.tipo'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria ON Mercadoria.PLU = Pedido_itens.PLU '
		SET @String2 = @String2 + ' INNER JOIN Mercadoria_Loja ON MERCADORIA_LOJA.FILIAL=PEDIDO.FILIAL AND  Pedido_itens.PLU = MERCADORIA_LOJA.PLU '
		SET @String2 = @String2 + ' INNER JOIN Cliente ON cliente.Codigo_Cliente = pedido.Cliente_Fornec '
		SET @String2 = @String2 + @Where 
		SET @String2 = @String2 + ' GROUP BY MERCADORIA.PLU, '
		SET @String2 = @String2 + ' Pedido.Data_cadastro, Pedido.Pedido, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente,Pedido.hora_cadastro,Pedido.Data_entrega,Pedido.hora '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)

go 
insert into Versoes_Atualizadas select 'Versão:1.276.827', getdate();