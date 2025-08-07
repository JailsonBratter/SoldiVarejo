
/****** Object:  Index [PK__Cliente_contato__5FE90D57]    Script Date: 09/04/2015 10:29:38 ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Cliente_contato]') AND name = N'PK__Cliente_contato__5FE90D57')
	ALTER TABLE [dbo].[Cliente_contato] DROP CONSTRAINT [PK__Cliente_contato__5FE90D57]
GO
	UPDATE Cliente_contato SET id_Meio_Comunicacao ='' WHERE id_Meio_Comunicacao IS NULL
GO
ALTER TABLE Cliente_contato ALTER COLUMN id_Meio_Comunicacao varchar(50) NOT NULL
go

/****** Object:  Index [PK__Cliente_contato__5FE90D57]    Script Date: 09/04/2015 10:29:38 ******/
ALTER TABLE [dbo].[Cliente_contato] ADD PRIMARY KEY CLUSTERED 
(
	[Codigo_Cliente] ASC,
	[Meio_comunicacao] ASC,
	[id_Meio_comunicacao] ASc
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF,  IGNORE_DUP_KEY = OFF,  ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


