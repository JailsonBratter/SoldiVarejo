-- Alteraçoes Tabelas ==============================================================================
	
	ALTER TABLE CONTA_A_PAGAR ADD conferido tinyint
GO
--PROCEDURE ========================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_conta_a_pagar
end

go
create  procedure [dbo].[sp_rel_conta_a_pagar](
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
																ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
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
	
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Duplicata= case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			Fornecedor.CNPJ ,
			[Tipo pagamento]=tipo_pagamento,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_pagar.codigo_centro_custo					
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

GO
--PROCEDURE ========================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Fin_FluxoCaixa
end
go

CREATE     PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
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
	
	
	select count(*) as Qtde into #cupons from saida_Estoque where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte --AND Data_Cancelamento IS NULL 
					AND Filial = @FILIAL),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte),0)
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND FILIAL = @FILIAL AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', CONVERt(VARCHAR,Sum(isnull(convert(decimal(18,2),Total,0),0))) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte
	UNION ALL
	SELECT 'VENDAS CUPOM',CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte --AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0))
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP',  CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and Filial = @filial and len(isnull(cpf_cnpj,''))>10)))
	UNION ALL
	SELECT '% DE VENDAS COM NFP', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100))
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS', CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0)))
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL AND Filial = @FILIAL)),0))	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, CONVERT(VARCHAR(90),Total) From Totais Where Descricao = 'NF DEVOLUCAO'

	UNION ALL
	
	SELECT 'RESULTADO TOTAL',
		CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO')))
	FROM  TOTAIS
	Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial)))
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial)))
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
    SELECT 'GERENCIAL ',CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE with( index(IX_Saida_Estoque)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0))
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + '.00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,CONVERt(VARCHAR,SUM(TOTAL))
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',CONVERt(VARCHAR,SUM(TOTAL))
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  

