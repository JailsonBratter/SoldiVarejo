alter table conta_a_pagar add lancamento_simples tinyint 

go 
alter table conta_a_receber add origem_lancamento varchar(3)
GO 

GO
ALTER TRIGGER [dbo].[tgi_atu_dados] ON [dbo].[Lista_finalizadora] 

FOR INSERT

AS

 
declare @doc as varchar(20),
	@cliente as varchar(20),
	@filial as varchar(20),
	@cupom as varchar(20),
	@autorizacao as varchar(9),
	@nro_parcelas as int,
	@parcela as int,
	@emissao as datetime,
	@id_fin as varchar(20),
	@pdv as int,
	@fin as int,
	@de as varchar(16),
	@valor as decimal(12,2),
	@op as varchar(9),
	@venc as datetime,
	@total as decimal(12,2),
	@taxa as decimal(12,2),
	@cheque as varchar(10),
	@agencia as varchar(6),
	@banco as varchar(3),
	@conta as varchar(10),
	@ptx decimal(12,2),
	@ptxacre decimal(12,2),
	@dias int,
	@bandeira varchar(20),
	@doc_car varchar(10),
	@emissao_car datetime,
	@pdv_car int,
	@fin_car int,
	@de_car varchar(20),
	@acre decimal(12,2)

select @doc = documento,  @cliente = codigo_cliente,  @filial = filial, @cupom = cupom, @autorizacao = autorizacao, @nro_parcelas = numero_parcelas, @parcela = parcela, @emissao = emissao, 
	@bandeira = id_finalizadora, @pdv = pdv, @fin = finalizadora, @de = documento_emitido, @valor = valor, @op = operador, @venc = vencimento, @total = total, @taxa = taxa, 
	@cheque = cheque, @agencia = agencia, @banco = banco, @conta = conta from INSERTED

if @nro_parcelas = 0
begin
	select @nro_parcelas = 1
	select @parcela = 1
end

if @parcela = 0
begin
	select @parcela = 1
end

if @total = 0 or @total is null
begin
	select @total = @valor
end

if  @nro_parcelas > 1
begin
	select @dias = dias, @id_fin = id_cartao, @acre = isnull(acrecimo,0) from cartao where  filial = @filial and nro_finalizadora = @fin and id_cartao = @id_fin and BANDEIRA = @bandeira
	select @ptx = taxa, @ptxacre = 0 from cartao_taxa_parcelamento where filial = @filial and finalizadora = @fin and id_finalizadora = @id_fin and numero_parcelas = @nro_parcelas
	select @valor = (@total / @nro_parcelas)
end

if @ptx is null or  @nro_parcelas = 1
begin
	select @ptx = taxa, @ptxacre = acrecimo, @dias = dias, @id_fin = id_cartao, @acre = isnull(acrecimo,0) from cartao where filial = @filial and nro_finalizadora = @fin and BANDEIRA = @bandeira
end

select @taxa = (@valor * (@ptx/100)) + @acre

if @parcela = 1
begin
	update lista_finalizadora set parcela = @parcela, total = @total, taxa = @taxa, valor = @valor, vencimento = master.dbo.F_BR_PROX_DIA_UTIL(@emissao + (@dias * @parcela)) where documento = @doc and filial = @filial
		and emissao = @emissao and pdv = @pdv and finalizadora = @fin and documento_emitido = @de and documento_emitido_car = '9999999999999999' and cupom = @cupom
		and operador = @op and autorizacao = @autorizacao and parcela <= @parcela and id_finalizadora = @bandeira
	update conta_a_receber set valor = valor + @valor, taxa = isnull(taxa,0) + @taxa, vencimento = master.dbo.F_BR_PROX_DIA_UTIL(@emissao + (isnull(@dias,0) * @parcela))  where  documento = @doc and filial = @filial  and emissao = @emissao and finalizadora = @fin and isnull(origem_lancamento,'') <> 'PED' 
end

if @nro_parcelas > 1
begin
	select @parcela = @parcela+1
	WHILE @parcela <= @nro_parcelas 
	BEGIN
		
		select @doc_car = null
		
		select @doc_car = documento, @emissao_car = emissao, 	@pdv_car = pdv, @fin_car=finalizadora, @de_car = documento_emitido from conta_a_receber 
			where finalizadora = @fin and emissao = @emissao and vencimento = master.dbo.F_BR_PROX_DIA_UTIL(@emissao + (@dias * @parcela)) 
				and id_finalizadora = @bandeira and filial = @filial
		
		if @doc_car is null
		begin
			insert into conta_a_receber (documento, filial, emissao, entrada, pdv, finalizadora, id_finalizadora, documento_emitido, valor, desconto, vencimento, status, taxa, obs) 
				values(ltrim(cast(@cupom as varchar)) + '-P'+cast(@parcela as varchar), @filial, @emissao, @emissao, 0, @fin, @bandeira, '9999999999999999', 0,0, master.dbo.F_BR_PROX_DIA_UTIL(@emissao + (@dias * @parcela)), 1, 0, 'LANCAMENTO AUTOMATICO TGR')
		
			select  @doc_car = ltrim(cast(@cupom as varchar)) + '-P' + cast(@parcela as varchar), @emissao_car = @emissao, @pdv_car = 0, @fin_car = @fin, @de_car = '9999999999999999'
		end

		if @nro_parcelas = (@parcela)
		begin
			select @valor = @valor + (@total - (@valor * @nro_parcelas))
		end

		insert into lista_finalizadora (documento, codigo_cliente, filial, valor, emissao, pdv, finalizadora, documento_emitido, documento_emitido_car, cupom, operador, autorizacao, 
			numero_parcelas, parcela, vencimento, id_finalizadora, total, taxa, cheque, agencia, banco, conta) 
			values(@doc_car, @cliente, @filial, @valor, @emissao, @pdv, @fin, @de, '9999999999999999',
			@cupom, @op, @autorizacao, @nro_parcelas, @parcela, master.dbo.F_BR_PROX_DIA_UTIL(@emissao + (@dias * (@parcela))), @bandeira, @total, @taxa, @cheque,
			@agencia, @banco, @conta)

		update conta_a_receber set valor = valor + @valor, taxa = isnull(taxa,0) + @taxa where  documento = @doc_car and filial = @filial  and emissao = @emissao and finalizadora = @fin

		select @parcela = @parcela+1	
	END
end


go


 
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
			@PLU					As Varchar(17) = '',
			@REF_Fornecedor			As Varchar(20) = '',
            @Descricao				As Varchar(60) = '',
			@Cliente				As Varchar(40) = '',
			@Simples    			As VarChar(3) = '',
			@Vendedor				as Varchar(30) = ''			
AS
                
	Declare @String               As nVarchar(3000)
	Declare @String2			  As nVarchar(1024)
	Declare @Where                As nVarchar(1024)

	SET @Where = ' WHERE PEDIDO.FILIAL='+ CHAR(39) +@Filial+ CHAR(39) +' AND Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
	SET @Where = @Where + 'AND Pedido.Data_Cadastro BETWEEN '  + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)	
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
	IF @Vendedor <> ''
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)		
	-- ** Filtro Pedido Simples
	IF @Simples <> '' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' Data = CONVERT(VARCHAR, Pedido.Data_cadastro, 103) , Pedido.Pedido,'
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
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)

go


INSERT INTO [dbo].[PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('CONT_PAGAR_CC_CAIXA'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,''
           ,'CENTRO CUSTO DE LANCAMENTO CONTA A PAGAR CAIXA'
           ,'C'
           ,''
           ,0
           ,''
           ,0
           ,0
           ,GETDATE()
           ,0)
GO


