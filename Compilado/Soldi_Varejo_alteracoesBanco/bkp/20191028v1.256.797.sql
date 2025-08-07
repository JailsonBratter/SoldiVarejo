-- ****** RODAR COMANDO POR COMANDO *****

alter table NF drop constraint df_nf_serie
go
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table nf alter column serie int not null
end
else
begin
	alter table nf add serie int not null default(1)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NF_Item') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table NF_Item alter column serie int not null
end
else
begin
	alter table NF_Item add serie int not null default(1)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NF_pagamento') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table NF_pagamento alter column serie int not null
end
else
begin
	alter table NF_pagamento add serie int not null default(1)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_log') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table nf_log alter column serie int not null
end
else
begin
	alter table nf_log add serie int
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('NF_Item_log') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table NF_Item_log alter column serie int not null
end
else
begin
	alter table NF_Item_log add serie int
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_justificativa_edicao') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table nf_justificativa_edicao alter column serie int not null
end
else
begin
	alter table nf_justificativa_edicao add serie int
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('nf_justificativa_edicao') 
            AND  UPPER(COLUMN_NAME) = UPPER('cliente_fornecedor'))
begin
	alter table nf_justificativa_edicao alter column cliente_fornecedor varchar(100)
end
else
begin
	alter table nf_justificativa_edicao add cliente_fornecedor varchar(100)
end 
go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_pagar') 
            AND  UPPER(COLUMN_NAME) = UPPER('serie'))
begin
	alter table conta_a_pagar alter column serie int not null
end
else
begin
	alter table conta_a_pagar add serie int not null default(1)
end 
go 


/****** Object:  Index [PK_NF]    Script Date: 28/10/2019 16:04:49 ******/
ALTER TABLE [dbo].[NF] DROP CONSTRAINT [PK_NF] WITH ( ONLINE = OFF )
go 
ALTER TABLE [dbo].[NF_Item] DROP CONSTRAINT [PK_NF_Item] WITH ( ONLINE = OFF )
go

ALTER TABLE [dbo].[NF_Pagamento] DROP CONSTRAINT [PK_NF_Pagamento] WITH ( ONLINE = OFF )
GO
/****** Object:  Index [PK_Conta_a_pagar]    Script Date: 29/10/2019 10:23:22 ******/
ALTER TABLE [dbo].[Conta_a_pagar] DROP CONSTRAINT [PK_Conta_a_pagar] WITH ( ONLINE = OFF )
GO



update nf set serie = SUBSTRING(id, 23,3)
update nf_item set serie = SUBSTRING(id, 23,3) from NF where nf_item.Codigo = NF.Codigo and nf_item.Cliente_Fornecedor=NF.Cliente_Fornecedor and nf_item.Filial = NF.Filial and nf_item.Tipo_NF = NF.Tipo_NF
update nf_pagamento set serie = SUBSTRING(id, 23,3) from NF where nf_pagamento.Codigo = NF.Codigo and nf_pagamento.Cliente_Fornecedor=NF.Cliente_Fornecedor and nf_pagamento.Filial = NF.Filial and nf_pagamento.Tipo_NF = NF.Tipo_NF
update nf_justificativa_edicao set serie = 1 
update nf_justificativa_edicao set cliente_fornecedor = (Select top 1 cliente_fornecedor from nf where nf.codigo = nf_justificativa_edicao.codigo_nota and nf.Tipo_NF = 2)
update Conta_a_pagar set serie = 1 

GO


/****** Object:  Index [PK_NF]    Script Date: 28/10/2019 16:04:50 ******/
ALTER TABLE [dbo].[NF] ADD  CONSTRAINT [PK_NF] PRIMARY KEY CLUSTERED 
(
	[Codigo] ASC,
	[Cliente_Fornecedor] ASC,
	[Tipo_NF] ASC,
	[Filial] ASC,
	[Serie] Asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



alter table nf_item alter column Num_item int not null
GO

/****** Object:  Index [PK_NF_Item]    Script Date: 28/10/2019 16:56:55 ******/
ALTER TABLE [dbo].[NF_Item] ADD  CONSTRAINT [PK_NF_Item] PRIMARY KEY CLUSTERED 
(
	[Filial] ASC,
	[Codigo] ASC,
	[Cliente_Fornecedor] ASC,
	[Tipo_NF] ASC,
	[PLU] ASC,
	[Num_item] ASC,
	[serie] asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


/****** Object:  Index [PK_NF_Pagamento]    Script Date: 28/10/2019 16:58:24 ******/


SET ANSI_PADDING ON
GO

/****** Object:  Index [PK_NF_Pagamento]    Script Date: 28/10/2019 16:58:24 ******/
ALTER TABLE [dbo].[NF_Pagamento] ADD  CONSTRAINT [PK_NF_Pagamento] PRIMARY KEY CLUSTERED 
(
	[Vencimento] ASC,
	[Filial] ASC,
	[Codigo] ASC,
	[Cliente_Fornecedor] ASC,
	[Tipo_NF] ASC,
	[Serie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO





/****** Object:  Index [PK_Conta_a_pagar]    Script Date: 29/10/2019 10:23:22 ******/
ALTER TABLE [dbo].[Conta_a_pagar] ADD  CONSTRAINT [PK_Conta_a_pagar] PRIMARY KEY CLUSTERED 
(
	[Documento] ASC,
	[Fornecedor] ASC,
	[Filial] ASC,
	[Valor] ASC,
	[emissao] ASC,
	[documento_emitido] ASC,
	[serie] asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO





go 
insert into Versoes_Atualizadas select 'Versão:1.256.797', getdate();