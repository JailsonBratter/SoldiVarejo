

IF OBJECT_ID (N'Documento_Eletronico', N'U') IS NULL
begin

CREATE TABLE [dbo].[Documento_Eletronico](
	[Filial] [varchar](20) NULL,
	[Tipo] [int] NULL,
	[Data] [datetime] NULL,
	[Caixa] [int] NULL,
	[Documento] [varchar](20) NULL,
	[ID_Chave] [varchar](47) NULL,
	[ID_Chave_Cancelamento] [varchar](47) NULL,
	[Nro_Serie_Equipamento] [varchar](20) NULL,
	[Operador] [int] NULL,
	[CFe_XML] [text] NULL,
	[CFe_XML_Cancelamento] [text] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


end 
go
IF OBJECT_ID (N'SKU', N'U') IS NULL
begin



-- Cria tabela SKU. Utiliza a tabela se necessário conforme parametro ATIVA_SKU_ECOMMERCE
CREATE TABLE [dbo].[SKU](
	[SKU] [varchar](17) NOT NULL,
	[usado] [tinyint] NULL,
	[Filial] [varchar](20) NOT NULL,
 CONSTRAINT [PK__SKU__7C4554E3] PRIMARY KEY CLUSTERED 
(
	[SKU] ASC,
	[Filial] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

end 
go


-- Utiliza a tabela Plu com Exemplo
Insert Into SKU
Select * From PLU where not exists(select * from sku where sku.sku = plu.plu)
go
-- Marca todos os SKU para usado = 0 
Update SKU Set Usado = 0
go

-- Cria na tabela Mercadoria coluna Descricao_Web. Utiliza se necessário conforme parametro ATIVA_SKU_ECOMMERCE
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Descricao_Web'))
begin
	alter table Mercadoria alter column Descricao_Web varchar(100)
end	
else
begin
	alter table Mercadoria add Descricao_Web varchar(100)
end 
go

-- Utilizar por padrão a descrição atual da Mercadoria. Utiliza se necessário conforme parametro ATIVA_SKU_ECOMMERCE
Update Mercadoria Set Descricao_Web = Descricao
go

-- Cria na tabela Mercadoria coluna SKU. Utiliza se necessário conforme parametro ATIVA_SKU_ECOMMERCE
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('SKU'))
begin
	alter table Mercadoria alter column SKU varchar(17)
end	
else
begin
	alter table Mercadoria add SKU varchar(17)
end 
go

-- Marca todos os SKU existentes na tabela Mercadoria para usado = 1. Utiliza se necessário conforme parametro ATIVA_SKU_ECOMMERCE
Update SKU Set Usado = 1 From mercadoria M where SKU.SKU = M.SKU

GO

-- Criando Parametro caso tenha necessidade de trabalhar com SKU para Ecommerce, por padrão FALSE.
if not exists(select 1 from PARAMETROS where PARAMETRO='ATIVA_SKU_ECOMMERCE')
begin	
INSERT INTO PARAMETROS VALUES('ATIVA_SKU_ECOMMERCE', GETDATE(), 'FALSE',GETDATE(), 'FALSE','ENVIA CODIGO DO CAMPO SKU PARA ECOMMERCE','L',0,1,'',0,0,NULL,0)
end



go 

insert into Versoes_Atualizadas select 'Versão:1.292.862', getdate();