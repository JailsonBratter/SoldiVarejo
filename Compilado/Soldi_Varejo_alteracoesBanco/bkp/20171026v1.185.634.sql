
if not exists(select 1 from PARAMETROS where PARAMETRO='PDV_BLOQ_OPERACAO')
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
           ('PDV_BLOQ_OPERACAO'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'NÃO PERMITE FECHAMENTO DA TESOURARIA ENQUANTO O CAIXA ESTIVER OPERANDO'
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

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.185.634', getdate();
GO

