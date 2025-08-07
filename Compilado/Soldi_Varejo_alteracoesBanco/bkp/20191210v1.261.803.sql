IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('usuario_alteracao'))
begin
	alter table nf alter column usuario_alteracao varchar(20)
end
else
begin
	alter table nf add usuario_alteracao varchar(8)
end 
go 


 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Fiscal_vendas]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].sp_rel_Fiscal_vendas
END
GO

create procedure [dbo].[sp_rel_Fiscal_vendas] (
	@filial varchar(20)
	,@dataDe varchar(11)
	,@dataAte varchar(11)
	,@plu varchar(17)
	)
as
begin 
	Select s.Plu
		, Ean=(Select top 1 ean  from ean where plu = s.plu )
		,m.Descricao
		,NCM = m.cf 
		,[CST ICMS]=t.Indice_ST
		,[Perc Icms]=s.Aliquota_ICMS
		,[CST Pis Cofins ]=m.cst_saida
		,pis= m.Pis_perc_Saida
		,cofins = m.cofins_perc_Saida
		,Valor= sum(isnull(s.vlr-desconto,0))
		,[Total Icms]= convert(decimal(18,2),(sum(isnull(s.vlr-desconto,0)) * s.aliquota_icms) / 100)
		,[Total Pis]= convert(decimal(18,2),(sum(isnull(s.vlr-desconto,0)) * m.Pis_Perc_Saida) / 100)
		,[Total Cofins]= convert(decimal(18,2),(sum(isnull(s.vlr-desconto,0)) * Cofins_Perc_Saida) / 100)
	into #FiscalVenda from saida_estoque as s
	 inner join mercadoria as m on s.plu= m.plu
	 inner join Tributacao as t on m.Codigo_Tributacao = t.Codigo_Tributacao
	where s.Data_movimento  between @dataDe and @dataAte
	group by
		s.filial
		,s.plu
		,m.Descricao
		,m.cf 
		,t.Indice_ST
		,s.Aliquota_ICMS
		,m.cst_saida
		,m.Pis_Perc_Saida
		,m.Cofins_Perc_Saida

	
	Select Plu
		,Ean
		,Descricao
		,NCM 
		,[CST ICMS]
		,[Perc Icms]
		,[CST Pis Cofins ]
		,pis
		,cofins 
		,Valor= sum(Valor)
		,[Total Icms]= sum([Total Icms])
		,[Total Pis]=sum([Total Pis])
		,[Total Cofins]= sum([Total Cofins]) 
	from #FiscalVenda
	group by Plu
		,Ean
		,Descricao
		,NCM 
		,[CST ICMS]
		,[Perc Icms]
		,[CST Pis Cofins ]
		,pis
		,cofins
		order by convert(int,PLU)
end 

 go
insert into Versoes_Atualizadas select 'Versão:1.261.803', getdate();