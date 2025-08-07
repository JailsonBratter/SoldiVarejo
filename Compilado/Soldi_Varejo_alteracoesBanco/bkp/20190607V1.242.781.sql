

--update Saida_estoque set  coo = substring(id_chave,35,6) 
--where coo = 0 and id_chave is not null


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_ICMSIPI_C860_G]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_ICMSIPI_C860_G]
GO

CREATE PROC [dbo].[sp_EFD_ICMSIPI_C860_G]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8)
As
BEGIN
--[sp_EFD_ICMSIPI_C860_G] 'MATRIZ','20190201', '20190204'
		select 
			REG = 'C860',
			NR_SAT= S.numero_serie
		
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and (CONVERT(DATE,S.Data_Movimento) between @DataInicio and @DataFim)
		  AND S.id_Chave IS NOT NULL	
		  AND S.numero_serie IS NOT NULL

		group by S.numero_serie
				
				
	
				
			
END
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_EFD_PisCofins_C860]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].SP_EFD_PisCofins_C860
GO
CREATE PROCEDURE SP_EFD_PisCofins_C860
(	@FILIAL VARCHAR(20),
	@DATAINI VARCHAR(8),
	@DATAFIM VARCHAR (8),
	@NR_SAT VARCHAR(20)
	)
as
--EXEC SP_EFD_PisCofins_C860 'MATRIZ', '20190201' , '20190228','509444'
SELECT S.NUMERO_SERIE ,S.COO,S.DATA_MOVIMENTO
INTO #SAIDA_SPED FROM Saida_estoque S 
WHERE s.filial = @FILIAL and S.Data_movimento BETWEEN @DATAINI AND @DATAFIM
AND S.id_Chave IS NOT NULL	

SELECT REG='C860'
	  ,COD_MOD = '59'
	  ,NR_SAT = S.numero_serie
	  ,DT_DOC = CONVERT(VARCHAR,S.Data_Movimento,103)
	  ,DOC_INI = MIN(S.COO)
	  ,DOC_FIM = MAX(S.COO)
FROM #SAIDA_SPED AS S
WHERE S.numero_serie = @NR_SAT
GROUP BY S.NUMERO_SERIE, S.Data_movimento
ORDER BY DATA_MOVIMENTO

GO 



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_EFD_PisCofins_C870]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].SP_EFD_PisCofins_C870
GO
CREATE PROCEDURE SP_EFD_PisCofins_C870
(	@FILIAL VARCHAR(20),
	@DATA VARCHAR(8),
	@NR_SAT VARCHAR(10)
	)
as
--EXEC SP_EFD_PisCofins_C870 'MATRIZ', '20190201' , '509444'
SELECT S.*
INTO #SAIDA_SPED870 FROM Saida_estoque S 
WHERE s.filial = @FILIAL and S.Data_movimento = @DATA
AND S.numero_serie=@NR_SAT  AND S.id_Chave IS NOT NULL	

SELECT REG='C870'
	  ,COD_ITEM = S.PLU
	  ,CFOP = CASE WHEN S.NRO_ECF = 4 THEN '5405' ELSE '5102' END
	  ,VL_ITEM= SUM((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0)))
	  ,VL_DESC =SUM(isnull(S.Desconto,0))
	  ,CST_PIS = M.cst_saida
	  ,VLR_BC_PIS = CASE WHEN M.cst_saida ='01' THEN SUM((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0))) ELSE 0 END
	  ,ALIQ_PIS =CASE WHEN M.cst_saida ='01' THEN M.Pis_Perc_Saida ELSE 0 END 
	  ,VL_PIS = SUM(CASE WHEN M.cst_saida ='01' THEN (((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0))) * M.Pis_Perc_Saida)/100 ELSE 0 END )
	  ,CST_COFINS =  M.cst_saida
	  ,VLR_BC_COFINS =  CASE WHEN M.cst_saida ='01' THEN SUM((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0))) ELSE 0 END
	  ,ALIQ_COFINS =CASE WHEN M.cst_saida ='01' THEN M.Cofins_Perc_Saida ELSE 0 END 
	  ,VL_COFINS = SUM(CASE WHEN M.cst_saida ='01' THEN (((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0)) )* M.Cofins_Perc_Saida)/100 ELSE 0 END )
	  ,COD_CTA = (SELECT TOP 1 COD_CONTA FROM Conta_Contabil WHERE FILIAL = S.FILIAL AND isnull(entrada,0) =0 )
	  ,NATUREZA_RECEITA = Case When (m.cst_saida = '04' And  Isnull(m.Codigo_Natureza_Receita,'') = '999') Then '202' Else ISNULL(m.Codigo_Natureza_Receita, '') End
FROM #SAIDA_SPED870 AS S
	 INNER JOIN MERCADORIA  AS M ON S.PLU = M.PLU
GROUP BY S.PLU
		,CASE WHEN S.NRO_ECF = 4 THEN '5405' ELSE '5102' END
		,M.cst_saida
		,M.Pis_Perc_Saida
		,M.Cofins_Perc_Saida
		,S.FILIAL
		,m.Codigo_Natureza_Receita


GO 

if object_id('EFD_M400') is null
begin
CREATE TABLE [dbo].[EFD_M400](
	[REG] [varchar](4) NULL,
	[CST] [varchar](2) NULL,
	[VL_TOT_REC] [numeric](14, 2) NULL
) ON [PRIMARY]
end 
GO

if object_id('EFD_M410') is null
begin
CREATE TABLE [dbo].[EFD_M410](
	[REG] [varchar](4) NULL,
	[NAT_REC] [varchar](3) NULL,
	[VL_TOT_REC] [numeric](14, 2) NULL,
	[CST] [varchar](2) NULL,
	[CONTA_CONTABIL] [varchar](50) NULL
) ON [PRIMARY]

end 

go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0200]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0200]
GO
CREATE       procedure [dbo].[sp_EFD_PisCofins_0200]
	@Filial	AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8),
	@Tipo			AS Integer
AS

SELECT Distinct Plu into #0200Plu
					FROM SAIDA_ESTOQUE with (index (IX_Saida_Estoque))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND DATA_MOVIMENTO BETWEEN @DataInicio AND @DataFim AND data_cancelamento IS NULL 
						AND (
						(@TIPO =1 AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial and Controle_Filial_PDV.NGS = 0 AND Controle_Filial_PDV.SAT <> 0 ))
						OR @TIPO=2
						)
						
						
SELECT Distinct PLU into #0200NF_Item 
		FROM 
		NF_ITEM A with (index (IX_NF_Item_0200))
		INNER JOIN NF B ON A.FILIAL = B.FILIAL AND A.CLIENTE_FORNECEDOR = B.CLIENTE_FORNECEDOR AND A.CODIGO = B.CODIGO  
		INNER JOIN Natureza_operacao NatOp ON NatOp.Codigo_operacao=b.Codigo_operacao
WHERE
	A.FILIAL = @Filial
AND 
	DATA BETWEEN @DataInicio AND @DataFim
	AND (
				(@TIPO =1 AND  b.Tipo_NF=2 
					and (CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(B.Producao_NFe, 0) = 1 AND B.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 
					and a.codigo_operacao not in (6929,5929,5927,6927)
					and (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 ))
					)
				OR 
				@TIPO = 0 
			)

and ISNULL(b.nf_Canc,0)<>1

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
		ALIQ_ICMS = (SELECT TOP 1 SAIDA_ICMS FROM Tributacao WHERE Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao),
		CEST = 
		
		CASE WHEN (SELECT COUNT(*) FROM CEST WHERE CEST.CEST = MERCADORIA.CEST and cest.NCM = mercadoria.cf ) > 0 
		AND LEN(RTRIM(LTRIM(CONVERT(VARCHAR, ISNULL(Mercadoria.CEST, 0))))) = 7 
		THEN CONVERT(VARCHAR, Mercadoria.CEST)  ELSE '' END --CASE WHEN Isnull(Cest,'') = Isnull((Select Max(CEST) From CEST where cest.CEST = Mercadoria.CEST),0) Then Isnull(Cest,'') else '' end
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

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_EFD_PisCofins_0400]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_EFD_PisCofins_0400]
GO
CREATE         PROC [dbo].[sp_EFD_PisCofins_0400]
	@FILIAL VARCHAR(20),
	@DATAINI VARCHAR(8),
	@DATAFIM VARCHAR (8),
	@Tipo  AS Integer
as

--SP_EFD_PISCOFINS_0400 'MATRIZ', '20141001', '20141031'
SELECT DISTINCT '|0400|' + convert(varchar (4),CODIGO_OPERACAO) + '|' + rtrim(ltrim(descricao)) + '|'  
FROM NATUREZA_OPERACAO
WHERE CODIGO_OPERACAO IN (

SELECT DISTINCT 
	a.CODIGO_OPERACAO
	FROM
		NF a INNER JOIN NF_Item b ON 
					a.FILIAL = b.FILIAL 
					AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR 
					AND a.Codigo = b.Codigo		
		INNER JOIN Mercadoria M ON b.PLU = m.PLU
		LEFT OUTER JOIN Tributacao c ON c.Codigo_Tributacao = b.Codigo_Tributacao AND c.Filial = b.Filial
		

	WHERE
		a.Filial = @FILIAL
		AND a.Codigo_Operacao not in (5929, 6929)
		AND a.Data BETWEEN @DATAINI AND @DATAFIM
		AND (
				(@TIPO =1 AND  b.Tipo_NF=2 
					and (CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(a.Producao_NFe, 0) = 1 AND a.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 
					)
				OR 
				@TIPO = 0 
			)

		)
		
go 


if object_id('Prato_Dia_Produtos') is null
begin
	CREATE TABLE [dbo].[Prato_Dia_Produtos](
		[PLU] [varchar](17) NULL
	) ON [PRIMARY]
end 

go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Ativa_CE'))
begin
	alter table Mercadoria alter column Ativa_CE tinyint
	alter table Mercadoria alter column Departamento_CE varchar(9)
	alter table Mercadoria alter column Departamento_Aux varchar(9)
end
else
begin
	alter table Mercadoria add Ativa_CE tinyint
	alter table Mercadoria add Departamento_CE varchar(9)
	alter table Mercadoria add Departamento_Aux varchar(9)
end
 go 
 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Departamento') 
            AND  UPPER(COLUMN_NAME) = UPPER('cardapio'))
begin
	alter table Departamento alter column cardapio tinyint
end
else
begin
	alter table Departamento add cardapio tinyint
end 
go 



go 
if object_id('Soldi_Gusto_CE_Departamento') is null
begin
	CREATE TABLE [dbo].[Soldi_Gusto_CE_Departamento](
		[Grupo_Grafico] [varchar](9) NULL,
		[Descricao] [varchar](30) NULL,
		[Codigo_Departamento] [varchar](9) NULL,
		[Dep_Ativa_CE] [tinyint] NULL
	) ON [PRIMARY]
end 
else
begin 
	IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
				WHERE UPPER(TABLE_NAME) = UPPER('Soldi_Gusto_CE_Departamento') 
				AND  UPPER(COLUMN_NAME) = UPPER('Dep_Ativa_CE'))
	begin
		alter table Soldi_Gusto_CE_Departamento alter column Dep_Ativa_CE tinyint
	end
	else
	begin
		alter table Soldi_Gusto_CE_Departamento add Dep_Ativa_CE tinyint
	end 
end
go 
 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_SOLDI_GUSTO_CE]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Cons_SOLDI_GUSTO_CE]
GO
CREATE PROCEDURE [dbo].[sp_Cons_SOLDI_GUSTO_CE]
	
	AS
begin
	update Mercadoria set Departamento_aux = Codigo_departamento where Ativa_CE = 2
	update Mercadoria set Codigo_departamento = Departamento_ce where Ativa_CE = 2

	BEGIN
		DELETE FROM Prato_Dia_Produtos

		DECLARE @STRING AS NVARCHAR(500)
		DECLARE @NREG	AS INTEGER

		SET @STRING = 'INSERT INTO Prato_Dia_Produtos SELECT PLU FROM MERCADORIA WHERE ISNULL(PRATO_DIA, 0) = 1 AND ' + 
			 CASE 
				WHEN  DATEPART(DW,GETDATE()) = 1 THEN 'PRATO_DIA_1'
				WHEN  DATEPART(DW,GETDATE()) = 2 THEN 'PRATO_DIA_2'
				WHEN  DATEPART(DW,GETDATE()) = 3 THEN 'prato_dia_3'
				WHEN  DATEPART(DW,GETDATE()) = 4 THEN 'PRATO_DIA_4'
				WHEN  DATEPART(DW,GETDATE()) = 5 THEN 'PRATO_DIA_5'
				WHEN  DATEPART(DW,GETDATE()) = 6 THEN 'PRATO_DIA_6'
				ELSE 'PRATO_DIA_7' END + ' = 1'
		EXECUTE (@STRING)
		SELECT @NREG = COUNT(*) FROM Prato_Dia_Produtos
		DELETE FROM Soldi_Gusto_CE_Departamento WHERE Descricao = '.PRATO DO DIA'
		IF @NREG > 0
			INSERT INTO Soldi_Gusto_CE_Departamento SELECT '987654321', '.PRATO DO DIA', '987654321', 1

	END

	BEGIN
		SELECT SEQ = 1, ELEMENTO = '<CADASTROS>'
		UNION 
		SELECT 2, '<COMANDAS>'
		UNION 
		SELECT 3, '<ROW IDCOMANDA = ' + CHAR(34) + RTRIM(LTRIM(CONVERT(VARCHAR,COMANDA))) + CHAR(34) + '/>' FROM COMANDA_CONTROLE WHERE STATUS IN('00', '02')
		UNION 
		SELECT 4, '</COMANDAS>'
		UNION 
		SELECT 5, '<DEPARTAMENTO>'
		UNION 
		--SELECT 6, '<ROW IDDEPARTAMENTO = ' + CHAR(34) +  REPLICATE('0', 3- LEN(RTRIM(LTRIM(CONVERT(VARCHAR, CODIGO_GRUPO))))) + RTRIM(LTRIM(CONVERT(VARCHAR, CODIGO_GRUPO))) + CHAR(34) + ' DESCRICAO=' + CHAR(34) + RTRIM(LTRIM(DESCRICAO_GRUPO)) + CHAR(34) + '/>' FROM GRUPO WHERE Codigo_Grupo IN('1','2') --and SUBSTRING(CODIGO_DEPARTAMENTO,1,3) IN('002')
		SELECT distinct 6, '<ROW IDDEPARTAMENTO = ' + CHAR(34) + grupo_grafico + CHAR(34) + ' DESCRICAO=' + CHAR(34) + RTRIM(LTRIM(descricao)) + CHAR(34) + '/>' FROM Soldi_gusto_CE_Departamento WHERE grupo_grafico <> '' and Dep_Ativa_CE = 1--d SUBSTRING(CODIGO_DEPARTAMENTO,1,3) IN('002')
		UNION 		
		SELECT 7, '</DEPARTAMENTO>'
		UNION 
		SELECT 8, '<PRODUTOS>'
		UNION 
		SELECT 9, '<ROW IDPRODUTO=' + CHAR(34) + RTRIM(LTRIM(M.PLU)) + CHAR(34) + ' DESCRICAO='+CHAR(34) + REPLACE(m.Descricao_resumida, CHAR(34), '') + CHAR(34) + ' PRECO=' + CHAR(34) + CONVERT(VARCHAR, M.PRECO) + CHAR(34) + ' IDDEPARTAMENTO=' + CHAR(34) + '987654321' + CHAR(34) + '/>' FROM  Mercadoria m  Where m.Inativo = 0  and m.Ativa_CE <> 0 and m.PLU IN(SELECT PLU FROM Prato_Dia_Produtos)
		UNION 
		SELECT 9, '<ROW IDPRODUTO=' + CHAR(34) + RTRIM(LTRIM(M.PLU)) + CHAR(34) + ' DESCRICAO='+CHAR(34) + REPLACE(m.Descricao_resumida, CHAR(34), '') + CHAR(34) + ' PRECO=' + CHAR(34) + CONVERT(VARCHAR, M.PRECO) + CHAR(34) + ' IDDEPARTAMENTO=' + CHAR(34) + c.Grupo_Grafico + CHAR(34) + '/>' FROM  Mercadoria m inner join Soldi_gusto_CE_Departamento c on m.Codigo_departamento = c.codigo_departamento Where c.grupo_grafico <> '' and m.Inativo = 0  and m.Ativa_CE <> 0 and m.PLU NOT IN(SELECT PLU FROM Prato_Dia_Produtos)
		UNION 
		SELECT 10, '</PRODUTOS>'
		UNION 
		SELECT 11, '<OBSERVACOES>'
		UNION 
		SELECT 12, '<ROW IDPRODUTO=' + CHAR(34) + RTRIM(LTRIM(PLU)) + CHAR(34) + ' OBS='+CHAR(34) + OBS + CHAR(34) + ' IDAD=' + CHAR(34) + PLU_ITEM_ADC + CHAR(34) + ' OBRIGATORIA=' + CHAR(34) + CASE WHEN ISNULL(OBRIGATORIO,0) = 0 THEN 'NAO' ELSE 'SIM' END + CHAR(34) + '/>' FROM MERCADORIA_OBS 
		UNION 
		SELECT 13, '</OBSERVACOES>'
		UNION 
		SELECT 14, '<OPERADORES>'
		UNION 
		SELECT 15, '<ROW IDOPERADOR=' + CHAR(34) + RTRIM(LTRIM(NOME)) + CHAR(34) + ' NIVEL='+CHAR(34) + CONVERT(VARCHAR(1),ISNULL(CANCELA_ITEM,0)) + CHAR(34) + ' SENHA=' + CHAR(34) + RTRIM(LTRIM(SENHA)) + CHAR(34) + '/>' FROM FUNCIONARIO WHERE ISNULL(USA_PALM,0) = 1 
		UNION 
		SELECT 16, '</OPERADORES>'
		UNION 
		SELECT 20, '<MOTIVOSCANCELAMENTO>'
        UNION
        SELECT 21, '<ROW IDMOTIVO=' + CHAR(34) +'1' + CHAR(34) + ' TEXTO=' + CHAR(34) +'ERRO LCTO' + CHAR(34) + '/>'
        UNION
        SELECT 21, '<ROW IDMOTIVO=' + CHAR(34) +'2' + CHAR(34) + ' TEXTO=' + CHAR(34) +'ERRO SISTEMA' + CHAR(34) + '/>'
        UNION
        SELECT 21, '<ROW IDMOTIVO=' + CHAR(34) +'3' + CHAR(34) + ' TEXTO=' + CHAR(34) +'GERENTE' + CHAR(34) + '/>'
        UNION
        SELECT 22, '</MOTIVOSCANCELAMENTO>'
        UNION
        SELECT 23, '</CADASTROS>'
		ORDER BY 1
	END

	update Mercadoria set Codigo_departamento = Departamento_Aux where Ativa_CE = 2

end

go 

 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_NFe_Det]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_NFe_Det]
GO
CREATE   procedure [dbo].[sp_NFe_Det]
	@id varchar(47)
	as

	select ID,
		ide_nNF,
		emit_cnpj,
		ide_dEmi,
		det_prod_cProd,
		det_prod_cEAN,
		det_prod_xProd,
		det_prod_NCM,
		det_prod_EXTIPI,
		det_prod_genero,
		det_prod_CFOP,
		det_prod_uCOM,
		det_prod_qCOM,
		det_prod_vUnCOM,
		det_prod_vProd,
		det_prod_cEANTrib,
		det_prod_uTrib,
		det_prod_qTrib,
		det_prod_vUnTrib,
		det_prod_vFrete,
		det_prod_vSeg,
		det_prod_vDesc,
		det_prod_DI,
		det_prod_DetEspecifico,
		det_icms_orig,
		det_icms_CST,
		det_icms_modBC,
		det_icms_pRedBC,
		det_icms_vBC,
		det_icms_pICMS,
		det_icms_vICMS,
		det_icms_modBCST,
		det_icms_pMVAST,
		det_icms_pRedBCST,
		det_icms_vBCST,
		det_icms_pICMSST,
		det_icms_vICMSST,
		det_ipi_clEnq,
		det_ipi_CNPJProd,
		det_ipi_cSelo,
		det_ipi_qSelo,
		det_ipi_cEnq,
		det_ipi_CST,
		det_ipi_vBC,
		det_ipi_pIPI,
		det_ipi_vIPI,
		det_ipi_qUnid,
		det_ipi_vUnid,
		det_II_vBC,
		det_II_vDespAdu,
		det_II_vII,
		det_II_vIOF,
		det_pis_CST,
		det_pis_vBC,
		det_pis_pPIS,
		det_pis_vPIS,
		det_pis_qBCProd,
		det_pis_vAliqProd,
		det_pisst_vBC,
		det_pisst_pPIS,
		det_pisst_vPIS,
		det_pisst_qBCProd,
		det_pisst_vAliqProd,
		det_cofins_CST,
		det_cofins_vBC,
		det_cofins_pCOFINS,
		det_cofins_vCOFINS,
		det_cofins_qBCProd,
		det_cofins_vAliqProd,
		det_cofinsst_vBC,
		det_cofinsst_pCOFINS,
		det_cofinsst_vCOFINS,
		det_cofinsst_qBCProd,
		det_cofinsst_vAliqProd,
		det_issqn2g_vBC,
		det_issqn2g_vAliq,
		det_issqn2g_vISSQN,
		det_issqn2g_cMunFG,
		det_issqn2g_cListServ,
		det_issqn2g_cSitTrib,
		det_icms_CSOSN,
		det_icms_pCredSN,
		det_icms_vCredICMSSN,
		det_nItem,
		det_prod_vOutro,
		det_prod_indTot,
		det_prod_CEST,
		det_ICMS_vBCFCP,
		det_ICMS_pFCP,
		det_ICMS_vFCP,
		det_ICMS_vBCFCPST,
		det_ICMS_pFCPST,
		det_ICMS_vFCPST = convert(float,det_ICMS_vFCPST)
	from 
		nfe_xml 
	where 
		id=@id 
		and det_prod_cprod is not null
	Order By
		det_nItem


go 

	insert into Versoes_Atualizadas select 'Versão:1.242.781', getdate();
GO
