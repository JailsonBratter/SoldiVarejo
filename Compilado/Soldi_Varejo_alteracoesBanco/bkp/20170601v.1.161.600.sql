

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('operadores') 
            AND  UPPER(COLUMN_NAME) = UPPER('OpCaixa'))
begin
	alter table operadores alter column OpCaixa tinyint
end
else
begin
		
		alter table operadores add OpCaixa tinyint

end 

go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('comanda_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('Usuario_Cancelamento'))
begin
	alter table comanda_item alter column Usuario_Cancelamento varchar(20) 
end
else
begin
		
		alter table comanda_item add Usuario_Cancelamento varchar(20) default ''

end 
go 

/****** Object:  StoredProcedure [dbo].[SP_REL_COMANDA_HISTORICO]    Script Date: 06/02/2017 10:55:42 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_HISTORICO]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO]
GO




--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO] (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(5),
		@finalizado varchar(5),
		@usuarioCancelamento varchar(20)
		)
		 AS
		 /* 
set @filial='MATRIZ'
set @comanda='1414'
set @dataInicio='20150901' 
set @datafim = '20160122'

set @horaInicio='00:00' 
set @horafim='23:59' 
-- set @status =''
set @usuario ='TODOS'
set @cancelamento='TODOS'
set @finalizado ='TODOS'
		
	*/
--sp_help comanda_item
--select * from Comanda_Item where data between '20150901' and '20160122'
select  i.Comanda, 
	   Data=convert(varchar,i.data,103),
	   Hora = convert(varchar,i.hora_evento,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.Usuario,
	   i.Plu,
	   m.Descricao,
	   i.Qtde,
	   i.Unitario,
	   i.Total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	   ,[Usuario Canc]= i.Usuario_Cancelamento  
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and  (len(@comanda)=0 or CONVERT(VARCHAR(5),i.comanda)=@comanda)  
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
	  and (@usuario='TODOS' OR  i.usuario = @usuario ) 
	  and (@cancelamento='TODOS' OR  (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento ))
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And(@finalizado='TODOS' OR case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado)
	  and (@usuarioCancelamento = 'TODOS' OR i.Usuario_Cancelamento = @usuarioCancelamento)
order by i.hora_evento desc 
	  
	  	  	  


GO





IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.161.600', getdate();
GO