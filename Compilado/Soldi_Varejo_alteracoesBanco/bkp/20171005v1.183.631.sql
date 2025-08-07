--Não permitir mesmo codigo natureza operacao

/****** Object:  Index [PK_Natureza_operacao]    Script Date: 10/05/2017 16:33:35 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Natureza_operacao]') AND name = N'PK_Natureza_operacao')
	ALTER TABLE [dbo].[Natureza_operacao] DROP CONSTRAINT [PK_Natureza_operacao]
GO


/****** Object:  Index [PK_Natureza_operacao]    Script Date: 10/05/2017 16:33:35 ******/
ALTER TABLE [dbo].[Natureza_operacao] ADD  CONSTRAINT [PK_Natureza_operacao] PRIMARY KEY CLUSTERED 
(
	[Codigo_operacao] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.182.630', getdate();
GO

