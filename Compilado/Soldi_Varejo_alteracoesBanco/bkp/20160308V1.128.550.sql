if not exists(select 1 from SEQUENCIAIS where [TABELA_COLUNA]='DEVOLUCAO.PEDIDO')
begin	

INSERT INTO [SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           (
           'DEVOLUCAO.PEDIDO'
           ,'SEQUENCIA DE DEVOLUCAO'
           ,'0'
           ,6
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0
           )

END
GO
if not exists(select 1 from SEQUENCIAIS where [TABELA_COLUNA]='DEVFORNECEDOR.PEDIDO')
begin	

INSERT INTO [SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           (
           'DEVFORNECEDOR.PEDIDO'
           ,'SEQUENCIA DE DEVOLUCAO'
           ,'0'
           ,6
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0
           )

END
GO
