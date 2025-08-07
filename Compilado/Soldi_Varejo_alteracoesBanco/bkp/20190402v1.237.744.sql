
IF  OBJECT_ID('LISTA_PADRAO', N'U') IS NULL
begin 
	
	CREATE TABLE [dbo].[LISTA_PADRAO](
		[ID] [int] IDENTITY(1,1) NOT NULL,
		[DESCRICAO] [varchar](80) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[ID] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

END 
go 

IF  OBJECT_ID('LISTA_PADRAO_ITENS', N'U') IS NULL
begin 


CREATE TABLE [dbo].[LISTA_PADRAO_ITENS](
	[ID_LISTA] [int] NULL,
	[PLU] [varchar](20) NULL
) ON [PRIMARY]

end
GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Contrato_fornecedor') 
            AND  UPPER(COLUMN_NAME) = UPPER('Qtde_minima'))
begin
	alter table Contrato_fornecedor alter column Qtde_minima numeric(18,2)
end
else
begin
	alter table Contrato_fornecedor add Qtde_minima numeric(18,2)
end 
go 

GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('prog_seg'))
begin
	alter table mercadoria alter column prog_seg tinyint
	alter table mercadoria alter column prog_ter tinyint
	alter table mercadoria alter column prog_qua tinyint
	alter table mercadoria alter column prog_qui tinyint
	alter table mercadoria alter column prog_sex tinyint
	alter table mercadoria alter column prog_sab tinyint
	alter table mercadoria alter column prog_dom tinyint
end
else
begin
	alter table mercadoria add prog_seg tinyint
	alter table mercadoria add prog_ter tinyint
	alter table mercadoria add prog_qua tinyint
	alter table mercadoria add prog_qui tinyint
	alter table mercadoria add prog_sex tinyint
	alter table mercadoria add prog_sab tinyint
	alter table mercadoria add prog_dom tinyint
end 
go 


GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('plu_receita'))
begin
	alter table mercadoria alter column plu_receita varchar(20)
	
end
else
begin
	alter table mercadoria add plu_receita varchar(20)
end 
go 


GO
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Cotacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('Descricao'))
begin
	alter table Cotacao alter column Descricao varchar(100)
	
end
else
begin
	alter table Cotacao add  Descricao varchar(100)
end 
go 



go
	insert into Versoes_Atualizadas select 'Versão:1.237.744', getdate();
GO
