IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperadorCancelamento]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperadorCancelamento]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Fin_PorOperadorCancelamento](

      @FILIAL			AS VARCHAR(17),
      @Datade           As DATETIME,
      @Dataate			As DATETIME,
      @Operador         As varchar(20),
      @Pdv				As varchar(20))

as

if len(@Operador) =0 OR @Pdv='TODOS'

      begin

            set @Operador ='%'     

      end
      
if (LEN(@Pdv) = 0 OR @Pdv='TODOS')

		begin
			set @Pdv = '%'
			end
 

select b.Nome, a.Pdv, convert(varchar,a.emissao,103)Data  

      , Isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv = c.pdv and isnull(c.cancelado,0)=0), 0) as  Vendas

      , isnull((select sum(total) from lista_finalizadora c where c.emissao=a.emissao and a.operador=c.operador and a.pdv= c.pdv and   isnull(c.cancelado,0)=1),0)as  Cancelados
      

      from lista_finalizadora a inner join operadores b on  a.operador= b.id_operador

 

      where a.filial=@FILIAL and b.nome like @Operador and a.emissao between @Datade and @Dataate And a.pdv like @Pdv

group by a.operador,a.Pdv, b.nome,a.emissao

order by a.Pdv, b.nome

 
GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Resumo_Vendas_Pdv]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_Resumo_Vendas_Pdv]
end
GO
--PROCEDURES =======================================================================================
CREATE  Procedure [dbo].[sp_rel_Resumo_Vendas_Pdv] 
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
		IF (LEN(ISNULL(@PDV,'')) > 0 and @PDV <> 'TODOS')
			SET @String = @String + ' and CAIXA_SAIDA IN (' +@PDV+ ')'
		
	end	
				
	SET @String = @String + 'GROUP BY Filial,Data_Movimento, Caixa_Saida'
	EXECUTE(@String)
--	print @string
end


go 
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_PorOperador]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_PorOperador]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_PorOperador](
	@FILIAL 	  AS VARCHAR(17),
	@Datade		  As DATETIME,
	@Dataate	  As DATETIME,
	@Caixa   	  As varchar(8),
	@Grupo        As Varchar(60),
    @subGrupo	  as Varchar(60),
    @Departamento as Varchar(60),
    @Familia	  as Varchar(60),
    @fornecedor   as Varchar(40) 
            )
as

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

if(LEN(@fornecedor)=0 and @Caixa='TODOS')
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
	inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
	--inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
	--	and isnull(Operadores.Nome ,'') like @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
else
begin 
	if(@Caixa<>'TODOS')
	begin
		select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque   a with(index(ix_Rel_fin_porOperador)) inner join mercadoria b on a.plu =b.plu  
	inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
	--INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
	--inner join Operadores on l.operador = Operadores.ID_Operador	
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		and (Select top 1 Operadores.Nome from Operadores inner join Lista_finalizadora l on l.operador = Operadores.ID_Operador where a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial   ) = @Caixa
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
	end
else
  begin
	select a.plu as PLU,b.descricao AS DESCRICAO,c.Descricao_grupo as GRUPO,C.descricao_subgrupo AS SUBGRUPO,C.descricao_departamento DEPARTAMENTO,F.Descricao_Familia AS FAMILIA ,sum(Qtde) as Qtde ,((Sum(isnull(vlr,0))-SUM(ISNULL(desconto,0)))+SUM(isnull(Acrescimo,0))) as Valor 
	from saida_estoque a with(index(ix_Rel_fin_porOperador))  inner join mercadoria b on a.plu =b.plu  inner join w_br_cadastro_departamento c on b.codigo_departamento = c.codigo_departamento left join familia f on b.codigo_familia=f.codigo_familia 
		 inner join Fornecedor_Mercadoria fm on a.PLU = fm.plu 	
		 INNER JOIN Lista_finalizadora L with(index(ix_Lista_rel_fin_PorOperador)) ON  a.Documento=L.Documento and a.Caixa_Saida = l.pdv and l.Emissao = a.Data_movimento and l.filial =a.Filial
		inner join Operadores on l.operador = Operadores.ID_Operador	
				
	where a.filial=@FILIAL and  data_cancelamento is null and Data_movimento >=@Datade and 
		Data_movimento <=@Dataate 
		and (@Caixa='TODOS' OR isnull(Operadores.Nome ,'') like @Caixa)
		and c.Descricao_grupo like @Grupo
		and c.descricao_subgrupo like @subGrupo
		and c.descricao_departamento like @Departamento
		and isnull(f.Descricao_Familia,'') like @Familia
		and fm.fornecedor = @fornecedor
	group by a.plu,b.descricao,c.Descricao_grupo,c.Descricao_subgrupo,c.Descricao_departamento,f.Descricao_familia
	order by b.descricao
  end
end

  
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_ResumoPorOperador]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Fin_ResumoPorOperador]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Fin_ResumoPorOperador](
	@FILIAL 	AS VARCHAR(17),
	@Data		As Varchar(8),
	@Operador	As Varchar(40),
	@Pdv		As Varchar(4),
	@horade		As varchar(5),
	@horaate	As varchar(5))
as
	
if len(@horade)= 0
	begin
		set @horade = '0'
	end
if len(@horaate) = 0
	begin
		set @horaate = '24'
	end
	
Declare @pIndex int 
Declare @idOp int 

if @Operador <> 'TODOS'
begin
	set @pIndex= CHARINDEX('-', @Operador)	
    set @idOp = SUBSTRING(@Operador,0,@pIndex)

end


Select 
	isnull(finalizadora.Finalizadora,'Outra') as Finalizadora, sum(ISNULL(total,0)) as valor

From 	Lista_Finalizadora
Left Outer Join	finalizadora on 
	lista_finalizadora.finalizadora = finalizadora.nro_finalizadora
Where
	lista_finalizadora.filial = @filial and 
	Emissao = @data and 
	(operador=@idOp or LEN(@operador)=0 )and
	(pdv = @Pdv or @pdv='TODOS')and
	isnull(cancelado,0) = 0 And
	exists(Select * from saida_Estoque where
			data_movimento = @data and 
			convert(decimal(18,2),substring(replace(hora_venda,':','.'),1,5)) 
					between isnull(convert(decimal(18,2), replace(@horade,':','.')),0) and isnull(convert(decimal(18,2), replace(@horaate,':','.')),24)
			and lista_finalizadora.cupom = saida_Estoque.documento)
group by 
	finalizadora.Finalizadora


GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Pedido_Venda_Analitico]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Pedido_Venda_Analitico]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Analitico]
            @Filial					As Varchar(20),
            @DataDe                 As Varchar(8),
            @DataAte				As Varchar(8),
			@PLU					As Varchar(17) = '',
			@REF_Fornecedor			As Varchar(20) = '',
            @Descricao				As Varchar(60) = '',
			@Cliente				As Varchar(40) = '',
			@Simples    			As VarChar(3) = '',
			@Vendedor				as Varchar(30) = ''			
AS
                
	Declare @String               As nVarchar(3000)
	Declare @String2			  As nVarchar(1024)
	Declare @Where                As nVarchar(1024)

	SET @Where = ' WHERE PEDIDO.FILIAL='+ CHAR(39) +@Filial+ CHAR(39) +' AND Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
	SET @Where = @Where + 'AND Pedido.Data_Cadastro BETWEEN '  + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)	
	--//Filtro PLU
	IF @PLU <> ''
		SET @Where = @Where + ' AND Pedido_Itens.PLU = ' + CHAR(39) + @PLU + CHAR(39)
	--** Filtro Descricao	
	IF @Descricao <> ''
		SET @Where = @Where + ' AND Mercadoria.Descricao LIKE' + CHAR(39) + '%' + @Descricao + '%' + CHAR(39)
	--** Filtro Ref_Fornecedor
	IF @REF_Fornecedor <> ''
		SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE' + CHAR(39) + @REF_Fornecedor + '%' + CHAR(39)
	--** Filtro Cliente
	IF @Cliente <> ''
		BEGIN
			SET @Where = @Where + ' AND (Cliente.Nome_Cliente LIKE ' + CHAR(39) + '%' + @Cliente + '%' + CHAR(39)
			SET @Where = @Where + '	OR Replace(Replace(Replace(Cliente.CNPJ, ' + CHAR(39) + '.' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '/' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + '), ' + CHAR(39) + '-' + CHAR(39) + ',' + CHAR(39) + CHAR(39) + ') LIKE ' + CHAR(39) + '%' + @Cliente + '%'+ CHAR(39) + ')'
		END
	--//Filtro Vendedor
	IF @Vendedor <> ''
		SET @Where = @Where + ' AND Pedido.Funcionario = ' + CHAR(39) + @Vendedor + CHAR(39)		
	-- ** Filtro Pedido Simples
	IF @Simples <> 'TODOS' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' Data = CONVERT(VARCHAR, Pedido.Data_cadastro, 103) , Pedido.Pedido,'
		SET @String = @String + ' Cliente.Nome_Cliente, ISNULL(Pedido.Funcionario, ' + CHAR(39) + CHAR(39) + ') AS Vendedor, MERCADORIA.PLU, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String = @String + ' Qtde= CONVERT(NUMERIC(12,2), ISNULL(SUM(PEDIDO_ITENS.QTDE * Pedido_Itens.Embalagem), 0)), '
		SET @String = @String + ' Vlr = CONVERT(DECIMAL(12,2), ISNULL(SUM(PEDIDO_ITENS.TOTAL),0))'
		SET @String = @String + ' FROM Pedido '
		SET @String2 = ' INNER JOIN Pedido_ITENS ON PEDIDO_ITENS.Pedido = PEDIDO.Pedido and pedido.tipo=pedido_itens.tipo'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria ON Mercadoria.PLU = Pedido_itens.PLU '
		SET @String2 = @String2 + ' INNER JOIN Mercadoria_Loja ON MERCADORIA_LOJA.FILIAL=PEDIDO.FILIAL AND  Pedido_itens.PLU = MERCADORIA_LOJA.PLU '
		SET @String2 = @String2 + ' INNER JOIN Cliente ON cliente.Codigo_Cliente = pedido.Cliente_Fornec '
		SET @String2 = @String2 + @Where 
		SET @String2 = @String2 + ' GROUP BY MERCADORIA.PLU, '
		SET @String2 = @String2 + ' Pedido.Data_cadastro, Pedido.Pedido, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)


GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_fin_cartao]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_fin_cartao]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_fin_cartao]
	(
	@filial     varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@cartao		varchar(50)
	)
	as
Select Documento,
	Cartao=ca.id_cartao, 
	Pdv = cr.pdv,
	Emissao=convert(varchar,Emissao,103),
	Vencimento=CONVERT(varchar,vencimento,103),
	Valor,
	[Taxa $]=cr.taxa,
	Desconto,
	Acrescimo = isnull(acrescimo,0), 
	Total=((isnull(Valor,0)-isnull(Desconto,0))+isnull(acrescimo,0))-isnull(cr.taxa,0) 
	from Conta_a_receber cr
		inner join Cartao ca on
		(			cr.finalizadora = ca.nro_Finalizadora 
				and convert(int,cr.id_Bandeira)=convert(int,ca.id_Bandeira) 
				and rtrim(ltrim(cr.Rede_Cartao))=rtrim(ltrim(ca.id_rede))
				)
where CR.FILIAL =@filial AND
	(
		(@tipo ='EMISSAO' AND (Emissao between @datade and @dataate))
		OR
		(@tipo ='VENCIMENTO' AND (VENCIMENTO between @datade and @dataate))
	  )
	  AND (@cartao='TODOS' OR CA.ID_CARTAO = @cartao )
	  
order by ca.id_cartao,cr.pdv


go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Outras_Movimentacoes]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Outras_Movimentacoes]
end
GO
--PROCEDURES =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@plu				AS Varchar(20),
	@ean				as varchar(20),
	@ref				as Varchar(20),
	@descricao			as Varchar(40),
	@Movimentacao	As Varchar(30) = ''
AS
	Declare @String	As nVarchar(1024)
		
BEGIN
	SET @String = ' SELECT'
	SET @String = @String + ' i.Codigo_inventario As Codigo,'
	SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
	SET @String = @String + ' i.tipoMovimentacao,'
	SET @String = @String + ' i.Usuario,'
	SET @String = @String + ' ii.plu,'
	SET @String = @String + ' m.descricao,'
	SET @String = @String + ' ii.Contada AS Qtde,'
	SET @String = @String + ' ii.custo as Vlr_Unit,'
	SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
	SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
	SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
	SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
	SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
	BEGIN
		IF @Movimentacao<> 'TODOS' 
			SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@plu ,'')) > 0 
			SET @String = @String + ' AND ii.plu = ' + CHAR(39) + @plu + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ean ,'')) > 0 
			SET @String = @String + ' AND ean = ' + CHAR(39) + @ean + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ref ,'')) > 0 
			SET @String = @String + ' AND m.Ref_fornecedor = ' + CHAR(39) + @ref + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@descricao ,'')) > 0 
			SET @String = @String + ' AND m.descricao like ' + CHAR(39) + '%'+@descricao +'%'+ CHAR(39)
	END
	SET @String = @String + ' ORDER BY 1'

-- PRINT @STRING
 EXECUTE(@String)

END


 

 
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_COMANDA_HISTORICO]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_COMANDA_HISTORICO]
end
GO
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_COMANDA_HISTORICO] (
		@filial varchar(30),
		@comanda varchar(20),
		@dataInicio VARCHAR(8),@dataFim VARCHAR(8), 
		@horaInicio time ,@horafim time,
		--@status varchar(10),
		@usuario varchar(20),
		@cancelamento varchar(4),
		@finalizado varchar(4))
		AS 
/*
set @comanda='1144'
set @dataInicio='20150501' 
set @datafim = '20150514'

set @horaInicio='00:00' 
set @horafim='23:59' 
--set @status =''
set @usuario =''
set @cancelamento=''
set @finalizado =''
*/		
--sp_help comanda_item
--select * from Comanda_Item where data between '20150514' and '20150515'
select i.comanda, 
	   data=convert(varchar,i.data,103),
	   hora = convert(varchar,i.hora_evento,108),
	 --  Status = case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END,
	   i.usuario,
	   i.plu,
	   m.descricao,
	   i.qtde,
	   i.unitario,
	   i.total,
	   Cancelado=case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END,	 
	   Finalizado=case when i.data_cancelamento IS NOT NULL then '---' else case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END end	 
	     
	    
from Comanda_Item i  WITH (INDEX(index_historico_comanda)) inner join mercadoria m on i.plu=m.PLU
where  i.filial = @filial   
	  and i.comanda like case when @comanda<>'TODOS' then @comanda else '%'end
	  and i.data between @dataInicio+' 00:00' and @dataFim +' 23:59'
	  and CONVERT(varchar , i.hora_evento,108) between @horaInicio and @horafim
	  and i.usuario like case when @usuario<>'TODOS' then @usuario else '%' end 
	  and (  (case when i.data_cancelamento IS NOT NULL THEN 'SIM' ELSE 'NAO' END = @cancelamento
			OR @cancelamento='TODOS' ))
	--  AND ( case when c.status=0 then 'LIVRE'ELSE CASE WHEN c.STATUS=2 THEN 'ABERTA' ELSE  'BLOQUEADA' END END=@status
	--		OR LEN(@status)=0)
	  And( case when ISNULL(i.cupom,0) <>0 then 'SIM' ELSE 'NAO' END = @finalizado
			OR @finalizado='TODOS')
order by i.hora_evento desc 
	  
	  	  	  




