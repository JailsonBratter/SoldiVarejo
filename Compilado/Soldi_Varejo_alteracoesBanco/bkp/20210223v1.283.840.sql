
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].SP_REL_HISTORIO_ENTRADA_SAIDA') AND type in (N'P', N'PC'))
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

if(LEN(@ean)>0)
begin
	set @plu= (Select top 1 PLU from EAN where EAN = @ean);
end

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
	 From pedido_itens itemPd inner join pedido pd on pd.Filial=@FILIAL and  pd.Pedido = itempd.Pedido and  pd.Tipo = itemPd.tipo
		INNER JOIN Natureza_operacao Eop on pd.cfop = Eop.Codigo_operacao
		INNER JOIN Mercadoria M ON M.PLU=itemPd.PLU
	Where (itemPd.PLU=@plu OR (LEN(@REF)>0 AND  M.Ref_fornecedor=@Ref)) and  pd.Data_cadastro between @DataInicio AND @DataFim and Eop.Baixa_estoque ='1' and pd.Tipo=1
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
	From Saida_estoque se with( index(IX_Saida_Estoque_01)) 
	INNER JOIN Mercadoria M ON M.PLU = SE.PLU
	Where (se.PLU=@plu OR (LEN(@REF)>0 AND M.Ref_fornecedor=@Ref)) and se.Filial=@FILIAL and se.data_cancelamento is null and se.Data_movimento between @DataInicio AND @DataFim
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
	 From (
Select Data =b.data ,
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
		
WHERE a.Filial=@FILIAL and A.PLU = @plu AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
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

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_EstoqueNaData]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_EstoqueNaData]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_Rel_EstoqueNaData]
	@Filial			as varchar(20),
	@Tipo			as varchar(20),	
	@Data			as DateTime

As

set @Data = @Data + 1

Begin


SELECT 
	M.PLU,
	m.descricao,
	M.PRECO_CUSTO,
	M.SALDO_ATUAL,
	m.Tipo,
	SaidaPDV = isnull((Select Sum(s.Qtde) From saida_estoque s with (index=ix_saida_estoque) inner join mercadoria_loja l on s.plu = l.plu
						Where s.Filial = @Filial 
						And s.Data_movimento between @Data  
						And Getdate() 
						And s.data_cancelamento is null
						And S.PLU = M.PLU), 0),
	SaidaNF = isnull((Select Sum( ((i.qtde * i.embalagem)  )  )
						From nf Inner Join Natureza_operacao n on n.codigo_operacao = nf.Codigo_operacao 
								Inner Join nf_item i on nf.Cliente_Fornecedor = i.Cliente_Fornecedor 
								And nf.Tipo_NF = i.Tipo_NF 
								And nf.codigo = i.codigo 
						Where isnull(nf.nf_canc,0) <> 1
						And nf.tipo_nf = 1
						And n.baixa_estoque = 1
						And nf.Data between @Data And GETDATE()
						And NF.FIlial = @Filial
						And i.PLU = m.plu), 0),
	OutrasSaidas = Isnull((
			(select Isnull(SUM(Contada),0) from inventario_itens i inner join inventario v on i.Codigo_inventario = v.Codigo_inventario 
			and v.status = 'ENCERRADO'
			and v.data between @Data And GETDATE()
			and i.PLU = m.plu
			and exists(select * from Tipo_movimentacao t where t.Movimentacao = v.tipoMovimentacao and t.saida = 1)) 
		),0),								
	OutrasEntradas = Isnull((
			(select Isnull(SUM(Contada),0) from inventario_itens i inner join inventario v on i.Codigo_inventario = v.Codigo_inventario 
			and v.status = 'ENCERRADO'
			and v.data between @Data And GETDATE()
			and i.PLU = m.plu
			and exists(select * from Tipo_movimentacao t where t.Movimentacao = v.tipoMovimentacao and t.saida = 0)) 
		),0),						
	EntradaNF = isnull((Select Sum( ((i.qtde * i.embalagem)  )  )
						From nf Inner Join Natureza_operacao n on n.codigo_operacao = nf.Codigo_operacao 
								Inner Join nf_item i on nf.Cliente_Fornecedor = i.Cliente_Fornecedor 
								And nf.Tipo_NF = i.Tipo_NF 
								And nf.codigo = i.codigo 
						Where isnull(nf.nf_canc,0) <> 1
						And nf.tipo_nf = 2
						And n.baixa_estoque = 1
						And nf.Data between @Data And GETDATE()
						And NF.FIlial = 'MATRIZ'
						And i.PLU = m.plu), 0)
into #lixo
From 
	Mercadoria m
Where
	(@Tipo = 'TODOS' OR   @Tipo like '%|'+m.Tipo+'|%')	

Select 
	PLU, 
	Descricao, 
	Departamento = (Select w.descricao_departamento 
						From W_BR_CADASTRO_DEPARTAMENTO w Inner Join mercadoria m 
						on w.codigo_departamento = m.Codigo_departamento 
						Where m.plu = #lixo.PLU),
	Tipo,
	Custo = convert(decimal(18,2),preco_custo), 
	Saldo = (saldo_atual + OutrasSaidas + SaidaPDV + SaidaNF - EntradaNF - OutrasEntradas),
	[Valor Estoque] = convert(decimal(18,2),(saldo_atual + OutrasSaidas + SaidaPDV + SaidaNF - EntradaNF - OutrasEntradas)) * preco_custo
into #estoque 
From 
	#lixo
Where 
	(saldo_atual + OutrasSaidas + SaidaPDV + SaidaNF - EntradaNF - OutrasEntradas) > 0
	
Select * From 
	#estoque 
Order by 7 Desc
End

go 



--Tabela Tipo

 

Alter table Tipo ADD Compra tinyint

Alter table Tipo ADD Estoque tinyint

Alter table Tipo ADD PLUAssociado tinyint

go

--Tabela Mercadoria

-- Colunas

--Trigger

 

alter table mercadoria add ImpAux Tinyint

Alter table Mercadoria ADD Venda_Com_Senha Tinyint

Alter table Mercadoria Add PLU_Vinculado varchar(17)
Alter Table Mercadoria add fator_Estoque_Vinculado int

 

GO

 

/****** Object:  Trigger [dbo].[utg_atualiza_saldo_atual_tipo]    Script Date: 23/02/2021 12:58:34 ******/

 

 

ALTER TRIGGER [dbo].[utg_atualiza_saldo_atual_tipo] ON [dbo].[Mercadoria]

--INSTEAD OF UPDATE

FOR UPDATE

AS

 

declare

  @filial varchar(20),

  @plu varchar(17),

  @qtdeSubtrai decimal(9,3),

  @Fator_Conv Decimal(9,3),

  @QtdeSoma Decimal(9,3),

  @Qtde Decimal(9,3),

  @PLUVinculado VARCHAR(17)

 

  SELECT @PLUVinculado = DELETED.PLU_Vinculado FROM DELETED

 

  if @PLUVinculado <>''

                BEGIN

                   declare x_updatem cursor for select DELETED.filial, Mercadoria.Plu_Vinculado, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Mercadoria.fator_Estoque_Vinculado

                   from DELETED

                   inner join INSERTED

                                 on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)

                                  inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU

                                  inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.PLUAssociado,0) = 1

                END

  ELSE

                BEGIN

                   declare x_updatem cursor for select DELETED.filial, Item.Plu_item, DELETED.Saldo_Atual * -1, INSERTED.saldo_atual, Item.fator_conversao

                   from DELETED

                   inner join INSERTED

                                 on (DELETED.filial = INSERTED.filial and DELETED.PLU = INSERTED.PLU)

                                  inner join Mercadoria ON Mercadoria.PLU = DELETED.PLU

                                  inner join Tipo ON Tipo.Tipo = Mercadoria.Tipo  AND ISNULL(Tipo.Movimenta_Estoque_Item,0) = 1

                                  inner join Item ON Item.Plu = DELETED.PLU

                END

Open x_updatem

 

FETCH NEXT FROM x_updatem

INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv

 

 

WHILE @@FETCH_STATUS = 0

 

BEGIN

       BEGIN

             if update(saldo_Atual)

 

                    SET @Qtde = @qtdeSubtrai + @QtdeSoma

 

                    begin

                           update mercadoria set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu

                           update mercadoria_loja set saldo_atual = isnull(saldo_atual,0) + (@qtde * @Fator_Conv) where plu =  @plu and filial = @filial

                    end

       end

 

FETCH NEXT FROM x_updatem

INTO @filial, @plu, @qtdeSubtrai, @QtdeSoma, @Fator_Conv

END

close x_updatem

 

deallocate x_updatem

 

GO

insert into Versoes_Atualizadas select 'Versão:1.283.840', getdate();