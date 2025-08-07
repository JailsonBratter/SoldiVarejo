-- ALTERAÇÕES TABELA ================================================================================================
ALTER TABLE Controle_Filial_PDV ADD Diretorio_Carga Varchar(70) Default ''
GO
ALTER TABLE Controle_Filial_PDV ADD NGS Tinyint DEFAULT 0
GO
ALTER TABLE FILIAL DROP COLUMN DIRETORIO_TRANSMITE, DIRETORIO_RESPOSTA



-- ALTERÇÕES PROCEDURE ===============================================================================================


ALTER PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
	@Filial			Varchar(20),
	@TipoCadastro	Int = 0,
	@Alterados		int = 0
AS
	Declare @StringSQL	AS nVarChar(3000)
	Declare @StringSQL2 As nVarChar(3000)
	Declare @Where		As nVarChar(1000)
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SET @StringSQL = ''
	SET @StringSQL2 = ''
	
	
	
	SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
		+ '	Mercadoria.descricao, Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then Peso_Variavel.Codigo else 0 end,' 
		+ '	pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
		+ '	MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
		+ '	Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS '  
		+ '	FROM Mercadoria '
		+ '	INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
		+ ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
		--+ '	INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
		+ ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
		+ ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
		+ '	INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
		+ ' WHERE ISNULL(Inativo, 0) <= 0  '
	
	IF @TipoCadastro < 100 
		BEGIN
			SET @Where = ' AND Tributacao.Filial = ' + CHAR(39) + 'MATRIZ' + CHAR(39)
		END
	ELSE
		BEGIN
			SET @Where = ' AND Peso_Variavel.Codigo > 0 '
		END
	IF @Alterados =1
		begin
			SET @Where = @Where+ ' AND estado_mercadoria=1'
		end 
	
	SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)
		
	-- Adicionar o EAN
	IF @TipoCadastro < 100
		BEGIN
			SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
				+ '	Mercadoria.descricao, Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then Peso_Variavel.Codigo else 0 end,' 
				+ '	pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
				+ '	MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
				+ '	Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS '  
				+ '	FROM Mercadoria '
				+ ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
				+ '	INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
		--		+ '	INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
				+ ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
				+ ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
				+ '	INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
				+ ' WHERE ISNULL(Inativo, 0) <= 0 '
				+ ' AND Tributacao.Filial = ' + CHAR(39) + @Filial + CHAR(39)
		END


	SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2'

	--PRINT (@StringSQL)
	EXEC(@StringSQL) 	
END


GO 
--==========================================================================================================================


ALTER procedure [dbo].[sp_NFe_Cobr]
	@id varchar(47)
	as

	SELECT 
		ID,
		emit_cnpj,
		ide_nNF,
		ide_dEmi,
		cobr_fat_nFat,
		cobr_fat_vOrig,
		cobr_fat_vDesc,
		cobr_fat_vLiq,
		cobr_dup_ndup,
		cobr_dup_dVenc,
		cobr_dup_vDup 
	FROM
		nfe_xml 
	WHERE
		(cobr_dup_ndup is not null)-- or cobr_fat_vLiq is not null) 
		and id=@id
	order by cobr_dup_dVenc

