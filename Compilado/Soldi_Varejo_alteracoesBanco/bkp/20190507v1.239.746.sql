
IF OBJECT_ID('[sp_EFD_ICMSIPI_C800_GDT]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800_GDT]
end 

go

--[sp_EFD_ICMSIPI_C800_GDT] 'MATRIZ','20180601', '20180630'
create PROC [dbo].[sp_EFD_ICMSIPI_C800_GDT]
	@Filial		AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim    As varchar(8),
	@NR_SAT	    AS VARCHAR(50)
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
		  AND S.numero_serie = @NR_SAT
		group by S.numero_serie
		        ,REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/','')
				,convert(varchar,s.data_movimento,102)
		order by convert(varchar,s.data_movimento,102)
				
			
END

GO 


IF OBJECT_ID('[sp_EFD_PisCofins_C400]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_PisCofins_C400]
end 

go
-- STORED PROCEDURE
-- sp_EFD_PisCofins_C400 'MATRIZ', '20190201', '20190228'
create    Procedure [dbo].[sp_EFD_PisCofins_C400]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8)
AS                            
BEGIN
	SELECT 
		REG = 'C400',
		COD_MOD = '2D',
		ECF_MOD = a.Modelo,
		ECF_FAB = a.FabEqu,
		ECF_CX = rtrim(ltrim(convert(varchar(3),a.Caixa)))
	FROM
		Controle_Filial_PDV a
	WHERE
		a.Filial = @Filial
		AND EXISTS(SELECT * FROM Fiscal b WHERE
				a.Filial = b.Filial
				AND a.Caixa = b.Caixa
--				AND a.Caixa NOT IN (1)
--				AND a.FabEqu = b.FabEqu
				AND REPLACE(CONVERT(VARCHAR,b.Data,102),'.','') BETWEEN @DataInicio AND @DataFim)
		AND ISNULL(NGS,0)= 0
		and isnull(a.sat,0) <> 1
	ORDER BY 1,2
END

GO 


/****** Object:  Index [ix_SPED_C850]    Script Date: 26/03/2019 16:01:11 ******/
CREATE NONCLUSTERED INDEX [ix_SPED_C850] ON [dbo].[Saida_estoque]
(
	[Filial] ASC,
	[Data_movimento] ASC,
	[numero_serie] ASC,
	[id_chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO





GO 


IF OBJECT_ID('[sp_EFD_ICMSIPI_C850]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C850]
end 

go
--[sp_EFD_ICMSIPI_C800] 'MATRIZ','20190201', '509446'
--[sp_EFD_ICMSIPI_C850] 'MATRIZ','35190259610469000195590005094460781847878205','20190201', '509446'
CREATE  PROC [dbo].[sp_EFD_ICMSIPI_C850]
	@Filial		AS Varchar(20),
	@CHV_CFE	as varchar(44),
	@DATA     AS VARCHAR(8),
	@NR_SAT     AS VARCHAR(50)
As
BEGIN
	

	BEGIN
		SELECT
		       	REG = 'C850',
		       	CST_ICMS = '0' + RTRIM(LTRIM(CASE WHEN b.NRO_ECF = 4 THEN '60' 
							  WHEN b.NRO_ECF = 5 THEN '40' 
							  WHEN b.NRO_ECF = 6 THEN '41'
							ELSE '00' END )),
		       	CFOP = CASE WHEN b.NRO_ECF = 4 THEN '5405' ELSE '5102' END,
			--** Aliuota ICMS	
			ALIQ_ICMS =  CASE WHEN b.NRO_ECF = 4 THEN 0 ELSE b.Aliquota_ICMS END,
			VL_OPR = b.VLR,
			VL_BC_ICMS = CASE WHEN b.NRO_ECF = 4 OR b.NRO_ECF = 5 THEN 0 ELSE (ISNULL(((B.VLR+ISNULL(B.ACRESCIMO,0))-ISNULL(B.Desconto,0)),0)) END
		INTO
			#EFD_ICMSIPI_C850
		FROM
		
		       Mercadoria a INNER JOIN Saida_Estoque b with (INDEX=ix_SPED_C850) ON a.PLU = b.PLU
		
		       INNER JOIN Tributacao t ON t.Codigo_Tributacao = a.Codigo_Tributacao AND t.Filial = b.Filial
		WHERE
		       b.Filial = @Filial
			   AND B.DATA_MOVIMENTO = @DATA
			   AND B.NUMERO_SERIE = @NR_SAT
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
			VL_ICMS = SUM(CASE WHEN ALIQ_ICMS =0 THEN 0 ELSE convert(numeric(18,2),(ISNULL(VL_BC_ICMS * (ALIQ_ICMS/100),0)))END ),
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



GO 


GO


IF OBJECT_ID('[sp_EFD_ICMSIPI_C800]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_C800]
end 

go

-- exec [sp_EFD_ICMSIPI_C800] 'MATRIZ','20190207','509446'
CREATE PROC [dbo].[sp_EFD_ICMSIPI_C800]
	@Filial		AS Varchar(20),
	@Data		As Varchar(8),
	@NR_SAT  	As Varchar(40)
	
As
BEGIN
	

		select 
			REG = 'C800',
			COD_MOD ='59',
			COD_SIT = '00',
			NUM_CFE = substring(s.id_chave,35,6) , --S.coo,
			DT_DOC = REPLACE(CONVERT(VARCHAR,S.Data_Movimento,103),'/',''),
			VL_CFE = ((isnull(S.vlr,0)-isnull(S.Desconto,0))+(isnull(S.Acrescimo,0))),
			VL_PIS = (S.TotalPis),
			VL_COFINS = (S.TotalCofins),
			CNPJ_CPF = REPLACE(REPLACE(REPLACE(c.CNPJ,'.',''),'-',''),'/',''),
			NR_SAT = S.numero_serie,
			CHV_CFE = REPLACE(S.id_Chave,'Cfe',''),
			VL_DESC =(S.Desconto),
			VL_MERC = (S.VLR),
			VL_OUT_DA =(isnull(s.acrescimo,0)),
			VL_BASE_ICMS = CASE WHEN S.NRO_ECF = 4 OR S.NRO_ECF = 5 THEN 0 ELSE (ISNULL(((S.VLR+ISNULL(S.ACRESCIMO,0))-ISNULL(S.Desconto,0)),0)) END,
			ALIQ_ICMS =S.Aliquota_ICMS,
			VL_PIS_ST =NULL,
			VL_COFINS_ST =NULL
		INTO #C800 	
		FROM Mercadoria a
		inner join Saida_estoque S on a.plu = S.plu and a.filial = S.filial
		left join cliente as c on s.codigo_cliente = c.codigo_cliente
		WHERE S.filial = @filial 
		  and S.data_cancelamento is null  
		  and (CONVERT(DATE,S.Data_Movimento) =@Data)
		  AND S.numero_serie = @NR_SAT
		 AND S.id_Chave IS NOT NULL	
		
	
	
select 
			REG,
			COD_MOD ,
			COD_SIT ,
			NUM_CFE , --S.coo,
			DT_DOC ,
			VL_CFE = SUM(VL_CFE),
			VL_PIS = SUM(VL_PIS),
			VL_COFINS = SUM(VL_COFINS),
			CNPJ_CPF ,
			NR_SAT ,
			CHV_CFE ,
			VL_DESC =SUM(VL_DESC ),
			VL_MERC = SUM(VL_MERC),
			VL_OUT_DA =SUM(VL_OUT_DA),
			VL_ICMS =SUM( CASE WHEN ALIQ_ICMS=0 THEN 0 ELSE CONVERT(NUMERIC(18,2),(VL_BASE_ICMS*(ALIQ_ICMS/100))) END),
			VL_PIS_ST ,
			VL_COFINS_ST 
FROM #C800

GROUP BY REG,
			COD_MOD ,
			COD_SIT ,
			NUM_CFE , --S.coo,
			DT_DOC ,
			CNPJ_CPF ,
			NR_SAT ,
			CHV_CFE ,
			VL_PIS_ST ,
			VL_COFINS_ST 
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
		  and (CONVERT(DATE,S.Data_Movimento) =@Data)
		  AND S.numero_serie = @NR_SAT
		  AND S.id_Chave IS NOT NULL	
		group by S.COO
		        ,CONVERT(DATE,S.Data_Movimento)
				,c.CNPJ
				,S.numero_serie
				,id_Chave

			
END




IF OBJECT_ID('[sp_EFD_ICMSIPI_H001]', 'P') IS NOT NULL
begin 
	drop procedure [sp_EFD_ICMSIPI_H001]
end 

go
create Procedure [dbo].[sp_EFD_ICMSIPI_H001]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim        As varchar(8)
AS                            
BEGIN
-- SP_EFD_ICMS_IPI_H001 'MATRIZ' , '20190101', '20190120'
DECLARE @QTDE INT  

SELECT @QTDE = COUNT(*) 
FROM MERCADORIA_ESTOQUE_MES
where filial = @filial 
	and data between @DataInicio and @DataFim

	SELECT 
	--** Campo [REG] Texto fixo contendo "0001" C 004 - O
	'|H001' + 
	--** Campo [IND_MOV] Indicador de movimento: N 001 - O
		--** 0 - Bloco com dados informados;
		--** 1 - Bloco sem dados informados.
	CASE WHEN  @QTDE >0 THEN '|0|' ELSE '|1|' END

		RETURN                          
                      
END

go 



IF OBJECT_ID('sp_EFD_ICMSIPI_H005', 'P') IS NOT NULL
begin 
	drop procedure sp_EFD_ICMSIPI_H005
end 

go
create procedure [dbo].[sp_EFD_ICMSIPI_H005]
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim        As varchar(8)

--EFD_ICMSIPI_H005 'MATRIZ','31032013'
AS
	SELECT 
		REG = 'H005',
		DT_INV = REPLACE(CONVERT(VARCHAR,DATA,103),'/','')	,
		VL_INV = SUM(CONVERT(DECIMAL(12,2),Saldo * Preco_Custo)),
		MOT_INV = '01'
	FROM 
		MERCADORIA_ESTOQUE_MES
	WHERE
		Data between @DataInicio and @DataFim
	AND	
		ISNULL(Saldo,0) > 0 AND ISNULL(Preco_Custo, 0) > 0
	GROUP BY REPLACE(CONVERT(VARCHAR,DATA,103),'/','')	
	

go 


IF OBJECT_ID('sp_EFD_ICMSIPI_C190', 'P') IS NOT NULL
begin 
	drop procedure sp_EFD_ICMSIPI_C190
end 

GO

--sp_EFD_ICMSIPI_C190 'MATRIZ','20180201','20180228','VEPEA',218996
create         Procedure [dbo].[sp_EFD_ICMSIPI_C190]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8),
	@Fornecedor		As Varchar(20),
	@NroNota		As Varchar(10)
AS                            
BEGIN
	BEGIN
		IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND TABLE_NAME='EFD_C190')
			BEGIN
				DROP TABLE EFD_C190

				CREATE TABLE EFD_C190
					(REG VARCHAR(4),
					CST_ICMS VARCHAR(4),
					CFOP VARCHAR(4),
					ALIQ_ICMS NUMERIC(5,2),
					VL_OPR NUMERIC(14,2),
					VL_BC_ICMS NUMERIC(14,2),
					VL_ICMS NUMERIC(12,2),
					VL_BC_ICMS_ST NUMERIC(12,2),
					VL_ICMS_ST NUMERIC(12,2),
					VL_RED_BC NUMERIC(12,2),
					VL_IPI  NUMERIC(12,2),
					COD_OBS  VARCHAR(250))
			END
		ELSE
			BEGIN
				CREATE TABLE EFD_C190
					(REG VARCHAR(4),
					CST_ICMS VARCHAR(4),
					CFOP VARCHAR(4),
					ALIQ_ICMS NUMERIC(5,2),
					VL_OPR NUMERIC(14,2),
					VL_BC_ICMS NUMERIC(14,2),
					VL_ICMS NUMERIC(12,2),
					VL_BC_ICMS_ST NUMERIC(12,2),
					VL_ICMS_ST NUMERIC(12,2),
					VL_RED_BC NUMERIC(12,2),
					VL_IPI  NUMERIC(12,2),
					COD_OBS  VARCHAR(250))
			END

	END	

	BEGIN

		SELECT 
			REG = 'C190',
			CST_ICMS =  case when len(t.CST_SPED) >= 3 then  t.CST_SPED else '0' + t.CST_SPED End,--case when len(t.CST_SPED) >= 3 Then Isnull(t.CST_SPED,t.Indice_ST) when b.Codigo_operacao in (1551,1556,1933) Then convert(varchar,Isnull(m.Origem,'0')) + '90' when Isnull(b.origem,'0') = '1' then '2' + Isnull(t.CST_SPED,t.Indice_ST) else convert(varchar,Isnull(b.origem,Isnull(m.Origem,'0'))) + Isnull(t.CST_SPED,t.Indice_ST) End,			
			CFOP = CONVERT(VARCHAR(4), b.Codigo_Operacao),
			ALIQ_ICMS = Convert(Numeric(5,2),ISNULL(case when b.Tipo_NF = 1 then b.Aliquota_ICMS else case when ISNULL(t.Incide_ICMS, 0) = 0 then 0 else CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.Entrada_ICMS end end end,0)),
			--VL_OPR = Convert(Numeric(18,2),ISNULL(b.Total-(((b.Total + isnull(b.IPIV, 0) + isnull(b.IVA, 0) + isnull(b.Despesas, 0))*b.Desconto)/100),0)),
			--VL_OPR = Convert(Numeric(18,2),ISNULL(a.Total,0)),
			VL_OPR = Convert(Numeric(18,2),Isnull(b.Total,0)+Isnull(b.IVA,0)+Isnull(b.DESPESAS,0)+ISNULL(IPIV,0)),
			VL_BC_ICMS = b.base_icms, --Convert(Numeric(18,2),case when a.codigo_operacao = 5929 then 0 else case when a.Cliente_Fornecedor = 'VEPEA' then a.Base_Calculo else (CASE WHEN ISNULL(t.Incide_ICMS, 0) = 1 THEN  ((b.Total * (1 - (ISNULL(t.Redutor,0) / 100))) - CASE WHEN b.Desconto >0 THEN (b.Desconto/100) * b.Total ELSE 0 END) ELSE 0 End) end End),
			VL_ICMS = b.icmsv,-- Convert(Numeric(18,2),( case when a.codigo_operacao = 5929 then 0 when ISNULL(t.Incide_ICMS, 0) = 1 THEN case when a.Cliente_Fornecedor = 'VEPEA' then a.ICMS_Nota else (b.total-(CASE WHEN b.Desconto >0 THEN (b.Desconto/100) * b.Total ELSE 0 END)) End ELSE 0 End * (CASE WHEN b.Tipo_NF = 1 then t.Saida_ICMS else CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.ICMS_Efetivo  end end / 100))),			
			VL_BC_ICMS_ST = CASE WHEN ISNULL(t.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(t.Incide_ICM_Subistituicao, 0) = 1 THEN b.Base_IVA ELSE Case When b.Tipo_NF = 1 Then b.Base_IVA Else 0 END END,
			VL_ICMS_ST = CASE WHEN ISNULL(t.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(t.Incide_ICM_Subistituicao, 0) = 1 THEN b.IVA ELSE Case When b.Tipo_NF = 1 Then b.IVA Else 0 END END,
			VL_RED_BC = Convert(Numeric(12,2),(CASE WHEN t.Indice_ST = '20' THEN  (b.Total * (case when b.Tipo_NF = 1 then isnull(t.Redutor, 0) else isnull(b.Redutor_Base, 0) end / 100)) ELSE 0 END)),
			VL_IPI = CASE WHEN ISNULL(t.IPI_EmOutrasDespesas, 0) = 0 THEN Isnull(b.IPIV,0) ELSE 0 END,
			COD_OBS = ' '
		INTO 
			#C190
		FROM
			NF a 
			INNER JOIN NF_Item b ON a.FILIAL = b.FILIAL AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR AND a.Codigo = b.Codigo	AND a.Tipo_NF  = b.Tipo_NF		
			INNER JOIN Tributacao t ON t.Codigo_Tributacao = b.Codigo_Tributacao
			INNER JOIN Mercadoria m ON m.PLU = b.PLU
	
		WHERE
			a.Filial = @Filial
			AND a.Data BETWEEN @DataInicio AND @DataFim
			AND LTRIM(RTRIM(LTRIM(a.Cliente_Fornecedor))) = @Fornecedor
			AND Convert(Numeric,a.Codigo) = @NroNota
	END
	BEGIN
		INSERT INTO 
			EFD_C190
		SELECT 
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS,
			VL_OPR = SUM(VL_OPR),
			VL_BC_ICMS =  SUM(VL_BC_ICMS),
			VL_ICMS = SUM(VL_ICMS),
			VL_BC_ICMS_ST = SUM(VL_BC_ICMS_ST),
			VL_ICMS_ST = SUM(VL_ICMS_ST),
			VL_RED_BC = AVG(VL_RED_BC),
			VL_IPI = SUM(VL_IPI),
			COD_OBS
		FROM
			#C190

		GROUP BY
			REG,
			CST_ICMS,
			CFOP,
			ALIQ_ICMS,
			COD_OBS
		ORDER BY 1,2
	END
            
		   SELECT * FROM EFD_C190                  
                      
END
go 

IF OBJECT_ID('[sp_Rel_Resumo_Vendas]', 'P') IS NOT NULL
begin 
	drop procedure [sp_Rel_Resumo_Vendas]
end 

go

create     Procedure [dbo].[sp_Rel_Resumo_Vendas]
            @Filial		  As Varchar(20),
            @DataDe		  As Varchar(8),
            @DataAte	  As Varchar(8),
            @plu		  As Varchar (20),
            @descricao	  As varchar(50),
            @grupo	      As Varchar(50),
            @subGrupo	  As varchar(50),
            @departamento as varchar(50),
            @relatorio	  as varchar(40)

 

AS
-- EXEC sp_Rel_Resumo_Vendas 'MATRIZ','20161101','20161130','','','','','','PEDIDO SIMPLES'
-- @relatorio = TODOS ,CUPOM , NOTA SAIDA, PEDIDO SIMPLES 	
	IF OBJECT_ID(N'tempdb..#tmpVendas', N'U') IS NOT NULL   
	begin
		DROP TABLE #tmpVendas;  
	end
CREATE TABLE #tmpVendas
(
	Data varchar(30),
	Dia_Semana varchar(30),
	Venda Decimal(18,2),
	Clientes int,
	Venda_MD  Decimal (12,2),
	NFP INT,
	Vlr_NFP Decimal(18,2),
	Perc_NFP Decimal(8,2)
)

declare @ini_periodo varchar(5),@fim_periodo varchar(5), @dias_periodo int
select @ini_periodo = inicio_periodo
	 , @fim_periodo =fim_periodo
	 , @dias_periodo = dias_periodo 
		
from filial where filial = @filial
		



if(@relatorio='TODOS' OR @relatorio='CUPOM')
BEGIN
SELECT
            saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            Vlr = Convert(Decimal(15,2),SUM(isnull(Vlr,0)-isnull(Desconto,0)+isnull(acrescimo,0))),
            CPF_CNPJ,
			hora_venda
		  INTO
				#Lixo
      FROM
            Saida_Estoque with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria  as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  

            
      WHERE
            saida_estoque.Filial = @Filial
      AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
      and   hora_venda between '00:00:00' and '23:59:59'
      AND   Data_Movimento BETWEEN @DataDe AND dateadd(day,@dias_periodo,@DataAte)
      AND   Data_Cancelamento IS NULL
      AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
      
      GROUP BY
           Saida_estoque.Filial,
            Data_Movimento,
            Caixa_Saida,
            Documento,
            CPF_CNPJ,
			Hora_venda;

  

insert into #tmpVendas
 SELECT

      Data = Convert(Varchar,Data_Movimento,103),
      Dia_Semana = CASE
            WHEN DATEPART(dw, Data_Movimento) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, Data_Movimento) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, Data_Movimento) = 3 THEN 'TERCA'
            WHEN DATEPART(dw, Data_Movimento) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, Data_Movimento) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, Data_Movimento) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, Data_Movimento) = 7 THEN 'SABADO'

      END,

      Venda =       (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  ),
      Clientes =    (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ),
      Venda_MD =    CONVERT(DECIMAL(12,2), 
						CASE WHEN (SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) > 0 THEN

                  ((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) FROM #LIXO WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ) /  (SELECT COUNT(*) FROM #Lixo WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) ))

                  ELSE 0 END),

      NFP =         (SELECT COUNT(*) FROM #Lixo WHERE ( #lixo.CPF_CNPJ <> '') and ((#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  or (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)  )),

      Vlr_NFP =     ISNULL((SELECT Convert(Decimal(18,2),SUM(ISNULL(VLR,0))) 
							FROM #Lixo 
							WHERE (isnull(#lixo.CPF_CNPJ,'') <> '')   
							and (
									(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)
								or  (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo))
							),0),

     

      Perc_NFP =    CASE WHEN (
							SELECT Convert(Decimal(18,2),SUM(VLR)) 
							FROM #Lixo 
							WHERE (isnull(#lixo.CPF_CNPJ,'') <> '')  and 
							(
							(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo )  
							or 
							(#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo)
							)  
						) > 0 
				THEN

                  CONVERT(DECIMAL(8,2), 
				  (
					(
						SELECT Convert(Decimal(18,2),SUM(VLR)) 
						FROM #Lixo 
						WHERE (#lixo.CPF_CNPJ <> '') and 
						(
							(#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo )  
							or 
							(#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) 
						) 
					) /
                    (	
						SELECT Convert(Decimal(18,2),SUM(#Lixo.Vlr)) 
						FROM #Lixo 
						WHERE (#LIXO.Data_Movimento = Saida_Estoque.Data_Movimento and #Lixo.Hora_venda >= @ini_periodo)  
								or 
							  (#LIXO.Data_Movimento = dateadd(day,@dias_periodo,Saida_Estoque.Data_Movimento) and #Lixo.Hora_venda <= @fim_periodo) 
					)) * 100)

                  ELSE 0 END
	 
      FROM

            Saida_Estoque  with (index(ix_analise_de_vendas_por_dia))
            inner join mercadoria as m with (index(PK_Mercadoria)) on m.PLU = Saida_Estoque.PLU
			INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  


      WHERE

            Saida_Estoque.Filial = @Filial

    

	  AND (LEN(@PLU)=0 OR Saida_estoque.PLU = @plu)
	  AND (Data_Movimento BETWEEN @DataDe AND @DataAte)
	  AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	  and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	  and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
	   
      GROUP BY

            Data_Movimento
 END

if(@relatorio='TODOS' OR @relatorio='NOTA SAIDA')
BEGIN
insert into #tmpVendas
select 
	CONVERT(VARCHAR,N.Emissao,103) AS DATA ,
	  Dia_Semana = CASE
            WHEN DATEPART(dw, N.Emissao) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, N.Emissao) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, N.Emissao) = 3 THEN 'TERCA'
            WHEN DATEPART(dw, N.Emissao) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, N.Emissao) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, N.Emissao) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, N.Emissao) = 7 THEN 'SABADO'

      END,
      SUM(ni.TOTAL-(isnull(ni.Total,0)*isnull(ni.desconto,0)/100)) AS Venda,
      
      (
		  	Select COUNT(*) 
		from nf 
			inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 

		where NF.FILIAL=@filial 
				and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
				and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403') 
				AND  (NF.Emissao= n.Emissao )
				AND   NF.TIPO_NF = 1 
				AND ISNULL(NF.nf_Canc,0)=0	
				and nf.status='AUTORIZADO'
				AND (NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											

											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM nf WHERE nf.Emissao = n.Emissao) > 0 THEN

                  (
                  Select isnull(SUM(nii.TOTAL-(isnull(nii.Total,0)*isnull(nii.desconto,0)/100)),0) 
					from nf 
					inner join nf_item as nii on NF.codigo=nii.codigo and NF.Filial=nii.Filial and NF.Tipo_NF = nii.Tipo_NF
					inner join mercadoria as mi on mi.PLU = nii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
							and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
							and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
							 AND (NF.Emissao= n.Emissao )
							 AND NF.TIPO_NF = 1
							 and nf.status='AUTORIZADO'
							 AND (LEN(@PLU)=0 OR nii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from nf 
					inner join Natureza_operacao as np on nf.Codigo_operacao = np.Codigo_operacao 
					where NF.FILIAL=@filial 
								AND  (NF.Emissao= n.Emissao )
								AND   NF.TIPO_NF = 1	
								and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
								and nf.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
								and nf.status='AUTORIZADO'
								AND (
						NF.Codigo IN (SELECT DISTINCT CODIGO 
											FROM NF_Item as nii  inner join mercadoria as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  NF as N
inner join nf_item as ni on ni.codigo=n.codigo and ni.Filial=n.Filial and n.Tipo_NF = ni.Tipo_NF
inner join mercadoria as m on m.PLU = ni.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
inner join Natureza_operacao as np on n.Codigo_operacao = np.Codigo_operacao 
WHERE  N.FILIAL=@filial 
AND  (N.Emissao BETWEEN @DataDe AND @DataAte)
	 and np.Saida = 1  and isnull(np.NF_devolucao,0) =0
	 and n.Codigo_operacao IN ('5102','5405','5402','5403','6102','6405','6401','6403')
	 and n.status='AUTORIZADO'						
	 AND   N.TIPO_NF = 1	and isnull(n.nf_Canc,0)<>1
	 AND (LEN(@PLU)=0 OR NI.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY N.Emissao 
END


if(@relatorio='TODOS' OR @relatorio='PEDIDO SIMPLES')
BEGIN
insert into #tmpVendas
select 
	CONVERT(VARCHAR,p.Data_cadastro,103) AS DATA ,
	  Dia_Semana = CASE
            WHEN DATEPART(dw, p.Data_cadastro) = 1 THEN 'DOMINGO'
            WHEN DATEPART(dw, p.Data_cadastro) = 2 THEN 'SEGUNDA'
            WHEN DATEPART(dw, p.Data_cadastro) = 3 THEN 'TERCA'
            WHEN DATEPART(dw, p.Data_cadastro) = 4 THEN 'QUARTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 5 THEN 'QUINTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 6 THEN 'SEXTA'
            WHEN DATEPART(dw, p.Data_cadastro) = 7 THEN 'SABADO'

      END,
      SUM(pit.TOTAL) AS Venda,
      
      (
		  	Select COUNT(*) 
		from pedido  with (index(ix_pedido_fluxo_caixa))
		where pedido.pedido_simples = 1  
				AND  (pedido.Data_cadastro= p.Data_cadastro )
				and isnull(pedido.Status,0)<>3
				AND   pedido.Tipo = 1 
				and pedido.FILIAL=@filial 
				AND (pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as	nii										
											inner join mercadoria  as mi on mi.PLU = nii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO as cdi  ON cdi.codigo_departamento = mi.Codigo_departamento  
											WHERE  (LEN(@PLU)=0 OR nii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
										)
							)
      ) 
      as Clientes ,
      (
      CONVERT(DECIMAL(12,2), CASE WHEN (SELECT Convert(Decimal(18,2),SUM(TOTAL)) FROM pedido WHERE pedido.Data_cadastro = p.Data_cadastro) > 0 THEN

                  (
                  Select isnull(SUM(pii.total),0) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					inner join pedido_itens as pii on pedido.pedido=pii.Pedido and pedido.Filial=pii.Filial and pedido.Tipo = pii.Tipo
					inner join mercadoria as mi on mi.PLU = pii.PLU
					INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
					where pedido.FILIAL=@filial 
							 AND (pedido.Data_cadastro= p.Data_cadastro)
							 AND pedido.TIPO = 1
							 and pedido.pedido_simples = 1
							 AND (LEN(@PLU)=0 OR pii.PLU = @plu)
							 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
							 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
							 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
							) 
							 /   
					(	
					Select COUNT(*) 
					from pedido with (index(ix_pedido_fluxo_caixa))
					where pedido.FILIAL=@filial 
								AND  (pedido.Data_cadastro= p.Data_cadastro )
								AND   pedido.TIPO = 1	
								and pedido.pedido_simples = 1
								
								AND (
						pedido.pedido IN (SELECT DISTINCT pedido 
											FROM pedido_Itens as pii  inner join mercadoria as mi on mi.PLU = pii.PLU
											INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cdi ON cdi.codigo_departamento = mi.Codigo_departamento  
											 WHERE  (LEN(@PLU)=0 OR pii.PLU = @plu)
													 AND (LEN(@grupo)=0 or cdi.Descricao_grupo = @grupo)
													 and (LEN(@subGrupo)=0 or cdi.descricao_subgrupo = @subGrupo)
													 and (len(@departamento)=0 or cdi.descricao_departamento = @departamento)
											
													  )
									)
					)

                  ELSE 0 END)
                  
                  
      ) as [Venda_MD],
      0 as [NFP],
      0 AS [Vlr NFP],
      0 as [Perc NP]

from  Pedido as p with (index(ix_pedido_fluxo_caixa))
inner join Pedido_itens  as pit with (index(ix_sp_rel_resumo_vendas)) on pit.Pedido=p.pedido and pit.Filial=p.Filial and p.Tipo = pit.Tipo
inner join mercadoria as m on m.PLU = pit.PLU
INNER JOIN W_BR_CADASTRO_DEPARTAMENTO AS cd ON cd.codigo_departamento = m.Codigo_departamento  
WHERE p.pedido_simples = 1   
	AND  (p.Data_cadastro BETWEEN @DataDe AND @DataAte)
	 and isnull(p.Status,0)<>3
	 AND   p.TIPO = 1	
	 and p.FILIAL=@filial 
	 AND (LEN(@PLU)=0 OR pit.PLU = @plu)
	 AND (LEN(@grupo)=0 or cd.Descricao_grupo = @grupo)
	 and (LEN(@subGrupo)=0 or cd.descricao_subgrupo = @subGrupo)
	 and (len(@departamento)=0 or cd.descricao_departamento = @departamento)
GROUP BY p.Data_cadastro
END






Select 
			Data,
			Dia_Semana,
			Venda,
			 Clientes ,
			 [Venda_MD],
			 NFP,
			 VLR_NFP,
			 PERC_NFP
from (
 Select 
			ORDEM='0000',
			 Data,
			Dia_Semana,
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas
GROUP BY Data,Dia_Semana
UNION ALL
 Select 
			ORDEM='ZZZZ',
			'|-SUB-|TOTAL',
			'',
			Sum(Venda) as Venda,
			'|-NI-|' +CONVERT(VARCHAR,SUM(Clientes)) as Clientes ,
			SUM(venda)/SUM(Clientes) as [Venda_MD],
			SUM(nfp) as NFP,
			SUM(VLR_NFP) as VLR_NFP,
			SUM(PERC_NFP) AS PERC_NFP
	from 
	#tmpVendas

) as a 

ORDER BY ordem ,Data ;

--EXEC @SQL;



GO 


	-- STORED PROCEDURE
-- sp_EFD_PisCofins_C170 'MATRIZ', '20180801', '20180831', 'BIMBO DO BRASIL', 4770
ALTER   Procedure [dbo].[sp_EFD_PisCofins_C170]   
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
		VL_BC_ICMS = b.base_icms, --CASE WHEN ISNULL(t.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(t.Redutor,0) / 100))) ELSE 0 End,
		ALIQ_ICMS = CASE WHEN b.Tipo_NF = 1 then t.Saida_ICMS else case when ISNULL(t.Incide_ICMS, 0) =  0 THEN 0 ELSE CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.Entrada_ICMS end end  end,
		VL_ICMS = b.icmsv,--CASE WHEN ISNULL(t.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(t.Redutor, 0) / 100))) ELSE 0 End * (CASE WHEN b.Tipo_NF = 1 then t.Saida_ICMS else CASE WHEN t.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE t.Entrada_ICMS END end / 100),
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
				(@TIPO =1 AND  b.Tipo_NF=2 and(CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(a.Producao_NFe, 0) = 1 AND a.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 )
				OR 
				@TIPO = 0 
			)
	ORDER BY 1,2

	RETURN                          

END


GO 


ALTER         procedure [dbo].[sp_EFD_PisCofins_0200]
	@Filial	AS Varchar(20),
	@DataInicio	As Varchar(8),
	@DataFim	As Varchar(8)
AS

SELECT Distinct Plu into #0200Plu
					FROM SAIDA_ESTOQUE with (index (IX_Saida_Estoque))
					WHERE FILIAL = @Filial
						--AND SAIDA_ESTOQUE.PLU = MERCADORIA.PLU
						AND SAIDA_ESTOQUE.CAIXA_SAIDA IN (SELECT CAIXA FROM CONTROLE_FILIAL_PDV WHERE Controle_Filial_PDV.Filial = Saida_estoque.filial and Controle_Filial_PDV.NGS = 0 and sat <>1)
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
and (b.Tipo_NF=2 or (isnull(NatOp.NF_devolucao,0)=1 AND a.codigo_operacao > 6000 ))
and(CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(B.Producao_NFe, 0) = 1 AND B.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 
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

  ALTER  Procedure [dbo].[sp_EFD_PisCofins_C100]   
	@Filial			AS Varchar(20),
	@DataInicio		As Varchar(8),
	@DataFim		As Varchar(8)
AS                            
BEGIN



	SELECT 
		REG = 'C100',
		IND_OPER = '0',
		IND_EMIT = CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(a.Producao_NFe, 0) = 1 AND a.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END ,
		COD_PART = a.Cliente_Fornecedor,
		COD_MOD = case when a.id <> '' then '55' else '01'end,
		COD_SIT = '00',
		SER = case when Substring(a.id,23,3) = '' then '001' else Substring(a.id,23,3) end, --,'001',		
		NUM_DOC = a.Codigo,
		CHV_NFE = isnull(a.id,''),
		DT_DOC = a.Emissao,
		DT_E_S = CASE WHEN a.Data < a.Emissao THEN a.Emissao ELSE a.Data END,
		VL_DOC = SUM(b.Total + ISNULL(b.IPIV,0) + ISNULL(b.IVA,0) + ISNULL(b.Despesas,0)),
		IND_PGTO = '0',
		VL_DESC = a.desconto,
		VL_ABAT_NF = CONVERT(FLOAT,0),
		VL_MERC = SUM(b.TOTAL),
		IND_FRT = '1',
		VL_FRT = SUM(ISNULL(a.Frete,0)),
		VL_SEG = CONVERT(FLOAT,0),
		VL_OUT_DA = SUM(ISNULL(b.Despesas,0) + ISNULL(CASE WHEN ISNULL(c.ICMSST_EmOutrasDespesas, 0) = 1 THEN Isnull(b.IVA,0) ELSE 0 END,0)+ISNULL(CASE WHEN ISNULL(c.IPI_EmOutrasDespesas, 0) = 1 THEN Isnull(b.IPIV,0) ELSE 0 END,0)),
		VL_BC_ICMS = sum(b.base_icms),--SUM(CASE WHEN ISNULL(c.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(c.Redutor,0) / 100))) ELSE 0 End),
		VL_ICMS = sum(b.icmsv),--SUM(CASE WHEN ISNULL(c.Incide_ICMS, 0) = 1 THEN  (b.Total * (1 - (ISNULL(c.Redutor,0) / 100))) ELSE 0 End * (CASE WHEN b.Tipo_NF = 1 then b.Aliquota_ICMS else CASE WHEN c.CST_SPED = '101' and ISNULL(b.pCredSN, 0) > 0 THEN b.pCredSN ELSE c.Entrada_ICMS END END/ 100)),
		VL_BC_ICMS_ST = SUM(CASE WHEN ISNULL(c.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(c.Incide_ICM_Subistituicao, 0) = 1 THEN Isnull(b.Base_IVA,0) ELSE 0 END),
		VL_ICMS_ST = SUM(CASE WHEN ISNULL(c.ICMSST_EmOutrasDespesas, 0) = 0 AND ISNULL(c.Incide_ICM_Subistituicao, 0) = 1 THEN Isnull(b.IVA,0) ELSE 0 End),
		VL_IPI = SUM(CASE WHEN ISNULL(c.IPI_EmOutrasDespesas, 0) = 0 THEN Isnull(b.IPIV,0) ELSE 0 END),
		VL_PIS = SUM(CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN CONVERT(INT, M.cst_entrada) BETWEEN 50 AND 67 AND ISNULL(M.Pis_Perc_Entrada , 0) > 0 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (ISNULL(M.Pis_Perc_Entrada , 0) / 100)) ELSE 0 END END),
		VL_COFINS = SUM(CASE WHEN b.Tipo_NF = 2 AND (b.CODIGO_OPERACAO BETWEEN 1900 AND 1999 OR b.CODIGO_OPERACAO BETWEEN 2900 AND 2999) THEN 0 ELSE CASE WHEN CONVERT(INT, M.cst_entrada) BETWEEN 50 AND 67 AND ISNULL(M.Cofins_Perc_Entrada , 0) > 0 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (ISNULL(M.Cofins_Perc_Entrada , 0) / 100)) ELSE 0 END END),
		VL_PIS_ST = CONVERT(FLOAT,0),
		VL_COFINS_ST = CONVERT(FLOAT,0)
	FROM
		NF a INNER JOIN NF_Item b ON 
					a.FILIAL = b.FILIAL 
					AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR 
					AND a.Codigo = b.Codigo		
		INNER JOIN Mercadoria M ON b.PLU = m.PLU
		LEFT OUTER JOIN Tributacao c ON c.Codigo_Tributacao = b.Codigo_Tributacao --AND c.Filial = b.Filial

	WHERE
		a.Filial = @Filial
		AND a.Data BETWEEN @DataInicio AND @DataFim
		AND a.Tipo_NF = '2'
	GROUP BY 
		a.id,
		a.Cliente_Fornecedor,
		a.Tipo_NF,
		a.Desconto,
		a.Codigo,
		a.Emissao,
		a.Data,
		a.Base_Calculo,
		a.ICMS_Nota,
		a.Producao_NFe,
		a.status
 Union All
	SELECT 
		REG = 'C100',
		IND_OPER = '1',
		IND_EMIT = CASE WHEN a.Tipo_NF = '2' THEN '1'ELSE '0' END ,
		COD_PART = a.Cliente_Fornecedor,
		COD_MOD = '55',
		COD_SIT = '00',
		SER = case when Substring(a.id,23,3) = '' then '001' else Substring(a.id,23,3) end, --,'001',		
		NUM_DOC = a.Codigo,
		CHV_NFE = case when a.id is null then isnull(dbo.funChave('35',substring(replace(convert(varchar,a.Emissao,2),'.',''),1,4),replace(replace(replace(f.cnpj,'.',''),'/',''),'-',''),'55',replicate('0', 3 - len(rtrim(ltrim(convert(varchar(3), f.serie_nfe))))) + rtrim(ltrim(convert(varchar(3), f.serie_nfe))), rtrim(ltrim(a.codigo)),'1', f.chave_xml),'') when a.id = '' then isnull(dbo.funChave('35',substring(replace(convert(varchar,a.Emissao,2),'.',''),1,4),replace(replace(replace(f.cnpj,'.',''),'/',''),'-',''),'55',replicate('0', 3 - len(rtrim(ltrim(convert(varchar(3), f.serie_nfe))))) + rtrim(ltrim(convert(varchar(3), f.serie_nfe))), rtrim(ltrim(a.codigo)),'1', f.chave_xml),'')  else a.id end ,
		DT_DOC = a.Emissao,
		DT_E_S = a.Data,
		VL_DOC = SUM(b.Total + ISNULL(b.IPIV,0) + ISNULL(b.IVA,0) + ISNULL(b.Despesas,0)),
		IND_PGTO = '0',
		VL_DESC = a.desconto,
		VL_ABAT_NF = CONVERT(FLOAT,0),
		VL_MERC = SUM(b.TOTAL),
		IND_FRT = '1',
		VL_FRT = SUM(ISNULL(a.Frete,0)),
		VL_SEG = CONVERT(FLOAT,0),
		VL_OUT_DA = SUM(ISNULL(b.Despesas,0)),
		--VL_BC_ICMS = (ISNULL(a.Base_Calculo,0)),
		--VL_ICMS = (ISNULL(a.ICMS_Nota,0)),
		VL_BC_ICMS = SUM(CASE WHEN c.Indice_ST = '00' OR c.Indice_ST = '20' THEN  (b.Total * (1 - (ISNULL(b.Redutor_Base,c.redutor) / 100))) ELSE 0 End),
		VL_ICMS = SUM(CASE WHEN c.Indice_ST = '00' OR c.Indice_ST = '20' THEN  (b.Total * (1 - (ISNULL(b.Redutor_Base,c.Redutor) / 100))) ELSE 0 End * (case when a.Tipo_NF = 1 then c.Saida_ICMS else b.Aliquota_ICMS end / 100)),
		VL_BC_ICMS_ST = SUM(ISNULL(b.Base_IVA,0)),
		VL_ICMS_ST = SUM(ISNULL(b.IVA,0)),
		VL_IPI = SUM(ISNULL(b.IPIV,0)),
		--VL_PIS = SUM(ISNULL(b.PISV,0)),
		--VL_COFINS = SUM(ISNULL(b.CofinsV,0)),
		VL_PIS = SUM(CASE WHEN CONVERT(INT, M.cst_saida) = 1 AND ISNULL(M.Pis_Perc_Saida , 0) > 0 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (ISNULL(M.Pis_Perc_Saida , 0) / 100)) ELSE 0 END),
		VL_COFINS = SUM(CASE WHEN CONVERT(INT, M.cst_saida) = 1 AND ISNULL(M.Pis_Perc_Saida , 0) > 0 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (ISNULL(M.Cofins_Perc_Saida  , 0) / 100)) ELSE 0 END),
		
		--VL_PIS = SUM(CASE WHEN m.incide_pis = 1 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (0.0165)) ELSE 0 END),
		--VL_COFINS = SUM(CASE WHEN m.incide_pis = 1 THEN convert(decimal(18,2),convert(decimal(18,2),b.Total) * (0.076)) ELSE 0 END),
		VL_PIS_ST = CONVERT(FLOAT,0),
		VL_COFINS_ST = CONVERT(FLOAT,0)
	FROM
		NF a INNER JOIN NF_Item b ON 	
					a.FILIAL = b.FILIAL 
					AND a.CLIENTE_FORNECEDOR = b.CLIENTE_FORNECEDOR 
					AND a.Codigo = b.Codigo		
		INNER JOIN Mercadoria M ON b.PLU = m.PLU
		INNER JOIN Filial f on a.filial = f.filial
		LEFT OUTER JOIN Tributacao c ON c.Codigo_Tributacao = b.Codigo_Tributacao --AND c.Filial = b.Filial
	WHERE
		a.Filial = @Filial
		--AND a.Cliente_Fornecedor = @Fornecedor
		AND a.Data BETWEEN @DataInicio AND @DataFim
		AND a.Tipo_NF = '1'
		And a.codigo_operacao not in (5929, 6929)
		AND isnull(a.NF_Canc,0) <> 1
	GROUP BY 
		a.Cliente_Fornecedor,
		a.Tipo_NF,
		a.Desconto,
		a.Codigo,
		a.Emissao,
		a.Data,
		a.Base_Calculo,
		a.ICMS_Nota,
		f.cnpj,
		f.chave_xml,
		f.serie_nfe,
		a.id

	ORDER BY 8
	RETURN                          
                      
END


go 
--PROCEDURES =======================================================================================
ALTER        PROC [dbo].[sp_EFD_PisCofins_0400]
	@FILIAL VARCHAR(20),
	@DATAINI VARCHAR(8),
	@DATAFIM VARCHAR (8)
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
		and (b.Tipo_NF=2 and(CASE WHEN a.Tipo_NF = '2' THEN CASE WHEN  ISNULL(a.Producao_NFe, 0) = 1 AND a.status = 'AUTORIZADO' THEN '0' ELSE '1' END ELSE '0' END )=1 )
		)
		

go
	insert into Versoes_Atualizadas select 'Versão:1.239.746', getdate();
GO
