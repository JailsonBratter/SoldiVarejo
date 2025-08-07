alter table funcionario add Usa_Terminal tinyint


GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Venda_pedido_simplificado]    Script Date: 10/22/2015 08:39:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@nome		as Varchar(30) = ''
		
    	as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20150101','20151023','160','SIM',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),UNITARIO = ROUND( UNITARIO ,2),VALOR=ROUND(SUM(p.total),2)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario

	UNION ALL

	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),0,VALOR=ROUND(SUM(p.total),0)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte
	
end
else		
begin
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			--VlrVenda=ROUND(Total,2), 
			
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
			FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
			where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo ),
			
			[Total Venda]=ROUND(Total,2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			ISNULL(Pedido.funcionario, '') As Vendedor 
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		  AND 
			Pedido.Status in (1,2)
          And 
			isnull(Pedido.funcionario,'') like (case when @nome  <> '' then @Nome else '%' end)
		UNION ALL 
		Select 
			Simples = '',
			Pedido ='',
			Data='', 
			Cod='',
			Nome_Cliente='TOTAL',
			[Total Compra] = (SELECT Convert(Decimal(12,2),Isnull(SUM(m.Preco_Custo*P.Qtde),0))
				FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU INNER JOIN PEDIDO PD ON PD.Pedido = P.PEDIDO And P.tipo = PD.Tipo
				where   P.Filial = Pedido.Filial  AND PD.Data_cadastro between @DataDe and @DataAte AND PD.Status in (1,2) and Pd.Tipo=1) ,
						
		    [Total Venda]=ROUND(SUM(Total),2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P INNER JOIN PEDIDO PD ON PD.Pedido = P.PEDIDO And P.tipo = PD.Tipo where  P.Filial = Pedido.Filial AND 
			PD.Tipo =1 AND PD.Status in (1,2) AND PD.Data_cadastro between @DataDe and @DataAte ),
			Vendedor = ''
		 From pedido inner join cliente on pedido.Cliente_Fornec= cliente.Codigo_Cliente 
		 Where  
			CONVERT(VARCHAR, Pedido.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
		  and 
			pedido.Data_cadastro between @DataDe and @DataAte
		  AND 
			Pedido.Tipo =1 
		 AND 
			Pedido.Status in (1,2)
		  And 
			isnull(Pedido.funcionario,'') like (case when @nome  <> '' then @Nome else '%' end)
		GROUP BY PEDIDO.Filial		 
 end

GO
/****** Object:  StoredProcedure [dbo].[sp_rel_conta_a_pagar]    Script Date: 10/22/2015 09:46:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER  procedure [dbo].[sp_rel_conta_a_pagar](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@fornecedor	varchar(250),
	@valor		VARCHAR(11), 
	@status	   varchar(10),
	@centrocusto varchar(10),
	@Cheque		varchar(30),
	@Conferido varchar(10)
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
	if(LEN(@Conferido)>1)
	begin
		set @where = @where + ' And conferido =1 '	
	end 
	
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
	
	if LEN(@Cheque)>0
	begin
		set @Where = @Where + ' and Conta_a_pagar.Numero_cheque= '+ char(39)+ @Cheque+ char(39) 
	end
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
/****** Object:  StoredProcedure [dbo].[sp_rel_produtos_estoque]    Script Date: 10/22/2015 16:18:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[sp_rel_produtos_estoque] 'MATRIZ',64345,'','','','','','','',''

ALTER  procedure [dbo].[sp_rel_produtos_estoque]
@filial varchar(20) ,
@plu varchar(20)
,@ean varchar(20)
,@ref varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@saldo varchar(14)
as
Declare @String	As Varchar(MAX)
Declare @Fantasia as varchar(50)

--'Depois tem que tirar esse parametro direto 
Set @Fantasia = (select Max(Fantasia)  from filial)

begin

    SET @String = ' Select a.PLU,'
    begin 
    --'Depois tem que tirar esse parametro direto 
    if(@Fantasia='CHAMANA')
		SET @String = @String + 'Ref_Forn = a.Ref_Fornecedor,'    
	end	

    SET @String = @String + 'a.Descricao,'    
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
    begin 
    
    --'Depois tem que tirar esse parametro direto 
    if(@Fantasia <> 'CHAMANA')
		SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
	End		
    SET @String = @String + 'isnull(l.Preco_Custo,0) AS [PRECO CUSTO],'
    SET @String = @String + 'isnull(l.Preco,0) AS [PRECO VENDA],'    
    SET @String = @String + 'isnull(l.saldo_atual,0) AS [SALDO ATUAL],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco_Custo,0)))[VALOR ESTOQUE CUSTO],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco,0)))[VALOR ESTOQUE VENDA]'    
    SET @String = @String + ' from '
    SET @String = @String + ' Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b '
    SET @String = @String + ' on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial) '
    SET @String = @String + ' inner join mercadoria_loja l on a.plu=l.PLU '
    SET @String = @String + ' left join Familia c on  a.Codigo_familia =c.Codigo_familia '
    SET @String = @String + ' left join EAN on a.plu = ean.plu '
    SET @String = @String + ' where a.Inativo <>1 '
    begin 
    if(len(@plu)>0)
		SET @String = @String + ' AND a.PLU= ' + CHAR(39) + @plu + CHAR(39)
	end	
	begin
		if(LEN(@descricao)>0) 
			SET @String = @String + ' and a.Descricao like '+ CHAR(39) +'%'+@descricao+'%'+ CHAR(39) 
	end
	begin 
		if(LEN(@grupo)>0)
		SET @String = @String + ' and (b.Descricao_grupo = '+ CHAR(39) +@grupo+ CHAR(39) +')'
	end	
	begin
		if(LEN(@subGrupo)>0)
		SET @String = @String + ' and (b.descricao_subgrupo = '+CHAR(39)+@subGrupo+CHAR(39)+')'
	end
	begin 
		if(LEN(@departamento)>0)
			SET @String = @String + ' and (b.descricao_departamento = '+CHAR(39)+ @departamento+CHAR(39)+')'
	end
	begin
		if(LEN(@familia)>0)
			SET @String = @String + ' and (a.Descricao_familia = '+CHAR(39)+@familia+CHAR(39)+')' 
	end

		SET @String = @String + ' and l.Filial = '+char(39)+@filial+CHAR(39)
	begin
		if(LEN(@ean)>0)
		SET @String = @String + ' and (ean.EAN ='+char(39)+@ean+char(39)+')'
	end
	begin 
		if(LEN(@ean)>0)
		 SET @String = @String + ' 	and (a.Ref_fornecedor = '+char(39)+@ref+CHAR(39)+')'
	end
	begin
	if(@saldo='Igual a Zero')
		 SET @String = @String + ' 	and (l.saldo_atual=0)'
	end
	begin
	if(@saldo='Menor que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual<0)'
	end
	begin
	if(@saldo='Maior que Zero')
		 SET @String = @String + ' 	and (l.saldo_atual>0)'
	end
	--PRINT @STRING	
	 EXECUTE(@String)	
  end




