

/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_B001]    Script Date: 02/13/2017 10:13:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_B001]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_B001]
GO

create Procedure [dbo].[sp_EFD_PisCofins_B001]
AS                            
BEGIN

	SELECT 
	--** Campo [REG] Texto fixo contendo "B001" C 004 - O
	'|B001' + 
	--** Campo [IND_MOV] Indicador de movimento: N 001 - O
		--** 0 - Bloco com dados informados;
		--** 1 - Bloco sem dados informados.
	'|1|'

		RETURN                          
                      
END

go 






--sp_EFD_PisCofins_1010 

ALTER procedure [dbo].[sp_EFD_PisCofins_1010]

AS
	SELECT '|1010|N|N|N|N|N|N|S|N|N|N|N|N|'





	GO 
	
	
	-- STORED PROCEDURE
-- sp_EFD_PisCofins_C170 'MATRIZ', '20180801', '20180831', 'BIMBO DO BRASIL', 4770
ALTER    Procedure [dbo].[sp_EFD_PisCofins_C170]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8),
	@Fornecedor		As Varchar(20),
	@NroNota		As Varchar(10)
AS                            
BEGIN

	SELECT 
		REG = 'C170',
		NUM_ITEM = b.Num_Item,
		COD_ITEM = b.PLU,
		DESCR_COMPL = m.Descricao,
		QTD = b.Qtde,
		UNID = CASE WHEN m.Peso_Variavel = 'PESO' THEN 'KG' ELSE 'UN' END,
		VL_ITEM = b.Total,
		VL_DESC = CASE WHEN b.Desconto >0 THEN (b.Desconto/100) * b.Total ELSE 0 END,
		IND_MOV = '0',
		CST_ICMS = case when len(t.CST_SPED) >= 3 then  t.CST_SPED else '0' + t.CST_SPED End,
		CFOP = CONVERT(VARCHAR(4),b.Codigo_Operacao),
		COD_NAT = CONVERT(VARCHAR(4),a.Codigo_Operacao),
		VL_BC_ICMS = CASE WHEN ISNULL(t.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(t.Redutor,0) / 100))) ELSE 0 End,
		ALIQ_ICMS = CASE WHEN b.Tipo_NF = 1 then t.Saida_ICMS else case when ISNULL(t.Incide_ICMS, 0) =  0 THEN 0 ELSE CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.Entrada_ICMS end end  end,
		VL_ICMS = CASE WHEN ISNULL(t.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(t.Redutor, 0) / 100))) ELSE 0 End * (CASE WHEN b.Tipo_NF = 1 then t.Saida_ICMS else CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.Entrada_ICMS END end / 100),
		VL_BC_ICMS_ST = CASE WHEN ISNULL(t.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(t.Incide_ICM_Subistituicao, 0) = 1 THEN Isnull(b.Base_IVA,0) ELSE 0 END,
		ALIQ_ST = CASE WHEN ISNULL(t.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(t.Incide_ICM_Subistituicao, 0) = 1 THEN t.Entrada_ICMS Else 0 End,
		VL_ICMS_ST = CASE WHEN ISNULL(t.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(t.Incide_ICM_Subistituicao, 0) = 1 THEN Isnull(b.IVA,0) ELSE 0 End,
		IND_APUR = 0,
		CST_IPI = CASE WHEN b.Codigo_Operacao >5000 THEN '99' ELSE '49' END,
		COD_ENQ = '',
		VL_BC_IPI = CASE WHEN ISNULL(t.IPI_EmOutrasDespesas, 0) = 0 AND ISNULL(b.IPIV, 0) > 0 THEN b.Total ELSE 0 END,
		ALIQ_IPI = CASE WHEN ISNULL(t.IPI_EmOutrasDespesas, 0) = 0 AND ISNULL(b.IPI, 0) > 0 THEN Isnull(b.IPI,0) ELSE 0 END,
		VL_IPI = CASE WHEN ISNULL(t.IPI_EmOutrasDespesas, 0) = 0 AND (ISNULL(b.IPIV, 0) > 0 OR ISNULL(b.IPI, 0) > 0) THEN Isnull(b.IPIV,0) ELSE 0 END,
		CST_PIS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN '98' ELSE CASE WHEN b.tipo_nf = 2 THEN m.Cst_Entrada ELSE m.Cst_Saida END END, 
		VL_BC_PIS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) OR (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN b.Total ELSE 0 END END,
		ALIQ_PIS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE 
						WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) THEN m.Pis_Perc_Entrada
						WHEN (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN m.Pis_Perc_Saida
						ELSE 0 
					END END,
		QUANT_BC_PIS = '', 
		ALIQ_PIS_QUANT = '',
		VL_PIS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) OR (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) 
					 THEN convert(decimal(12,2),b.Total * (CASE 
						WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) THEN m.Pis_Perc_Entrada
						WHEN (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN m.Pis_Perc_Saida
						ELSE 0 	END / 100)) ELSE 0 END END,
		CST_COFINS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN '98' ELSE CASE WHEN b.tipo_nf = 2 THEN m.Cst_Entrada ELSE m.Cst_Saida END END, 
		VL_BC_COFINS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) OR (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN b.Total ELSE 0 END END,
		ALIQ_COFINS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE 
						WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) THEN m.COFINS_Perc_Entrada
						WHEN (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN m.COFINS_Perc_Saida
						ELSE 0 
					END END,
		QUANT_BC_COFINS = '', 
		ALIQ_COFINS_QUANT = '',
		VL_COFINS = CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) OR (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) 
					 THEN convert(decimal(12,2),b.Total * (CASE 
						WHEN (b.Tipo_NF = 2 AND CONVERT(INT, m.CST_Entrada) BETWEEN 50 AND 67) THEN m.COFINS_Perc_Entrada
						WHEN (b.Tipo_NF = 1 AND CONVERT(INT, m.CST_Saida) BETWEEN 1 AND 2) THEN m.COFINS_Perc_Saida
						ELSE 0 
					END / 100)) ELSE 0 END END,
		COD_CTA =  (SELECT TOP 1 COD_CONTA FROM Conta_Contabil WHERE ENTRADA =CASE WHEN b.Tipo_NF = 2 THEN 1 ELSE 0 END) -- CASE WHEN b.Tipo_NF = 2 THEN '41101000-2' ELSE '31101000-1' END--,
		,VLR_ABAT_NT = 0	
	FROM
		NF a 
		INNER JOIN NF_Item b ON a.FILIAL = b.FILIAL AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR AND a.Codigo = b.Codigo		
		INNER JOIN Tributacao t ON t.Codigo_Tributacao = b.Codigo_Tributacao
		INNER JOIN Mercadoria m ON m.PLU = b.PLU
		INNER JOIN Natureza_Operacao NatOp ON NatOp.Codigo_Operacao = a.Codigo_Operacao
	WHERE
		a.Filial = @Filial
		AND a.Data BETWEEN @DataInicio AND @DataFim
		AND LTRIM(RTRIM(LTRIM(a.Cliente_Fornecedor))) = @Fornecedor
		AND Convert(Numeric,a.Codigo) = @NroNota
		--AND (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 ))
	ORDER BY 1,2

	RETURN                          

END

--SELECT * FROM NF

/****** Object:  StoredProcedure [dbo].[sp_EFD_ICMSIPI_C190]    Script Date: 12/08/2014 09:47:25 ******/
SET ANSI_NULLS ON

	
	
	
insert into Versoes_Atualizadas select 'PROCEDURES_SPED', getdate();
GO






