
/****** Object:  StoredProcedure [dbo].[sp_br_Mercadoria_Acum]    Script Date: 06/22/2017 11:06:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_Mercadoria_Acum]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_br_Mercadoria_Acum]
GO


--=============================
-- sp_br_mercadoria_Acum '27456', 'matriz'
CREATE    PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
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


  
	select ORDEM =substring(convert(varchar,lixo.dataref,102),1,7),
		   MesAno = substring(convert(varchar,lixo.dataref,103),4,7),
		   qtde = sum(ISNULL(SAI.Qtde,0)), 
		   vlr = sum(ISNULL(SAI.vlr,0)-ISNULL(Desconto,0)), 
		   PrcMD =convert(decimal(9,2),avg(case when isnull(SAI.Vlr,0) > 0 and
						isnull(SAI.Qtde,0) > 0 then isnull(SAI.Vlr,0)/isnull(SAI.Qtde,0) else 0
					end))
	From #lixo lixo left join saida_Estoque Sai (index=IX_saida_estoque_01 ) on 
		substring(convert(varchar,lixo.dataref,102),1,7)=substring(convert(varchar,sai.data_movimento,102),1,7) 
		And Sai.Data_Cancelamento is null 
		AND SAI.PLU=@PLU 
		AND Filial=@Filial
	group by substring(convert(varchar,lixo.dataref,102),1,7), substring(convert(varchar,lixo.dataref,103),4,7)
	
	order by substring(convert(varchar,lixo.dataref,102),1,7) desc
	
	end


GO



/****** Object:  StoredProcedure [dbo].[sp_br_Mercadoria_Acum_Dia]    Script Date: 06/22/2017 11:32:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_br_Mercadoria_Acum_Dia]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia]
GO

-- sp_br_mercadoria_acum_dia '10057', 'matriz'
CREATE     PROCEDURE [dbo].[sp_br_Mercadoria_Acum_Dia] 
	@PLU Char(6), @Filial varchar(20) output
AS
	begin
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
		Dia = case 
			when datepart(weekday, #temUlt30.dia)= 1 then 'DOM'
			when datepart(weekday, #temUlt30.dia)= 2 then 'SEG'
			when datepart(weekday, #temUlt30.dia)= 3 then 'TER'
			when datepart(weekday, #temUlt30.dia)= 4 then 'QUA'
			when datepart(weekday, #temUlt30.dia)= 5 then 'QUI'
			when datepart(weekday, #temUlt30.dia)= 6 then 'SEX'
			else 'SAB' end + '-' + substring(convert(varchar,#temUlt30.dia, 103), 1, 5),
		--data_movimento, 
		qtde =  isnull(sum(qtde),0),
		vlr = isnull(sum(vlr),0), 
		PrcMD = ISNULL(
		convert(decimal(9,2),
		avg(case when isnull(saida_estoque.Vlr,0) > 0 and
					  isnull(saida_estoque.Qtde,0) > 0 then
					isnull(saida_estoque.Vlr,0)/isnull(saida_estoque.Qtde,0) else 0 end)),0)
		from #temUlt30 
		
		left join saida_Estoque WITH (index=ix_saida_estoque_01) on 
		#temUlt30.plu collate database_default = saida_estoque.plu collate database_default and 
		convert(varchar,#temUlt30.dia, 101)=convert(varchar,Data_Movimento, 101) and data_cancelamento is null 
		and saida_estoque.Filial collate database_default = @Filial collate database_default
		where 
		
		#temUlt30.plu collate database_default = @PLU collate database_default
		and #temUlt30.dia >= dateadd(Day,-30,GetDate())
		
		group by #temUlt30.dia, #temUlt30.plu
		order by convert(varchar,#temUlt30.dia, 102) Desc
		
		
end







GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.166.607', getdate();
GO