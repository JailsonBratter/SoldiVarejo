
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('manifesto_consultas') 
            AND  UPPER(COLUMN_NAME) = UPPER('cStat'))
begin
	alter table manifesto_consultas alter column cStat varchar(3)
end
else
begin
	alter table manifesto_consultas add cStat varchar(3)
end 
go 

if not exists(select 1 from PARAMETROS where PARAMETRO='TEMPO_MANIFESTO')
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
           ('TEMPO_MANIFESTO'
           ,GETDATE()
           ,'60'
           ,GETDATE()
           ,'5'
           ,'TEMPO EM MINUTOS DE INTERVALO DE CONSULTA DO MANIFESTO'
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
go 

insert into Versoes_Atualizadas select 'Versão:1.314.897', getdate();