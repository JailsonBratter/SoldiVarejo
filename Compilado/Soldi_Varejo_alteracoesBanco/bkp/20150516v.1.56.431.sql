GO
/****** Object:  StoredProcedure [dbo].[sp_br_Mercadoria_Acum_Dia]    Script Date: 05/15/2015 17:11:13 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO


-- sp_br_mercadoria_acum_dia '10057', 'matriz'
ALTER     PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia] 
	@PLU Char(6), @Filial varchar(20) output
AS
	begin
		select 
		Dia = case 
			when datepart(weekday, saida_estoque.data_movimento)
= 1 then 'Dom'
			when datepart(weekday, saida_estoque.data_movimento)
= 2 then 'Seg'
			when datepart(weekday, saida_estoque.data_movimento)
= 3 then 'Ter'
			when datepart(weekday, saida_estoque.data_movimento)
= 4 then 'Qua'
			when datepart(weekday, saida_estoque.data_movimento)
= 5 then 'Qui'
			when datepart(weekday, saida_estoque.data_movimento)
= 6 then 'Sex'
			else 'Sab' end + '-' + substring(convert(varchar,
Data_Movimento, 101), 1, 5),
		--data_movimento, 
		qtde = sum(qtde), vlr = sum(vlr), PrcMD =
convert(decimal(9,2),avg(case when isnull(saida_estoque.Vlr,0) > 0 and
isnull(saida_estoque.Qtde,0) > 0 then
isnull(saida_estoque.Vlr,0)/isnull(saida_estoque.Qtde,0) else 0 end))
		from saida_Estoque (index=ix_saida_estoque_01) inner join
mercadoria on saida_Estoque.plu = mercadoria.plu where 
		saida_estoque.Filial = @Filial
		and saida_Estoque.plu = @PLU
		and data_movimento >= dateadd(Day,-15,GetDate())
		and data_cancelamento is null 
		group by data_movimento, saida_estoque.plu, descricao
		order by data_movimento Desc
	end


GO
/****** Object:  StoredProcedure [dbo].[sp_br_Mercadoria_Acum]    Script Date: 05/15/2015 17:05:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--=============================
--sp_br_mercadoria_Acum '23938', 'matriz'
ALTER    PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
	@PLU Char(6), @Filial varchar(20) output
AS

begin
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

	select MesAno = substring(convert(varchar,lixo.dataref,102),1,7),
qtde = sum(ISNULL(SAI.Qtde,0)), vlr = sum(ISNULL(SAI.vlr,0)-ISNULL(Desconto,0)), PrcMD =
convert(decimal(9,2),avg(case when isnull(SAI.Vlr,0) > 0 and
isnull(SAI.Qtde,0) > 0 then isnull(SAI.Vlr,0)/isnull(SAI.Qtde,0) else 0
end))
	From #lixo lixo left join saida_Estoque Sai (index
=IX_saida_estoque_01 ) on substring(convert(varchar,lixo.dataref,102),1,7)=
substring(convert(varchar,sai.data_movimento,102),1,7) And Sai.Data_Cancelamento is null AND SAI.PLU=@PLU AND
Filial=@Filial
	group by substring(convert(varchar,lixo.dataref,102),1,7)
	
	order by 1 desc
	
	end












