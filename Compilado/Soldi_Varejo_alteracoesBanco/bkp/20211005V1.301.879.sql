if not exists(select 1 from PARAMETROS where PARAMETRO='OBS_ORCAMENTO')
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
           ('OBS_ORCAMENTO'
           ,GETDATE()
           ,''
           ,GETDATE()
           ,'CONFIRMAR VALORES E FORMAS DE PAGAMENTOS QUE PODEM SOFRER ALTERAÇÕES'
           ,'OBSERVAÇÃO IMPRESSÃO ORCAMENTO'
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
insert into Versoes_Atualizadas select 'Versão:1.301.879', getdate();