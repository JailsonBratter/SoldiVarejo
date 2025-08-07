

 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0150]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0150]
END
GO
Create Procedure [dbo].[sp_EFD_PisCofins_0150] 
	@Filial Varchar(20),
	@data_ini Varchar(23),
	@data_Fim Varchar(23)

AS              
     --Exec SP_EFD_PISCOFINS_0150 'MATRIZ', '20120301', '20120331'         
BEGIN

	DECLARE  @errno   INT,                            
	@errmsg  VARCHAR(255)                            


SELECT 
	--** Campo [REG] Texto fixo contendo "0140" C 004 - O
	REG = '0150' , 
	COD_PART = Fornecedor.Fornecedor,
	--** Campo NOME Nome empresarial da entidade. C 100 - O
	NOME = CASE WHEN LEN(RTRIM(LTRIM(ISNULL(razao_social,'')))) < 100 THEN LTRIM(RTRIM(ISNULL(razao_social,''))) ELSE SUBSTRING(LTRIM(RTRIM(ISNULL(razao_social,''))), 1, 100) END ,      
	PAIS = '1058',
	--** Campo CNPJ Número de inscrição da entidade no CNPJ. N 014* - OC
    CNPJ = case when isnull(pessoa_fisica,0)=1 then '' else SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14) end ,
	CPF = case when isnull(pessoa_fisica,0)=0 then '' else SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14) end ,
	--** Campo IE Inscrição Estadual
        IE = SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(IE,'')), '.', ''), '/', ''), '-', ''), 1, 14),
	--** Campo COD_MUN Código do município do domicílio fiscal da entidade, conforme a tabela IBGE N 007* - O
	COD_MUN = isnull(Fornecedor.codMun, ISNULL((SELECT TOP 1 RTRIM(LTRIM(Munic)) 
				FROM Unidade_Federacao 
				WHERE RTRIM(LTRIM(NOME_MUNIC)) = RTRIM(LTRIM(Fornecedor.Cidade)) 
					and fornecedor.uf = sigla_uf),'3550308')),
	SUFRAMA = '',
	[END] = RTRIM(LTRIM(ISNULL(ENDERECO,''))),
	NUM = RTRIM(LTRIM(ISNULL(ENDERECO_NRO,''))),
	COMPL = '',
	BAIRRO = RTRIM(LTRIM(ISNULL(BAIRRO,'')))

FROM 
	Fornecedor        
WHERE 
		
	rtrim(ltrim(fornecedor)) in 
		(select rtrim(ltrim(a.cliente_fornecedor))
			from nf a
			inner join nf_item b on 
				a.filial=b.filial 
				and a.cliente_fornecedor= b.cliente_fornecedor
				and a.codigo=b.codigo 
			where
				data between @Data_ini and @Data_Fim)
				and filial=@filial
Union All

SELECT 
	--** Campo [REG] Texto fixo contendo "0140" C 004 - O
	REG = '0150' , 
	COD_PART = Cliente.Codigo_Cliente,
	--** Campo NOME Nome empresarial da entidade. C 100 - O
	NOME = CASE WHEN LEN(RTRIM(LTRIM(ISNULL(Nome_Cliente,'')))) < 100 THEN LTRIM(RTRIM(ISNULL(Nome_Cliente,''))) ELSE SUBSTRING(LTRIM(RTRIM(ISNULL(Nome_Cliente,''))), 1, 100) END ,      
	PAIS = '1058',
	--** Campo CNPJ Número de inscrição da entidade no CNPJ. N 014* - OC
        CNPJ =case when isnull(cliente.pessoa_juridica,0) = 1 then SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 14) else '' end,
	CPF = case when isnull(cliente.pessoa_juridica,0) = 0 then SUBSTRING(REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(cnpj,'')), '.', ''), '/', ''), '-', ''), 1, 11) else '' end ,
	--** Campo IE Inscrição Estadual
        IE = SUBSTRING(REPLACE(REPLACE(REPLACE(replace(RTRIM(ISNULL(IE,'')),'isento',''), '.', ''), '/', ''), '-', ''), 1, 14),
	--** Campo COD_MUN Código do município do domicílio fiscal da entidade, conforme a tabela IBGE N 007* - O
	COD_MUN = ISNULL((SELECT TOP 1 RTRIM(LTRIM(Munic)) 
				FROM Unidade_Federacao 
				WHERE RTRIM(LTRIM(NOME_MUNIC)) = RTRIM(LTRIM(cliente.Cidade)) 
					and cliente.uf = sigla_uf),'3550308'),
	SUFRAMA = '',
	[END] = RTRIM(LTRIM(ISNULL(ENDERECO,''))),
	NUM = RTRIM(LTRIM(ISNULL(ENDERECO_NRO,''))),
	COMPL = '',
	BAIRRO = RTRIM(LTRIM(ISNULL(BAIRRO,'')))

FROM 
	Cliente        
WHERE 
		
	rtrim(ltrim(codigo_cliente)) in 
		(select rtrim(ltrim(a.cliente_fornecedor))
			from nf a
			inner join nf_item b on 
				a.filial=b.filial 
				and a.cliente_fornecedor= b.cliente_fornecedor
				and a.codigo=b.codigo 
			where
				data between @Data_ini and @Data_Fim)

RETURN                          

	error:                            
		--RAISERROR @errno @errmsg                            
		RETURN                          
END




go

 
 
 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_Mercadoria_Acum]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_br_Mercadoria_Acum]
END
GO
Create  PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
	@PLU Char(6), @Filial varchar(20) output
AS

begin
-- sp_br_mercadoria_Acum '27456', 'matriz'
	declare @contador as integer
	declare @data as datetime

	set @contador = -12
	set @data = dateadd(month, @contador,getdate())
	
	create table #lixo (dataref datetime)
	
	while @contador <= 0
      begin
        insert into #lixo select dateadd(month, @contador, getdate())
		set @contador = @contador + 1
      end

	Select Data_movimento =sai.Data_movimento
		  ,Qtde = sum(sai.Qtde)
		  ,Vlr = sum(ISNULL(Sai.vlr,0)-ISNULL(sai.Desconto,0))
	 into #venda From #lixo lixo left join saida_Estoque Sai with(index=IX_saida_estoque_01 ) on 
						substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,sai.data_movimento,102),1,7) 
		And Sai.Data_Cancelamento is null 
		AND SAI.PLU=@PLU 
		AND Filial=@Filial
	 group by sai.Data_movimento
  
  insert into #venda
	  Select nf.emissao 
			 ,Qtde = sum(ni.Qtde*ni.Embalagem)
			 ,Vlr = sum(ni.total)
	  from #lixo lixo left join nf  on 
			substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,nf.Emissao,102),1,7)  
			inner join nf_item as ni on nf.codigo=ni.Codigo 
								and nf.Tipo_NF = ni.Tipo_NF
								and nf.filial = ni.filial 
								and nf.Cliente_Fornecedor = ni.Cliente_Fornecedor
			inner join Natureza_operacao as nop on nf.Codigo_operacao = nop.Codigo_operacao
			where 	isnull(nf.nf_Canc,0)=0
					AND ni.PLU=@PLU 
					AND nf.Filial=@Filial
					and nop.Saida = 1
	   group by nf.Emissao				



	select ORDEM =substring(convert(varchar,lixo.dataref,102),1,7),
		   MesAno = substring(convert(varchar,lixo.dataref,103),4,7),
		   qtde = sum(ISNULL(SAI.Qtde,0)), 
		   vlr = sum(ISNULL(SAI.vlr,0)), 
		   PrcMD =convert(decimal(9,2),avg(case when isnull(SAI.Vlr,0) > 0 and
						isnull(SAI.Qtde,0) > 0 then isnull(SAI.Vlr,0)/isnull(SAI.Qtde,0) else 0
					end))
	From #lixo lixo left join #venda Sai  on 
		substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,sai.data_movimento,102),1,7) 
	group by substring(convert(varchar,lixo.dataref,102),1,7), substring(convert(varchar,lixo.dataref,103),4,7)
	order by substring(convert(varchar,lixo.dataref,102),1,7) desc
	
end


go
insert into Versoes_Atualizadas select 'Versão:1.262.806', getdate();






















