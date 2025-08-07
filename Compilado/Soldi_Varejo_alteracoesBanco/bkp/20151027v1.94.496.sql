IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_ProdutosAlterados]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_ProdutosAlterados
end
go

-- sp_Rel_ProdutosAlterados 'MATRIZ', null, null, '20150422', '20150519'

create      Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20) = '',
                @Descricao              As Varchar(60) = '',
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40)


AS

Declare @String               As nVarchar(max)
Declare @Where               As nVarchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO, mercadoria_loja.PRECO_CUSTO,'
		SET @String = @String + 'MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
		SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu'
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'

		Set @Where = ' WHERE Mercadoria.Data_Alteracao BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)
               
           
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND ean.ean = '+ CHAR(39) + @ean + CHAR(39)

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end
     	
	  IF LEN(ISNULL(@DESCRICAO,'')) > 0
		BEGIN
            SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)

        END      


		EXECUTE(@String + @Where)

END

 

GO 
	 

	 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Produto_Promocao]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_Produto_Promocao
end
go


CREATE PROCEDURE [dbo].[sp_rel_Produto_Promocao] 
@filial varchar(20),
@plu varchar(20),
@ean varchar(20),
@ref varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
as

/*
set @plu ='';
set @ean ='';
set @descricao ='';
set @grupo ='BEBIDAS';
set @subGrupo ='';
set @departamento ='';
set @familia ='';
*/

select PLU=m.plu,e.EAN,NCM=M.CF,REF_FORN=M.CF, GRUPO=c.Descricao_grupo,SUBGRUPO=C.descricao_subgrupo,DEPARTAMETO=C.descricao_departamento,
		PRECO=l.Preco,PROMOCAO= case when l.promocao=1then l.preco_promocao else 0 end,
		INICIO= case when l.promocao=1 then convert(varchar, l.Data_Inicio,103) else '' end,
		FIM=case when l.promocao=1 then CONVERT(varchar ,l.Data_Fim,103) else '' end
		
	
	from mercadoria m inner join mercadoria_loja l on m.plu = l.PLU
								 left join EAN e on m.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (m.codigo_departamento= c.codigo_Departamento and m.filial=c.filial)
		WHERE  (M.PLU=@plu or len(@plu)=0)
		and (E.EAN=@ean or LEN(@ean)=0)
		and (M.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (M.Descricao_familia = @familia or LEN(@familia)=0)										 
		and l.Filial = @filial
		and (LEN(@ref)=0 or m.Ref_fornecedor like @ref+'%')
		AND L.Promocao=1
		
		
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_listagem_de_precos]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_listagem_de_precos
end
go
CREATE procedure [dbo].[sp_rel_listagem_de_precos]
	@FILIAL VARCHAR(20),
	@plu  varchar(20),
	@ean  varchar(20),
	@descricao varchar(50),
	@referencia varchar(20),
	@grupo varchar(20)
	,@subGrupo varchar(20)
	,@departamento varchar(20)
	,@familia varchar(40)
	,@NCM VARCHAR(20)

	
as

SELECT
	 m.PLU, EAN = ISNULL(ean.EAN, ''), NCM = ISNULL(m.CF, ''),m.REF_Fornecedor AS REF_FORN, m.DESCRICAO, l.Preco AS Venda
	FROM Mercadoria m INNER JOIN Mercadoria_Loja l ON m.PLU = l.PLU LEFT OUTER JOIN EAN ON ean.PLU = m.PLU 
	INNER JOIN W_BR_CADASTRO_DEPARTAMENTO C ON M.Codigo_departamento = C.codigo_departamento
	WHERE ISNULL(l.Preco, 0) > 0
	and (LEN(@plu)=0 or l.PLU=@plu)
	and (LEN(@ean)=0 or ean.EAN=@ean)
	and (LEN(@descricao)=0 or m.descricao like @descricao+'%')
	and (LEN(@referencia)=0 or m.Ref_fornecedor = @referencia)
	AND L.Filial = @FILIAL
	AND (LEN(@grupo)=0 OR C.Descricao_grupo=@grupo)
	AND (LEN(@subGrupo)=0 OR C.descricao_subgrupo=@subGrupo)
	AND (LEN(@departamento)=0 OR C.descricao_departamento=@departamento)
	AND (LEN(@familia)=0 OR M.Descricao_familia=@familia)
	AND (LEN(@NCM)=0 OR M.cf=@NCM)
	
		
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_listagem_de_precos_custo_margem]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_listagem_de_precos_custo_margem
end
go
CREATE procedure [dbo].[sp_rel_listagem_de_precos_custo_margem]
	@FILIAL VARCHAR(20),
	@plu  varchar(20),
	@ean  varchar(20),
	@descricao varchar(50),
	@referencia varchar(20),
	@grupo varchar(20)
	,@subGrupo varchar(20)
	,@departamento varchar(20)
	,@familia varchar(40)
	,@NCM VARCHAR(20)
	
as
  SELECT
     m.PLU, EAN = ISNULL(ean.EAN, ''), NCM = ISNULL(m.CF, ''),m.REF_Fornecedor AS REF_FORN, m.descricao, l.Preco_Custo as Custo, Convert(Decimal(7, 2),l.Margem) as Mrg, l.Preco AS Venda
    FROM Mercadoria m INNER JOIN Mercadoria_Loja l ON m.PLU = l.PLU LEFT OUTER JOIN EAN ON ean.PLU = m.PLU
    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO C ON M.Codigo_departamento = C.codigo_departamento
	
    WHERE ISNULL(l.Preco, 0) > 0
	and (LEN(@plu)=0 or l.PLU=@plu)
	and (LEN(@ean)=0 or ean.EAN=@ean)
	and (LEN(@descricao)=0 or m.descricao like @descricao+'%')
	and (LEN(@referencia)=0 or m.Ref_fornecedor = @referencia)
	AND L.Filial = @FILIAL
	AND (LEN(@grupo)=0 OR C.Descricao_grupo=@grupo)
	AND (LEN(@subGrupo)=0 OR C.descricao_subgrupo=@subGrupo)
	AND (LEN(@departamento)=0 OR C.descricao_departamento=@departamento)
	AND (LEN(@familia)=0 OR M.Descricao_familia=@familia)
	AND (LEN(@NCM)=0 OR M.cf=@NCM)
	
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Fin_FluxoCaixa]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Fin_FluxoCaixa
end
go

CREATE     PROCEDURE [dbo].[sp_Rel_Fin_FluxoCaixa] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8)
AS

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Totais]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)

Begin

Drop table Totais

End
	
	Create table Totais
(
	Descricao  varchar(90),
	Total	 Decimal(18,2)
);
	
	
	select count(*) as Qtde into #cupons from saida_Estoque where 
		Filial  = @Filial And
		Data_Movimento BETWEEN @DataDe AND @DataAte AND 
		Data_Cancelamento IS NULL
	group by documento, filial, caixa_saida
	
	----# Insere Todo Faturamento em uma tabela temp (Para fazer calculo) #------
	Insert into Totais
	SELECT 'TOTAL FATURAMENTO' Descricao,CONVERT(VARCHAR,
					Isnull((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte --AND Data_Cancelamento IS NULL 
					AND Filial = @FILIAL),0)
					+Isnull((SELECT Sum(isnull(convert(decimal(10,2),nf_item.Total),0)) from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte),0)
					+Isnull((SELECT Sum(isnull(Total,0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte),0)
					)  Total

	----# Insere Todas VENDAS CANCELADAS do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'VENDAS CANCELADAS', CONVERT(VARCHAR,ISNULL((SELECT SUM(VLR-isnull(desconto,0)) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND FILIAL = @FILIAL AND Data_Cancelamento IS NOT NULL),0))  				
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'ITENS CANCELADOS PEDIDO', CONVERt(VARCHAR,ISNULL(Sum(isnull(Total,0)),0)) from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte And Pedido.Status in (3)

	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'AJUSTE DEVOLUCAO', isnull(CONVERt(VARCHAR,Convert(Decimal(18,2),Sum(isnull(Contada*Custo,0)))),0) from Inventario_itens itens inner join Inventario i on i.Codigo_inventario =  itens.Codigo_inventario 
		where Data BETWEEN @DataDe AND @DataAte  and status = 'ENCERRADO' And tipoMovimentacao = 'DEVOLUCAO'		
	
	----# Insere Todos ITENS CANCELADOS PEDIDO do PDV em uma tabela temp (Para fazer calculo) #------					
	INSERT into Totais
	SELECT 'NF DEVOLUCAO', CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(10,2),nf.Total),0)),0)) from nf Where nf.codigo_operacao in ('1202', '5202', '5411', '6202') and nf.Tipo_NF=2 and Isnull(nf.nf_Canc,0) = 0 and nf.Emissao between @dataDe and @DataAte		
		
	SELECT '' As Descritivo,'' As Total
	UNION ALL
	SELECT 'VENDAS PEDIDOS SIMPLIFICADOS', replace(CONVERt(VARCHAR,Sum(isnull(convert(decimal(18,2),Total,0),0))),'.',',') from pedido where Tipo=1 and  pedido_simples=1 and pedido.Data_cadastro between @dataDe and @DataAte  
	UNION ALL
	SELECT 'VENDAS NOTA FISCAL', replace(CONVERt(VARCHAR,ISNULL(Sum(isnull(convert(decimal(18,2),nf_item.Total),0)),0)),'.',',') from nf_item inner join nf on nf_item.codigo= nf.Codigo  where nf_item.codigo_operacao in ('5102','5405','5402','5403','6102','6405','6401','6403') and nf_item.Tipo_NF=1 and nf.Emissao between @dataDe and @DataAte
	UNION ALL
	SELECT 'VENDAS CUPOM',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte --AND Data_Cancelamento IS NULL 
			AND Filial = @FILIAL),0)),'.',',')
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'TOTAL FATURAMENTO'
					 
	UNION ALL
				
		SELECT '',''
	UNION ALL
	SELECT 'NUMERO DE CLIENTES', Convert(varchar, (select count(*) from #Cupons))		
	UNION ALL
	SELECT 'VENDAS COM NFP', REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and Filial = @filial and len(isnull(cpf_cnpj,''))>10))),'.',',')
	UNION ALL
	SELECT '% DE VENDAS COM NFP',replace(CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial and len(isnull(cpf_cnpj,''))>10) / (Select sum(isnull(vlr-isnull(desconto,0),0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null)*100)),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS VENDIDOS',REPLACE( CONVERT(VARCHAR,CONVERt(Int,ISNULL((SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND Filial = @FILIAL),0))),'.',',')
	UNION ALL
	SELECT 'QTDE ITENS CANCELADOS VENDA', replace(CONVERT(VARCHAR,ISNULL(convert(Numeric(18),(SELECT SUM(QTDE) FROM SAIDA_ESTOQUE WHERE Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NOT NULL AND Filial = @FILIAL)),0)),'.',',')	
	UNION ALL
	SELECT '',''
	
	UNION ALL
	SELECT Descricao,replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'VENDAS CANCELADAS'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'ITENS CANCELADOS PEDIDO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'AJUSTE DEVOLUCAO'
	UNION ALL
	SELECT Descricao, replace(CONVERT(VARCHAR(90),Total),'.',',') From Totais Where Descricao = 'NF DEVOLUCAO'

	UNION ALL
	
	SELECT 'RESULTADO TOTAL',
		replace(CONVERT(VARCHAR(90),Convert(Decimal(18,2),SUM(Total) - (Select SUM(Total) From TOTAIS  Where Descricao = 'VENDAS CANCELADAS') - (Select SUM(Total) From TOTAIS  Where Descricao = 'ITENS CANCELADOS PEDIDO') - (Select SUM(Total) From TOTAIS  Where Descricao = 'AJUSTE DEVOLUCAO') - (Select Sum(Total) From TOTAIS Where Descricao = 'NF DEVOLUCAO'))),'.',',')
	FROM  TOTAIS
	Where Descricao = 'TOTAL FATURAMENTO'

	UNION ALL
	SELECT '',''
	UNION ALL
	SELECT 'DESCONTOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(desconto,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'ACRESCIMOS SERVICOS',REPLACE( CONVERT(VARCHAR,convert(numeric(18,2),(Select sum(isnull(Acrescimo,0)) from saida_estoque where data_movimento between @DataDe and @DataAte and data_cancelamento is null and filial = @filial))),'.',',')
	UNION ALL
	SELECT 'INDUSTRIA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS EMITIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'CONTRAVALES DIGITAIS RECEBIDOS', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'PAGAMENTO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT 'ESTORNO DE DEPOSITO EM CONTA ASSINADA', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
    SELECT 'GERENCIAL ',replace(CONVERT(VARCHAR,ISNULL((SELECT SUM(convert(decimal(18,2), (VLR-isnull(desconto,0)))) FROM SAIDA_ESTOQUE with( index(IX_Saida_Estoque)) WHERE Filial = @FILIAL AND Data_Movimento BETWEEN @DataDe AND @DataAte AND Data_Cancelamento IS NULL AND CONVERT(NUMERIC, ISNULL(COO,0)) <= 0 ),0)),'.',',')
	UNION ALL
	SELECT 'REPIQUE', CONVERT(VARCHAR,0) + ',00'
	UNION ALL
	SELECT '',''
	UNION ALL
	
	SELECT 'Forma de Pagamento','Valor Total'
	UNION ALL
	SELECT FINALIZADORA.FINALIZADORA,replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO between @dataDe and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL
		GROUP BY FINALIZADORA.FINALIZADORA
	UNION ALL	
	SELECT 'Valor Total',replace(CONVERt(VARCHAR,SUM(TOTAL)),'.',',')
		FROM LISTA_FINALIZADORA 
		INNER JOIN FINALIZADORA ON LISTA_FINALIZADORA.FINALIZADORA = FINALIZADORA.NRO_FINALIZADORA
		WHERE EMISSAO  between @datade and @DataAte and isnull(Cancelado,0) = 0 and Lista_finalizadora.filial  = @FILIAL 
	UNION ALL
	SELECT '',''
	  

go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Movimento_Venda]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Movimento_Venda
end
go

CREATE Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),

                @plu               As varchar(17),

                @cupom             As varchar(20),

                @pdv               as varchar(2),
                
                @horaInicio      as varchar(5),
				
				@horafim	     as varchar(5),
				@cancelados		as varchar(5)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,lista.EMISSAO,103),

                             lista.PDV,

                             CUPOM = lista.DOCUMENTO,

                             VLR = (SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                         ),
                             CANCELADOS = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento),

                             FINALIZADORA = lista.id_finalizadora,
							
							[COMANDA/PEDIDOS] =  (SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And st.data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)
                             

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                                   and pdv like (case when @pdv <> '' then @pdv else '%' end)
								   and (
										@cancelados='TODOS' 
										OR (@cancelados ='SIM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='NAO' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))=0) ) 
																						   
						GROUP BY lista.Emissao, lista.pdv, lista.Documento ,lista.id_finalizadora

           

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR =(SELECT isnull(convert(decimal(18,2),SUM(list1.Total )),0) FROM Lista_finalizadora list1

                             INNER JOIN Finalizadora ON list1.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE list1.Filial = @FILIAL And Isnull(Cancelado,0) = 0 
									AND (list1.Emissao = lista.Emissao)
                                   and list1.pdv =lista.pdv
                                   and list1.documento = lista.documento
                         ),
                             CANCELADO = (SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento
                                    ),

                             FINALIZADORA = id_finalizadora,
                             
                             [COMANDA/PEDIDOS] = (SELECT Max(ComandaPedidoCupom) FROM Saida_estoque st

								WHERE st.Filial = @FILIAL And data_cancelamento is null 
									AND (st.Data_movimento = lista.Emissao)
                                   and st.Caixa_Saida =lista.pdv
                                   and st.documento = lista.documento)

                        FROM

                             Lista_finalizadora lista

                             INNER JOIN Finalizadora ON lista.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE lista.Filial = @FILIAL  AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 

                         and pdv like (case when @pdv <> '' then @pdv else '%' end)
						 and (
										@cancelados='TODOS' 
										OR (@cancelados ='SIM' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																							WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																								AND (st.Data_movimento = lista.Emissao)
																							   and st.Caixa_Saida =lista.pdv
																							   and st.documento = lista.documento))>0) 
										OR (@cancelados='NAO' AND ((SELECT isnull(convert(decimal(18,2),SUM(st.vlr)),0) FROM Saida_estoque st

																						WHERE st.Filial = @FILIAL And data_cancelamento is Not null 
																							AND (st.Data_movimento = lista.Emissao)
																						   and st.Caixa_Saida =lista.pdv
																						   and st.documento = lista.documento))=0) ) 
									
                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                            

                  END

      END

 

ELSE IF (@plu<>'' AND @cupom='')

BEGIN

      SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),
                        Hora = convert(varchar,Hora_venda),

                        pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )

                        and (len(@plu)=0 or s.PLU = @plu )

                        And s.Data_Cancelamento is null

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

						 and s.Data_movimento between @DataDe aND @DataAte
                         and (LEN(@pdv)=0 or l.pdv = @pdv)
                         and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 

                        --Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by l.Emissao , Hora_venda

      END

ELSE

      BEGIN

           

            SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),
					    pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total=replace(convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)),'.',',') 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                       
						union all

--                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
						SELECT '',

                        '|-CANCELADO-|',
					     pdv=convert(varchar,L.pdv) ,

                        '|-'+S.PLU+'-|',

                        '|-'+M.Descricao+'-|',

                        Qtde=replace(convert(varchar,S.Qtde),'.',','),

                        Vlr=replace(convert(varchar,S.vlr),'.',','),

                        [-Desconto]=replace(convert(varchar,isnull(s.desconto,0)),'.',','),

                        [+Acrescimo]=replace(convert(varchar,isnull(s.Acrescimo,0)),'.',','),

                        Total='0,000' 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is NOT null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                        
                        union all

                        select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total)))

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END



	
	