IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE SP_REL_HISTORIO_ENTRADA_SAIDA
end
GO


--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA](
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@plu as Varchar(20), 
@ean as varchar(20),
@Ref as Varchar(20),
@descricao as varchar(40)


)
As


DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

--set @DataInicio ='20140401'
--set @DataFim ='20140910'
--Set @plu ='1'

Create table #OUTROS
(
	DATA  DATETIME ,
	NF_ENTRADA DECIMAL(18,2),
	NF_ENTRADA_PRECO DECIMAL(18,2),
	OUTRAS_MOVIMENTACOES DECIMAL(18,2),
	NF_Saida DECIMAL(18,2),
	NF_Saida_Preco	 DECIMAL(18,2),
	Cupom DECIMAL(18,2),  
	Cupom_Preco DECIMAL(18,2)
);

--select * from pedido_itens

IF @PedEstoque = 'TRUE'
INSERT INTO  #OUTROS select Data =pd.Data_cadastro  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,OUTRAS_MOVIMENTACOES =0 
		   ,[NF Saida] =Sum(itemPd.Qtde*itemPd.Embalagem)
		   ,[NF Saida Preco]	= Sum(itemPd.total)
		  ,[Cupom] = 0  
		  ,[Cupom Preco]=0
	 from pedido_itens itemPd inner join pedido pd on pd.Filial=@FILIAL and  pd.Pedido = itempd.Pedido and  pd.Tipo = itemPd.tipo
		inner join Natureza_operacao Eop on pd.cfop = Eop.Codigo_operacao
		INNER JOIN Mercadoria M ON M.PLU=itemPd.PLU
	where (itemPd.PLU=@plu OR (LEN(@REF)>0 AND  M.Ref_fornecedor=@Ref)) and  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
	group by pd.Data_cadastro,itemPd.total
ELSE
begin
INSERT INTO  #OUTROS 
	select Data =se.Data_movimento  ,
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,OUTRAS_MOVIMENTACOES =0 
		   ,[NF Saida] =0
		   ,[NF Saida Preco]	= 0
		  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
		  ,[Cupom Preco]=Sum(se.vlr)	
	from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	where (se.PLU=@plu OR (LEN(@REF)>0 AND M.Ref_fornecedor=@Ref)) and se.Filial=@FILIAL and se.Data_movimento between @DataInicio AND @DataFim
	group by se.Data_movimento
END;


Select Data = CONVERT(varchar,todos.data,103)
		,[Entrada]=Sum(todos.[NF Entrada])
		,[Entrada_Valor]=Sum(todos.[NF Compra Preco])
		,[Outras_Mov]=SUM(OUTRAS_MOVIMENTACOES)
		,[Saida_Outros] = SUM(todos.[NF Saida])
		,[Saida_Valor] = Sum(todos.[NF Saida Preco])
		,[Cupom] = SUM(todos.[Cupom])
		,[Cupom_Valor] = Sum(todos.[Cupom Preco])
	 from (
select Data =b.data ,
	   [NF Entrada] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (Eitem.PLU = @plu or  (LEN(@REF)>0 AND eitem.codigo_referencia=@Ref)  )AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF Compra Preco]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (Eitem.PLU = @plu or (LEN(@REF)>0 AND eitem.codigo_referencia=@Ref))AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1   ),0) 				
	   ,OUTRAS_MOVIMENTACOES =0
		   
	   ,[NF Saida] =isnull((Select SUM (isnull(sitem.Qtde,0)*ISNULL(sitem.Embalagem,0)) 
								from NF_Item Sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							
							WHERE (Sitem.PLU = @plu or (LEN(@REF)>0 AND Sitem.codigo_referencia=@Ref ) ) AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 ),0) 
	   ,[NF Saida Preco]	= isnull((Select Sum(isnull(sitem.Total,0)) 
							from NF_Item sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							WHERE (Sitem.PLU = @plu or (LEN(@REF)>0 AND Sitem.codigo_referencia=@Ref) )AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and Sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1   ),0) 				
	  ,[Cupom] = 0.00-- isnull((Select SUM(isnull(se.Qtde,0)) from Saida_estoque se with( index(IX_Saida_Estoque_01)) where se.plu =@plu and se.Data_movimento = b.data),0) 
	  ,[Cupom Preco]=0.00	
		
from NF_Item a inner join nf b on a.codigo = b.Codigo 
				inner join Natureza_operacao op on b.Codigo_operacao = op.Codigo_operacao 
		
WHERE A.PLU = @plu AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
group by Data 

UNION ALL

Select
	DATA   ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	OUTRAS_MOVIMENTACOES =SUM(OUTRAS_MOVIMENTACOES) ,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by DATA

UNION ALL
Select INV.Data,
		NF_ENTRADA = 0,
		NF_ENTRADA_PRECO = 0 ,
		OUTRAS_MOVIMENTACOES =  SUM(case when tm.Saida =0 then ISNULL(item.Contada,0) 
													 when tm.saida=1 then (ISNULL(item.contada,0)*-1) 
													 when tm.saida=2 then (ISNULL(item.Contada,0)-ISNULL(item.Saldo_atual,0))
													end 
													   )  ,
		NF_Saida= 0,
		NF_Saida_Preco= 0	 ,
		Cupom = 0,  
		Cupom_Preco= 0 								   
	from Inventario inv 
			inner join Tipo_movimentacao tm on inv.tipoMovimentacao = tm.Movimentacao
			inner join Inventario_itens item on inv.Codigo_inventario = item.Codigo_inventario and inv.Filial = item.Filial
	where item.PLU =@plu and inv.Data between @DataInicio AND @DataFim   and status='ENCERRADO' AND INV.Filial = @FILIAL
	group by inv.Data
	
) as todos
group by todos.Data
order by convert(varchar ,todos.Data,102) desc
















