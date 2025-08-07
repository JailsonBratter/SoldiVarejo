
/****** Object:  Index [PK_Conta_a_receber]    Script Date: 01/10/2017 09:23:30 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Conta_a_receber]') AND name = N'PK_Conta_a_receber')
	ALTER TABLE [dbo].[Conta_a_receber] DROP CONSTRAINT [PK_Conta_a_receber]
GO

alter table conta_a_receber alter column Documento varchar(50) not null

go

/****** Object:  Index [PK_Conta_a_receber]    Script Date: 01/10/2017 09:23:30 ******/
ALTER TABLE [dbo].[Conta_a_receber] ADD  CONSTRAINT [PK_Conta_a_receber] PRIMARY KEY CLUSTERED 
(
	[Documento] ASC,
	[Emissao] ASC,
	[Vencimento] ASC,
	[operador] ASC,
	[finalizadora] ASC,
	[documento_emitido] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/****** Object:  Index [PK_Conta_a_pagar]    Script Date: 01/10/2017 09:27:30 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Conta_a_pagar]') AND name = N'PK_Conta_a_pagar')
ALTER TABLE [dbo].[Conta_a_pagar] DROP CONSTRAINT [PK_Conta_a_pagar]
GO

alter table conta_a_pagar alter column Documento varchar(50) not null

go 
/****** Object:  Index [PK_Conta_a_pagar]    Script Date: 01/10/2017 09:27:30 ******/
ALTER TABLE [dbo].[Conta_a_pagar] ADD  CONSTRAINT [PK_Conta_a_pagar] PRIMARY KEY CLUSTERED 
(
	[Documento] ASC,
	[Fornecedor] ASC,
	[Filial] ASC,
	[Valor] ASC,
	[emissao] ASC,
	[documento_emitido] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.153.587', getdate();
GO