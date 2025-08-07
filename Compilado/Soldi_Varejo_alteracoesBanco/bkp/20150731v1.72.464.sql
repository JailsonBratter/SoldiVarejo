if not exists(select 1 from PARAMETROS where PARAMETRO='PED_AVISTA_NAO_FINANCEIRO')
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
           ('PED_AVISTA_NAO_FINANCEIRO'
           ,GETDATE()
           ,'TRUE'
           ,GETDATE()
           ,'TRUE'
           ,'PEDIDO QUANDO PAGAMENTO FOR A VISTA, NÃO GERA CONTA A RECEBER'
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