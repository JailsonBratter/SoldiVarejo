
alter table grupo_cc add modalidade varchar(20);

---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================
---==========================================================================================================================================================================================================================================================================================


ALTER  procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10)
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
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
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
			ValorPagar = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_pagar.codigo_centro_custo					
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



ALTER  procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250), 
	@status	   varchar(10),
	@centrocusto varchar(10)
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
	
	if LEN(@centrocusto)>0
	begin
		set @Where = @Where + ' and Conta_a_receber.codigo_centro_custo= '+ char(39)+ @centrocusto+ char(39) 
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
			ValorReceber = Isnull(Valor,0) - Isnull(Desconto,0) + Isnull(Acrescimo,0),
			[CENTRO CUSTO]= Conta_a_receber.codigo_centro_custo						
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

DROP PROCEDURE VENDAS_POR_GRUPO ; 

go 

CREATE PROCEDURE [dbo].[SP_REL_VENDAS_POR_GRUPO] @FILIAL VARCHAR(20),@datade varchar(10), @dataate varchar(10),@grupo varchar(20)
as
begin 
	-- exec SP_REL_VENDAS_POR_GRUPO 'MATRIZ','20141001','20141001','ACOUGUE'
	-- select top 10 * from saida_estoque
declare @total decimal(12,2)	

if len(@grupo)>0 
	begin
	
		select @total = SUM (vlr) from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and (a.Data_movimento between @datade and @dataate) and c.Descricao_grupo = @grupo				
		
		select COD=c.codigo_departamento,DESCRICAO= c.descricao_departamento , Venda = sum(a.vlr),[%]=CONVERT(DECIMAL(12,2),((sum(a.vlr)/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and(a.Data_movimento between @datade and @dataate )and c.Descricao_grupo = @grupo  and a.data_cancelamento is null				
		group by c.codigo_departamento,c.descricao_departamento

	end
else
begin

		select @total = SUM (vlr) from Saida_estoque where Filial=@FILIAL and Data_movimento between @datade and @dataate
		select COD=c.codigo_grupo,DESCRICAO= c.descricao_grupo , Venda = sum(a.vlr),[%]=CONVERT(DECIMAL(12,2),((sum(a.vlr)/@total)*100)) 
		from Saida_estoque a inner join mercadoria b on a.PLU = b.plu 
							 inner join W_BR_CADASTRO_DEPARTAMENTO c on b.Codigo_departamento = c.codigo_departamento
		where a.Filial=@FILIAL and a.Data_movimento between @datade and @dataate	and a.data_cancelamento is null				
		group by c.codigo_grupo,c.Descricao_grupo
end
end
