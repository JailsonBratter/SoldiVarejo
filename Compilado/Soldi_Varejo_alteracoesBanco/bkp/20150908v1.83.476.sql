GO

 

/****** Object:  Table [dbo].[Busca_Preco]    Script Date: 09/01/2015 15:47:37 ******/

SET ANSI_NULLS ON

GO

 

SET QUOTED_IDENTIFIER ON

GO

 

SET ANSI_PADDING ON

GO

 

CREATE TABLE [dbo].[Busca_Preco](

                [Codigo] [varchar](14) NOT NULL,

                [Descricao] [varchar](80) NULL,

                [Preco] [numeric](12, 2) NULL,

CONSTRAINT [PK_Busca_Preco] PRIMARY KEY CLUSTERED

(

                [Codigo] ASC

)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

) ON [PRIMARY]

 

GO

 

SET ANSI_PADDING OFF

GO



GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_pedido_simplificado]    Script Date: 09/04/2015 10:33:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--=========================================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_simplificado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Venda_pedido_simplificado
end
GO
CREATE  PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@nome		as Varchar(30) = ''
		
    	as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20141201','20141220','',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),2),UNITARIO = ROUND( UNITARIO ,2),'','','',''
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario
	UNION ALL
	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),0),0,'','','','' 
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte

	--GROUP BY P.PLU,M.Descricao,P.unitario	
	
end
else		
begin
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			--VlrVenda=ROUND(Total,2), 
			
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
			FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
			where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			
			[Total Venda]=ROUND(Total,2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			ISNULL(Pedido.funcionario, '') As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			Pedido.funcionario like (case when @nome  <> '' then @Nome else '%' end)
				 
 end


GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Fin_FluxoCaixa]    Script Date: 09/04/2015 10:38:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--sp_rel_fin_fluxocaixa 'matriz', '20141001', '20141001'
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Fin_FluxoCaixa
end

GO

CREATE           PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
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
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte),0)
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
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
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)
	UNION ALL
	SELECT 'AJUSTE DEVOLUCAO', CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))
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
	  













