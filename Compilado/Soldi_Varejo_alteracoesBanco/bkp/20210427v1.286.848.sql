if not exists(select 1 from PARAMETROS where PARAMETRO='PED_HAB_STATUS')
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
           ('PED_HAB_STATUS'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'HABILITA EDICAO DE STATUS PEDIDO'
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

insert into Versoes_Atualizadas select 'Vers�o:1.286.848', getdate();