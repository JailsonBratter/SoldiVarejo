if OBJECT_ID(N'Produtos_Carga',N'U') is null
BEGIN
	CREATE TABLE [dbo].[Produtos_Carga](
		[ID_Carga] [varchar](40) NULL,
		[ID_Produto] [int] NULL,
		[CodigoBarras] [nvarchar](17) NULL,
		[Descricao] [nvarchar](50) NULL,
		[ID_Depto] [int] NULL,
		[Preco] [float] NULL,
		[Unidade] [nvarchar](5) NULL,
		[Peso] [float] NULL,
		[Pesavel] [int] NOT NULL,
		[FatorConversao] [float] NULL,
		[ID_Tributacao] [int] NULL,
		[DataAlteracao] [datetime] NULL,
		[DataCarga] [datetime] NULL,
		[ICMS] [float] NULL,
		[PercentualImposto] [float] NULL,
		[Preco0] [float] NULL,
		[Preco1] [float] NULL,
		[Preco2] [float] NULL,
		[Preco3] [float] NULL,
		[Preco4] [float] NULL,
		[Preco5] [float] NULL,
		[UN] [varchar](2) NULL,
		[Origem] [tinyint] NULL,
		[CST_ICMS] [varchar](3) NULL,
		[CFOP] [numeric](5, 0) NULL,
		[NCM] [varchar](10) NULL,
		[Qtde_Tributaria] [numeric](9, 3) NULL,
		[Carga_Tributaria_Municipal] [numeric](5, 2) NULL,
		[Carga_Tributaria_Estadual] [numeric](5, 2) NULL,
		[Carga_Tributaria_Federal] [numeric](5, 2) NULL,
		[CEST] [numeric](7, 0) NULL,
		[CST_PIS] [varchar](2) NULL,
		[CST_Cofins] [varchar](2) NULL,
		[Aliquota_PIS] [numeric](5, 2) NULL,
		[Aliquota_Cofins] [numeric](5, 2) NULL,
		[Bebida_Alcoolica] [tinyint] NULL,
		[Preco_Atacado] [float] NULL,
		[Margem_Atacado] [float] NULL,
		[Quantidade_Atacado] [int] NULL,
		[Embalagem] [int] NULL,
		[Promocao] [tinyint] NULL,
		[OfertaInicio] [datetime] NULL,
		[OfertaFim] [datetime] NULL,
		[PrecoPromocao] [float] NULL,
		[VendaComSenha] [bit] NULL,
		[ImpAux] [bit] NULL
	) ON [PRIMARY]
END 

 GO
 if OBJECT_ID(N'Promocao_carga',N'U') is null
BEGIN

	CREATE TABLE [dbo].[Promocao_carga](
		[ID_carga] [varchar](40) NULL,
		[Codigo] [int] NOT NULL,
		[Tipo] [int] NOT NULL,
		[Inicio] [datetime] NOT NULL,
		[Fim] [datetime] NOT NULL,
		[Descricao] [varchar](50) NULL,
		[Param_Base] [float] NULL,
		[Param_Brinde] [float] NULL
	) ON [PRIMARY]
END
GO

if OBJECT_ID(N'Promocao_base_carga',N'U') is null
BEGIN
	CREATE TABLE [dbo].[Promocao_base_carga](
		[ID_carga] [varchar](40) NULL,
		[Codigo_promo] [int] NOT NULL,
		[Plu] [int] NULL
	) ON [PRIMARY]
END
GO

if OBJECT_ID(N'Promocao_brinde_carga',N'U') is null
BEGIN

	CREATE TABLE [dbo].[Promocao_brinde_carga](
		[ID_carga] [varchar](40) NULL,
		[Codigo_promo] [int] NOT NULL,
		[Plu] [int] NULL
	) ON [PRIMARY]
END
GO
if OBJECT_ID(N'[carga_pdv]',N'U') is null
BEGIN
	CREATE TABLE [dbo].[carga_pdv](
		[ID_Carga] [varchar](40) NULL,
		[Caixa] [numeric](3, 0) NULL,
		[status] [tinyint] NOT NULL
	) ON [PRIMARY]
	
END
GO
ALTER TABLE [dbo].[carga_pdv] ADD  DEFAULT ((0)) FOR [status]
GO

if OBJECT_ID(N'[Tabela_Preco_carga]',N'U') is null
BEGIN
	CREATE TABLE [dbo].[Tabela_Preco_carga](
		[ID_Carga] [varchar](40) NULL,
		[Codigo_tabela] varchar(6) NULL,
		[filial] varchar(20) NOT NULL,
		[Nro_tabela] [decimal](2, 0) NULL,
		[porc] [numeric](18, 2) NULL,
		[PLU] VARCHAR(17) NULL
	) ON [PRIMARY]
	
END
GO


insert into Versoes_Atualizadas select 'Versão:1.302.880', getdate();