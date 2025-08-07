

if not exists(select 1 from PARAMETROS where PARAMETRO='NF_ENT_NAO_PLU')
begin	
INSERT INTO [PARAMETROS]
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
           ('NF_ENT_NAO_PLU'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'BLOQUEIA ALTERAÇÃO DE PLU DE NOTAS IMPORTADAS'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_Produtos_nf]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Rel_Venda_Produtos_nf]
END
GO


CREATE  PROCEDURE [dbo].[sp_Rel_Venda_Produtos_nf] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@Codigo_Cliente varchar(50)
as
begin
		-- exec sp_Rel_Venda_Produtos_nf 'MATRIZ','20190101','20191208',''
	Select ordem= cliente.codigo_cliente
		,nf.Emissao
		,nf.CODIGO
		,convert(varchar,NF.Data,103) Data
		, nf.Cliente_Fornecedor Cod
		,cliente.Nome_Cliente
		,nf_item.plu
		,nf_item.Descricao
		,Qtde= (nf_item.Qtde*nf_item.Embalagem)
		,nf_item.Total
		into #itensClientes
	from NF inner join cliente on NF.Cliente_Fornecedor= cliente.Codigo_Cliente 
			INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO 	
			inner join Natureza_operacao  as  Nto on nf.Codigo_operacao = nto.Codigo_operacao 	
	where NF.Tipo_NF = 1  
	and nto.Saida = 1
	and (convert(Date,NF.Emissao)between @DataDe and @DataAte )
	and (len(@Codigo_Cliente)=0 or cliente.Codigo_Cliente=@Codigo_Cliente )
		 
  Select PLU,DESCRICAO, QTDE ,TOTAL   from (
	  Select ORDEM =ORDEM+'BBB',  plu , descricao ,QTDE = SUM(qtde),TOTAL= SUM(total) 
	  from   #itensClientes 
	  group by ORDEM, plu , descricao 
	  union all 
	  SELECT ORDEM =ORDEM+'AAA','|-TITULO-|' + COD,NOME_CLIENTE,SUM(qtde),SUM(total) 
	  FROM #itensClientes
	  GROUP BY ORDEM, COD,Nome_Cliente
	 

  ) as a 
  ORDER BY ORDEM 

end
	



GO 
insert into Versoes_Atualizadas select 'Versão:1.253.794', getdate();