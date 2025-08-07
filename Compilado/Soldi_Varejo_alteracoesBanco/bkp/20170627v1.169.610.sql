
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tabela_preco') 
            AND  UPPER(COLUMN_NAME) = UPPER('porc'))
begin
	alter table tabela_preco alter column porc numeric(18,2)
end
else
begin
		
		alter table tabela_preco add porc numeric(18,2) not null

end 
go 


IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Conta_Corrente]') AND name = N'PK_Conta_Corrente')
ALTER TABLE [dbo].[Conta_Corrente] DROP CONSTRAINT [PK_Conta_Corrente]
GO


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_corrente') 
            AND  UPPER(COLUMN_NAME) = UPPER('filial'))
begin
	alter table conta_corrente alter column filial varchar(20) not null
end
else
begin
		
	alter table conta_corrente add filial varchar(20) not null

end 
go 



/****** Object:  Index [PK_Conta_Corrente]    Script Date: 06/28/2017 16:46:01 ******/
ALTER TABLE [dbo].[Conta_Corrente] ADD  CONSTRAINT [PK_Conta_Corrente] PRIMARY KEY CLUSTERED 
(
	[id_cc] ASC,
	[Banco] ASC,
	[Agencia] ASC,
	[Conta] ASC,
	[filial] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('vTotTrib'))
begin
	alter table nf alter column vTotTrib numeric(18,2)
end
else
begin
	alter table nf add vTotTrib numeric(18,2)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('vTotTrib'))
begin
	alter table nf_item alter column vTotTrib numeric(18,2)
end
else
begin
	alter table nf_item add vTotTrib numeric(18,2)
end 
go 

IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tI_Fornecedor]'))
	DROP TRIGGER [dbo].[tI_Fornecedor]
	
	
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[tD_Fornecedor]'))
	DROP TRIGGER [dbo].[tD_Fornecedor]

	



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.169.610', getdate();
GO