if not exists(select 1 from PARAMETROS where PARAMETRO='QTDE_IMPRESSAO_PEDIDO')
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
           ('QTDE_IMPRESSAO_PEDIDO'
           ,GETDATE()
           ,'1'
           ,GETDATE()
           ,'1' 
           ,'QTDE IMPRESSAO PEDIDO'
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


insert into Versoes_Atualizadas select 'Versão:1.215.707', getdate();
GO

