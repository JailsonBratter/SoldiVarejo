

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE]
GO


CREATE PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE](
--declare
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@GRUPO as Varchar(40), 
@SUBGRUPO as Varchar(40),
@DEPARTAMENTO AS VARCHAR(40),
@FAMILIA    AS VARCHAR(40),
@FORNECEDOR AS VARCHAR(50)
)
As


DECLARE @PedEstoque  varchar(10);
Set @PedEstoque =(Select VALOR_ATUAL  from PARAMETROS where PARAMETRO='BAIXA_ESTOQUE_PED_VENDA' );

--set @DataInicio ='20150302'
--set @DataFim ='20150330'
--Set @GRUPO ='PADARIA'
--set @SUBGRUPO ='PAES'
--set @DEPARTAMENTO='PAES'
--set @FAMILIA =''
--drop table #OUTROS

--exec SP_REL_HISTORIO_ENTRADA_SAIDA_CONSOLIDADE @Filial='MATRIZ', @dataInicio = '20180101',  @dataFim = '20180228',  @grupo = '',  @subGrupo = '',  @departamento = '',  @Familia = '',  @fornecedor = 'MARALAR' 


Create table #OUTROS
(
	PLU  VARCHAR(17) COLLATE SQL_Latin1_General_CP1_CI_AS,
	Ref_Fornecedor varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS,
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
INSERT INTO  #OUTROS select PLU =itemPd.PLU  ,Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
		   ,[NF Saida] =Sum(itemPd.Qtde*itemPd.Embalagem)
		   ,[NF Saida Preco]	= Sum(itemPd.total)
		  ,[Cupom] = 0  
		  ,[Cupom Preco]=0
	 from pedido_itens itemPd inner join pedido pd on pd.Filial=@FILIAL and  pd.Pedido = itempd.Pedido and  pd.Tipo = itemPd.tipo
		inner join Natureza_operacao Eop on pd.cfop = Eop.Codigo_operacao
		INNER JOIN Mercadoria M ON M.PLU=itemPd.PLU
		INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
	where (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
		AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
		AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
		AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
		and (len(@fornecedor)=0 or m.plu in (Select plu from Fornecedor_Mercadoria where fornecedor = @FORNECEDOR))
		AND  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
		and pd.Filial = @FILIAL
	group by itemPd.PLU , m.Ref_Fornecedor
ELSE
begin
INSERT INTO  #OUTROS 
	select PLU =se.PLU  , Ref_Fornecedor = Isnull(M.Ref_fornecedor,''), 
		   [NF Entrada] =0
		   ,[NF Entrada Preco]	= 0
		   ,[OUTRAS_MOVIMENTACOES]=0
		   ,[NF Saida] =0
		   ,[NF Saida Preco]	= 0
		  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
		  ,[Cupom Preco]=Sum(se.vlr)	
	from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
	where (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
		AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
		AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
		AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
		and (len(@fornecedor)=0 or m.plu in (Select plu from Fornecedor_Mercadoria where fornecedor = @FORNECEDOR))
		and  se.Data_movimento between @DataInicio AND @DataFim
		and se.Filial = @FILIAL
	group by se.PLU , m.Ref_Fornecedor
END;



Select PLU =todos.PLU ,Ref_fornecedor= isnull(m.Ref_Fornecedor,'')
		,DESCRICAO = M.DESCRICAO
		,[Entrada]=Sum(todos.NF_ENTRADA)
		,[Entrada_Valor]=Sum(todos.NF_ENTRADA_PRECO)
		,[Outras_Mov]=SUM(OUTRAS_MOVIMENTACOES)
		,[Saida_Outros] = SUM(todos.NF_Saida)
		,[Saida_Valor] = Sum(todos.NF_Saida_Preco)
		,[Cupom] = SUM(todos.Cupom)
		,[Cupom_Valor] = Sum(todos.Cupom_Preco)
	 from (

select PLU =isnull(A.PLU,''), Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
	   [NF_ENTRADA] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE (LEN(@FORNECEDOR)=0  OR Enf.Cliente_Fornecedor =@FORNECEDOR ) AND Eitem.PLU = A.PLU and (Enf.Data between @DataInicio AND @DataFim ) 
							    AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF_ENTRADA_PRECO]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem with( index(IX_NF_ITEM_01)) inner join nf Enf  on Enf.Filial =Eitem.Filial and Enf.Cliente_Fornecedor= Eitem.Cliente_Fornecedor and  Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							where (LEN(@FORNECEDOR)=0  OR Enf.Cliente_Fornecedor =@FORNECEDOR ) AND  Eitem.PLU = A.PLU  and (Enf.Data between @DataInicio AND @DataFim ) 
									 AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1  AND Eop.Baixa_estoque=1   ),0) 				
	   ,[OUTRAS_MOVIMENTACOES]=0
	   ,[NF_Saida] =isnull((Select SUM (isnull(sitem.Qtde,0)*ISNULL(sitem.Embalagem,0)) 
								from NF_Item Sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							where  Sitem.PLU = A.PLU   and (Snf.Data between @DataInicio AND @DataFim ) 
							AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 ),0) 
	   ,[NF_Saida_Preco]	= isnull((Select Sum(isnull(sitem.Total,0)) 
							from NF_Item sitem with( index(IX_NF_ITEM_01)) inner join nf Snf  on Snf.Filial =Sitem.Filial and Snf.Cliente_Fornecedor= Sitem.Cliente_Fornecedor and  Sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							where  Sitem.PLU = A.PLU and (Snf.Data between @DataInicio AND @DataFim  )
							 AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and Sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1   ),0) 				
	  ,[Cupom] = 0.00-- isnull((Select SUM(isnull(se.Qtde,0)) from Saida_estoque se with( index(IX_Saida_Estoque_01)) where se.plu =@plu and se.Data_movimento = b.data),0) 
	  ,[Cupom_Preco]=0.00	
		
from NF_Item a inner join nf b on a.codigo = b.Codigo 
				inner join Natureza_operacao op on b.Codigo_operacao = op.Codigo_operacao 
				INNER JOIN Mercadoria M ON M.PLU =A.PLU
							INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
							WHERE (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
								AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
								AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
								AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
								AND (LEN(@FORNECEDOR)=0  OR b.Cliente_Fornecedor =@FORNECEDOR ) 
								AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 
								and a.Filial=@FILIAL
					GROUP BY A.PLU , m.Ref_Fornecedor


UNION ALL
Select
	PLU   , Ref_Fornecedor ,
	NF_ENTRADA=SUM(NF_ENTRADA) ,
	NF_ENTRADA_PRECO = SUM(NF_ENTRADA_PRECO) ,
	[OUTRAS_MOVIMENTACOES]=0,
	NF_Saida= SUM(NF_Saida) ,
	NF_Saida_Preco= SUM(NF_Saida_Preco)	 ,
	Cupom = sum(cupom),  
	Cupom_Preco= SUM(Cupom_Preco)  
from  #OUTROS
group by PLU ,Ref_Fornecedor 
 UNION ALL
Select ITEM.PLU, Ref_Fornecedor = Isnull(m.Ref_Fornecedor,''),
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
			INNER JOIN Mercadoria M ON M.PLU =ITEM.PLU
							INNER JOIN W_BR_CADASTRO_DEPARTAMENTO DP ON M.Codigo_departamento = DP.codigo_departamento
							
	WHERE (LEN(@DEPARTAMENTO)=0 OR DP.descricao_departamento=@DEPARTAMENTO ) 
								AND (LEN(@SUBGRUPO)=0 OR DP.descricao_subgrupo=@SUBGRUPO )
								AND (LEN(@GRUPO)=0 OR DP.Descricao_grupo=@GRUPO )
								AND (LEN(@FAMILIA)=0 OR M.Descricao_familia=@FAMILIA )
								and inv.Data between @DataInicio AND @DataFim   and status='ENCERRADO' AND INV.Filial = @FILIAL
								and (len(@fornecedor)=0 or item.plu in (Select plu from Fornecedor_Mercadoria where fornecedor = @FORNECEDOR))
	group by Item.PLU , m.Ref_Fornecedor




) as todos
INNER JOIN Mercadoria M ON M.PLU =todos.PLU
group by todos.PLU, m.Ref_Fornecedor, 
M.Descricao
order by CONVERT(DECIMAL,todos.PLU)  




go 

	insert into Versoes_Atualizadas select 'Versão:1.247.786', getdate();
GO
