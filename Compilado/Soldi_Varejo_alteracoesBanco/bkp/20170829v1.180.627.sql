IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NF_DANFE]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[sp_NF_DANFE]
end
go 



--sp_NF_DANFE 'MATRIZ', '1', '372', '1', 'N'
create PROCEDURE [dbo].[sp_NF_DANFE]
	@FILIAL				VARCHAR(20),
	@CLIENTE_FORNECEDOR VARCHAR(20),
	@NUMERO				VARCHAR(20),
	@TIPONF				VARCHAR(1),
	@TIPOORIGEM			VARCHAR(1)
AS



	IF object_id('tempdb..#DUPLICATAS') IS NOT NULL 
		BEGIN
			 DROP TABLE #DUPLICATAS
		END
	CREATE TABLE #DUPLICATAS (
						FILIAL	VARCHAR(20) DEFAULT '',
						CLIENTE_FORNECEDOR VARCHAR(30) DEFAULT '',
						NUMERO VARCHAR(20) DEFAULT '',
						PARCELA01 VARCHAR(100) DEFAULT '',
						PARCELA02 VARCHAR(100) DEFAULT '',
						PARCELA03 VARCHAR(100) DEFAULT '',
						PARCELA04 VARCHAR(100) DEFAULT '',
						PARCELA05 VARCHAR(100) DEFAULT '',
						PARCELA06 VARCHAR(100) DEFAULT '',
						PARCELA07 VARCHAR(100) DEFAULT '',
						PARCELA08 VARCHAR(100) DEFAULT '',
						PARCELA09 VARCHAR(100) DEFAULT '',
						PARCELA10 VARCHAR(100) DEFAULT '',
						PARCELA11 VARCHAR(100) DEFAULT '',
						PARCELA12 VARCHAR(100) DEFAULT '',
						PARCELA13 VARCHAR(100) DEFAULT '',
						PARCELA14 VARCHAR(100) DEFAULT '',
						PARCELA15 VARCHAR(100) DEFAULT '',
						PARCELA16 VARCHAR(100) DEFAULT '',
						PARCELA17 VARCHAR(100) DEFAULT '',
						PARCELA18 VARCHAR(100) DEFAULT '',
						PARCELA19 VARCHAR(100) DEFAULT '',
						PARCELA20 VARCHAR(100) DEFAULT '',
						PARCELA21 VARCHAR(100) DEFAULT '',
						PARCELA22 VARCHAR(100) DEFAULT '',
						PARCELA23 VARCHAR(100) DEFAULT '',
						PARCELA24 VARCHAR(100)  DEFAULT '')
	
	INSERT INTO #DUPLICATAS (FILIAL, CLIENTE_FORNECEDOR, NUMERO) VALUES (@FILIAL, @CLIENTE_FORNECEDOR, @NUMERO)

	IF @TIPOORIGEM = 'P'
		BEGIN
			DECLARE @FORADOESTADO				AS TINYINT
			DECLARE @SIMPLES					AS TINYINT
			--DECLARE @PLU						AS VARCHAR(20)
			--DECLARE @CONDICAOICMS				AS INTEGER
			--DECLARE @ORIGEM						AS INTEGER
			--DECLARE @TOTAL						AS FLOAT
			--DECLARE @ALIQUOTAICMS				AS FLOAT
			--DECLARE @ALIQICMSDEST				AS FLOAT
			--DECLARE @MVA						AS FLOAT
			--DECLARE @ALIQICMSDESTSIMPLES		AS FLOAT
			--DECLARE @MVASIMPLES					AS FLOAT 
			--DECLARE @CFOP						AS INTEGER
			--DECLARE @BCST						AS FLOAT
			--DECLARE @ICMSRETIDO					AS FLOAT
			--DECLARE @BC							AS FLOAT
			--DECLARE @VICMS						AS FLOAT
			--DECLARE @CST						AS VARCHAR(3)
			--DECLARE @VICMSST					AS FLOAT
			
			--DECLARE @TOTALPRODUTOS				AS FLOAT
			--DECLARE @TOTALBC					AS FLOAT
			--DECLARE @TOTALVICMS					AS FLOAT
			--DECLARE @TOTALBCST					AS FLOAT
			--DECLARE @TOTALICMSRETIDO			AS FLOAT 

			--SELECT 
			--	f.Filial,
			--	ForaDoEstado = 	CASE WHEN f.UF <> c.UF THEN 1 ELSE 0 END,
			--	Simples = CASE WHEN ISNULL(c.Opt_Simples_nac, 0) = 1 THEN 1 ELSE 0 END,
			--	c.Codigo_Cliente,
			--	p.Pedido as Documento,
			--	p.natureza_operacao,
			--	m.PLU,
			--	m.Descricao,
			--	m.CF AS NCM,
			--	m.Und,
			--	i.Embalagem,
			--	m.Origem,
			--	CASE WHEN f.UF <> c.UF THEN m.Cod_Trib_Inter ELSE a.ICMS_Interestadual END AS AliquotaICMS,
			--	--** Colunas Pedido
			--	i.Qtde AS Qtde,
			--	CASE WHEN ISNULL(i.Qtde, 0) > 0 THEN CONVERT(DECIMAL(12,2), i.Total / i.Qtde) ELSE 0 END AS Unitario,
			--	i.Total,
			--	CondicaoICMS = CASE WHEN ISNULL(a.condicao_icms,'1') = 1 OR RTRIM(LTRIM(a.condicao_icms)) = '' THEN 1 ELSE a.condicao_icms END,
			--	AliqICMSDest = ISNULL(a.icms_interestadual, 0),
			--	MVA = ISNULL(a.mva, 0),
			--	AliqICMSDestSimples = ISNULL(a.icms_estado_simples, 0),
			--	MVASimples = ISNULL(a.mva_simples, 0),
			--	CFOP = CONVERT(NUMERIC(4), CASE WHEN f.UF <> c.UF THEN 6000 ELSE 5000 END),
			--	BC = CONVERT(NUMERIC(14,2), 0),
			--	VICMS = CONVERT(NUMERIC(14,2), 0),
			--	BCST = CONVERT(NUMERIC(14,2), 0),
			--	ICMSRetido = CONVERT(NUMERIC(12,2), 0),
			--	CST = '    ',
			--	p.nome_transportadora AS Transportadora,
			--	TipoFrete = 
			--			CASE 
			--				WHEN ISNULL(p.tipo_transporte,'') = 'CIF'THEN '0-Emitente' 
			--			ELSE 
			--				CASE  
			--					WHEN ISNULL(p.tipo_transporte,'') = 'FOB' THEN '1-Destinatario' 
			--				ELSE '' 
			--				END 
			--			END
			--INTO
			--	#PREDANFE
			--FROM 
			--	FILIAL f
			--	INNER JOIN Pedido p ON p.Filial = f.Filial
			--	INNER JOIN Pedido_itens i ON p.filial = i.filial AND p.Pedido = i.Pedido 
			--	INNER JOIN Cliente c ON p.Cliente_Fornec = c.Codigo_Cliente 
			--	INNER JOIN Mercadoria m ON m.PLU = i.PLU
			--	LEFT OUTER JOIN aliquota_imp_estado a ON a.NCM = m.cf AND a.UF = c.UF 
			--	LEFT OUTER JOIN Transportadora t ON p.nome_transportadora = t.Nome_transportadora 
			--WHERE
			--	p.Pedido = @NUMERO AND p.Cliente_Fornec = @CLIENTE_FORNECEDOR

			--IF @@ROWCOUNT > 0
			--	BEGIN
			--		SET @CST = ''
			--		SET @CFOP = 0
			--		SET @BC = 0
			--		SET @VICMS = 0
			--		SET @BCST = 0
			--		SET @ICMSRETIDO = 0

			--		DECLARE Impostos_Cursor	CURSOR FOR SELECT  ForaDoEstado, Simples, PLU, CondicaoICMS, Origem, Total, AliquotaICMS, AliqICMSDest, MVA, AliqICMSDestSimples, MVASimples FROM #PREDANFE
			--		OPEN Impostos_Cursor
			--		FETCH NEXT FROM Impostos_Cursor INTO @FORADOESTADO, @SIMPLES, @PLU, @CONDICAOICMS, @ORIGEM, @TOTAL, @ALIQUOTAICMS, @ALIQICMSDEST, @MVA, @ALIQICMSDESTSIMPLES, @MVASIMPLES
			--		WHILE @@FETCH_STATUS = 0
			--			BEGIN
			--				--** Retido na FONTES
			--				IF @CONDICAOICMS = 2
			--					BEGIN
			--						SET @CST = '60'
			--						SET @CFOP = 405
			--						SET @BC = 0
			--						SET @VICMS = 0
			--						SET @BCST = 0
			--						SET @ICMSRETIDO = 0
			--					END
			--				--** Sem diferenciação
			--				IF @CONDICAOICMS = 1
			--					BEGIN
			--						--** Calcular ICMS PRÓPRIO
			--						SET @BC = @TOTAL 
			--						SET @VICMS = @TOTAL * (@ALIQUOTAICMS / 100)
			--						--** Calcular se existe IVA
			--						IF @MVA > 0 OR @MVA > 0 OR @ALIQICMSDEST > 0 OR @ALIQICMSDESTSIMPLES > 0
			--							BEGIN
			--								SET @CST = '10'
			--								SET @CFOP = 403
			--								--** Checa se a empresa é SIMPLES NACIONAL
			--								IF @SIMPLES = 1
			--									BEGIN
			--										IF @ALIQICMSDESTSIMPLES = 0
			--											SET @ALIQICMSDESTSIMPLES = @ALIQICMSDEST
			--										IF @MVASIMPLES = 0
			--											SET @MVASIMPLES = @MVA 
									
			--										SET @BCST = @TOTAL * (1 + (@MVASIMPLES / 100))
			--										SET @VICMSST = @BCST * (@ALIQICMSDESTSIMPLES / 100)
			--									END
			--								ELSE
			--									BEGIN
			--										SET @BCST = @TOTAL * (1 + (@MVA / 100))
			--										SET @VICMSST = @BCST * (@ALIQICMSDEST / 100) 
			--									END
			--								SET @ICMSRETIDO = @VICMSST - @VICMS 
			--							END
			--						ELSE
			--							BEGIN
			--								SET @CST = '00'
			--								SET @CFOP = 102
			--							END
			--					END
			--				--** Atualiza DADOS
			--				UPDATE #PREDANFE SET BC = @BC, BCST = @BCST, VICMS = @VICMS, ICMSRetido = @ICMSRETIDO , CFOP = (CFOP + @CFOP), CST = CONVERT(VARCHAR(1), @ORIGEM) + @CST  WHERE PLU = @PLU 
			--				FETCH NEXT FROM Impostos_Cursor INTO @FORADOESTADO, @SIMPLES, @PLU, @CONDICAOICMS, @ORIGEM, @TOTAL, @ALIQUOTAICMS, @ALIQICMSDEST, @MVA, @ALIQICMSDESTSIMPLES, @MVASIMPLES
			--			END
			--		CLOSE Impostos_Cursor
			--		DEALLOCATE Impostos_Cursor

			--		--** Gerando TOTAIS
			--		SELECT
			--			@TOTALPRODUTOS = SUM(#PREDANFE.Total),
			--			@TOTALBC = SUM(#PREDANFE.Total),
			--			@TOTALVICMS = SUM(#PREDANFE.VICMS),
			--			@TOTALBCST = SUM(#PREDANFE.BCST),
			--			@TOTALICMSRETIDO = SUM(#PREDANFE.ICMSRetido)
			--		FROM #PREDANFE
			
			--		SELECT 
			--			GETDATE() AS Emissao, 
			--			0 AS Num_item, 
			--			dbo.Filial.Razao_Social, 
			--			d.Filial, 
			--			'000000' AS Codigo,
			--			dbo.Filial.serie_nfe as Serie,
			--			'' AS Protocolo, 
			--			Cliente.Codigo_Cliente as Cliente_Fornecedor,
			--			ID = REPLICATE('0', 44),
			--			ID_nf = dbo.F_MASCARA_ID(''), 
			--			d.PLU, 
			--			d.Descricao, 
			--			d.und, 
			--			d.CST, 
			--			d.Qtde, 
			--			d.Embalagem, 
			--			d.Unitario, 
			--			d.Total, 
			--			d.aliquotaicms AS Aliquota_ICMS,
			--			0 AS IPI, 
			--			0 as IPIV, 
			--			d.Natureza_operacao AS Natureza, 
			--			dbo.F_FORMATAR_IE(dbo.Filial.IE, dbo.Filial.UF) AS IE, 
			--			d.NCM, 
			--			d.CFOP, 
			--			CONVERT(DECIMAL(12, 2),0) AS Desconto, 
			--			CONVERT(DECIMAL(12, 2),0) AS despesas, 
			--			d.BCST as base_iva, 
			--			d.ICMSRetido as iva, 
			--			d.bc as base_icms, 
			--			d.VICMS as icmsv, 
			--			1 AS Saida, 
			--			dbo.F_FORMATAR_CPF_CNPJ(dbo.Filial.CNPJ, 1) AS CNPJ,
			--			Cliente.Nome_Cliente AS DEST_Razao_Social,
			--			dbo.F_FORMATAR_IE(Cliente.IE,Cliente.uf) AS DEST_IE, 
			--			Cliente.CNPJ AS DEST_CNPJ,
			--			RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE(
			--			(
			--				Select top 1 id_meio_comunicacao 
			--				from cliente_contato 
			--				where codigo_cliente =cliente.Codigo_Cliente 
			--					and (Meio_Comunicacao like '%FONE%' OR Meio_Comunicacao like '%CELULAR%') 
			--			),'(',''),')',''), '-',''),'.',''))) AS Telefone,
			--			ISNULL(Cliente.Endereco, '') AS DEST_LOGRADOURO, 
			--			ISNULL(Cliente.complemento_end, '') AS DEST_COMPLEMENTO, 
			--			Cliente.endereco_nro AS DEST_NUMERO, 
			--			Cliente.Bairro AS DEST_BAIRRO, 
			--			Cliente.Cidade AS DEST_CIDADE, 
			--			Cliente.UF AS DEST_UF, 
			--			Cliente.CEP AS DEST_CEP, 
			--			0 AS Frete, 
			--			0 AS Pgto, 
			--			@TOTALPRODUTOS AS TotalProduto, 
			--			@TOTALBC AS TotalBCICMS, 
			--			@TOTALVICMS AS TotalICMS, 
			--			@TOTALBCST AS TotalBCST, 
			--			@TOTALICMSRETIDO AS TotalST, 
			--			CONVERT(DECIMAL(12,2), 0) AS TotalIPI, 
			--			CONVERT(DECIMAL(12,2), 0) AS TotalFrete, 
			--			CONVERT(DECIMAL(12,2), 0) AS TotalDespesas, 
			--			CONVERT(DECIMAL(12,2), 0) AS TotalDesconto, 
			--			@TOTALPRODUTOS + @TOTALICMSRETIDO AS TotalNF,
			--			ISNULL(dup.PARCELA01, '') AS PARCELA01,
			--			ISNULL(dup.PARCELA02, '') AS PARCELA02,
			--			ISNULL(dup.PARCELA03, '') AS PARCELA03,
			--			ISNULL(dup.PARCELA04, '') AS PARCELA04,
			--			ISNULL(dup.PARCELA05, '') AS PARCELA05,
			--			ISNULL(dup.PARCELA06, '') AS PARCELA06,
			--			ISNULL(dup.PARCELA07, '') AS PARCELA07,
			--			ISNULL(dup.PARCELA08, '') AS PARCELA08,
			--			ISNULL(dup.PARCELA09, '') AS PARCELA09,
			--			ISNULL(dup.PARCELA10, '') AS PARCELA10,
			--			ISNULL(dup.PARCELA11, '') AS PARCELA11,
			--			ISNULL(dup.PARCELA12, '') AS PARCELA12,
			--			ISNULL(dup.PARCELA13, '') AS PARCELA13,
			--			ISNULL(dup.PARCELA14, '') AS PARCELA14,
			--			ISNULL(dup.PARCELA15, '') AS PARCELA15,
			--			ISNULL(dup.PARCELA16, '') AS PARCELA16,
			--			ISNULL(dup.PARCELA17, '') AS PARCELA17,
			--			ISNULL(dup.PARCELA18, '') AS PARCELA18,
			--			ISNULL(dup.PARCELA19, '') AS PARCELA19,
			--			ISNULL(dup.PARCELA20, '') AS PARCELA20,
			--			ISNULL(dup.PARCELA21, '') AS PARCELA21,
			--			ISNULL(dup.PARCELA22, '') AS PARCELA22,
			--			ISNULL(dup.PARCELA23, '') AS PARCELA23,
			--			ISNULL(dup.PARCELA24, '') AS PARCELA24,
			--			EnderecoCompleto = REPLACE(RTRIM(LTRIM(dbo.Filial.Endereco)) + ' ' 
			--			+ RTRIM(LTRIM(dbo.Filial.endereco_nro)) + '-' + RTRIM(LTRIM(dbo.Filial.bairro)) + ' ' 
			--			+ RTRIM(LTRIM(dbo.Filial.CEP)) + ' ' + RTRIM(LTRIM(dbo.Filial.CIDADE)) + ' ' 
			--			+ RTRIM(LTRIM(dbo.Filial.UF)) + ' ' + 'Fone:' + dbo.Filial.Telefone,'  ', ' '),
			--			'' AS IDBarra,
			--			dbo.Filial.Logo,
			--			Producao_NFe = 2,
			--			Status_NFe = '', 
			--			ISNULL(t.Razao_social,'') as TRazao_Social,
			--			ISNULL(t.estado,'') as TUF,
			--			ISNULL(t.CNPJ,'') as TCNPJ,
			--			ISNULL(t.Cidade, '') as TMunicipio,
			--			rtrim(ltrim(ISNULL(t.endereco,'')))  as TEndereco,
			--			ISNULL(T.IE,'') AS TIE,
			--			Observacao = '',
			--			d.TipoFrete

			--		FROM
			--			#PREDANFE d 
			--			INNER JOIN dbo.Filial ON dbo.Filial.Filial = d.Filial 
			--			INNER JOIN dbo.Cliente ON dbo.Cliente.Codigo_Cliente = d.Codigo_Cliente
			--			LEFT OUTER JOIN dbo.Transportadora t ON d.Transportadora = t.Nome_transportadora,
			--			#DUPLICATAS dup
			--	END
		END
	ELSE
		BEGIN
			--** EFETUADO A INCLUSAO NA TABELA TEMPORARIA #DUPLICATAS
			DECLARE @FILIAL_DUP				AS VARCHAR(20)
			DECLARE @CLIENTE_FORNECEDOR_DUP	AS VARCHAR(30)
			DECLARE @DOCUMENTO_DUP			AS VARCHAR(10)
			DECLARE @VENCIMENTO_DUP			AS DATETIME
			DECLARE @VALOR_DUP				AS DECIMAL(12,2)
			DECLARE @DUPLICATA_DUP			AS VARCHAR(50)
			DECLARE @N_DUP					AS INTEGER
			DECLARE @SQLAUX					AS NVARCHAR(1000)
			
			DECLARE Duplicatas CURSOR FOR 
					SELECT Filial,  Cliente_Fornecedor, Codigo, Vencimento, Valor 
					FROM NF_Pagamento
					WHERE NF_Pagamento.Filial = @FILIAL AND NF_Pagamento.Cliente_Fornecedor = @CLIENTE_FORNECEDOR AND NF_Pagamento.Codigo = @NUMERO
			OPEN Duplicatas
			FETCH NEXT FROM Duplicatas INTO @FILIAL_DUP, @CLIENTE_FORNECEDOR_DUP, @DOCUMENTO_DUP, @VENCIMENTO_DUP, @VALOR_DUP
			SET @N_DUP = 1
			WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @DUPLICATA_DUP = @DOCUMENTO_DUP + '/' + RTRIM(LTRIM(CONVERT(VARCHAR(2), @N_DUP))) +'   ' + SPACE(15) + CONVERT(VARCHAR, @VENCIMENTO_DUP, 103)  + '   ' + SPACE(2) + REPLACE(CONVERT(VARCHAR, @VALOR_DUP), '.', ',') 
					SET @SQLAUX = 'UPDATE #DUPLICATAS SET PARCELA' + CASE WHEN @N_DUP > 9 THEN CONVERT(VARCHAR(2), @N_DUP) ELSE '0' + CONVERT(VARCHAR(1), @N_DUP) END  + ' = ' + CHAR(39) + @DUPLICATA_DUP + CHAR(39)
					EXECUTE(@SQLAUX)
					SET @N_DUP = @N_DUP + 1
					FETCH NEXT FROM Duplicatas INTO @FILIAL_DUP, @CLIENTE_FORNECEDOR_DUP, @DOCUMENTO_DUP, @VENCIMENTO_DUP, @VALOR_DUP						
				END
			CLOSE Duplicatas
			DEALLOCATE Duplicatas
			
			SELECT   
                         dbo.NF.Emissao, 
			 dbo.nf.Data,
                         dbo.NF_Item.Num_item, 
                         dbo.Filial.Razao_Social, 
                         dbo.NF.Filial, dbo.NF.Codigo, 
						 dbo.Filial.serie_nfe as Serie, 
						 ISNULL(dbo.NF.numero_protocolo,'') AS Protocolo,
                         dbo.nf.Cliente_Fornecedor,
                         ID = case when isnull(dbo.NF.id,'')='' then REPLICATE('0',44) else dbo.BarcodeCode128(isnull(dbo.NF.id,0)) end,
                         dbo.F_MASCARA_ID(ID) as ID_nf, 
                         dbo.NF_Item.PLU, 
                         dbo.Mercadoria.Descricao, 
                         dbo.Mercadoria.und, 
                         CONVERT(VARCHAR(1), ISNULL(dbo.Mercadoria.ORIGEM, 0)) + isnull(dbo.NF_Item.cst_icms,'') AS CST, 
                         dbo.NF_Item.Qtde, 
                         dbo.NF_Item.Embalagem, 
                         dbo.NF_Item.Unitario, 
                         dbo.NF_Item.Total, 
                         dbo.NF_Item.aliquota_icms,
						 ISNULL(dbo.NF_Item.IPI, 0) AS IPI, 
                         ISNULL(dbo.NF_Item.IPIV, 0) AS IPIV, 
                        dbo.Natureza_operacao.Descricao AS Natureza, 
                        dbo.F_FORMATAR_IE(dbo.Filial.IE, dbo.Filial.UF) AS IE, 
                        dbo.NF_Item.NCM, 
                        dbo.NF_Item.codigo_operacao AS CFOP, 
                         CONVERT(DECIMAL(12, 2),isnull(dbo.NF_Item.Desconto,0) / 100 * isnull(dbo.NF_Item.Total,0)) AS Desconto, 
                         dbo.NF_Item.despesas, 
                         dbo.NF_Item.base_iva, 
                         dbo.NF_Item.iva, 
                         isnull(dbo.NF_Item.base_icms,0) as base_icms, 
                         isnull(dbo.NF_Item.icmsv,0) as icmsv, 
                         dbo.Natureza_operacao.Saida, 
                         dbo.F_FORMATAR_CPF_CNPJ(dbo.Filial.CNPJ, 1) AS CNPJ,
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.Razao_social ELSE Cliente.Nome_Cliente END AS DEST_Razao_Social,
                         dbo.F_FORMATAR_IE(CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.IE ELSE Cliente.IE END, CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.UF ELSE Cliente.uf END) AS DEST_IE, 
                         
                         CASE WHEN nf.Dest_Fornec = 1 THEN 
							Fornecedor.CNPJ 
						 ELSE 
							Cliente.CNPJ 
						 END AS DEST_CNPJ, 
 						RTRIM(LTRIM(REPLACE(REPLACE(REPLACE(REPLACE((
							Select top 1 id_meio_comunicacao 
							from cliente_contato 
							where codigo_cliente =cliente.Codigo_Cliente 
								and (Meio_Comunicacao like '%FONE%' OR Meio_Comunicacao like '%CELULAR%') 
						),'(',''),')',''), '-',''),'.',''))) AS Telefone,
						 CASE WHEN nf.Dest_Fornec = 1 THEN 
							RTRIM(LTRIM(ISNULL(Fornecedor.Endereco, '')))+','+ISNULL(Fornecedor.Endereco_nro, '') 
						 ELSE 
							RTRIM(LTRIM(ISNULL(Cliente.Endereco, '')))+','+isnull(cliente.endereco_nro,'') 
                         END AS DEST_LOGRADOURO, 
                         
                         CASE WHEN nf.Dest_Fornec = 1 THEN '' ELSE ISNULL(Cliente.complemento_end, '') END AS DEST_COMPLEMENTO, 
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.Endereco_nro ELSE Cliente.endereco_nro END AS DEST_NUMERO, 
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.Bairro ELSE Cliente.Bairro END AS DEST_BAIRRO, 
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.Cidade ELSE Cliente.Cidade END AS DEST_CIDADE, 
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.UF ELSE Cliente.UF END AS DEST_UF, 
                         CASE WHEN nf.Dest_Fornec = 1 THEN Fornecedor.CEP ELSE Cliente.CEP END AS DEST_CEP, 
                         dbo.NF.Frete, 
                         1 AS Pgto, 
                         isnull(dbo.NF.total_produto,0) AS TotalProduto, 
                         isnull(dbo.NF.Base_Calculo,0) AS TotalBCICMS, 
                         dbo.NF.ICMS_Nota AS TotalICMS, 
                         dbo.NF.Base_Calc_Subst AS TotalBCST, 
                         dbo.NF.ICMS_ST AS TotalST, 
                         dbo.NF.IPI_Nota AS TotalIPI, 
                         dbo.NF.Frete AS TotalFrete, 
                         dbo.NF.Despesas_financeiras AS TotalDespesas, 
                         isnull(dbo.NF.Desconto,0) AS TotalDesconto, 
                         dbo.NF.Total AS TotalNF,
						ISNULL(dup.PARCELA01, '') AS PARCELA01,
						ISNULL(dup.PARCELA02, '') AS PARCELA02,
						ISNULL(dup.PARCELA03, '') AS PARCELA03,
						ISNULL(dup.PARCELA04, '') AS PARCELA04,
						ISNULL(dup.PARCELA05, '') AS PARCELA05,
						ISNULL(dup.PARCELA06, '') AS PARCELA06,
						ISNULL(dup.PARCELA07, '') AS PARCELA07,
						ISNULL(dup.PARCELA08, '') AS PARCELA08,
						ISNULL(dup.PARCELA09, '') AS PARCELA09,
						ISNULL(dup.PARCELA10, '') AS PARCELA10,
						ISNULL(dup.PARCELA11, '') AS PARCELA11,
						ISNULL(dup.PARCELA12, '') AS PARCELA12,
						ISNULL(dup.PARCELA13, '') AS PARCELA13,
						ISNULL(dup.PARCELA14, '') AS PARCELA14,
						ISNULL(dup.PARCELA15, '') AS PARCELA15,
						ISNULL(dup.PARCELA16, '') AS PARCELA16,
						ISNULL(dup.PARCELA17, '') AS PARCELA17,
						ISNULL(dup.PARCELA18, '') AS PARCELA18,
						ISNULL(dup.PARCELA19, '') AS PARCELA19,
						ISNULL(dup.PARCELA20, '') AS PARCELA20,
						ISNULL(dup.PARCELA21, '') AS PARCELA21,
						ISNULL(dup.PARCELA22, '') AS PARCELA22,
						ISNULL(dup.PARCELA23, '') AS PARCELA23,
						ISNULL(dup.PARCELA24, '') AS PARCELA24,
						EnderecoCompleto = REPLACE(RTRIM(LTRIM(dbo.Filial.Endereco)) + ' ' 
						+ RTRIM(LTRIM(dbo.Filial.endereco_nro)) + '-' + RTRIM(LTRIM(dbo.Filial.bairro)) + ' ' 
						+ RTRIM(LTRIM(dbo.Filial.CEP)) + ' ' + RTRIM(LTRIM(dbo.Filial.CIDADE)) + ' ' 
						+ RTRIM(LTRIM(dbo.Filial.UF)) + ' ' + 'Fone:' + dbo.Filial.Telefone,'  ', ' '),
						dbo.NF.IDBarra,
						dbo.Filial.Logo,
						Producao_NFe = CASE WHEN ISNULL(dbo.NF.Producao_NFe,2) <> 1 THEN 2 END,
						Status_NFe = ISNULL(dbo.NF.Status,''),
						ISNULL(t.Razao_social,'') as TRazao_Social,
						ISNULL(t.estado,'') as TUF,
						ISNULL(t.CNPJ,'') as TCNPJ,
						ISNULL(t.Cidade, '') as TMunicipio,
						rtrim(ltrim(ISNULL(t.endereco,'')))  as TEndereco,
						ISNULL(T.IE,'') AS TIE,
						ISNULL(dbo.NF.Observacao,'') AS Observacao,
						TipoFrete = 
								CASE 
									WHEN ISNULL(dbo.NF.tipo_frete,0) = 0 THEN '0-Emitente' 
								ELSE  '1-Destinatario' END
						
                         
			FROM         dbo.NF 
						 INNER JOIN dbo.NF_Item ON dbo.NF.Filial = dbo.NF_Item.Filial AND dbo.NF.Cliente_Fornecedor = dbo.NF_Item.Cliente_Fornecedor AND dbo.NF.Codigo = dbo.NF_Item.Codigo 
						 INNER JOIN dbo.Mercadoria ON dbo.Mercadoria.PLU = dbo.NF_Item.PLU 
						 INNER JOIN dbo.Filial ON dbo.Filial.Filial = dbo.NF.Filial 
						 INNER JOIN dbo.Natureza_operacao ON dbo.NF.Codigo_operacao = dbo.Natureza_operacao.Codigo_operacao 
						 LEFT OUTER JOIN
                         dbo.Cliente ON dbo.Cliente.Codigo_Cliente = dbo.NF.Cliente_Fornecedor LEFT OUTER JOIN
                         dbo.Fornecedor ON dbo.Fornecedor.Fornecedor = dbo.NF.Cliente_Fornecedor LEFT OUTER JOIN
						 dbo.Transportadora t ON dbo.NF.nome_transportadora = t.Nome_transportadora
						 ,#DUPLICATAS Dup --ON Dup.NUMERO = dbo.NF.Codigo 

			WHERE
				(dbo.NF.Filial = @FILIAL  AND dbo.NF.Codigo = @NUMERO  AND RTRIM(LTRIM(dbo.NF.Cliente_Fornecedor)) = @CLIENTE_FORNECEDOR  AND dbo.NF.Tipo_NF = @TIPONF )
				
		END




GO 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Controle_Filial_PDV') 
            AND  UPPER(COLUMN_NAME) = UPPER('SAT'))
begin
	alter table Controle_Filial_PDV alter column SAT TINYINT
end
else
begin
		
	alter table Controle_Filial_PDV add SAT TINYINT

end 
go 



go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.180.627', getdate();
GO




