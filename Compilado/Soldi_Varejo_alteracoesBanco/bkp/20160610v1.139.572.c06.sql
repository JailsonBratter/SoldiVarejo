
/****** Object:  Index [PK_Conta_Corrente]    Script Date: 06/10/2016 12:26:39 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Conta_Corrente]') AND name = N'PK_Conta_Corrente')
ALTER TABLE [dbo].[Conta_Corrente] DROP CONSTRAINT [PK_Conta_Corrente]
GO
alter table conta_corrente alter column filial varchar(20) not null
go
/****** Object:  Index [PK_Conta_Corrente]    Script Date: 06/10/2016 12:26:39 ******/
ALTER TABLE [dbo].[Conta_Corrente] ADD  CONSTRAINT [PK_Conta_Corrente] PRIMARY KEY CLUSTERED 
(
	[id_cc] ASC,
	[Banco] ASC,
	[Agencia] ASC,
	[Conta] ASC,
	[filial] asc
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

