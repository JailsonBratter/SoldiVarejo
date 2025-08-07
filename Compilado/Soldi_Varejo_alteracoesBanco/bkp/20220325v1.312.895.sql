
CREATE PROCEDURE [dbo].[sp_Cons_PLUsVinculado]

@plu as varchar(17)

 

AS

 

DECLARE @pluvinculado AS VARCHAR(17)

DECLARE @estoque AS INT

DECLARE @QtdeBase AS INT

 

--sp_cons_PLUsVinculado '30960'

BEGIN

SELECT

@estoque = ISNULL(tipo.estoque, 0),

@pluvinculado = rtrim(ltrim(ISNULL(m.Plu_Vinculado, ''))),

@QtdeBase = ISNULL(m.fator_Estoque_Vinculado, 0)

FROM

mercadoria m inner join tipo on m.tipo = tipo.tipo

WHERE

    m.plu = @plu

 

if @estoque  = 1

begin

select

m.plu,

m.tipo,

l.preco_custo,

l.margem,

l.preco,

ISNULL(m.PLU_Vinculado, '') AS PLU_Vinculado,

isnull(m.fator_Estoque_Vinculado, 0) AS Fator,

QtdeBase = @QtdeBase,

m.preco_atacado,

m.terceiro_preco

 

from mercadoria m

inner join mercadoria_loja l on m.plu = l.plu

inner join tipo on m.tipo = tipo.tipo

where m.PLU_Vinculado  = @plu

end

else

begin

SELECT

m.plu,

m.tipo,

l.preco_custo,

l.margem,

l.preco,

ISNULL(m.PLU_Vinculado, '') AS PLU_Vinculado,

isnull(m.fator_Estoque_Vinculado, 0) AS Fator,

QtdeBase = @QtdeBase,

m.preco_atacado,

m.terceiro_preco

 

FROM mercadoria m

inner join mercadoria_loja l on m.plu = l.plu

inner join tipo on m.tipo = tipo.tipo

where (m.PLU_Vinculado  = @pluvinculado or m.plu = @pluvinculado)

and m.plu <> @plu

end

END

GO

 

alter table fornecedor add Inativo Tinyint DEFAULT 0


go 


--PROCEDURES =======================================================================================

-- exec sp_Rel_Fin_FluxoCaixa 'MATRIZ','20211205','20211205'
ALTER  PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS
Begin 
	if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

	Begin

	Drop table Totais

	End
	
		Create table Totais
	(
		Descricao  varchar(400),
		Total	 Decimal(18,2)
	);
	
	
		select count(*) as Qtde into #cupons from saida_Estoque WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) where 
			Filial  = @Filial And
			Data_Movimento BETWEEN @DataDe AND @DataAte AND 
			Data_Cancelamento IS NULL
		group by documento, filial, caixa_saida
	
		insert into #cupons 
		select COUNT(*) from nf where nf.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf.Filial = @FILIAL and nf.status='AUTORIZADO'
		group by Codigo
	
		insert into #cupons
		select COUNT(*) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  and pedido.Status <>3
		GROUP by pedido
	
		----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
		Insert into Totais
		SELECT 'TOTAL FATURAMENTO' AS Descricao,CONVERT(VARCHAR,
						Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) WHERE Filial = @FILIAL and  Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
						),0)
						+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)-nf_item.desconto_valor) from nf_item inner join nf on nf_item.codigo= nf.Codigo AND nf_item.Filial = nf.FILIAL where  nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO' ),0) 
						+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
						)  Total

		----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
		INSERT into Totais
		SELECT 'ITENS CANCELADOS NO CUPOM', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) 
																		FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) 
																		WHERE  FILIAL = @FILIAL 
																		   and Data_Movimento BETWEEN @DataDe AND @DataAte  
																		   AND Data_Cancelamento IS NOT NULL
																		   AND ISNULL(cupom_cancelado,0)=0 ),0))  
		INSERT into Totais
		SELECT 'CUPONS CANCELADOS FINALIZADOS', CONVERT(VARCHAR,ISNULL((SELECT SUM(s.VLR-isnull(s.desconto,0)) 
																		FROM SAIDA_ESTOQUE s WITH(INDEX(Ix_Fluxo_Caixa_Vendas))  Inner Join Lista_finalizadora l
																				On s.Documento = l.Cupom AND s.Filial = l.Filial AND s.Data_Movimento = l.Emissao AND s.Caixa_Saida = l.Pdv
																		WHERE  s.FILIAL = @FILIAL 
																		   AND s.Data_Movimento BETWEEN @DataDe AND @DataAte  
																		   AND s.Data_Cancelamento IS NOT NULL
																		   AND ISNULL(s.cupom_cancelado,0)=1 ),0))  				
		INSERT into Totais
		SELECT 'CUPONS CANCELADOS NAO FINALIZADOS', CONVERT(VARCHAR,ISNULL((SELECT SUM(SE.VLR-isnull(SE.desconto,0)) 
																		FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) 
																		WHERE  FILIAL = @FILIAL 
																		   AND Data_Movimento BETWEEN @DataDe AND @DataAte  
																		   AND Data_Cancelamento IS NOT NULL
																		   AND 	not exists(select * from Lista_finalizadora l 
																			Where l.documento = se.documento and l.Emissao = se.data_movimento
																				And l.pdv = se.caixa_saida
																				And l.filial = se.Filial)
																		   ),0))  

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
		SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(convert(decimal(18,2),isnull(Total,0),0))),'.',',') from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte and pedido.Status <>3 
		UNION ALL
		SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)-(isnull(NF_Item.Total,0)*isnull(nf_item.desconto,0)/100)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  AND nf_item.Filial = nf.FILIAL where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO'
		UNION ALL
		SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) WHERE FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
				AND Filial = @FILIAL),0)),'.',',')
		UNION ALL
		SELECT '|-SUB-|'+Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
		UNION ALL
				
			SELECT '',''
		UNION ALL
		SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
		UNION ALL
		SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) where Filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10))),'.',',')
		UNION ALL
		SELECT 'PORC DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) where filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
		UNION ALL
		--SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL ),0))),'.',',')
		--UNION ALL
		SELECT '',''
	
		UNION ALL
		SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL )),0)),'.',',')	
		UNION ALL
	
		SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS NO CUPOM'
		UNION ALL
		SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'CUPONS CANCELADOS FINALIZADOS'
		UNION ALL
		SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'CUPONS CANCELADOS NAO FINALIZADOS'
		UNION ALL
		SELECT '',''
		UNION ALL
		SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
		UNION ALL
		SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
		UNION ALL
		SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

		--UNION ALL
	
		--SELECT 'RESULTADO TOTAL',
		--	replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
		--FROM  TOTAIS
		--Where Descricao = 'TOTAL FATURAMENTO'

		UNION ALL
		SELECT '',''
		UNION ALL
		SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
		UNION ALL
		SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) where filial = @filial and  data_movimento between @DataDe and @DataAte and data_cancelamento is null ))),'.',',')
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
		SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa_Vendas)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
		UNION ALL
		SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
		UNION ALL
		SELECT '', ''
		UNION ALL
		SELECT 'IFOOD |-SUB-|', ''
		UNION ALL
		SELECT 'QUANTIDADE DE PEDIDOS iFOOD', replace(CONVERT(VARCHAR,
		ISNULL((SELECT COUNT(*) 
			FROM Pedido INNER JOIN iFood_Gerencial ifood ON pedido.Numero_Pedido_Origem = ifood.Pedido_ID 
			WHERE pedido.Filial = @FILIAL AND pedido.Data_cadastro BETWEEN @DataDe AND (@DataAte + ' 23:59:59') AND Pedido.Status NOT IN(3)), 0)
			+  ISNULL((SELECT COUNT(*) FROM PEDIDO WHERE Data_cadastro BETWEEN @DataDe AND (@DataAte + ' 23:59:59') 
			AND LEN(RTRIM(LTRIM(Numero_Pedido_Origem))) > 15 AND NUMERO_PEDIDO_ORIGEM NOT  IN(SELECT PEDIDO_ID FROM iFood_Gerencial)), 0)			
			),'.',',')
		UNION ALL
		SELECT 'VALOR VENDAS iFOOD', replace(CONVERT(VARCHAR,
		ISNULL((SELECT SUM(iSNULL(ifood_Total_Pago, 0)) 
			FROM Pedido INNER JOIN iFood_Gerencial ifood ON pedido.Numero_Pedido_Origem = ifood.Pedido_ID 
			WHERE pedido.Filial = @FILIAL AND pedido.Data_cadastro BETWEEN @DataDe AND (@DataAte + ' 23:59:59') 
			AND Pedido.Status NOT IN(3)), 0)        
			+  ISNULL((SELECT SUM(Total - ISNULL(DESCONTO, 0))  FROM PEDIDO WHERE Pedido.Data_cadastro BETWEEN @DataDe AND (@DataAte + ' 23:59:59')  AND LEN(RTRIM(LTRIM(Numero_Pedido_Origem))) > 15 AND
	NUMERO_PEDIDO_ORIGEM NOT  IN(SELECT PEDIDO_ID FROM iFood_Gerencial)), 0)	
		
		
		
			),'.',',')
		UNION ALL
		SELECT 'VALOR GERENCIAL iFOOD', replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(iSNULL(iFood_Total_Desconto, 0)) FROM Pedido INNER JOIN iFood_Gerencial ifood ON pedido.Numero_Pedido_Origem = ifood.Pedido_ID WHERE pedido.Filial = @FILIAL AND pedido.Data_cadastro BETWEEN @DataDe AND (@DataAte + ' 23:59:59') AND Pedido.Status NOT IN(3)), 0)),'.',',')
		UNION ALL
		SELECT '', ''
		UNION ALL
	
		SELECT 'Forma de Pagamento |-SUB-|','Valor Total'
		UNION ALL
		SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
			FROM LISTA_FINALIZADORA  WITH(INDEX(ix_Lista_Fluxo_Caixa))
			INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
			WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
			GROUP BY FINALIZADORA.FINALIZADORA
		UNION ALL	
		SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(ISNULL(TOTAL,0))),'.',',')
			FROM LISTA_FINALIZADORA WITH(INDEX(ix_Lista_Fluxo_Caixa))
			INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
			WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
		UNION ALL
		SELECT '',''
end 

go 

insert into Versoes_Atualizadas select 'Versão:1.312.895', getdate();
