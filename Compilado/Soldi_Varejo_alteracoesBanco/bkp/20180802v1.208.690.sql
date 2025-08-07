
if not exists(select 1 from PARAMETROS where PARAMETRO='EMAIL_SSL')
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
           ('EMAIL_SSL'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'CONFIGURA E-MAIL SSL TRUE'
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


insert into Versoes_Atualizadas select 'Versão:1.208.690', getdate();
GO


