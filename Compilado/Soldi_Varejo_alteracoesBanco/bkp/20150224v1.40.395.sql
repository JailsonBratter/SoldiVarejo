
alter table conta_a_receber add tipo_recebimento varchar(20)

go 

--=============================
CREATE NONCLUSTERED INDEX [IX_Saida_Estoque_01] ON [dbo].[Saida_estoque] 
(
	[Filial] ASC,
	[PLU] ASC,
	[Data_movimento] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

--=============================
ALTER    PROCEDURE [dbo].[sp_br_Mercadoria_Acum] 
	@PLU Char(6), @Filial varchar(20) output
AS

begin
	declare @contador as integer

declare @data as datetime

 

set @contador = -13

set @data = dateadd(month, @contador,getdate())

 

create table #lixo (dataref datetime)

 

while @contador < 0

      begin

            insert into lixo select dateadd(month, @contador, getdate())

            set @contador = @contador + 1

         

      end

	select MesAno = substring(convert(varchar,lixo.dataref,102),1,7), qtde = sum(ISNULL(SAI.Qtde,0)), vlr = sum(ISNULL(SAI.vlr,0))
	From lixo left join saida_Estoque Sai (index =IX_saida_estoque_01 ) on substring(convert(varchar,lixo.dataref,102),1,7)= substring(convert(varchar,sai.data_movimento,102),1,7)AND SAI.PLU=@PLU AND Filial=@Filial
	group by substring(convert(varchar,lixo.dataref,102),1,7)
	
	order by 1 desc
	
	end
