
update web_telas set ordem = '200' where cod ='R002' and item ='20-Vendas_Por_Cliente'
update web_telas set ordem = '210' where cod ='R002' and item ='21-Analise_de_Vendas_por_Hora'


DECLARE @TELA VARCHAR(20) , @ITEM VARCHAR(50)
SET @TELA = 'R002'
SET @ITEM ='22-Vendas_Por_Clientes_Nota_Fiscal'


if not exists(select 1 from web_telas where cod =@TELA and item =@ITEM ) 
 begin
  insert into web_telas 
   values(@TELA,@ITEM,(SELECT MAX(ORDEM) FROM WEB_TELAS WHERE COD =@TELA)+10)
 end 

SET @TELA = 'R004'
SET @ITEM ='04-Mapa_Resumo_SAT'


if not exists(select 1 from web_telas where cod =@TELA and item =@ITEM ) 
 begin
  insert into web_telas 
   values(@TELA,@ITEM,(SELECT MAX(ORDEM) FROM WEB_TELAS WHERE COD =@TELA)+10)
 end 


 
SET @TELA = 'R003'
SET @ITEM ='15-Contas_a_Pagar_Saldo_Geral'


if not exists(select 1 from web_telas where cod =@TELA and item =@ITEM ) 
 begin
  insert into web_telas 
   values(@TELA,@ITEM,(SELECT MAX(ORDEM) FROM WEB_TELAS WHERE COD =@TELA)+10)
 end 


 
 
SET @TELA = 'R005'
SET @ITEM ='05-Produtos_Por_Validade'


if not exists(select 1 from web_telas where cod =@TELA and item =@ITEM ) 
 begin
  insert into web_telas 
   values(@TELA,@ITEM,(SELECT MAX(ORDEM) FROM WEB_TELAS WHERE COD =@TELA)+10)
 end 


 
SET @TELA = 'R007'
SET @ITEM ='02-Comanda_Vendas'


if not exists(select 1 from web_telas where cod =@TELA and item =@ITEM ) 
 begin
  insert into web_telas 
   values(@TELA,@ITEM,(SELECT MAX(ORDEM) FROM WEB_TELAS WHERE COD =@TELA)+10)
 end 

GO 

IF OBJECT_ID('[sp_rel_fin_Ficha_Cliente_Pedido]', 'P') IS NOT NULL
begin 
      drop procedure [sp_rel_fin_Ficha_Cliente_Pedido]
end 

go
CREATE  procedure [dbo].[sp_rel_fin_Ficha_Cliente_Pedido](
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@tipo varchar(50),
@Codigo_cliente varchar(50),
@status varchar(10),
@TipoRec varchar(20),
@AVista Varchar(10),
@simples    as varchar(5)     
)
--sp_rel_fin_Ficha_Cliente_Pedido 'MATRIZ', '20150101', '20161231', 'EMISSAO', '2369', '', '', '', ''
As

Declare @String as nvarchar(4000)
Declare @Where as nvarchar(4000)

SET @AVista = CASE
WHEN @AVista = 'AVISTA' THEN 'AV'
WHEN @AVista = 'PRAZO' THEN 'PZ'
ELSE ''
END
Begin
--Monta Clausula Where da Procura
set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and '
set @where = @where + @tipo + ' between ' + char(39) + @datade + char(39) + ' and ' + char(39) + @dataate + char(39)
--Verifica se o Parametro @fornecedor tem conteudo
if len(rtrim(ltrim(@Codigo_cliente))) > 0
Begin
set @where = @where + ' And cliente.Codigo_Cliente = '+ char(39) + @Codigo_Cliente + char(39) 
End

if LEN(@status)>0
begin
set @Where = @Where + ' and '+ (CASE WHEN @STATUS='ABERTO' THEN ' Conta_a_receber.status =1'
WHEN @STATUS='CONCLUIDO' THEN ' Conta_a_receber.status =2'
WHEN @STATUS='CANCELADO' THEN ' Conta_a_receber.status =3'
WHEN @STATUS='LANCADO' THEN ' Conta_a_receber.status =4'
ELSE  ' and Conta_a_receber.status =1' end --'status like '+CHAR(39)+'%'+CHAR(39) END

)

end
if LEN(@status)=0
begin
set @Where = @Where + ' and Conta_a_receber.status <>3'

end

IF LEN(@TipoRec) >0
begin
SET @Where = @Where + ' and Conta_a_receber.Tipo_Recebimento like '+ CHAR(39) + '%' + @TipoRec + '%' + CHAR(39)
end

IF LEN(@AVista ) >0
begin
SET @Where = @Where + ' and CASE WHEN ISNULL(dbo.Tipo_Pagamento.A_Vista, 1) = 1 THEN ' + CHAR(39) + 'AV' +CHAR(39) + ' ELSE ' + CHAR(39) + 'PZ' + CHAR(39) + ' END = ' + CHAR(39) + @AVista + CHAR(39)
end

SET @Where = @Where + ' AND PD.Tipo =1 AND Pd.Status in (1,2) And CONVERT(VARCHAR, PD.pedido_simples) LIKE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'SIM' + CHAR(39) + ' THEN  '+ CHAR(39) +  '1' + CHAR(39) + '  ELSE case when ' + CHAR(39) + @simples + CHAR(39) + ' = '+ CHAR(39) + 'NAO' + CHAR(39) + ' THEN  '+ CHAR(39) +  '0' + CHAR(39) + ' ELSE ' + CHAR(39) +  '%' + CHAR(39) + 'END END '

SET @Where = @Where + ' AND LEN(Ltrim(Rtrim(Conta_a_receber.Documento))) > 3 ' 



--Monta Select
set @string = 'select
					Simples_Cliente = Convert(varchar(200),Ltrim(Rtrim(Case When Isnull(PD.pedido_simples,0) = 1 Then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END ' + ' + ' + CHAR(39) + SPACE(1) + CHAR(39) + ' + ' + 'Ltrim(Rtrim(cliente.Codigo_Cliente))' + ' + ' + CHAR(39) + '  -  ' + CHAR(39) + '+' + 'Ltrim(Rtrim(cliente.nome_Cliente))))) ' + ', 
					Documento = rtrim(ltrim(documento)),
					Codigo = Convert(int,Ltrim(Rtrim(cliente.Codigo_Cliente))),
					Cliente = Substring(Ltrim(Rtrim(cliente.nome_Cliente)),1,1),
					Nome = ' + CHAR(39) + 'VALOR TOTAL DO CLIENTE ' + CHAR(39) + ',
					VlrReceber = Isnull(Conta_a_receber.Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0),
					VlrAberto = case WHEN conta_a_Receber.status = 1 then Isnull(Valor,0) - Isnull(Conta_a_receber.Desconto,0) + Isnull(Conta_a_receber.Acrescimo,0) - Isnull(Conta_a_receber.Valor_Pago,0) else 0 end,
					convert(varchar ,emissao,103) as Emissao,
					convert(varchar ,vencimento,103) as Vencimento,
					Prazo = Convert(Varchar,DATEDIFF(DAY,GETDATE(), vencimento )) ,
					TipoRecebimento = ISNULL(Conta_a_Receber.Tipo_Recebimento,' + CHAR(39) + CHAR(39) + ') ' +
		' into #t from dbo.Conta_a_receber Conta_a_receber 
					INNER JOIN Cliente ON conta_a_receber.codigo_cliente = cliente.codigo_cliente 
					LEFT OUTER JOIN dbo.Conta_Corrente Conta_corrente ON Conta_a_receber.id_cc = Conta_corrente.id_cc 
					LEFT OUTER JOIN dbo.Centro_Custo Centro_custo ON Conta_a_receber.Codigo_Centro_Custo = Centro_custo.Codigo_centro_custo 
																  AND Conta_a_receber.Filial = Centro_custo.filial

					Inner Join Pedido PD on Conta_a_receber.Codigo_Cliente = PD.Cliente_Fornec 
										And Substring(Conta_a_receber.Documento,2,LEN(PD.Pedido)) = PD.Pedido 
										And Conta_a_receber.Emissao = PD.Data_cadastro      
					LEFT OUTER JOIN dbo.Tipo_Pagamento on Tipo_Pagamento.Tipo_Pagamento = Conta_a_Receber.Tipo_Recebimento
'+@where+' Order By cliente.nome_Cliente, cliente.Codigo_Cliente, PD.pedido_simples '

Set @String = @string + 'insert into #t select Simples_Cliente + ' + CHAR(39) + ' - Sub Total|-SUB-|' + CHAR(39) + ', ' + CHAR(39) +  CHAR(39) + ',Codigo,cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Simples_Cliente,Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select Nome, ' + CHAR(39) + CHAR(39) + ',Codigo,Cliente,' + CHAR(39) + CHAR(39) + ',SUM(VlrReceber),Sum(VlrAberto), ' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' ,' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t Group by Codigo,Nome,Cliente order by 3 '
Set @String = @string + 'insert into #t select ' + CHAR(39) + 'TOTAL GERAL - FICHA DE CLIENTE|-SUB-|' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' +  CHAR(39) + '999999' + CHAR(39) + ',' + CHAR(39) + 'Z' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + 'SUM(VlrReceber),Sum(VlrAberto),' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ' from #t WHERE NOME = '+ CHAR(39) + 'VALOR TOTAL DO CLIENTE' + CHAR(39) 
Set @String = @string + 'Select Simples_Cliente , Documento, VlrReceber, VlrAberto, Emissao, Vencimento, TipoRecebimento From #t Where Simples_Cliente <> ' + CHAR(39) +'VALOR TOTAL DO CLIENTE'+ CHAR(39) + ' AND Simples_Cliente <> ' + CHAR(39) + CHAR(39) + ' Order by 1,2,3'

Exec(@string)
End



go 


IF OBJECT_ID('[sp_rel_produtos_estoque]', 'P') IS NOT NULL
begin 
      drop procedure [sp_rel_produtos_estoque]
end 

go
CREATE  procedure [dbo].[sp_rel_produtos_estoque]
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
,@LINHA VARCHAR(30)
as
Declare @String	As Varchar(MAX)
Declare @Fantasia as varchar(50)

--'Depois tem que tirar esse parametro direto 
Set @Fantasia = (select Max(Fantasia)  from filial)

begin

    SET @String = ' Select distinct a.PLU,'
    --begin 
    --'Depois tem que tirar esse parametro direto 
    --if(@Fantasia='CHAMANA')
		SET @String = @String + 'Ref = a.Ref_Fornecedor,'    
	--end	

    SET @String = @String + 'a.Descricao,'    
    SET @String = @String + 'b.descricao_grupo [GRUPO],'
    -- SET @String = @String + 'b.descricao_subgrupo[SUBGRUPO],'
    SET @String = @String + 'b.descricao_departamento [DEPARTAMENTO],'
   -- begin 
    
    --'Depois tem que tirar esse parametro direto 
   -- if(@Fantasia <> 'CHAMANA')
	--	SET @String = @String + 'isnull(c.Descricao_Familia,' + CHAR(39) + '' + CHAR(39) + ')[FAMILIA],'
	-- End		
    SET @String = @String + 'isnull(l.Preco_Custo,0) AS [PRECO CUSTO],'
    SET @String = @String + 'isnull(l.Preco,0) AS [PRECO VENDA],'    
    SET @String = @String + 'isnull(l.saldo_atual,0) AS [SALDO ATUAL],'
	SET @String = @String + 'isnull(a.estoque_minimo,0) AS [ESTOQUE MINIMO],'

    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco_Custo,0)))[VALOR ESTOQUE CUSTO],'
    SET @String = @String + 'convert(decimal(12,2),(isnull(l.Saldo_Atual,0)*isnull(l.Preco,0)))[VALOR ESTOQUE VENDA]'    
    SET @String = @String + ' from '
    SET @String = @String + ' Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b '
    SET @String = @String + ' on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial) '
    SET @String = @String + ' inner join mercadoria_loja l on a.plu=l.PLU '
    SET @String = @String + ' left join Familia c on  a.Codigo_familia =c.Codigo_familia '
    SET @String = @String + ' left join EAN on a.plu = ean.plu '
    SET @String = @String + ' left join LINHA on a.Cod_Linha = LINHA.codigo_linha'
    
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

	if(@saldo='Estoque minimo')
	begin
		 SET @String = @String + ' 	and (l.saldo_atual<=a.estoque_minimo) AND ISNULL(A.ESTOQUE_MINIMO,0)>0'
	end


	if(LEN(@LINHA)>0 )
	BEGIN
		SET @String = @String + ' 	and (LINHA.descricao_linha ='+char(39)+@LINHA+char(39)+')'
	END
	
	--PRINT @STRING	
	 EXECUTE(@String)	
  end

go


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cotacao_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('EAN'))
begin
	alter table cotacao_item alter column EAN varchar(20)
end
else
begin
	alter table cotacao_item add EAN varchar(20)
end 
go 

update cotacao_item set ean = isnull((Select top 1  ean from ean where plu = cotacao_item.Mercadoria),0)



go 

IF OBJECT_ID('[sp_Rel_Cliente_nf]', 'P') IS NOT NULL
begin 
      drop procedure [sp_Rel_Cliente_nf]
end 

go
CREATE PROCEDURE [dbo].[sp_Rel_Cliente_nf] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cnpj       as Varchar(20),
		@tipo       as varchar(20)
		as
BEGIN
	-- exec [sp_Rel_Cliente_nf] 'MATRIZ','20180301','20180307','49.930.514/0001-35','ANALITICO'
	
	IF(LEN(@CNPJ)>0)
	BEGIN 
		SET @CNPJ = LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(@CNPJ,'.',''),'/',''),'-','')))
	END 
	

	Select 
			Emissao,
			nf.Cliente_Fornecedor Cod,
			cliente.Nome_Cliente,
			cliente.nome_fantasia,
			cg.Grupo,
			Total = Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0)))
			
		into #tempVendaNF from NF inner join cliente on NF.Cliente_Fornecedor= cliente.Codigo_Cliente 
			     INNER JOIN NF_Item ON NF.Codigo = NF_ITEM.CODIGO and nf.Cliente_Fornecedor = nf_item.Cliente_Fornecedor and nf.Filial = nf_item.Filial
				 inner join cliente_grupo as cg on cg.id= cliente.grupo_empresa	
				 inner join Natureza_operacao np on.nf.Codigo_operacao = np.Codigo_operacao	
		 where (NF.Data between @DataDe and @DataAte ) 
		 AND Isnull(NF.NF_Canc,0) = 0 AND LTRIM(NF.Filial) = @FILIAL
		AND NF.Tipo_NF in (1,3) --Incluido a condição 3 - Emissao de Pedidos
		 and NP.NF_devolucao = 0 
			--AND nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403')  
			AND (LEN(@CNPJ)=0 OR LTRIM(RTRIM(REPLACE(REPLACE(REPLACE(CLIENTE.CNPJ,'.',''),'/',''),'-','')))= @CNPJ)
		 group by NF.EMISSAO,nf.Cliente_Fornecedor,cliente.Nome_Cliente,cliente.nome_fantasia, cg.grupo, nf.Total
		 ORDER BY CONVERT(VARCHAR,NF.EMISSAO,102) 



	if(@tipo='ANALITICO')
	BEGIN
		Select 
		CONVERT(VARCHAR,EMISSAO,103) EMISSAO,
		COD='SFT_'+Cod,
		Cliente= Nome_Cliente,
		Total 
		from #tempVendaNF		
	END
	ELSE	
	BEGIN
		if(@tipo='SINTETICO')
		begin  
			Select 
				CONVERT(VARCHAR,EMISSAO,103) EMISSAO,
				Cliente =nome_fantasia,
				Total = Sum(Total) 
			 from
			 #tempVendaNF
			 group by Emissao , nome_fantasia
		end
		else
		begin 
			if(len(@cnpj)=0)
			begin
				Select 
					Grupo,
					Total = Sum(total)
				from #tempVendaNF
				group by grupo
				order by sum(total) desc
			end
			else
			begin 
				Select 
					COD='SFT_'+Cod,
					Cliente=Nome_Cliente,
					Total = Sum(total)
				from #tempVendaNF
				group by COD, Nome_Cliente
				order by sum(total) desc
			end 

		end 
	END
END

go 


IF  OBJECT_ID('COTACAO_OBS_FORNECEDOR', N'U') IS NULL
begin 
	CREATE TABLE COTACAO_OBS_FORNECEDOR(
	FILIAL VARCHAR(20),
	FORNECEDOR VARCHAR(20),
	COTACAO INT,
	OBS TEXT
	)
END 






go
insert into Versoes_Atualizadas select 'Versão:1.226.727', getdate();
GO
