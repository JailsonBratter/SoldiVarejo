
ALTER Procedure [dbo].[sp_REL_CONTAS_A_RECEBER_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10),@tipo varchar(10),@status varchar(10),@cliente varchar(20)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,103),
		Dia_da_Semana = case 
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Entradas = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where Filial=@filial and 
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate
				AND (Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
																 WHEN @STATUS='CONCLUIDO' THEN '2'
																 WHEN @STATUS='CANCELADO' THEN '3'
																 WHEN @STATUS='LANCADO' THEN '4'
																ELSE '%' END
																) )
																and( isnull(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
																								
																							else '%' 
																							end
																							)

	Group by
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end
	ORDER BY CONVERT(VARCHAR,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end,102)
End

---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================



/****** Object:  StoredProcedure [dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO]    Script Date: 12/17/2014 15:23:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER Procedure [dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO] @filial varchar(20), @datade varchar(10), @dataate varchar(10),@tipo varchar(10),@status varchar(10),@fornecedor varchar(20)
as
--sp_pes_contas_a_pagar 'matriz', '20121218', '20121218'
	Begin
	select 
		Data = convert(varchar,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,103),
		Dia_da_Semana = case 
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 7 then 'Sabado'
								Else 'Domingo' end,

								 
		--Entradas = convert(numeric(18,2),0),
	--	Qtde = 0,
		Saidas = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from conta_a_pagar 
	where Filial=@filial and 
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate
				AND (Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
																 WHEN @STATUS='CONCLUIDO' THEN '2'
																 WHEN @STATUS='CANCELADO' THEN '3'
																 WHEN @STATUS='LANCADO' THEN '4'
																ELSE '%' END
																) )
				and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
						  	else '%'
						  	end)
	Group by
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end
	ORDER BY CONVERT(VARCHAR,case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
		end,102)
End


---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================



/****** Object:  StoredProcedure [dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO]    Script Date: 12/17/2014 15:24:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA_SIMPLIFICADO] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10), @tipo varchar(10), @STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
as
BEGIN
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[#tmp_Fluxo_simples]') AND type in (N'U')) 
	DROP TABLE #tmp_Fluxo_simples;
select A.DATA,A.dia_da_Semana,[RECEBER] =SUM(A.Receber),[PAGAR]=SUM(A.Pagar),[SALDO_DIA] =SUM( (A.Receber-A.Pagar)) into #tmp_Fluxo_simples FROM (

select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
		,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber = 0,
		Pagar = convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0))))
		
	from conta_a_pagar 
	where Filial =@FILIAL and
		case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
				between @datade and @dataate 
		
		
		
		AND (Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
																 WHEN @STATUS='CONCLUIDO' THEN '2'
																 WHEN @STATUS='CANCELADO' THEN '3'
																 WHEN @STATUS='LANCADO' THEN '4'
																ELSE '%' END
																) )
																and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
																						  when LEN (@CLIENTE)>0 then ''
																						  	else '%' 
																							end)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
union 		
		
select 
		Data = case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end,
		Dia_da_Semana = case 
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 2 then 'Segunda'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 3 then 'Terça'
								when datepart(dw, case  when @tipo='vencimento' then  vencimento
														when @tipo='emissao' then  emissao
														when @tipo='entrada' then Entrada
													end) = 4 then 'Quarta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 5 then 'Quinta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 6 then 'Sexta'
								when datepart(dw, case when @tipo='vencimento' then  vencimento
													when @tipo='emissao' then  emissao
													when @tipo='entrada' then Entrada
												end) = 7 then 'Sabado'
								Else 'Domingo' end,
		Receber =convert(Decimal(12,2),((sum(isnull(convert(Decimal(12,2),valor),0))+sum(isnull(convert(Decimal(12,2),acrescimo),0)))-sum(isnull(convert(Decimal(12,2),Desconto),0)))),
		Pagar = 0
		
	from Conta_a_receber LEFT JOIN Cliente ON Conta_a_receber.Codigo_Cliente= Cliente.Codigo_Cliente
	where Filial = @FILIAL and  case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end between @datade and @dataate  
				
				AND (Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
																 WHEN @STATUS='CONCLUIDO' THEN '2'
																 WHEN @STATUS='CANCELADO' THEN '3'
																 WHEN @STATUS='LANCADO' THEN '4'
																ELSE '%' END
																) )
																and( isnull(Cliente.Nome_Cliente,'') like case when LEN(@CLIENTE)>0 then @CLIENTE
																								 when LEN(@FORNECEDOR)>0 then ''	
																							else '%' 
																							end
																							)
		group by case when @tipo='vencimento' then  vencimento
					when @tipo='emissao' then  emissao
					when @tipo='entrada' then Entrada
				end
)A GROUP BY DATA,Dia_da_Semana
		 
		
		
	--	SELECT * FROM #tmp_Fluxo_simples
		select DATA = CONVERT(VARCHAR,A.Data,103),
			  [Dia da Semana]= A.Dia_da_Semana,
			  [A RECEBER]=A.RECEBER,
			  [A PAGAR]=A.PAGAR,
			  [SALDO DIA]=A.SALDO_DIA,
			  [SALDO_GERAL] = ((SELECT ISNULL(SUM(SALDO_DIA),0) FROM #tmp_Fluxo_simples B WHERE B.Data <A.Data)+A.SALDO_DIA) from #tmp_Fluxo_simples A ORDER BY CONVERT(VARCHAR,A.DATA,102)  ;
			  
END






---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[SP_REL_FLUXO_CAIXA] @FILIAL VARCHAR(20), @datade varchar(10), @dataate varchar(10),@STATUS VARCHAR(10),@CLIENTE VARCHAR(50),@FORNECEDOR VARCHAR(20)
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



go


--===========================================================
--===========================================================
--===========================================================
create PROCEDURE VENDAS_POR_GRUPO @FILIAL VARCHAR(20),@datade varchar(10), @dataate varchar(10),@grupo varchar(20)
as
begin 
	-- exec VENDAS_POR_GRUPO 'MATRIZ','20141001','20141001','ACOUGUE'
declare @total decimal(12,2)	

if len(@grupo)>0 
	begin
	
		select @total = SUM (vlr) from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and (a.Data_movimento between @datade and @dataate) and c.Descricao_grupo = @grupo				
		
		select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(a.vlr),[%]=CONVERT(DECIMAL(12,2),((sum(a.vlr)/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and(a.Data_movimento between @datade and @dataate )and c.Descricao_grupo = @grupo				
		group by c.codigo_departamento,c.descricao_departamento

	end
else
begin

		select @total = SUM (vlr) from Saida_estoque where Filial=@FILIAL and Data_movimento between @datade and @dataate
		select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = sum(a.vlr),[%]=CONVERT(DECIMAL(12,2),((sum(a.vlr)/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and a.Data_movimento between @datade and @dataate				
		group by c.codigo_grupo,c.Descricao_grupo
end
end



--===========================================================
--===========================================================
--===========================================================

alter table fornecedor_mercadoria alter column embalagem numeric(6)
go

alter table mercadoria alter column embalagem numeric(6)
go 

alter table nf_item alter column ipi numeric(12,4)




