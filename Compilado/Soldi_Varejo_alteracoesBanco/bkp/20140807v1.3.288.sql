
CREATE TABLE [dbo].[NF_log](
	[Codigo] [varchar](20) NOT NULL,
	[Cliente_Fornecedor] [varchar](50) NOT NULL,
	[Tipo_NF] [tinyint] NOT NULL,
	[Data] [datetime] NULL,
	[Codigo_operacao] [decimal](4, 0) NULL,
	[Codigo_operacao1] [decimal](4, 0) NULL,
	[Emissao] [datetime] NULL,
	[Filial] [varchar](20) NOT NULL,
	[Total] [decimal](12, 2) NULL,
	[Desconto] [decimal](12, 2) NULL,
	[Frete] [decimal](12, 2) NULL,
	[Seguro] [decimal](12, 2) NULL,
	[IPI_Nota] [decimal](12, 2) NULL,
	[Outras] [decimal](12, 2) NULL,
	[ICMS_Nota] [decimal](12, 2) NULL,
	[Estado] [tinyint] NULL,
	[Base_Calculo] [decimal](12, 2) NULL,
	[Despesas_financeiras] [decimal](12, 2) NULL,
	[Pedido] [varchar](8) NULL,
	[Base_Calc_Subst] [decimal](12, 2) NULL,
	[Observacao] [text] NULL,
	[nf_Canc] [tinyint] NULL,
	[nome_transportadora] [varchar](20) NULL,
	[qtde] [decimal](9, 3) NULL,
	[especie] [varchar](30) NULL,
	[marca] [varchar](5) NULL,
	[numero] [decimal](5, 0) NULL,
	[peso_bruto] [decimal](9, 3) NULL,
	[peso_liquido] [decimal](9, 3) NULL,
	[tipo_frete] [tinyint] NULL,
	[funcionario] [varchar](20) NULL,
	[centro_custo] [varchar](10) NULL,
	[encargo_financeiro] [decimal](12, 2) NULL,
	[ICMS_ST] [decimal](12, 2) NULL,
	[Pedido_cliente] [varchar](8) NULL,
	[Fornecedor_CNPJ] [varchar](18) NULL,
	[Placa] [varchar](8) NULL,
	[Endereco_Entrega] [varchar](50) NULL,
	[Desconto_geral] [decimal](12, 2) NULL,
	[nome_fantasia] [varchar](50) NULL,
	[boleto_recebido] [tinyint] NULL,
	[usuario] [varchar](20) NULL,
	[nfe] [tinyint] NULL,
	[XML] [tinyint] NULL,
	[ID] [varchar](44) NULL,
	[serie] [varchar](3) NULL,
	[status] [varchar](20) NULL,
	[usuario_alteracao] [varchar](50) NULL,
	[data_alteracao] [datetime] NULL
	)
	
GO	

CREATE TABLE [dbo].[NF_Item_log](
	[Filial] [varchar](20) NOT NULL,
	[Codigo] [varchar](20) NOT NULL,
	[Cliente_Fornecedor] [varchar](20) NOT NULL,
	[Tipo_NF] [tinyint] NOT NULL,
	[PLU] [varchar](17) NOT NULL,
	[Codigo_Tributacao] [numeric](3, 0) NULL,
	[Qtde] [numeric](9, 3) NULL,
	[Embalagem] [numeric](3, 0) NULL,
	[Unitario] [numeric](12, 4) NULL,
	[Desconto] [numeric](8, 4) NULL,
	[Total] [numeric](12, 4) NULL,
	[IPI] [numeric](5, 3) NULL,
	[Descricao] [varchar](80) NULL,
	[IPIV] [numeric](8, 2) NULL,
	[ean] [varchar](17) NULL,
	[iva] [numeric](12, 4) NULL,
	[base_iva] [numeric](12, 4) NULL,
	[margem_iva] [numeric](8, 4) NULL,
	[despesas] [numeric](12, 2) NULL,
	[CFOP] [numeric](4, 0) NULL,
	[CODIGO_REFERENCIA] [varchar](20) NULL,
	[aliquota_icms] [numeric](5, 2) NULL,
	[redutor_base] [numeric](5, 2) NULL,
	[codigo_operacao] [numeric](4, 0) NULL,
	[Frete] [numeric](8, 4) NULL,
	[Num_item] [int] NULL,
	[PISV] [float] NULL,
	[COFINSV] [float] NULL,
	[NCM] [varchar](30) NULL,
	[Und] [varchar](3) NULL,
	[Artigo] [varchar](5) NULL,
	[Peso_liquido] [numeric](9, 3) NULL,
	[Peso_Bruto] [numeric](9, 3) NULL,
	[Tipo] [varchar](20) NULL,
	[CF] [varchar](12) NULL,
	[CSTPIS] [varchar](3) NULL,
	[CSTCOFINS] [varchar](3) NULL,
	[pCredSN] [numeric](10, 2) NULL,
	[vCredicmssn] [numeric](10, 2) NULL,
	[usuario_alteracao] [varchar](50) NULL,
	[data_alteracao] [datetime] NULL
) 

GO



	alter table filial add
				Dt_Fechamento_estoque datetime ,
				Dt_Fechamento_financeiro datetime 


go



ALTER PROCEDURE [dbo].[sp_Rel_Venda_Filial] 
	@Data	AS VARCHAR(8),
	@totalizador as integer=0
AS
	DECLARE @TOTAL	AS FLOAT
	if(@totalizador=0)
	begin
	
	
	SELECT 
		Filial.Loja,
		Filial.Filial,
		Vendas = SUM(VLR - ISNULL(Desconto, 0))
	INTO 
		#MRG
	FROM 
		Filial INNER JOIN Saida_Estoque (INDEX=IX_SAIDA_ESTOQUE) ON Filial.Filial = Saida_Estoque.Filial
	WHERE
		Saida_Estoque.Data_Movimento = @Data
	AND
		Saida_Estoque.Data_Cancelamento IS NULL
	GROUP BY 
		Filial.Filial, Filial.Loja
	ORDER BY
		Filial.Loja

	SELECT @TOTAL = SUM(VENDAS) FROM #MRG
	
	SELECT *, Participacao = CONVERT(DECIMAL(12,2), (Vendas / @TOTAL) * 100) FROM #MRG
	UNION ALL
	SELECT 999, 'TOTAL ....>>', @TOTAL, 100
	end
	else
	begin
	
	
	SELECT 
		Filial.Loja,
		Filial.Filial,
		Vendas = SUM(VLR - ISNULL(Desconto, 0))
	INTO 
		#MRG2
	FROM 
		Filial INNER JOIN Saida_Estoque (INDEX=IX_SAIDA_ESTOQUE) ON Filial.Filial = Saida_Estoque.Filial
	WHERE
		Saida_Estoque.Data_Movimento = @Data
	AND
		Saida_Estoque.Data_Cancelamento IS NULL
	GROUP BY 
		Filial.Filial, Filial.Loja
	ORDER BY
		Filial.Loja

	SELECT @TOTAL = SUM(VENDAS) FROM #MRG2
	
	SELECT *, Participacao = CONVERT(DECIMAL(12,2), (Vendas / @TOTAL) * 100) FROM #MRG2
	end
	
	
