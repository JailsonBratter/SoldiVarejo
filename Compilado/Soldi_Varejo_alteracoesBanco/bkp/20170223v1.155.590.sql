
CREATE TABLE [dbo].[Mercadoria_Estoque_Mes](
                [Filial] [varchar](20) NOT NULL,
                [Data] [datetime] NOT NULL,
                [PLU] [varchar](20) NOT NULL,
                [Saldo] [decimal](15, 3) NULL,
                [Preco_Custo] [decimal](12, 2) NULL,
CONSTRAINT [PK_Mercadoria_Estoque_Mes] PRIMARY KEY CLUSTERED 
(
                [Filial] ASC,
                [Data] ASC,
                [PLU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Mercadoria_Estoque_Mes] ADD  DEFAULT ((0)) FOR [Saldo]
GO




if not exists(select 1 from PARAMETROS where PARAMETRO='NAO_CALCULA_PRECO_VENDA')
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
           ('NAO_CALCULA_PRECO_VENDA'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'NÃO ATUALIZA PRECO DE VENDA , CALCULANDO A MARGEM NA TELA DE PRODUTOS'
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.155.590', getdate();
GO