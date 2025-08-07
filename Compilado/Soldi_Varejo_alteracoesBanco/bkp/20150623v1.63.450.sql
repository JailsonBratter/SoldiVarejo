go
ALTER TABLE [dbo].[Mercadoria] ADD  CONSTRAINT [PK_Mercadoria] PRIMARY KEY CLUSTERED 
(
	[Filial] ASC,
	[PLU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

go

CREATE NONCLUSTERED INDEX [IX_Saida_estoque] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[PLU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
go


ALTER procedure [dbo].[sp_rel_imposto_cadastrado](
@filial varchar(20),
@plu varchar(20)
,@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@DataDe DateTime
,@DataAte DateTime
,@NCM VARCHAR(20)
)as


SELECT Distinct Plu into #ImpPlu
					FROM SAIDA_ESTOQUE  WITH(INDEX(IX_SAIDA_ESTOQUE))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND DATA_MOVIMENTO BETWEEN @DataDe AND @DataAte AND data_cancelamento IS NULL 
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial)
						
			
SELECT Distinct PLU into #ImpNF_Item 
		FROM 
		NF_ITEM A 
		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR And A.Filial = B.Filial 
WHERE
	A.FILIAL = @Filial
AND 
	DATA BETWEEN @DataDe AND @DataAte
and 
	B.nf_Canc<>1


Select DISTINCT
      mercadoria.PLU,
      Ean,
      NCM=CF ,
      Descricao,
      Cst_I_E = (select top 1 tributacao.indice_st from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao_ent),
      [Icms Entrada] = (select top 1 tributacao.entrada_icms from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao_ent),
      cst_I_S = (select top 1 tributacao.indice_st from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao),
      [Icms Saida] = (select top 1 tributacao.Saida_ICMS from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao),
      --Codigo_Natureza_receita,
      Cst_P_E= cst_entrada ,
      Cst_P_S = cst_saida 
      
From

      Mercadoria WITH(INDEX(PK_Mercadoria)) left join EAN e  on mercadoria.plu=e.PLU
      inner join W_BR_CADASTRO_DEPARTAMENTO c on (Mercadoria.filial=c.filial and Mercadoria.codigo_departamento= c.codigo_Departamento )

Where
	( Exists(Select * From #ImpPlu p Where p.Plu = mercadoria.Plu)
	Or	
		Exists(Select * From #ImpNF_Item i Where i.Plu = mercadoria.Plu)	
		)
		/*
		and (
			mercadoria.plu in (select plu from Saida_estoque WITH(INDEX(IX_SAIDA_ESTOQUE_01)) where filial=@filial and Data_movimento between @DataDe and @DataAte and data_cancelamento is null group by plu)
			or 
			mercadoria.plu in (select plu from nf_item WITH(INDEX(PK_NF_ITEM)) inner join nf on nf_item.Filial = nf.Filial and nf_item.codigo= nf.Codigo and nf_item.Cliente_Fornecedor=nf.Cliente_Fornecedor and nf_item.Tipo_NF =nf.Tipo_NF   where nf.Filial=@filial and nf.data between @DataDe and @DataAte and nf.nf_Canc<>1 group by plu)
			
			) 
		*/
		and Inativo = 0
		and (E.EAN=@ean or LEN(@ean)=0)
		and (Mercadoria.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (Mercadoria.Descricao_familia = @familia or LEN(@familia)=0)										 
		and (Mercadoria.cf = @NCM or len(@NCM)=0)
     