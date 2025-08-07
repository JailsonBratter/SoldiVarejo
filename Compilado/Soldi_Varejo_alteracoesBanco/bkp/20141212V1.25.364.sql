
CREATE PROCEDURE SP_REL_FLUXO_CAIXA_SIMPLIFICADO @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10),@STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo_simples]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo_simples;
select A.DATA,A.dia_da_Semana,[RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo_simples FROM (

select 
		Data = vencimento,
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber = 0,
		Pagar = sum(isnull(valor,0))
		
	from conta_a_pagar 
	where 
		vencimento between @datade and @dataate AND (Status= (CASE WHEN @STATUS='ABERTO' THEN 1
																 WHEN @STATUS='CONCLUIDO' THEN 2
																 WHEN @STATUS='CANCELADO' THEN 3
																 WHEN @STATUS='LANCADO' THEN 4
																ELSE 0 END
																) )
																and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
																						  when LEN (@CLIENTE)>0 then ''
																						  	else '%' 
																							end)
		group by Vencimento
union 		
		
select 
		Data = vencimento,
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber =sum(isnull(valor,0)),
		Pagar = 0
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where 
		(vencimento between @datade and @dataate ) AND (Status= (CASE WHEN @STATUS='ABERTO' THEN 1
																 WHEN @STATUS='CONCLUIDO' THEN 2
																 WHEN @STATUS='CANCELADO' THEN 3
																 WHEN @STATUS='LANCADO' THEN 4
																ELSE 0 END
																)
																)
																and( Cliente.Nome_Cliente like case when LEN(@CLIENTE)>0 then @CLIENTE
																								 when LEN(@FORNECEDOR)>0 then ''	
																							else '%' 
																							end
																							)
		group by Vencimento
)A GROUP BY DATA,Dia_da_Semana
		 
		
		
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO_GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo_simples B WHERE B.Data <A.Data)+A.SALDO_DIA) from #tmp_Fluxo_simples A ORDER BY CONVERT(VARCHAR,A.DATA,102)  ;
			  
END
go 


CREATE PROCEDURE SP_REL_FLUXO_CAIXA @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10),@STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo;

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo1]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo1;

   select A.DATA,A.dia_da_Semana,FORNEC_CLIENTE, [RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo FROM (

select 
		Data = vencimento,
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,
								
		FORNEC_CLIENTE= Fornecedor,
		Receber = 0,
		Pagar = sum(isnull(valor,0))
		
	from conta_a_pagar 
	where filial=@FILIAL and 
		vencimento between @datade and @dataate AND (Status= (CASE WHEN @STATUS='ABERTO' THEN 1
																 WHEN @STATUS='CONCLUIDO' THEN 2
																 WHEN @STATUS='CANCELADO' THEN 3
																 WHEN @STATUS='LANCADO' THEN 4
																ELSE 0 END
																) )
																and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
																						  when LEN (@CLIENTE)>0 then ''
																						  	else '%' 
																							end)
		group by Vencimento,conta_a_pagar.Fornecedor 
union 		
		
select 
		Data = vencimento,
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,
		FORNEC_CLIENTE= Cliente.Nome_Cliente,						
		Receber =sum(isnull(valor,0)),
		Pagar = 0
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where filial=@FILIAL and 
		(vencimento between @datade and @dataate ) AND (Status= (CASE WHEN @STATUS='ABERTO' THEN 1
																 WHEN @STATUS='CONCLUIDO' THEN 2
																 WHEN @STATUS='CANCELADO' THEN 3
																 WHEN @STATUS='LANCADO' THEN 4
																ELSE 0 END
																)
																)
																and( Cliente.Nome_Cliente like case when LEN(@CLIENTE)>0 then @CLIENTE
																								 when LEN(@FORNECEDOR)>0 then ''	
																							else '%' 
																							end
																							)
		group by Vencimento,Cliente.Nome_Cliente
)A GROUP BY DATA,Dia_da_Semana,FORNEC_CLIENTE

create table #tmp_Fluxo1 (ID INT IDENTITY, DATA DATETIME,DIA_DA_SEMANA VARCHAR(20),FORNEC_CLIENTE VARCHAR(50),RECEBER DECIMAL(12,2) ,PAGAR DECIMAL(12,2) , SALDO_DIA DECIMAL(12,2) )

INSERT INTO #tmp_Fluxo1 		
				select * from #tmp_Fluxo 
		 
		
	--select * from 	#tmp_Fluxo1
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [FORNEC/CLIENTE]=FORNEC_CLIENTE,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo1 B WHERE b.ID <a.ID)+A.SALDO_DIA) from #tmp_Fluxo1 A ORDER BY CONVERT(VARCHAR,A.DATA,102)  
			  
end



GO

CREATE Procedure [dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,vencimento,103),
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Saidas = sum(isnull(valor,0))
		
	from conta_a_pagar 
	where 
		vencimento between @datade and @dataate
	Group by
		Vencimento
	ORDER BY CONVERT(VARCHAR,VENCIMENTO,102)

End




GO 


CREATE Procedure [dbo].[sp_REL_CONTAS_A_RECEBER_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,vencimento,103),
		Dia_da_Semana = case 
								when datepart(dw, vencimento) = 2 then 'Segunda'
								when datepart(dw, vencimento) = 3 then 'Terça'
								when datepart(dw, vencimento) = 4 then 'Quarta'
								when datepart(dw, vencimento) = 5 then 'Quinta'
								when datepart(dw, vencimento) = 6 then 'Sexta'
								when datepart(dw, vencimento) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Saidas = sum(isnull(valor,0))
		
	from Conta_a_receber 
	where 
		vencimento between @datade and @dataate
	Group by
		Vencimento
	ORDER BY CONVERT(VARCHAR,VENCIMENTO,102)
End



