IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('pedido') 
            AND  UPPER(COLUMN_NAME) = UPPER('despesas'))
begin
	alter table pedido alter column despesas numeric(18,2)
end
else
begin
	alter table pedido add despesas numeric(18,2)
end 
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('TotalICMS'))
begin
	alter table saida_estoque alter column TotalICMS numeric(18,2)
	alter table saida_estoque alter column TotalPis numeric(18,2)
	alter table saida_estoque alter column TotalCofins numeric(18,2)
end
else
begin
	alter table saida_estoque add TotalICMS numeric(18,2)
	alter table saida_estoque add TotalPis numeric(18,2)
	alter table saida_estoque add TotalCofins numeric(18,2)
end 
go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_movimento'))
begin
	alter table saida_estoque alter column id_movimento bigint
end
else
begin
	alter table saida_estoque add id_movimento bigint
end 
go 



go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('sped_blocos') 
            AND  UPPER(COLUMN_NAME) = UPPER('bloco_grupo'))
begin
	alter table sped_blocos alter column bloco_grupo tinyint
end
else
begin
	alter table sped_blocos add bloco_grupo tinyint
end 
go 





IF OBJECT_ID('[sp_EFD_PisCofins_C170]', 'P') IS NOT NULL
begin 
      drop procedure [sp_EFD_PisCofins_C170]
end 

go

	
	-- STORED PROCEDURE
-- sp_EFD_PisCofins_C170 'MATRIZ', '20180801', '20180831', 'BIMBO DO BRASIL', 4770
CREATE  Procedure [dbo].[sp_EFD_PisCofins_C170]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8),
	@Fornecedor		As Varchar(20),
	@NroNota		As Varchar(10),
	@Tipo			AS Integer
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
		AND (
				(@TIPO =1 AND  (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 )))
				OR 
				@TIPO = 0 
			)
	ORDER BY 1,2

	RETURN                          

END

GO 


IF OBJECT_ID('[sp_EFD_PisCofins_B001]', 'P') IS NOT NULL
begin 
      drop procedure [sp_EFD_PisCofins_B001]
end 

go


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


/****** Object:  StoredProcedure [dbo].[sp_EFD_PisCofins_1010]    Script Date: 07/03/2019 12:09:44 ******/
IF OBJECT_ID('[sp_EFD_PisCofins_1010]', 'P') IS NOT NULL
begin 
      drop procedure [sp_EFD_PisCofins_1010]
end 

go

create procedure [dbo].[sp_EFD_PisCofins_1010]

AS
	SELECT '|1010|N|N|N|N|N|N|S|N|N|'



go 



IF OBJECT_ID('[sp_EFD_ICMSIPI_C800_G]', 'P') IS NOT NULL
begin 
      drop procedure [sp_EFD_ICMSIPI_C800_G]
end 

go

--[sp_EFD_ICMSIPI_C800_G] 'MATRIZ','20180601', '20180630'
create  PROC [dbo].[sp_EFD_ICMSIPI_C800_G]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8) 
As
BEGIN
		select 
			REG = 'C800',
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




IF OBJECT_ID('sp_EFD_ICMSIPI_C800', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800]
end 

go

--[sp_EFD_ICMSIPI_C800] 'MATRIZ','20180601', '20180630'

CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
	
As
BEGIN
	

		select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '00',
			NUM_CFE = S.coo,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/',''),
			VL_CFE = (SUM(S.vlr)-SUM(CONVERT(NUMERIC(18,2),S.Desconto)))+SUM(S.Acrescimo),
			VL_PIS = SUM(S.TotalPis),
			VL_COFINS = SUM(S.TotalCofins),
			CNPJ_CPF = REPLACE(REPLACE(REPLACE(c.CNPJ,'.',''),'-',''),'/',''),
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =SUM(S.Desconto),
			VL_MERC = SUM(S.VLR),
			VL_OUT_DA =SUM(s.acrescimo),
			VL_ICMS = SUM(S.TotalICMS),
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is null  
		  and (CONVERT(DATE,S.Data_Movimento) Between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				,c.CNPJ
				,S.numero_serie
				,id_Chave
	UNION ALL 
			select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '02',
			NUM_CFE = S.coo,
			DT_DOC = NULL,
			VL_CFE = NULL,
			VL_PIS = NULL,
			VL_COFINS = NULL,
			CNPJ_CPF = NULL,
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =NULL,
			VL_MERC = NULL,
			VL_OUT_DA =NULL,
			VL_ICMS = NULL,
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is NOT null  
		  and (CONVERT(DATE,S.Data_Movimento) Between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,CONVERT(DATE,S.Data_Movimento)
				,c.CNPJ
				,S.numero_serie
				,id_Chave

			
END


go 




IF OBJECT_ID('sp_EFD_ICMSIPI_C850', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C850]
end 

go

--[sp_EFD_ICMSIPI_C800] 'MATRIZ','20180601', '20180630'
--[sp_EFD_ICMSIPI_C850] 'MATRIZ','35180610564952000191590005069470014994802070'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C850]
	@Filial		AS Varchar(20),
	@CHV_CFE as varchar(44)
As
BEGIN
	

	BEGIN
		SELECT
		       	REG = 'C850',
		       	CST_ICMS = '0' + RTRIM(LTRIM(CASE WHEN b.NRO_ECF = 4 THEN '60' 
							  WHEN b.NRO_ECF = 5 THEN '40' 
							  WHEN b.NRO_ECF = 6 THEN '41'
							ELSE '00' END )),
		       	CFOP = CASE WHEN b.NRO_ECF = 4 THEN '5403' ELSE '5102' END,
			--** Aliuota ICMS	
			ALIQ_ICMS =  CASE WHEN b.NRO_ECF = 4 THEN 0 ELSE b.Aliquota_ICMS END,
			VL_OPR = b.VLR,
			VL_BC_ICMS = CASE WHEN b.NRO_ECF = 4 OR b.NRO_ECF = 5 THEN 0 ELSE b.VLR END
		INTO
			#EFD_ICMSIPI_C850
		FROM
		
		       Mercadoria a INNER JOIN Saida_Estoque b ON a.PLU = b.PLU
		
		       INNER JOIN Tributacao t ON t.Codigo_Tributacao = a.Codigo_Tributacao AND t.Filial = b.Filial
		WHERE
		       b.Filial = @Filial
		
		       AND b.Data_Cancelamento IS NULL
			   AND REPLACE(b.id_Chave,'Cfe','') = @CHV_CFE
		      
	END	
	--** 
	BEGIN
		SELECT 
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS,
			VL_OPR = SUM(VL_OPR),
			VL_BC_ICMS = SUM(ISNULL(VL_BC_ICMS,0)),
			VL_ICMS = SUM(ISNULL(VL_OPR * (ALIQ_ICMS/100),0)),
			COD_OBS = ' '
		
		FROM
			#EFD_ICMSIPI_C850
		GROUP BY
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS
	END
			
END


go 


IF OBJECT_ID('sp_EFD_ICMSIPI_C800_GDT', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800_GDT]
end 

go

--[sp_EFD_ICMSIPI_C800_GDT] 'MATRIZ','20180601', '20180630'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800_GDT]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8) 
As
BEGIN
		select 
			REG = 'C800',
			NR_SAT= S.numero_serie,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')			
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and (CONVERT(DATE,S.Data_Movimento) between @DataInicio and @DataFim)
		 AND S.id_Chave IS NOT NULL	
		 AND S.numero_serie IS NOT NULL
		group by S.numero_serie
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				
			
END

GO 



IF OBJECT_ID('sp_EFD_ICMSIPI_C800_G', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800_G]
end 

go

--[sp_EFD_ICMSIPI_C800_G] 'MATRIZ','20180601', '20180630'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800_G]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8) 
As
BEGIN
		select 
			REG = 'C800',
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

IF OBJECT_ID('sp_EFD_ICMSIPI_H001', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_H001]
end 

go
CREATE Procedure [dbo].[sp_EFD_ICMSIPI_H001]   
AS                            
BEGIN

	SELECT 
	--** Campo [REG] Texto fixo contendo "0001" C 004 - O
	'|H001' + 
	--** Campo [IND_MOV] Indicador de movimento: N 001 - O
		--** 0 - Bloco com dados informados;
		--** 1 - Bloco sem dados informados.
	'|0|'

		RETURN                          
                      
END

go 


IF OBJECT_ID('[sp_EFD_ICMSIPI_H005]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_H005]
end 

go
CREATE procedure [dbo].[sp_EFD_ICMSIPI_H005]
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8)

--EFD_ICMSIPI_H005 'MATRIZ','31032013'
AS
	SELECT 
		REG = 'H005',
		COD_ITEM = @DATAINICIO,
		VL_INV = SUM(CONVERT(DECIMAL(12,2),Saldo * Preco_Custo)),
		MOT_INV = '01'
	FROM 
		MERCADORIA_ESTOQUE_MES
	WHERE
		Data = @DATAINICIO
	AND	
		ISNULL(Saldo,0) > 0 AND ISNULL(Preco_Custo, 0) > 0
	
go 



IF OBJECT_ID('[sp_EFD_ICMSIPI_H010]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_H010]
end 

go
CREATE PROCEDURE [dbo].[sp_EFD_ICMSIPI_H010] 
	@FILIAL VARCHAR(20),
	@DATA VARCHAR(8)
AS
SELECT 
	REG = 'H010',
	COD_ITEM = MERCADORIA_ESTOQUE_MES.PLU,
	UNID = CASE WHEN ISNULL(MERCADORIA.PESO_VARIAVEL, 'NÃO') = 'PESO' THEN 'KG' ELSE 'UN' END,
	QTDE = CONVERT(DECIMAL(14,3), MERCADORIA_ESTOQUE_MES.SALDO),
	VL_UNIT = MERCADORIA_ESTOQUE_MES.PRECO_CUSTO,
	VL_ITEM = CONVERT(DECIMAL(12,2), MERCADORIA_ESTOQUE_MES.SALDO * MERCADORIA_ESTOQUE_MES.PRECO_CUSTO),
	IND_PROP = '0',
	COD_PART = '',
	TXT_COMPL = '',
	COD_CTA = '100100100'
FROM 
	MERCADORIA_ESTOQUE_MES INNER JOIN MERCADORIA ON MERCADORIA_ESTOQUE_MES.PLU = MERCADORIA.PLU
WHERE 
	MERCADORIA_ESTOQUE_MES.FILIAL = @FILIAL
AND
	MERCADORIA_ESTOQUE_MES.DATA = @DATA
AND 
	ISNULL(MERCADORIA_ESTOQUE_MES.SALDO, 0) > 0 AND ISNULL(MERCADORIA_ESTOQUE_MES.PRECO_CUSTO, 0) > 0

go 


IF OBJECT_ID('[sp_EFD_PisCofins_C410]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_PisCofins_C410]
end 

go
CREATE    PROCEDURE [dbo].[sp_EFD_PisCofins_C410]
	@Filial	As Varchar(20),
	@Caixa	As Integer,
	@Data	As Varchar(8),
	@COOINI As Integer,
	@COOFIM As Integer
AS

BEGIN
	SELECT
		REG = 'C410',
		VL_PIS  = SUM(CASE WHEN ISNULL(a.Incide_PIS,1) = 1 OR a.Incide_PIS = 0 THEN CONVERT(NUMERIC(12,2),b.VLR * 0.0165) ELSE 0 END ),
		VL_COFINS  = SUM(CASE WHEN ISNULL(a.Incide_PIS,1) = 1 OR a.Incide_PIS = 0 THEN CONVERT(NUMERIC(12,2),b.VLR * 0.076) ELSE 0 END )
	FROM
		Mercadoria a INNER JOIN Saida_Estoque b with (index = ix_saida_estoque) ON a.PLU = b.PLU
	WHERE 
		b.Filial = @Filial
		AND REPLACE(CONVERT(VARCHAR,b.Data_Movimento,102),'.','') = @Data
		AND a.PLU = b.PLU
		AND b.Data_Cancelamento IS NULL
		AND b.Caixa_Saida = @Caixa
		AND CONVERT(INT,b.Documento) BETWEEN @COOINI AND @COOFIM
END


go 

IF OBJECT_ID('[sp_EFD_PisCofins_C460]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_PisCofins_C460]
end 

go
CREATE   PROCEDURE [dbo].[sp_EFD_PisCofins_C460]
	@Filial	As Varchar(20),
	@Caixa	As Integer,
	@Data	As Varchar(8),
	@COOINI	As Integer,
	@COOFIM	As Integer
AS

BEGIN
	SELECT
		REG = 'C460',
		COD_MOD = '2D',
		COD_SIT = '00',
		NUM_DOC = b.Documento,
		DT_DOC = b.Data_Movimento,
		VL_DOC = SUM(VLR-Isnull(Desconto,0)+ISNULL(Acrescimo,0)),
		VL_PIS  = SUM(CASE WHEN ISNULL(a.Incide_PIS,1) = 1 OR a.Incide_PIS = 0 THEN CONVERT(NUMERIC(12,2),b.VLR-ISNULL(Desconto,0)+Isnull(Acrescimo,0) * 0.0165) ELSE 0 END ),
		VL_COFINS  = SUM(CASE WHEN ISNULL(a.Incide_PIS,1) = 1 OR a.Incide_PIS = 0 THEN CONVERT(NUMERIC(12,2),b.VLR-ISNULL(Desconto,0)+Isnull(Acrescimo,0) * 0.076) ELSE 0 END ),
		CPF_CNPJ = '',
		NOM_ADQ = ''
	FROM
		Mercadoria a INNER JOIN Saida_Estoque b with(index = ix_saida_estoque) ON a.PLU = b.PLU
	WHERE 
		b.Filial = @Filial
		--AND REPLACE(CONVERT(VARCHAR,b.Data_Movimento,102),'.','') = @Data
		AND b.Data_Movimento = @Data
		AND a.PLU = b.PLU
		AND b.Data_Cancelamento IS NULL
		AND b.Caixa_Saida = @Caixa
		AND Convert(Int,b.Documento) BETWEEN @COOINI AND @COOFIM
		AND b.Vlr > 0

	GROUP BY
		b.Documento,
		b.Data_Movimento,
		b.Caixa_Saida
END

go 



IF OBJECT_ID('[sp_EFD_PisCofins_0005]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_PisCofins_0005]
end 

go
CREATE  Procedure [dbo].[sp_EFD_PisCofins_0005] 
	@Filial Varchar(20)

AS              

BEGIN

	DECLARE  @errno   INT,                            
			 @errmsg  VARCHAR(255)                            


	SELECT 
	--** Campo [REG] Texto fixo contendo "0005" C 004 - O
	REG = '0005' , 
	--** Nome Fantasia
	COD_EST = FILIAL,
	--** CEP
	CEP = REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(CEP,'')), '.', ''), '/', ''), '-', ''),
	--** LOGRADOURO E ENDEREÇO DO IMOVEL
	[END] = RTRIM(LTRIM(ISNULL(ENDERECO,''))),
	NUM = RTRIM(LTRIM(ISNULL(ENDERECO_NRO,''))),
	COMPL = '',
	BAIRRO = RTRIM(LTRIM(ISNULL(BAIRRO,''))),
	FONE = '11' + REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(TELEFONE,'')), '.', ''), '/', ''), '-', ''),
	FAX = '11'+REPLACE(REPLACE(REPLACE(RTRIM(ISNULL(TELEFONE,'')), '.', ''), '/', ''), '-', ''),
	EMAIL = ''

	 FROM 
		FILIAL
	 WHERE 

		fILIAL in (@FILIAL)

	RETURN                          

	error:                            
		--RAISERROR @errno @errmsg                            
		RETURN                          
END


go 
IF OBJECT_ID('[sp_EFD_PisCofins_M400]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_PisCofins_M400]
end 

go
CREATE     PROC [dbo].[sp_EFD_PisCofins_M400]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
	
As
	BEGIN
	BEGIN
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='EFD_M400')
			BEGIN
				DROP TABLE EFD_M400
	
				Create TABLE EFD_M400 (REG Varchar(4), CST Varchar(2), VL_TOT_REC Numeric(14,2))
			END
		ELSE
			Create TABLE EFD_M400 (REG Varchar(4), CST Varchar(2), VL_TOT_REC Numeric(14,2))
	END

Insert Into EFD_M400
		select 
			REG = 'M400',
			CST = a.CST_Saida,
			VL_TOT_REC=sum(isnull(CONVERT(NUMERIC(14,2),b.vlr-ISNULL(Desconto,0)+Isnull(Acrescimo,0)),0))
		FROM Mercadoria a, 
		Saida_estoque b with (INDEX=IX_SAIDA_ESTOQUE)  --and a.filial = b.filial
		WHERE 
			b.filial = @filial and
			b.Data_Movimento Between @DataInicio and @DataFim And
			a.plu = b.plu and
			a.CST_Saida NOT IN ('01', '02') and
			b.data_cancelamento is null and
			exists(Select * from Controle_Filial_PDV 
						WHERE b.Caixa_Saida = Controle_Filial_PDV.Caixa And b.Filial = Controle_Filial_PDV.Filial) and
			exists(Select * from Fiscal 
						WHERE b.Caixa_Saida = Fiscal.Caixa 
						And b.Documento between fiscal.Num_seq_Primeiro_coo and cupom
						AND Convert(varchar,b.Data_Movimento,102) = Convert(Varchar,Fiscal.Data,102))
			--Replace(convert(varchar,b.data_movimento,102),'.','') between @DataInicio and @DataFim
		group by 
			a.CST_Saida
			
UNION ALL -------------------------------------------------- Unindo As Tableas ----------------------------------------------------		
		select 
			REG = 'M400',
			CST = a.CST_Saida,
			VL_TOT_REC=sum(isnull(CONVERT(NUMERIC(14,2), b.total),0))
		FROM 
			NF n 
			INNER JOIN NF_Item b on n.filial = b.filial and n.cliente_fornecedor = b.cliente_fornecedor and n.codigo = b.codigo
			INNER JOIN Mercadoria a on a.plu = b.plu
		WHERE 
			n.filial = @filial
			AND n.data BetWeen @DataInicio and @DataFim
			AND n.codigo_operacao not in (5929, 6929) AND
			a.CST_Saida NOT IN ('01', '02') and
			b.Tipo_Nf = 1 and
			isnull(n.nf_canc,0) <> 1
		group by 
			a.CST_Saida

Select REG, CST, sum(VL_TOT_REC) AS VL_TOT_REC, COD_CTA = (SELECT TOP 1 COD_CONTA FROM Conta_Contabil WHERE isnull(ENTRADA,0) =0), DESC_COMPL =''
	From EFD_M400
Group by 
	REG, CST

		RETURN
	END


go 


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tributacao') 
            AND  UPPER(COLUMN_NAME) = UPPER('CFOP_Entrada'))
begin
	alter table tributacao alter column CFOP_Entrada decimal(4,0)
end
else
begin
	alter table tributacao add CFOP_Entrada decimal(4,0)
end 
go 

update tributacao set CFOP_Entrada = case when cfop = 405  then 403 else cfop end 
GO 





IF OBJECT_ID('[sp_Movimento_Venda]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Movimento_Venda]
end 

go


CREATE  Procedure [dbo].[sp_Movimento_Venda]

                @Filial				As Varchar(20),
                @DataDe				As Varchar(8),
                @DataAte			As Varchar(8),
                @finalizadora		As varchar(30),
                @plu				As varchar(17),
                @cupom				As varchar(20),
                @pdv				as varchar(10),                
                @horaInicio			as varchar(5),				
				@horafim			as varchar(5),
				@cancelados			as varchar(15),
				@comanda			as varchar(20),
				@ext_sat			as varchar(6)

AS

begin 
--exec     sp_Movimento_Venda   @Filial='MATRIZ', @DataDe = '20180602',  @dataate = '20180602',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = 'TODOS',  @ext_sat = '' 

Select Data_movimento, S.Documento, S.PLU, m.DESCRICAO, S.QTDE, vlr, Desconto,  Acrescimo ,
	Caixa_Saida, Ext_SAT = '', Hora_venda, ComandaPedidoCupom, Finalizadora = 'NAO FINALIZADOS'
Into 
	#CupomSemPagamento
From 
	Saida_estoque s INNER JOIN Mercadoria m on s.plu = m.plu 
	Where 
		s.Data_movimento BETWEEN @DataDe AND @DataAte
	And 
		not s.data_cancelamento is null
	And 
		s.Filial = @FILIAL   
	And 
		not exists(select * from Lista_finalizadora l 
					Where l.documento = s.documento and l.Emissao = s.data_movimento
						And l.pdv = s.caixa_saida
						And l.filial = s.Filial)


--exec sp_Movimento_Venda @Filial='MATRIZ', @DataDe = '20180705',  @dataate = '20180705',  @horaInicio = '00:00',  @horafim = '23:59',  @finalizadora = 'TODOS',  @Pdv = 'TODOS',  @plu = '',  @cupom = '',  @comanda = '',  @cancelados = '',  @ext_sat = '' 
IF(@plu='' AND @cupom='' and @ext_sat ='')

      BEGIN

            IF(@finalizadora ='TODOS')

                  BEGIN

                        SELECT DISTINCT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT ] = 'SFT_'+SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
                             CONVERT(varchar , s.Hora_venda,108) as HORA,
                             
							[COMANDA/PEDIDOS] =  '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                        
                             
                             FINALIZADORA = lista.id_finalizadora,
							
						     VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) 
										FROM Lista_finalizadora list1
				                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 
								  			 --INNER JOIN  Saida_estoque S  with (index(IX_Movimento_venda_01)) ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim
									WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
											 AND (list1.Emissao = lista.Emissao)
											 and list1.pdv =lista.pdv
											 and list1.documento = lista.documento
											 AND LIST1.id_finalizadora = LISTA.id_finalizadora
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) 
										    FROM Saida_estoque st  with (index(IX_Movimento_venda_01)) 
											WHERE st.Filial = @FILIAL And data_cancelamento is not null 
													and CONVERT(varchar , st.Hora_venda,108) between @horaInicio and @horafim 
													AND (st.Data_movimento = lista.Emissao)
													and st.Caixa_Saida =lista.pdv
													and st.documento = lista.documento)
						INTO #LISTA_TEMP FROM Lista_finalizadora lista
                            INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
							INNER JOIN Saida_estoque S  with (index(IX_Movimento_venda_01))  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv	and s.Data_movimento = lista.Emissao
                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )
								  and CONVERT(varchar , s.Hora_venda,108) between @horaInicio and @horafim 
                                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
								   and (LEN(@cancelados)=0 OR
										
										(@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
																						   
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
						
						--Incluir Registros sem Pagamento
						Insert into #LISTA_TEMP
						Select  DISTINCT
							DATA = CONVERT(VARCHAR,c.Data_movimento,103)
						  , PDV = c.Caixa_Saida
						  , Cupom = Documento
						  , Ext_SAT
						  , CONVERT(varchar , C.Hora_venda,108) as HORA
						  , [COMANDA/PEDIDOS] = '_'+ Max(ComandaPedidoCupom)
						  , Finalizadora
						  , vlr = 0
						  , Cancelados = SUM(Vlr-isnull(Desconto,0)) 
						From 
							#CupomSemPagamento c 
						Where
							CONVERT(varchar , c.Hora_venda,108) between @horaInicio and @horafim 
						--And 
						--	(LEN(@comanda)=0 or c.ComandaPedidoCupom  like '%'+@comanda+'%')
						and 
							Finalizadora = (case when @cancelados = ''				 then 'NAO FINALIZADOS' 
													when @cancelados = 'NAO FINALIZADOS' then 'NAO FINALIZADOS' 
													when @cancelados = 'TODOS'			 then 'NAO FINALIZADOS' 
													else 'NAO APARECE NADA' end)								
						Group By 			
							c.Data_movimento, c.Caixa_Saida, Documento, Ext_SAT, Finalizadora	, HORA_VENDA																																	   
                       
                       ----*******************************************************---------
                       
					   
						UPDATE #LISTA_TEMP SET CANCELADOS = (CANCELADOS/(SELECT COUNT(B.CUPOM) FROM #LISTA_TEMP AS B WHERE  B.CUPOM = #LISTA_TEMP.CUPOM and b.pdv = #LISTA_TEMP.pdv)  ) 
						WHERE CUPOM IN (SELECT CUPOM FROM #LISTA_TEMP where CANCELADOS >0 GROUP BY CUPOM,pdv HAVING   COUNT(CUPOM) > 1   )
						AND CANCELADOS >0
						

                       SELECT DISTINCT * FROM #LISTA_TEMP
					   

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = lista.DOCUMENTO,
                             [Ext SAT] = 'SFT_'+SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
                             CONVERT(varchar , Hora_venda,108) AS HORA,

                             [COMANDA/PEDIDOS] = '_'+(SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),
                                   VLR =(SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                                   
                         ),        

                             FINALIZADORA = id_finalizadora,
                             
                     
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st  with (index(IX_Movimento_venda_01))

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento
                                    )

                      INTO #LISTA_TEMP2   FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 
                             INNER JOIN  Saida_estoque S  ON S.Documento=lista.Documento and s.Caixa_Saida = lista.pdv and s.Data_movimento = lista.Emissao

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 
						and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
                         and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
						 and (LEN(@cancelados)=0 OR
								(@cancelados ='TODOS' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='ITEM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=0 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0) 
										OR (@cancelados='CUPOM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null AND ISNULL(cupom_cancelado,0)=1 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))>0)												   
																						   
																						   ) 
									
                       and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                      --  GROUP BY Emissao, PDV, lista.DOCUMENTO ,id_finalizadora,CONVERT(varchar , Hora_venda,108),SUBSTRING(S.ID_CHAVE,35,6)




                             SELECT DISTINCT * FROM #LISTA_TEMP2

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='' and @ext_sat = '')

BEGIN

      SELECT CUPOM = S.Documento,
			 [Ext SAT] = SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),
             DATA = CONVERT(VARCHAR,s.Data_movimento,103),
             HORA = convert(varchar,Hora_venda),
	         PDV=convert(varchar,s.caixa_saida) ,
			 'PLU'+S.PLU as PLU,
			 DESCRICAO =M.Descricao,
             QTDE=replace(convert(varchar,S.Qtde),'.',','),
             VLR=replace(convert(varchar,S.vlr),'.',','),
			 [-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),
			 [+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),
			 TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

      FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
					--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
                    INNER JOIN Mercadoria M ON S.PLU = M.PLU      
	  where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )
                        and (len(@plu)=0 or s.PLU = @plu )
                        And s.Data_Cancelamento is null
                        and s.Data_movimento BETWEEN @DataDe  AND  @DataAte
						and s.Data_movimento between @DataDe aND @DataAte
                         --and (LEN(@pdv)=0 or l.pdv = @pdv)
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
						and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by s.Data_movimento , Hora_venda

      END

ELSE

      BEGIN
      
            SELECT	CUPOM = S.Documento, --1
					[Ext SAT] = SUBSTRING(isnull(S.ID_CHAVE,(
																Select TOP 1 S2.ID_CHAVE		 
																FROM SAIDA_ESTOQUE  AS S2
																WHERE S2.Documento = S.Documento
																  AND S2.Caixa_Saida = S.Caixa_Saida
																  AND S2.Data_movimento = S.Data_movimento
																  AND S2.Filial = S.Filial
																  AND S2.ID_Chave IS NOT NULL
															)
													)
							 					,35,6),--2
					DATA = CONVERT(VARCHAR,s.Data_movimento,103), --3
					PDV=convert(varchar,s.caixa_saida) ,--4
					'PLU'+S.PLU AS PLU, --5
					DESCRICAO = M.Descricao, --6
					QTDE=replace(convert(varchar,S.Qtde),'.',','),--7
					VLR=replace(convert(varchar,S.vlr),'.',','),--8
					[-DESCONTO]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
					[+ACRESCIMO]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
					TOTAL=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01)) 
						--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
						INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
						and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
                        and s.PLU like (case when @plu <>'' then @plu else '%' end )
                        and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                        And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                        and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
                       --Group by s.Documento,s.data_movimento,s.caixa_saida,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda,SUBSTRING(S.ID_CHAVE,35,6)
		union all                      
			SELECT '',--1
				   '',--2
                   '|-CANCELADO-|',--3
				   pdv=convert(varchar,s.caixa_saida) ,--4
                   '|-'+S.PLU+'-|',--5
                   '|-'+M.Descricao+'-|', --6
                   Qtde=replace(convert(varchar,S.Qtde),'.',','),--7
                   Vlr=replace(convert(varchar,S.vlr),'.',','),--8
                   [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),--9
                   [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),--10
                   Total='0,000' --11
            FROM Saida_estoque S  with (index(IX_Movimento_venda_01))
				--INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv
				INNER JOIN Mercadoria M ON S.PLU = M.PLU      
			where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )
					and (len(@ext_sat)=0 or SUBSTRING(S.ID_CHAVE,35,6)= @ext_sat)
					and s.PLU like (case when @plu <>'' then @plu else '%' end )
                    and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                	And s.Data_Cancelamento is NOT null
					and s.Data_movimento between @DataDe aND @DataAte 
                    and s.caixa_saida like (case when @pdv <> 'TODOS' then @pdv else '%' end)
                    and (LEN(@comanda)=0 or S.ComandaPedidoCupom  like '%'+@comanda+'%')	
		union all

            select  '',--1
					'',--2
					'',--3
					'',--4
					'',--5
					'|-'+ id_finalizadora+'-|'  ,--6
					'',--7
					'',--8
					'',--9
					'',--10
					 '|-'+replace(convert(varchar,(SUM(Lista_Finalizadora.Total))),'.',',')+'-|' --11
			from Lista_finalizadora
			where  Documento like (case when @cupom <>'TODOS' then @cupom  else '%' end  )
		
                   and Emissao BETWEEN @DataDe  AND  @DataAte
                   And Isnull(Cancelado,0) = 0
                   and pdv like (case when @pdv <> 'TODOS' then @pdv else '%' end)
			GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END


end







IF OBJECT_ID('[sp_Rel_Fin_FluxoCaixa]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Fin_FluxoCaixa]
end 

go


--PROCEDURES =======================================================================================

-- exec sp_Rel_Fin_FluxoCaixa 'MATRIZ','20160101','20160131'
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End
	
	Create table Totais
(
	Descricao  varchar(400),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque WITH(INDEX(Ix_Fluxo_Caixa)) where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	insert into #cupons 
	select COUNT(*) from nf where nf.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf.Filial = @FILIAL and nf.status='AUTORIZADO'
	group by Codigo
	
	insert into #cupons
	select COUNT(*) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  and pedido.Status <>3
	GROUP by pedido
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' AS Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL and  Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
					),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo AND nf_item.Filial = nf.FILIAL where  nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO' ),0) 
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS NO CUPOM', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) 
																	FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) 
																	WHERE  FILIAL = @FILIAL 
																	   and Data_Movimento BETWEEN @DataDe AND @DataAte  
																	   AND Data_Cancelamento IS NOT NULL
																	   AND ISNULL(cupom_cancelado,0)=0 ),0))  
	INSERT into Totais
	SELECT 'CUPONS CANCELADOS FINALIZADOS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) 
																	FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) 
																	WHERE  FILIAL = @FILIAL 
																	   and Data_Movimento BETWEEN @DataDe AND @DataAte  
																	   AND Data_Cancelamento IS NOT NULL
																	   AND ISNULL(cupom_cancelado,0)=1 ),0))  				
	INSERT into Totais
	SELECT 'CUPONS CANCELADOS NAO FINALIZADOS', CONVERT(VARCHAR,ISNULL((SELECT SUM(SE.VLR-isnull(SE.desconto,0)) 
																	FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa)) 
																	WHERE  FILIAL = @FILIAL 
																	   and Data_Movimento BETWEEN @DataDe AND @DataAte  
																	   AND Data_Cancelamento IS NOT NULL
																	   AND 	not exists(select * from Lista_finalizadora l 
																		Where l.documento = se.documento and l.Emissao = se.data_movimento
																			And l.pdv = se.caixa_saida
																			And l.filial = se.Filial)
																	   ),0))  

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		






	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(convert(decimal(18,2),isnull(Total,0),0))),'.',',') from pedido with(index(ix_pedido_fluxo_caixa)) where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte and pedido.Status <>3 
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)-(isnull(NF_Item.Total,0)*isnull(nf_item.desconto,0)/100)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  AND nf_item.Filial = nf.FILIAL where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte AND nf_item.Filial = @FILIAL and nf.status='AUTORIZADO'
	UNION ALL
	SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE FILIAL = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0)),'.',',')
	UNION ALL
	SELECT '|-SUB-|'+Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where Filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10))),'.',',')
	UNION ALL
	SELECT 'PORC DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and data_movimento between @DataDe and @DataAte and data_cancelamento is null  and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
	UNION ALL
	--SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL ),0))),'.',',')
	--UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE  Filial = @FILIAL and Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL )),0)),'.',',')	
	UNION ALL
	
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS NO CUPOM'
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'CUPONS CANCELADOS FINALIZADOS'
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'CUPONS CANCELADOS NAO FINALIZADOS'
	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

	--UNION ALL
	
	--SELECT 'RESULTADO TOTAL',
	--	replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
	--FROM  TOTAIS
	--Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque WITH(INDEX(Ix_Fluxo_Caixa)) where filial = @filial and  data_movimento between @DataDe and @DataAte and data_cancelamento is null ))),'.',',')
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
    SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WITH(INDEX(Ix_Fluxo_Caixa)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento |-SUB-|','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA  WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(ISNULL(TOTAL,0))),'.',',')
		FROM LISTA_FINALIZADORA WITH(INDEX(ix_Lista_Fluxo_Caixa))
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('status_pdv') 
            AND  UPPER(COLUMN_NAME) = UPPER('data_fechamento_movimento'))
begin
	alter table status_pdv alter column data_fechamento_movimento datetime
end
else
begin
	alter table status_pdv add data_fechamento_movimento datetime
end 
go 




IF OBJECT_ID('sp_tesouraria_cancelados', 'P') IS NOT NULL
begin 
      drop procedure sp_tesouraria_cancelados
end 

go

CREATE PROCEDURE sp_tesouraria_cancelados(
		@filial as varchar(40),
		@id_movimento as int,
		@pdv as int,
		@id_funcionario as int
		)
as 
-- exec sp_tesouraria_cancelados 'MATRIZ',239,101,81
begin 
	SELECT CANCELADO = 'Canc. item', TOTAL= SUM(SE.VLR-isnull(SE.desconto,0))
	 FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa)) 
	WHERE  FILIAL = @filial 	and id_movimento = @id_movimento  AND Data_Cancelamento IS NOT NULL
		AND SE.caixa_saida =@pdv AND se.codigo_funcionario =@id_funcionario
		AND ISNULL(cupom_cancelado,0)=0
	UNION ALL 
	SELECT CANCELADO = 'Canc. cupom não Finalizado', TOTAL= ISNULL(SUM(SE.VLR-isnull(SE.desconto,0)),0)
	 FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa)) 
	WHERE  FILIAL = @filial 	and id_movimento = @id_movimento  AND Data_Cancelamento IS NOT NULL
		AND SE.caixa_saida =@pdv AND se.codigo_funcionario =@id_funcionario
		AND NOT exists(select * from Lista_finalizadora l 
					Where l.documento = SE.documento and l.Emissao = SE.data_movimento
						And l.pdv = SE.caixa_saida
						And l.filial = SE.Filial)
	UNION ALL 
	SELECT CANCELADO = 'Canc. cupom Finalizado', TOTAL= ISNULL(SUM(SE.VLR-isnull(SE.desconto,0)),0)
	 FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa)) 
	WHERE  FILIAL = @filial	and id_movimento = @id_movimento AND Data_Cancelamento IS NOT NULL
		AND SE.caixa_saida =@pdv AND se.codigo_funcionario =@id_funcionario
		AND ISNULL(cupom_cancelado,0)=1 
		AND exists(select * from Lista_finalizadora l 
					Where l.documento = SE.documento and l.Emissao = SE.data_movimento
						And l.pdv = SE.caixa_saida
						And l.filial = SE.Filial)
	UNION ALL 
	SELECT CANCELADO = 'Total cancelamentos', TOTAL= SUM(SE.VLR-isnull(SE.desconto,0))
	 FROM SAIDA_ESTOQUE AS SE WITH(INDEX(Ix_Fluxo_Caixa)) 
	WHERE  FILIAL = @filial	and id_movimento = @id_movimento  AND Data_Cancelamento IS NOT NULL
	   AND SE.caixa_saida =@pdv AND se.codigo_funcionario =@id_funcionario
end


go 


update Status_Pdv set Data_Fechamento = null where Data_Fechamento = '1900-01-01' 
go 


Alter table nf_item alter column Embalagem numeric(10)
go

Alter table nf_item_log alter column Embalagem numeric(10)
GO 




IF OBJECT_ID('[sp_rel_conta_a_receber]', 'P') IS NOT NULL
begin 
      drop procedure [sp_rel_conta_a_receber]
end 

go

CREATE  procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@cnpj varchar(20)
)As

Declare @String as nvarchar(MAX)
Declare @Where as nvarchar(MAX)


Begin
--exec  sp_Rel_conta_a_Receber    @Filial='MATRIZ', @datade = '20190319',  @dataate = '20190319',  @tipo = 'emissao',  @Status = 'PREVISTO',  @valor = '',  @Cliente = '',  @cnpj = '03.667.884/0015-26',  @centrocusto = '' 
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + '  and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Cliente))) > 1
		Begin
			set @where = @where + ' And codigo_cliente = (' + char(39) +replace(@Cliente,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	if len(rtrim(ltrim(@valor))) > 1
	Begin
		set @where = @where + ' And valor ='+REPLACE(@valor,',','.')	
	End
	if LEN(@status)>0
	begin
		set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
																 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
																 WHEN @STATUS='CANCELADO' THEN ' status =3'
																 WHEN @STATUS='LANCADO' THEN ' status =4'
																ELSE 'status <> '+CHAR(39)+'3'+CHAR(39) END
																) 
	end
	
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
		
	if (len(@cnpj)>0)
	begin
		set @Where = @Where + ' and replace(replace(replace(c.cnpj,'+ char(39)+'.'+ char(39)+','+ char(39)+ char(39)+'),'+ char(39)+'-'+ char(39)+','+ char(39)+ char(39)+'),'+ char(39)+'/'+ char(39)+','+ char(39)+ char(39)+')= '
		set @Where = @Where +  char(39)+ replace(replace(replace(@cnpj,'.',''),'-',''),'/','')+ char(39) 
	end 

	--Monta Select
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Cliente = rtrim(ltrim(Conta_a_receber.Codigo_Cliente)) +'+char(39)+'-'+char(39)+'+c.Nome_cliente, 
			Obs ,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = convert(numeric,Isnull(Desconto,0)),
			Acrescimo = convert(numeric,Isnull(Acrescimo,0)),
			ValorReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_receber.codigo_centro_custo						
		from dbo.Conta_a_receber Conta_a_receber  
			LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  
				ON  Conta_a_receber.id_cc = Conta_corrente.id_cc 
				and conta_a_receber.filial=Conta_corrente.filial  
			LEFT OUTER JOIN dbo.Centro_Custo Centro_custo 
				 ON  Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo 
				 AND  Conta_a_receber.Filial = Centro_custo.filial
			INNER JOIN CLIENTE AS C ON Conta_a_receber.CODIGO_CLIENTE = C.CODIGO_CLIENTE
		'+@where+'  Order By convert(varchar,'+ @tipo +',102)  '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
	--	set @string = @string + 'Documento = rtrim(ltrim(documento)), '
		--set @string = @string + 'Fornecedor = rtrim(ltrim(fornecedor)), '
		--set @string = @string + @tipo + '= '+ @tipo + ', '
		--set @string = @string + 'Total = valor - isnull(desconto,0), '
		--set @string = @string + 'Status = case when status = 1 then '+ char(39) +' ABERTO' + char(39)
			--set @string = @string + 'when status = 2 then '+ char(39) +'CONCLUÍDO'+ char(39)
			--set @string = @string + 'WHEN status = 3 then '+ char(39) +'CANCELADO'+ char(39)
			--set @string = @string + 'WHEN status = 4 then '+ char(39) +'LANÇADO'+ char(39) + 'End '
		--set @string = @string + 'From Conta_a_pagar '
		--set @string = @string + @where
		--set @string = @string + ' Order By convert('+ @Tipo +',102) '
			--Print @string
	Exec(@string)
End



go
	insert into Versoes_Atualizadas select 'Versão:1.235.741', getdate();
GO
