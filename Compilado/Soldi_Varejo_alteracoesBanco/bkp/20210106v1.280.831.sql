

CREATE TABLE [dbo].[transferencias_contas](
	[id] [int] NOT NULL,
	[data] [datetime] NOT NULL,
	[filial] [varchar](20) NOT NULL,
	[usuario] [varchar](30) NOT NULL,
	[tipo] [varchar](20) NOT NULL,
	[conta_origem] [varchar](20) NOT NULL,
	[conta_destino] [varchar](20) NULL,
	[valor] [numeric](18, 2) NOT NULL,
	[descricao] [varchar](50) NULL,
	[obs] [nvarchar](max) NULL,
	[saldo_ant_origem] [numeric](18, 2) NULL,
	[saldo_ant_destino] [numeric](18, 2) NULL,
	[status] [varchar](40) NULL,
	[codigo_centro_custo] [varchar](10) NULL,
	[codigo_centro_custo_origem] [varchar](10) NULL,
	[codigo_centro_custo_destino] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[historico_mov_conta](
	[filial] [varchar](20) NULL,
	[data] [datetime] NULL,
	[Pagamento] [datetime] NULL,
	[id_cc] [varchar](20) NULL,
	[origem] [varchar](40) NULL,
	[cliente_fornecedor] [varchar](70) NULL,
	[documento] [varchar](20) NULL,
	[forma_pg] [varchar](50) NULL,
	[operacao] [varchar](40) NULL,
	[vlr] [numeric](12, 2) NULL,
	[usuario] [varchar](40) NULL,
	[estornado] [tinyint] NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[saldo_dia_conta](
	[FILIAL] [varchar](20) NULL,
	[data] [datetime] NULL,
	[id_cc] [varchar](20) NULL,
	[vlr_inicio] [decimal](12, 2) NULL,
	[total_Entrada] [numeric](12, 2) NULL,
	[total_saida] [numeric](12, 2) NULL,
	[vlr_final] [numeric](12, 2) NULL
) ON [PRIMARY]
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_CORRIGI_HIST_BANCARIO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_CORRIGI_HIST_BANCARIO]
end

GO

CREATE PROCEDURE [dbo].[SP_CORRIGI_HIST_BANCARIO] (
	@FILIAL VARCHAR(20),
	@BANCO VARCHAR(30),
	@P_DIA DATE,
	@P_FIM DATE= NULL
)
AS 
BEGIN

-- EXEC SP_CORRIGI_HIST_BANCARIO 'MW', 'INVISTA FIDC','20171102';
DECLARE @DIA DATE  ,@DIA_ANT DATE 
declare @SALDO_DIA NUMERIC(18,2),@SALDO_ANTERIOR NUMERIC(18,2),@SALDO_ATUAL NUMERIC(18,2)
DECLARE @T_ENTRADA NUMERIC(18,2), @T_SAIDA NUMERIC(18,2), @VLR_INICIO NUMERIC(18,2), @VLR_FINAL NUMERIC(18,2)
DECLARE @ULT_FECHAMENTO DATE


--IF(@P_FIM IS NULL)
--BEGIN

--	SELECT @ULT_FECHAMENTO = isnull(Max(mes_fechado),'2017-10-02') from fechamento_financeiro WHERE FILIAL = @FILIAL AND ISNULL(EXCLUIDO,0)=0
--	SET @ULT_FECHAMENTO  = DATEADD(MONTH,1,@ULT_FECHAMENTO);
--	--print(@ULT_FECHAMENTO)
--	--print(@P_DIA)
--	IF(@P_DIA < @ULT_FECHAMENTO)
--	BEGIN 
--		declare @msgErro varchar(50);
--		set @msgErro = 'MES '+ convert(varchar(2),datepart(m,@P_dia))+' ESTA FECHADO';
--		 RAISERROR (@msgErro, -- Message text.  
--				   16, -- Severity.  
--				   1 -- State.  
--				   );   
--				   RETURN 0;
--	END


--END



	 
     	--PRINT('IF ENTROU:');
		--SELECT @SALDO_ATUAL = SALDO FROM CONTA_CORRENTE WHERE ID_CC=@BANCO

		IF(@P_DIA <= '20171101')
		BEGIN
		 SET @P_DIA = '20171102'
		END 

		SET @P_FIM = ISNULL(@P_FIM,GETDATE());
		SET @DIA =@P_DIA;
		SET @DIA_ANT = DATEADD(DAY,-1,@DIA);

		

		delete from saldo_dia_conta where id_cc=@banco AND data <> '20171101' AND  data BETWEEN @P_DIA AND @P_FIM
		
		
		print (@dia)
		print(@p_fim)
		
		while (@dia<= @P_FIM)
		begin
			--PRINT('CTA:'+@BANCO +'- DIA:'+CONVERT(VARCHAR,@DIA)); 
			
			Select @VLR_INICIO =isnull(vlr_final,0) FROM SALDO_DIA_CONTA WHERE DATA= @DIA_ANT AND ID_CC=@BANCO 	
			
				--
				--PRINT('FILIAL '+CONVERT(VARCHAR,@FILIAL))
				--PRINT('BANCO '+CONVERT(VARCHAR,@BANCO))
				
				
				
				SELECT @T_ENTRADA= ISNULL(SUM(vlr),0) 
				FROM historico_mov_conta WHERE  convert(date,pagamento) = @DIA AND ID_CC=@BANCO
				and (
						 ( origem = 'CONTA_A_RECEBER' AND operacao='BAIXA')
						 or (origem = 'CONTA_A_PAGAR' AND operacao='ESTORNO')
						 or (origem = 'TRANSFERENCIA' AND forma_pg='CREDITO')
				)and isnull(estornado,0) = 0
				
				
				
				SELECT @T_SAIDA =ISNULL(SUM(vlr),0) 
				FROM historico_mov_conta WHERE pagamento = @DIA AND ID_CC=@BANCO
				and 
					(
							( origem = 'CONTA_A_PAGAR' AND operacao='BAIXA')
						 or ( origem = 'CONTA_A_RECEBER' AND operacao='ESTORNO')
						 or (origem = 'TRANSFERENCIA' AND forma_pg='DEBITO')
					 )
				and isnull(estornado,0) = 0

				
				
					SET @VLR_FINAL = ISNULL(@VLR_INICIO,0)+(ISNULL(@T_ENTRADA,0)- ISNULL(@T_SAIDA,0)); 
				
				--PRINT ('INICIO :' + CONVERT(VARCHAR,@VLR_INICIO ) )
				--PRINT ('ENTRADA :'+ CONVERT(VARCHAR,@T_ENTRADA ) )
				--PRINT ('SAIDA: ' +CONVERT(VARCHAR,@T_SAIDA ))
				--PRINT ('FIM: ' +CONVERT(VARCHAR,@VLR_FINAL ))
				INSERT INTO SALDO_DIA_CONTA
						VALUES(@FILIAL, @DIA,@BANCO,ISNULL(@VLR_INICIO,0),ISNULL(@T_ENTRADA,0),ISNULL(@T_SAIDA,0),ISNULL(@VLR_FINAL,0));
						
		
			SET @SALDO_DIA = NULL;
			SET @T_ENTRADA = NULL;
			SET @T_SAIDA = NULL;
			SET @VLR_INICIO = NULL;
			--SET @VLR_FINAL = NULL;
			
			SET @DIA_ANT = @DIA;
			SET @DIA= DATEADD(DAY,1,@DIA);

			IF(@DIA = CONVERT(DATE,GETDATE()))
			BEGIN
				UPDATE Conta_Corrente SET saldo = @VLR_FINAL WHERE id_cc = @BANCO AND FILIAL =@FILIAL
			END
		
	end 


END 

go 
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_corrente') 
            AND  UPPER(COLUMN_NAME) = UPPER('codigo_centro_custo'))
begin
	alter table conta_corrente alter column codigo_centro_custo varchar(9);
end
else
begin
	alter table conta_corrente add codigo_centro_custo varchar(9)
end 
go 




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_LANCAMENTOS_CONTA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_LANCAMENTOS_CONTA]
end

GO

--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_LANCAMENTOS_CONTA] 
						@FILIAL VARCHAR(20),
						@datade Date, 
						@dataate Date,
						@CLIENTE VARCHAR(50),
						@ID_CONTA VARCHAR(20),
						@lancamento varchar(20),
						@estorno  varchar(5)
as
BEGIN
	-- EXEC SP_REL_LANCAMENTOS_CONTA 'MW','2018-03-20' ,'2018-03-20','','BRADESCO','TODOS','NAO'

	-- Select * from historico_mov_conta where CONVERT(date, DATA) between '20170524' and '20170524' 
	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo]') AND type in (N'U')) 
		DROP TABLE #tmp_Fluxo;

	IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo1]') AND type in (N'U')) 
		DROP TABLE #tmp_Fluxo1;

	   select A.DATA,A.dia_da_Semana,documento,FORNEC_CLIENTE,Pagamento,Operacao, [RECEBER] =A.Receber,[PAGAR]=A.Pagar,[SALDO_DIA] = (A.Receber-A.Pagar) 
	   into #tmp_Fluxo
	   FROM (
   
		select 
				Data ,
				Dia_da_Semana = case 
										when datepart(dw, DATA) = 2 then 'Segunda'
										when datepart(dw, DATA) = 3 then 'Terça'
										when datepart(dw, DATA) = 4 then 'Quarta'
										when datepart(dw, DATA) = 5 then 'Quinta'
										when datepart(dw, DATA) = 6 then 'Sexta'
										when datepart(dw, DATA) = 7 then 'Sabado'
										Else 'Domingo' end,
				documento,
									
				FORNEC_CLIENTE= cliente_fornecedor,
				Pagamento,
				operacao,
				Receber = 0,
				Pagar = Isnull(Vlr,0)
			
			from historico_mov_conta 
			where (@lancamento='TODOS' OR @lancamento='DEBITO') AND  
				CONVERT(date, PAGAMENTO) between @datade and @dataate 
				and(LEN (@CLIENTE)=0 OR cliente_fornecedor =@CLIENTE)
				AND id_cc=@ID_CONTA
				and
					 (
					 ( origem = 'CONTA_A_PAGAR' AND operacao='BAIXA')
					 or (origem = 'CONTA_A_RECEBER' AND operacao='ESTORNO')
					 or (origem='TRANSFERENCIA' AND forma_pg='DEBITO')
					 )
				and (@estorno ='SIM' OR isnull(estornado,0) =0)
			
		union 		
			
		select 
				Data ,
				Dia_da_Semana = case 
										when datepart(dw, DATA) = 2 then 'Segunda'
										when datepart(dw, DATA) = 3 then 'Terça'
										when datepart(dw, DATA) = 4 then 'Quarta'
										when datepart(dw, DATA) = 5 then 'Quinta'
										when datepart(dw, DATA) = 6 then 'Sexta'
										when datepart(dw, DATA) = 7 then 'Sabado'
										Else 'Domingo' end,
				documento,						
				FORNEC_CLIENTE= cliente_fornecedor,
				Pagamento,
				operacao,
				Receber =Isnull(Vlr,0) ,
				Pagar = 0
			
			from historico_mov_conta 
			where( @lancamento='TODOS' OR @lancamento='CREDITO') AND   
			CONVERT(date, PAGAMENTO) between @datade and @dataate 
				and(LEN (@CLIENTE)=0 OR cliente_fornecedor =@CLIENTE)
				AND id_cc=@ID_CONTA
				and
					 (
					 ( origem = 'CONTA_A_RECEBER' AND operacao='BAIXA')
					 or (origem = 'CONTA_A_PAGAR' AND operacao='ESTORNO')
					 or (origem='TRANSFERENCIA' AND forma_pg='CREDITO')
					 )
				and (@estorno ='SIM' OR isnull(estornado,0) =0)
		)A 

	create table #tmp_Fluxo1 (
			ID INT IDENTITY, 
			DATA DATETIME,
			DIA_DA_SEMANA VARCHAR(20),
			Documento varchar(40),
			FORNEC_CLIENTE VARCHAR(60),
			PAGAMENTO DATETIME,
			OPERACAO VARCHAR(40),
			RECEBER DECIMAL(12,2) ,
			PAGAR DECIMAL(12,2) , 
			SALDO_DIA DECIMAL(12,2) )


	INSERT INTO #tmp_Fluxo1
	select DATA ,
		Dia_da_Semana = case 
							when datepart(dw, DATA) = 2 then 'Segunda'
							when datepart(dw, DATA) = 3 then 'Terça'
							when datepart(dw, DATA) = 4 then 'Quarta'
							when datepart(dw, DATA) = 5 then 'Quinta'
							when datepart(dw, DATA) = 6 then 'Sexta'
							when datepart(dw, DATA) = 7 then 'Sabado'
							Else 'Domingo' end,
				DOCUMENTO ='',
				FORNEC_CLIENTE='|-SUB-|SALDO INICIAL',
				PAGAMENTO =NULL,
				OPERACAO = '',
				Receber = NULL,
				pagar = NULL ,
			vlr_inicio 
		FROM saldo_dia_conta
		WHERE id_cc = @ID_CONTA   AND CONVERT(DATE,DATA) =@datade 


	INSERT INTO #tmp_Fluxo1 		
					select * from #tmp_Fluxo order by convert(varchar,pagamento,102)
		 
		
			select DATA = CONVERT(VARCHAR,A.Data,103),
				  [Dia da Semana]= A.Dia_da_Semana,
				  DOCUMENTO,
				  [DETALHES]=ISNULL(C.NOME_CLIENTE,FORNEC_CLIENTE),
				  [PAGAMENTO]= CONVERT(VARCHAR,Pagamento,103),
				  OPERACAO,
				  [CREDITO]=A.RECEBER,
				  [DEBITO]=A.PAGAR,
			
				  [SALDO GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo1 B WHERE b.ID <a.ID)+A.SALDO_DIA) 
		from #tmp_Fluxo1 A 
		LEFT JOIN Cliente AS C ON A.FORNEC_CLIENTE COLLATE Latin1_General_CI_AS = C.Codigo_Cliente COLLATE Latin1_General_CI_AS
	
		ORDER BY CONVERT(VARCHAR,A.PAGAMENTO,102), a.ID  
			  
end




insert into Versoes_Atualizadas select 'Versão:1.280.831', getdate();
