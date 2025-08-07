
CREATE NONCLUSTERED INDEX [ix_pedido_fluxo_caixa] ON [dbo].[Pedido] 
(
	[Tipo] ASC,
	[pedido_simples] ASC,
	[Data_cadastro] ASC,
	[Status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/****** Object:  Index [Ix_Fluxo_Caixa]    Script Date: 12/04/2015 12:02:25 ******/
CREATE NONCLUSTERED INDEX [Ix_Fluxo_Caixa] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[data_cancelamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/****** Object:  Index [ix_Lista_Fluxo_Caixa]    Script Date: 12/04/2015 12:08:41 ******/
CREATE NONCLUSTERED INDEX [ix_Lista_Fluxo_Caixa] ON [dbo].[Lista_finalizadora] 
(
	[finalizadora] ASC,
	[Emissao] ASC,
	[Cancelado] ASC,
	[filial] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_FluxoCaixa]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End
	
	Create table Totais
(
	Descricao  varchar(90),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque WITH(INDEX(Ix_Fluxo_Caixa)) where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL and  Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
					),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo AND nf_item.Filial = nf.FILIAL where  nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL ),0)
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte  AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(convert(decimal(18,2),isnull(Total,0),0))),'.',',') from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  AND nf_item.Filial = nf.FILIAL where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL 
	UNION ALL
	SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0)),'.',',')
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where Filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10))),'.',',')
	UNION ALL
	SELECT '% DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL ),0))),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL )),0)),'.',',')	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

	UNION ALL
	
	SELECT 'RESULTADO TOTAL',
		replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
	FROM  TOTAIS
	Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and  data_movimento between @DataDe and @DataAte and data_cancelamento is null ))),'.',',')
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
    SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA  WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  





--======================================================================================
ALTER TRIGGER [dbo].[i_SaldoCliente] ON [dbo].[Caderneta] 
FOR INSERT
AS

DECLARE @tipo varchar(20),
	@cliente varchar(8),
	@vlr decimal(12,2),
	@doc varchar(15),
	@emissao datetime,
	@pdv int


select @pdv = caixa_caderneta, @emissao = emissao_caderneta, @doc = documento_caderneta, @tipo = tipo, @cliente = codigo_cliente, @vlr = total_caderneta from inserted

IF @tipo = 'DEBITO'
begin
	update cliente set utilizado = isnull(utilizado,0) + @vlr where codigo_cliente = @cliente
end
else
begin
	update cliente set utilizado = isnull(utilizado,0) - @vlr where codigo_cliente = @cliente
	/*
	Registro duplicado conta_receber
	insert into conta_a_receber (documento, codigo_cliente, filial,  valor, desconto, obs, emissao, vencimento, pagamento, valor_pago, operador, pdv, finalizadora, id_finalizadora, documento_emitido, status, baixa_automatica) 
	values(@doc, @cliente, 'MATRIZ', @vlr, 0,'Lançamento automatico referente a pagamento de caderneta.', @emissao, @emissao, @emissao, @vlr, 0, @pdv, 14,0,0,1,1)
	*/
end


go 

ALTER TRIGGER [dbo].[u_SaldoCliente] ON [dbo].[Caderneta] 
FOR update
AS

DECLARE @tipo varchar(20),
	@cliente varchar(8),
	@vlr_o decimal(12,2),
	@vlr_n decimal(12,2),
	@doc varchar(15),
	@emissao datetime,
	@pdv int,
	@doc_o varchar(15),
	@emissao_o datetime,
	@pdv_o int




select @pdv = caixa_caderneta, @emissao = emissao_caderneta, @doc = documento_caderneta, @tipo = tipo, @cliente = codigo_cliente, @vlr_n = total_caderneta from inserted

select @vlr_o = total_caderneta, @doc_o = documento_caderneta, @emissao_o = emissao_caderneta, @pdv_o = caixa_caderneta from deleted

IF substring(@Tipo,1,1) = 'D'
begin
	update cliente set utilizado = isnull(utilizado,0) + @vlr_n - @vlr_o where codigo_cliente = @cliente
end
else
begin
	update cliente set utilizado = isnull(utilizado,0) - @vlr_n + @vlr_o where codigo_cliente = @cliente
	/*
	registro dublicado já lançado pelo PDV
	delete from conta_a_receber where documento = @doc_o and finalizadora = 14 and documento_emitido = 0 and emissao = @emissao_o and pdv = @pdv_o

	insert into conta_a_receber (documento, codigo_cliente, filial,  valor, desconto, obs, emissao, vencimento, pagamento, valor_pago, operador, pdv, finalizadora, id_finalizadora, documento_emitido, status, baixa_automatica) 
	values(@doc, @cliente, 'MATRIZ', @vlr_n, 0,'Lançamento automatico referente a pagamento de caderneta.', @emissao, @emissao, @emissao, @vlr_n, 0, @pdv, 14,0,0,1,1)
	*/
end



GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_FLUXO_CAIXA_SIMPLIFICADO]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10), @tipo varchar(10), @STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo_simples]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo_simples;
select A.DATA,A.dia_da_Semana,[RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo_simples FROM (

select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
		,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber = 0,
		Pagar = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from conta_a_pagar 
	where Filial =@FILIAL and
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
				between @datade and @dataate 
		
		
		
		AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>3))
							and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
														  when LEN (@CLIENTE)>0 then ''
																						  	else '%' 
																							end)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
union 		
		
select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber =convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0)))),
		Pagar = 0
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where Filial = @FILIAL and  case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate  
				
				AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>'3'))
																and( isnull(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
																								 when LEN(@FORNECEDOR)>0 then ''	
																							else '%' 
																							end
																							)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
)A GROUP BY DATA,Dia_da_Semana
		 
		
		
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO_GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo_simples B WHERE B.Data <A.Data)+A.SALDO_DIA) from #tmp_Fluxo_simples A ORDER BY CONVERT(VARCHAR,A.DATA,102)  ;
			  
END

go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_conta_a_pagar]
end
GO
--PROCEDURES =======================================================================================
CREATE  procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if(LEN(@Conferido)>1)
	begin
		set @where = @where + ' And conferido =1 '	
	end 
	
	if LEN(@status)>0
	begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
																 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
																 WHEN @STATUS='CANCELADO' THEN ' status =3'
																 WHEN @STATUS='LANCADO' THEN ' status =4'
																ELSE 'status <> '+CHAR(39)+'3'+CHAR(39) END
																) 
	end
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Dupl = case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			
			[Tipo pag]=tipo_pagamento,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_pagar.codigo_centro_custo,
			Banco = Conta_a_pagar.id_cc					
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
			left outer join Fornecedor on Conta_a_pagar.Fornecedor = Fornecedor.Fornecedor
		'+@where+'  Order By convert(varchar ,'+@tipo	+',102) '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
	--	set @string = @string + 'Documento = rtrim(ltrim(documento)), '
		--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
		--set @string = @string + @tipo + '= '+ @tipo + ', '
		--set @string = @string + 'Total = valor - isnull(desconto,0), '
		--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
			--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
			--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
			--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
		--set @string = @string + 'From Conta_a_pagar '
		--set @string = @string + @where
		--set @string = @string + ' Order By '+ @Tipo + ', Fornecedor, Documento'
	--Print @string
	Exec(@string)
End

go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO]
end
GO
--PROCEDURES =======================================================================================
CREATE Procedure [dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10),@tipo varchar(10),@status varchar(10),@fornecedor varchar(20)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,103),
		Dia_da_Semana = case 
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Saidas = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from conta_a_pagar 
	where Filial=@filial and 
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate
				AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>'3'))
				and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
						  	else '%'
						  	end)
	Group by
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end
	ORDER BY CONVERT(VARCHAR,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end,102)
End


---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================



go 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_receber]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_conta_a_receber]
end
GO
--PROCEDURES =======================================================================================
CREATE   procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250), 
	@status	   varchar(10),
	@centrocusto varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + '  and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Cliente))) > 1
		Begin
			set @where = @where + ' And codigo_cliente = (' + char(39) +replace(@Cliente,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if LEN(@status)>0
	begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
																 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
																 WHEN @STATUS='CANCELADO' THEN ' status =3'
																 WHEN @STATUS='LANCADO' THEN ' status =4'
																ELSE 'status <> '+CHAR(39)+'3'+CHAR(39) END
																) 
	end
	
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
		
	--Monta Select
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Cliente = rtrim(ltrim(Codigo_Cliente)), 
			Obs ,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = convert(numeric,Isnull(Desconto,0)),
			Acrescimo = convert(numeric,Isnull(Acrescimo,0)),
			ValorReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_receber.codigo_centro_custo						
		from dbo.Conta_a_receber Conta_a_receber  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_receber.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_receber.Filial = Centro_custo.filial
		
		'+@where+'  Order By convert(varchar,'+ @tipo +',102)  '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
	--	set @string = @string + 'Documento = rtrim(ltrim(documento)), '
		--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
		--set @string = @string + @tipo + '= '+ @tipo + ', '
		--set @string = @string + 'Total = valor - isnull(desconto,0), '
		--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
			--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
			--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
			--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
		--set @string = @string + 'From Conta_a_pagar '
		--set @string = @string + @where
		--set @string = @string + ' Order By convert('+ @Tipo +',102) '
			Print @string
	Exec(@string)
End


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_REL_CONTAS_A_RECEBER_SIMPLIFICADO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_REL_CONTAS_A_RECEBER_SIMPLIFICADO]
end
GO
--PROCEDURES =======================================================================================
CREATE  Procedure [dbo].[sp_REL_CONTAS_A_RECEBER_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10),@tipo varchar(10),@status varchar(10),@cliente varchar(20)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,103),
		Dia_da_Semana = case 
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Entradas = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where Filial=@filial and 
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate
				AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>'3'))
				and( isnull(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
														else '%' 
														end
													)

	Group by
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end
	ORDER BY CONVERT(VARCHAR,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end,102)
End

---==========================================================================================================================================================================================================================================================================================
go 

CREATE NONCLUSTERED INDEX [ix_Rel_Venda_Aliquota] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[Aliquota_ICMS] ASC,
	[data_cancelamento] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


