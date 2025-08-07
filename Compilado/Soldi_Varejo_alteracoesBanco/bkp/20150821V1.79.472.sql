
go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Fin_FluxoCaixa
end
GO

--sp_rel_fin_fluxocaixa 'matriz', '20141001', '20141001'

CREATE             PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS
	
	select count(*) as Qtde into #cupons from saida_Estoque where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf_item.Total),0)),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte
	UNION ALL
	SELECT 'VENDAS CUPOM',CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0))
	UNION ALL
	SELECT 'TOTAL FATURAMENTO',CONVERT(VARCHAR,
					(SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL)
					+(SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte)
					+(SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte)
					)
	UNION ALL
				
		SELECT '',''
	UNION ALL

	SELECT 'VENDAS COM NFP',  CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and Filial = @filial and len(isnull(cpf_cnpj,''))>10)))
	UNION ALL
	SELECT '% DE VENDAS COM NFP', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100))
	UNION ALL
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND FILIAL = @FILIAL AND Data_Cancelamento IS NOT NULL),0))
	UNION ALL
	SELECT 'ITENS VENDIDOS', CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0)))
	UNION ALL
	SELECT 'ITENS CANCELADOS', CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL AND Filial = @FILIAL)),0))
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))
	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS', CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial)))
	UNION ALL
	SELECT 'ACRESCIMOS', CONVERT(VARCHAR,0) + '.00'
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
	  













