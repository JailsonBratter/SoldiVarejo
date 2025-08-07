/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_0200]    Script Date: 01/03/2017 09:44:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0200]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0200]
GO

/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_0200]    Script Date: 01/03/2017 09:44:14 ******/


--sp_EFD_PisCofins_0200 'MATRIZ','20161101', '20161130'

CREATE         procedure [dbo].[sp_EFD_PisCofins_0200]
	@Filial	AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
AS

SELECT Distinct Plu into #0200Plu
					FROM SAIDA_ESTOQUE with (index (IX_Saida_Estoque))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial and Controle_Filial_PDV.NGS = 0)
						AND DATA_MOVIMENTO BETWEEN @DataInicio AND @DataFim AND data_cancelamento IS NULL --And Not PLU in(100100,99077))
						
						
SELECT Distinct PLU into #0200NF_Item 
		FROM 
		NF_ITEM A with (index (IX_NF_Item_0200))
		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR 
		INNER JOIN Natureza_operacao NatOp ON  NatOp.filial = b.Filial and NatOp.Codigo_operacao=b.Codigo_operacao
WHERE
	A.FILIAL = @Filial
AND 
	DATA BETWEEN @DataInicio AND @DataFim
and a.codigo_operacao not in (6929,5929)
and ISNULL(b.nf_Canc,0)<>1
and (b.Tipo_NF=2 or isnull(NatOp.NF_devolucao,0)=1 )
--AND 
--	EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR )
	
--SELECT Distinct PLU into #0200NF_ItemCliente 
--		FROM 
--		NF_ITEM A with (index (IX_NF_Item_0200))
--		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO --AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR 
--WHERE
--	A.FILIAL = @Filial
----AND
----	Not A.Codigo_Operacao in ('5102')	
--AND 
--	DATA BETWEEN @DataInicio AND @DataFim
--AND 
--	EXISTS(SELECT * FROM Cliente CI WHERE CI.Codigo_Cliente = B.CLIENTE_FORNECEDOR )


	SELECT
		REG = '0200',
		COD_ITEM = PLU,
		DESCR_ITEM = ISNULL(DESCRICAO,'PRODUTO'),
		COD_BARRA = ISNULL((SELECT TOP 1 EAN FROM Ean WHERE Ean.PLU = Mercadoria.PLU),''), 
		COD_ANT_ITEM = '',
		UNID_INV = CASE WHEN ISNULL(PESO_VARIAVEL, 'NÃO') = 'PESO' THEN 'KG' ELSE 'UN' END,
		TIPO_ITEM = '00',
		COD_NCM = CASE WHEN LEN(RTRIM(LTRIM(REPLACE(CF,'.','')))) = 8 THEN RTRIM(LTRIM(REPLACE(CF,'.',''))) ELSE '' END ,
		EX_IPI = '',
		COD_GEN = '',
		COD_LST = '',
		ALIQ_ICMS = (SELECT TOP 1 SAIDA_ICMS FROM Tributacao WHERE Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao)
	FROM
		MERCADORIA with (index (PK_Mercadoria))
	Where
		mercadoria.plu in (Select PLU From #0200Plu p )
	Or	
		mercadoria.plu in (Select PLU From #0200NF_Item i)	
	--Or	
		--Exists(Select * From #0200NF_ItemCliente ci Where ci.Plu = mercadoria.Plu)			
		
		/*
	WHERE 
		EXISTS(SELECT * 
					FROM 
					NF_ITEM A 
					INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR
					WHERE
						A.FILIAL = @FILIAL
						AND DATA BETWEEN @DATAINICIO AND @DATAFIM
						AND MERCADORIA.PLU = A.PLU AND EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR ))
	Or 
		EXISTS(SELECT * 
					FROM SAIDA_ESTOQUE 
					WHERE FILIAL = @FILIAL
						AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial)
						AND DATA_MOVIMENTO BETWEEN @DATAINICIO AND @DATAFIM AND data_cancelamento IS NULL) --And Not PLU in(100100,99077))
						*/
	--left outer join mercadoria_loja on mercadoria.plu = mercadoria.loja.plu
	--OR
		--(IsNull(Saldo_Atual, 0) > 0 AND ISNULL(Preco_Custo, 0) > 0)
		
	ORDER BY CONVERT(FLOAT,MERCADORIA.PLU)





GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_A001]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_A001]
GO

/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_0001]    Script Date: 01/03/2017 16:51:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- STORED PROCEDURE

CREATE Procedure [dbo].[sp_EFD_PisCofins_A001]   
AS                            
BEGIN

	SELECT 
	'|A001|1|'

		RETURN                          
                      
END

GO



/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_0200]    Script Date: 01/03/2017 09:44:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0200]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0200]
GO

--sp_EFD_PisCofins_0200 'MATRIZ','20180801', '20180831'

Create procedure [dbo].[sp_EFD_PisCofins_0200]
	@Filial	AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
AS

SELECT Distinct Plu into #0200Plu
					FROM SAIDA_ESTOQUE with (index (IX_Saida_Estoque))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial and Controle_Filial_PDV.NGS = 0)
						AND DATA_MOVIMENTO BETWEEN @DataInicio AND @DataFim AND data_cancelamento IS NULL --And Not PLU in(100100,99077))
						
						
SELECT Distinct PLU into #0200NF_Item 
		FROM 
		NF_ITEM A with (index (IX_NF_Item_0200))
		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR AND A.CODIGO = B.CODIGO  
		INNER JOIN Natureza_operacao NatOp ON NatOp.Codigo_operacao=b.Codigo_operacao
WHERE
	A.FILIAL = @Filial
AND 
	DATA BETWEEN @DataInicio AND @DataFim
and a.codigo_operacao not in (6929,5929,5927,6927)
and ISNULL(b.nf_Canc,0)<>1
--and (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 ))
--AND 
--	EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR )
	
--SELECT Distinct PLU into #0200NF_ItemCliente 
--		FROM 
--		NF_ITEM A with (index (IX_NF_Item_0200))
--		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO --AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR 
--WHERE
--	A.FILIAL = @Filial
----AND
----	Not A.Codigo_Operacao in ('5102')	
--AND 
--	DATA BETWEEN @DataInicio AND @DataFim
--AND 
--	EXISTS(SELECT * FROM Cliente CI WHERE CI.Codigo_Cliente = B.CLIENTE_FORNECEDOR )


	SELECT
		REG = '0200',
		COD_ITEM = PLU,
		DESCR_ITEM = ISNULL(DESCRICAO,'PRODUTO'),
		COD_BARRA = ISNULL((SELECT TOP 1 EAN FROM Ean WHERE Ean.PLU = Mercadoria.PLU),''), 
		COD_ANT_ITEM = '',
		UNID_INV = CASE WHEN ISNULL(PESO_VARIAVEL, 'NÃO') = 'PESO' THEN 'KG' ELSE 'UN' END,
		TIPO_ITEM = '00',
		COD_NCM = CASE WHEN LEN(RTRIM(LTRIM(REPLACE(CF,'.','')))) = 8 THEN RTRIM(LTRIM(REPLACE(CF,'.',''))) ELSE '' END ,
		EX_IPI = '',
		COD_GEN = '',
		COD_LST = '',
		ALIQ_ICMS = (SELECT TOP 1 SAIDA_ICMS FROM Tributacao WHERE Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao)
		,CEST = 
		CASE WHEN (SELECT COUNT(*) FROM CEST WHERE CEST.CEST = MERCADORIA.CEST and cest.NCM = mercadoria.cf ) > 0 
		AND LEN(RTRIM(LTRIM(CONVERT(VARCHAR, ISNULL(Mercadoria.CEST, 0))))) = 7 
		THEN CONVERT(VARCHAR, Mercadoria.CEST)  ELSE '' END 
	FROM
		MERCADORIA with (index (PK_Mercadoria))
	Where
		mercadoria.plu in (Select PLU From #0200Plu p )
	Or	
		mercadoria.plu in (Select PLU From #0200NF_Item i)	
	--Or	
		--Exists(Select * From #0200NF_ItemCliente ci Where ci.Plu = mercadoria.Plu)			
		
		/*
	WHERE 
		EXISTS(SELECT * 
					FROM 
					NF_ITEM A 
					INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CODIGO = B.CODIGO AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR
					WHERE
						A.FILIAL = @FILIAL
						AND DATA BETWEEN @DATAINICIO AND @DATAFIM
						AND MERCADORIA.PLU = A.PLU AND EXISTS(SELECT * FROM FORNECEDOR F WHERE F.FORNECEDOR = B.CLIENTE_FORNECEDOR ))
	Or 
		EXISTS(SELECT * 
					FROM SAIDA_ESTOQUE 
					WHERE FILIAL = @FILIAL
						AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial)
						AND DATA_MOVIMENTO BETWEEN @DATAINICIO AND @DATAFIM AND data_cancelamento IS NULL) --And Not PLU in(100100,99077))
						*/
	--left outer join mercadoria_loja on mercadoria.plu = mercadoria.loja.plu
	--OR
		--(IsNull(Saldo_Atual, 0) > 0 AND ISNULL(Preco_Custo, 0) > 0)
		
	ORDER BY CONVERT(FLOAT,MERCADORIA.PLU)



go 
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.152.586', getdate();
GO