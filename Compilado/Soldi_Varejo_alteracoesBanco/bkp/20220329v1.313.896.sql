if not exists(select 1 from PARAMETROS where PARAMETRO='PEDIDO_DESC_TOTAL')
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
           ('PEDIDO_DESC_TOTAL'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'TRUE'
           ,'PEDIDO VENDA CALCULA VALOR TOTAL DO DESCONTO'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
go 

insert into Versoes_Atualizadas select 'Versão:1.313.896', getdate();
