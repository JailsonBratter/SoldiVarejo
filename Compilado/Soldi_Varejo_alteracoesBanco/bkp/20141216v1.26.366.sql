
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================

ALTER Procedure [dbo].[sp_rel_Resumo_Vendas_Pdv] 
		@Filial		As Varchar(20),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@PDV		AS VARCHAR(10)

AS
	Declare @String	As nVarchar(1024)
begin
	set @String= '	SELECT '
	
	SET @String = @String + '	convert(varchar,Data_Movimento,103) as Data,'
	Set @String = @String + 'Dia_da_Semana = case 
								when datepart(dw, Data_Movimento) = 2 then '+ CHAR(39) +'Segunda'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 3 then '+ CHAR(39) +'Terça'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 4 then '+ CHAR(39) +'Quarta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 5 then '+ CHAR(39) +'Quinta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 6 then '+ CHAR(39) +'Sexta'+ CHAR(39) +
								'when datepart(dw, Data_Movimento) = 7 then '+ CHAR(39) +'Sabado'+ CHAR(39) +
								'Else '+ CHAR(39) +'Domingo'+ CHAR(39) +' end,'
	SET @String = @String + '	Caixa_Saida as PDV, '
	SET @String = @String + '	Valor = SUM(Vlr-isnull(Desconto,0)+isnull(acrescimo,0))'
		
	SET @String = @String + ' FROM '
	SET @String = @String + '	Saida_Estoque '
	SET @String = @String + ' WHERE '
	SET @String = @String + '	Filial = '+ CHAR(39) +@Filial+ CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Movimento BETWEEN '+ CHAR(39) + @DataDe + CHAR(39) +' AND '+ CHAR(39) +@DataAte + CHAR(39) 
	SET @String = @String + ' AND '
	SET @String = @String + '	Data_Cancelamento IS NULL ' 
	begin 
		IF LEN(ISNULL(@PDV,'')) > 0 
			SET @String = @String + ' and CAIXA_SAIDA IN (' +@PDV+ ')'
		
	end	
				
	SET @String = @String + 'GROUP BY Filial,Data_Movimento, Caixa_Saida'
	EXECUTE(@String)
end



---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================

go
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
	where
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
	where case when @tipo='vencimento' then  vencimento
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


go



ALTER  procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_pagar.Filial = '+ char(39) + @filial + char(39) + ' and '
	set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
	--Verifica se o Parametro @fornecedor tem conteudo
	if len(rtrim(ltrim(@Fornecedor))) > 1
		Begin
			set @where = @where + ' And fornecedor in (' + char(39) +replace(@fornecedor,',', char(39)+ ', '+char(39))+ char(39) + ')'
		End
	--Monta Select
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
																ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
																) 
	end
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Duplicata= case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(fornecedor)), 
			Obs ,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0)					
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
		
		'+@where+'  Order By convert(varchar ,'+@tipo	+',102) '--'+@where+'  Order By '+ @Tipo + ', Fornecedor, Documento '
	
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
		--set @string = @string + ' Order By '+ @Tipo + ', Fornecedor, Documento'
	Print @string
	Exec(@string)
End

---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================

go 

ALTER  procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250), 
	@status	   varchar(10)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
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
																ELSE 'status like '+CHAR(39)+'%'+CHAR(39) END
																) 
	end
		
	--Monta Select
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Cliente = rtrim(ltrim(Codigo_Cliente)), 
			Obs ,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = convert(numeric,Isnull(Desconto,0)),
			Acrescimo = convert(numeric,Isnull(Acrescimo,0)),
			ValorReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0)					
		from dbo.Conta_a_receber Conta_a_receber  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_receber.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_receber.Filial = Centro_custo.filial
		
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
			Print @string
	Exec(@string)
End



---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================

go

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
	where 
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

go

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
	where 
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

go


ALTER           PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(8),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60)
            )
as
	
if len(@Caixa)= 0
	begin
		set @Caixa = '%'
	end
if LEN(@Grupo)=0
	begin
		set @Grupo ='%'
	end
if LEN(@subGrupo)=0
	begin
		set @subGrupo = '%'
	end
if LEN(@Departamento)=0
	begin 
		set @Departamento = '%'
	end
if LEN(@Familia)=0
	begin
		set @Familia = '%'
	end


select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,Sum(vlr) as Valor 
from saida_estoque a inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 

where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 	Data_movimento <=@Dataate and isnull(caixa_saida,'') like @Caixa
	and c.Descricao_grupo like @Grupo
	and c.descricao_subgrupo like @subGrupo
	and c.descricao_departamento like @Departamento
	and isnull(f.Descricao_Familia,'') like @Familia
	
group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
order by b.descricao




