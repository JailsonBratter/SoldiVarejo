IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Controle_Filial_pdv') 
            AND  UPPER(COLUMN_NAME) = UPPER('Carga_Automatica'))
begin
			ALTER TABLE Controle_Filial_pdv
				 alter column Carga_Automatica Tinyint
			ALTER TABLE Controle_Filial_pdv
				 alter column Integracao_Vendas_Automatica Tinyint
			ALTER TABLE Controle_Filial_pdv
				 alter column Data_Ultima_Integracao_Vendas DateTime
			ALTER TABLE Controle_Filial_pdv
				 alter column ConnectionString Varchar(250)
end
else
begin
		ALTER TABLE Controle_Filial_pdv
		add
			Carga_Automatica Tinyint,
			Integracao_Vendas_Automatica Tinyint,
			Data_Ultima_Integracao_Vendas DateTime,
			ConnectionString Varchar(250)
end 
go 






GO

 
 IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria_Poky]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria_Poky]
END
GO

CREATE PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria_Poky]
    @Filial   VARCHAR(20),
    @Data     DATETIME
AS
--sp_Cons_Cadastro_Mercadoria_Poky 'CAFE ODEBRECHT', '2018-07-19 00:00:00.000'
BEGIN

	SELECT
			PLU = convert(int, m.plu )
			, EAN = isnull(ean.ean,m.plu)
			, M.Descricao_resumida
			, DEPARTAMENTO = convert(int, substring(isnull(m.codigo_departamento,0),1,3))
			, ml.preco
			, und
			, peso
			, pesov.codigo
			, Fator_conversao
			, t.Nro_ecf
			, ml.data_alteracao
			, DATAATUAL = getdate()
			, t.Saida_ICMS
			, imp.aliquota_imposto
			, ml.preco
			, PRC1 = 0
			, PRC2 = 0
			, PRC3 = 0
			, PRC4 = 0
			, PRC5 = 0
			, und
			, origem
			, CSTICMS = ltrim(rtrim(t.indice_st))
			, CFOP = case when ltrim(rtrim(t.indice_st)) in ('10', '30', '60', '70','201','202','203','500') then  5405 else 5102 end
			, NCM = m.cf
			, COL01 = 1
			, COL02 = 0
			, COL03 = 0
			, COL04 = 0
			, CEST = convert(numeric,case when cest = ''then  '0' else isnull(cest,'0') end )
			, CSTPIS =  m.cst_saida
			, CSTCOFINS = m.cst_saida
			, ALIQPIS = m.pis_perc_saida
			, ALIQCOFINS = m.cofins_perc_saida
			, m.alcoolico
			, ml.preco_atacado
			, ml.margem_atacado
			, ml.qtde_atacado
			, m.embalagem
			, ml.promocao
			, ml.data_inicio
			, ml.data_fim
			, ml.preco_promocao

	FROM mercadoria as m
				inner join mercadoria_loja as ml on m.plu = ml.plu
				left join ean on m.plu = ean.plu
				inner join tributacao as t on m.codigo_tributacao = t.codigo_tributacao
				inner join peso_variavel as pesov on rtrim(ltrim(m.peso_variavel)) = pesov.peso_variavel
				left join imposto_nota as imp on m.cf = imp.ncm

	where inativo = 0
	  and ml.filial = @Filial
	  AND M.DATA_ALTERACAO >= @Data

END
go 


ALTER   PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia] 
	@PLU Char(6), @Filial varchar(20) output
AS
	begin
	-- sp_br_mercadoria_acum_dia '10002', 'matriz'
--DECLARE @PLU Char(6), @Filial varchar(20) 

--set @PLU = '10057'
--set @Filial= 'MATRIZ'


IF object_id('tempdb..#temUlt30') IS NOT NULL 
BEGIN
	DROP TABLE #temUlt30
END

create table #temUlt30 
(
	plu varchar(6),
	dia datetime
)
insert into #temUlt30 
values  (@plu,dateadd(Day,-30,GetDate())),
		(@plu,dateadd(Day,-29,GetDate())),
		(@plu,dateadd(Day,-28,GetDate())),
		(@plu,dateadd(Day,-27,GetDate())),
		(@plu,dateadd(Day,-26,GetDate())),
		(@plu,dateadd(Day,-25,GetDate())),
		(@plu,dateadd(Day,-24,GetDate())),
		(@plu,dateadd(Day,-23,GetDate())),
		(@plu,dateadd(Day,-22,GetDate())),
		(@plu,dateadd(Day,-21,GetDate())),
		(@plu,dateadd(Day,-20,GetDate())),
		(@plu,dateadd(Day,-19,GetDate())),
		(@plu,dateadd(Day,-18,GetDate())),
		(@plu,dateadd(Day,-17,GetDate())),
		(@plu,dateadd(Day,-16,GetDate())),
		(@plu,dateadd(Day,-15,GetDate())),
		(@plu,dateadd(Day,-14,GetDate())),
		(@plu,dateadd(Day,-13,GetDate())),
		(@plu,dateadd(Day,-12,GetDate())),
		(@plu,dateadd(Day,-11,GetDate())),
		(@plu,dateadd(Day,-10,GetDate())),
		(@plu,dateadd(Day,-9,GetDate())),
		(@plu,dateadd(Day,-8,GetDate())),
		(@plu,dateadd(Day,-7,GetDate())),
		(@plu,dateadd(Day,-6,GetDate())),
		(@plu,dateadd(Day,-5,GetDate())),
		(@plu,dateadd(Day,-4,GetDate())),
		(@plu,dateadd(Day,-3,GetDate())),
		(@plu,dateadd(Day,-2,GetDate())),
		(@plu,dateadd(Day,-1,GetDate())),
		(@plu,GetDate())
		
-- select *	from #temUlt30 	;
		select 
			Dia = #temUlt30.dia
			,qtde =  isnull(sum(qtde),0),
			vlr = isnull(sum(vlr),0) 
			
	  into #venda from #temUlt30 
				left join saida_Estoque WITH (index=ix_saida_estoque_01) on 
					#temUlt30.plu collate database_default = saida_estoque.plu collate database_default 
				and convert(varchar,#temUlt30.dia, 101)=convert(varchar,Data_Movimento, 101) 
				and data_cancelamento is null 
				and saida_estoque.Filial collate database_default = @Filial collate database_default
		where 
		
			#temUlt30.plu collate database_default = @PLU collate database_default
			and #temUlt30.dia >= dateadd(Day,-30,GetDate())
		
		group by Saida_estoque.Data_movimento, #temUlt30.dia, #temUlt30.plu
		
		
		insert into #venda 
		select 
			#temUlt30.dia , 
			qtde =  isnull(sum(ni.Qtde),0),
			vlr = isnull(sum(ni.Total),0) 
			
	  from #temUlt30 
				inner join nf_item as ni on 
					#temUlt30.plu collate database_default = ni.plu collate database_default 
				inner join nf  on nf.codigo=ni.Codigo 
								and nf.Tipo_NF = ni.Tipo_NF
								and nf.filial = ni.filial 
								and nf.Cliente_Fornecedor = ni.Cliente_Fornecedor
								and convert(varchar,#temUlt30.dia, 101)=convert(varchar,nf.Emissao, 101) 
								and isnull(nf.nf_Canc,0) =0 
								and nf.Filial collate database_default = @Filial collate database_default
				inner join Natureza_operacao as nop on nf.Codigo_operacao = nop.Codigo_operacao
				
				
		where 
		
			#temUlt30.plu collate database_default = @PLU collate database_default
			and #temUlt30.dia >= dateadd(Day,-30,GetDate())
			and nop.Saida = 1
		group by nf.Emissao, #temUlt30.dia, #temUlt30.plu
		order by convert(varchar,#temUlt30.dia, 102) Desc
		


	Select Dia = case 
				when datepart(weekday, dia)= 1 then 'DOM'
				when datepart(weekday, dia)= 2 then 'SEG'
				when datepart(weekday, dia)= 3 then 'TER'
				when datepart(weekday, dia)= 4 then 'QUA'
				when datepart(weekday, dia)= 5 then 'QUI'
				when datepart(weekday, dia)= 6 then 'SEX'
				else 'SAB' end + '-' + substring(convert(varchar,dia, 103), 1, 5)
			--data_movimento
		 , qtde =sum(qtde)
		 , vlr  =sum(vlr)
		 ,PrcMD = ISNULL(
			convert(decimal(9,2),
			avg(case when isnull(Vlr,0) > 0 and
						  isnull(Qtde,0) > 0 then
						isnull(Vlr,0)/isnull(Qtde,0) else 0 end)),0)
	 From 	  #venda  
	group by dia
	order by convert(varchar,dia, 102) Desc
end

go 


ALTER  PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
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
insert into Versoes_Atualizadas select 'Versão:1.264.808', getdate();

