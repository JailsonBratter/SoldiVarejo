
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_conta_a_pagar]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_conta_a_pagar]
end
GO
--PROCEDURES =======================================================================================
CREATE    procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10),
	@tipoPagamento varchar(50)
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
	if(@Conferido<>'TODOS')
	begin
		IF(@Conferido='SIM')
		BEGIN
			set @where = @where + ' And conferido =1 '	
		END
		ELSE
		BEGIN
			set @where = @where + ' And ISNULL(conferido,0) =0 '	
		END
		 
	end 
	
	set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' status =1'
											 WHEN @STATUS='CONCLUIDO' THEN ' status =2'
											 WHEN @STATUS='CANCELADO' THEN ' status =3'
											 WHEN @STATUS='LANCADO' THEN ' status =4'
										ELSE 'status <> 3' END
																) 
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
	end
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
	
	if(@tipoPagamento <> 'TODOS')
	BEGIN
		set @Where = @Where + ' and Conta_a_pagar.TIPO_PAGAMENTO= '+ char(39)+ @tipoPagamento+ char(39) 
	END
	
	--Fornecedor.CNPJ ,
	
	set @string = 'select 
			convert(varchar ,emissao,103) as  Emissao, 
			convert(varchar ,entrada,103) as  Entrada, 
			convert(varchar ,vencimento,103) as  Vencimento, 
			Documento = rtrim(ltrim(documento)), 
			Dupl = case when duplicata= 1 then ' + char(39) +'Sim' + char(39) +' else  ' + char(39) +'Nao' + char(39) +' end,
			Fornecedor = rtrim(ltrim(conta_a_pagar.Fornecedor)) , 
			
			[Tipo pag]=tipo_pagamento,
			Prazo = DATEDIFF(DAY,GETDATE(), vencimento ) ,
			Valor = Isnull(Valor,0),
			Desconto = Isnull(Desconto,0),
			Acrescimo = Isnull(Acrescimo,0),
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_pagar.codigo_centro_custo,
			Banco = Conta_a_pagar.id_cc					
		from dbo.Conta_a_pagar Conta_a_pagar  LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente  ON  Conta_a_pagar.id_cc = Conta_corrente.id_cc  LEFT OUTER JOIN dbo.Centro_Custo Centro_custo  ON  Conta_a_pagar.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo AND  Conta_a_pagar.Filial = Centro_custo.filial
			left outer join Fornecedor on Conta_a_pagar.Fornecedor = Fornecedor.Fornecedor
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
	--Print @string
	Exec(@string)
End

GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO]
end
GO
--PROCEDURES =======================================================================================
CREATE  Procedure [dbo].[sp_REL_CONTAS_A_PAGAR_SIMPLIFICADO] 
	@filial varchar(20), 
	@datade varchar(10), 
	@dataate varchar(10),
	@tipo varchar(10),
	@status varchar(10),
	@fornecedor varchar(20),
	@tipoPagamento varchar(50)
	
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
				AND ((Status like (CASE WHEN @STATUS='ABERTO' THEN '1'
							 WHEN @STATUS='CONCLUIDO' THEN '2'
							 WHEN @STATUS='CANCELADO' THEN '3'
							 WHEN @STATUS='LANCADO' THEN '4'
							ELSE '%' END
							) )and (@STATUS ='CANCELADO' OR Status <>'3'))
				and (Fornecedor like case when LEN(@FORNECEDOR)>0 then @FORNECEDOR
						  	else '%'
						  	end)
			    and (@tipoPagamento ='TODOS' OR Conta_a_pagar.Tipo_Pagamento = @tipoPagamento)
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

