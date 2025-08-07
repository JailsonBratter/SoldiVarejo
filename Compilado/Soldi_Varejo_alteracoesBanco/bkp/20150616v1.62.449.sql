go 

alter table conta_a_receber add nota_servico tinyint

go

ALTER TABLE INVENTARIO_ITENS ALTER COLUMN CUSTO DECIMAL(12,2)

GO
alter table familia add Imprimir_Etiqueta_itens tinyint
go

Alter table Filial add data_retira_oferta datetime

GO


CREATE procedure sp_rel_imposto_cadastrado(
@filial varchar(20),
@plu varchar(20)
,@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@DataDe DateTime
,@DataAte DateTime
,@NCM VARCHAR(20)
)as





Select DISTINCT
      mercadoria.PLU,
      Ean,
      NCM=CF ,
      Descricao,
      Cst_I_E = (select top 1 tributacao.indice_st from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao_ent),
      [Icms Entrada] = (select top 1 tributacao.entrada_icms from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao_ent),
      cst_I_S = (select top 1 tributacao.indice_st from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao),
      [Icms Saida] = (select top 1 tributacao.Saida_ICMS from tributacao where tributacao.codigo_tributacao = mercadoria.codigo_tributacao),
      --Codigo_Natureza_receita,
      Cst_P_E= cst_entrada ,
      Cst_P_S = cst_saida 
      
From

      Mercadoria  left join EAN e on mercadoria.plu=e.PLU
      inner join W_BR_CADASTRO_DEPARTAMENTO c on (Mercadoria.codigo_departamento= c.codigo_Departamento and Mercadoria.filial=c.filial)

Where

		Inativo = 0
		and (
			mercadoria.plu in (select plu from Saida_estoque WITH(INDEX(IX_SAIDA_ESTOQUE_01)) where filial=@filial and Data_movimento between @DataDe and @DataAte and data_cancelamento is null group by plu)
			or 
			mercadoria.plu in (select plu from nf_item WITH(INDEX(PK_NF_ITEM)) inner join nf WITH(index(PK_NF)) on nf_item.codigo= nf.Codigo and nf_item.Tipo_NF =nf.Tipo_NF and nf_item.Cliente_Fornecedor=nf.Cliente_Fornecedor and nf_item.Filial = nf.Filial where nf.Filial=@filial and nf.data between @DataDe and @DataAte and nf.nf_Canc<>1 group by plu)
			
			) 
		and (E.EAN=@ean or LEN(@ean)=0)
		and (Mercadoria.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (Mercadoria.Descricao_familia = @familia or LEN(@familia)=0)										 
		and (Mercadoria.cf = @NCM or len(@NCM)=0)
     
      
      
   

GO

 

/****** Object:  StoredProcedure [dbo].[sp_Rel_ProdutosAlterados]    Script Date: 06/19/2015 11:32:53 ******/

SET ANSI_NULLS ON

GO

 

SET QUOTED_IDENTIFIER ON

GO

 

 

-- sp_Rel_ProdutosAlterados 'MATRIZ', null, null, '20150422', '20150519'

CREATE      Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),

                                                               @Ref_Fornecedor                                         As Varchar(20) = '',

                @Descricao                                                      As Varchar(60) = '',

                @DataDe                 As Varchar(8),

                @DataAte                                                         As Varchar(8)

AS

               

                Declare @String               As nVarchar(1024)

                                                               Declare @Where               As nVarchar(1024)

BEGIN

                --** Cria A String Com Os filtros selecionados

                               Set @Where = ' WHERE Mercadoria.Data_Alteracao BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)

                               Set @Where = @Where + ' And Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)

                               --** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver,

                --** o sistema faz um LIKE no campo descrição recebido no parametro.

        BEGIN

            IF LEN(ISNULL(@DESCRICAO,'')) > 0

            SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)

        END      

 

        BEGIN

            IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0

            SET @Where = @Where + ' AND Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)

        END      

                              

                              

                               -- ** Começa a Criar query na Variavel String

                               SET @String = 'SELECT '

 

                               SET @String = @String + 'mercadoria.PLU, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'), isnull((select top 1 ean.EAN from ean where ean.plu = mercadoria.plu), '+ CHAR(39) + CHAR(39) +') as ean,'

                               SET @String = @String + 'mercadoria.ref_fornecedor, mercadoria.descricao, CONVERT(VARCHAR, mercadoria.Data_Alteracao, 102) As DataAlteracao, mercadoria_loja.preco_custo,'

                               SET @String = @String + 'margem = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'

                               SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'

                               SET @String = @String + 'mercadoria_loja.preco as Venda from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu'

 

                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'

                               EXECUTE(@String + @Where)

END

 

GO

 



go

ALTER PROCEDURE [dbo].[sp_rel_Produto_Alterado] 
@filial varchar(20),
@plu varchar(20),@ean varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
,@DataDe DateTime
,@DataAte DateTime
,@NCM VARCHAR(20)
,@REF VARCHAR(20)
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

select PLU=m.plu,e.EAN,NCM=M.CF,[REF FORN]= M.Ref_fornecedor, DESCRICAO=m.descricao,GRUPO=c.Descricao_grupo,SUBGRUPO=C.descricao_subgrupo,DEPARTAMETO=C.descricao_departamento,
		PRECO=l.Preco,PROMOCAO= case when l.promocao=1then l.preco_promocao else 0 end,
		M.USUARIO ,ALTERADO =CONVERT(VARCHAR,M.Data_Alteracao,103) 		  
	
	from mercadoria m inner join mercadoria_loja l on m.plu = l.PLU
								 left join EAN e on m.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (m.codigo_departamento= c.codigo_Departamento and m.filial=c.filial)
		WHERE M.Data_Alteracao BETWEEN @DataDe AND @DataAte
			AND (M.PLU=@plu or len(@plu)=0)
		and (E.EAN=@ean or LEN(@ean)=0)
		and (M.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (c.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (c.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (c.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (M.Descricao_familia = @familia or LEN(@familia)=0)										 
		and l.Filial = @filial
		and (m.cf = @NCM or len(@NCM)=0)
		and (m.Ref_fornecedor like @REF or LEN(@REF)=0) 





GO
/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 06/16/2015 16:08:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[sp_Movimento_Venda] 'matriz','20150613','20150613','',1,'','','00:00','23:00'

ALTER Procedure [dbo].[sp_Movimento_Venda]

                @Filial          As Varchar(20),

                @DataDe          As Varchar(8),

                @DataAte         As Varchar(8),

                @finalizadora    As varchar(30),

                @plu               As varchar(17),

                @cupom             As varchar(20),

                @pdv               as varchar(2),
                
                @horaInicio      as varchar(5),
				
				@horafim	     as varchar(5)

AS

 

IF(@plu='' AND @cupom='')

      BEGIN

            IF(@finalizadora ='')

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR = convert(decimal(18,2),SUM(Lista_Finalizadora.Total )),

                             FINALIZADORA = id_finalizadora

                        FROM

                             Lista_finalizadora

                             INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE Lista_Finalizadora.Filial = @FILIAL And Isnull(Cancelado,0) = 0 AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                                   and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

           

                  END

            ELSE

                  BEGIN

                        SELECT

                             DATA = CONVERT(VARCHAR,EMISSAO,103),

                             PDV,

                             CUPOM = DOCUMENTO,

                             VLR = convert(decimal(18,2),SUM(Lista_Finalizadora.Total )),

                             FINALIZADORA = id_finalizadora

                        FROM

                             Lista_finalizadora

                             INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora 

                        WHERE Lista_Finalizadora.Filial = @FILIAL And Isnull(Cancelado,0) = 0 AND (Emissao BETWEEN @DataDe  AND  @DataAte )

                        AND finalizadora.Finalizadora  = @finalizadora 

                         and pdv like (case when @pdv <> '' then @pdv else '%' end)

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

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where (LEN(@cupom)=0 or  s.Documento  =  @cupom  )

                        and (len(@plu)=0 or s.PLU = @plu )

                        And s.Data_Cancelamento is null

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         and (LEN(@pdv)=0 or l.pdv = @pdv)
                         and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 

                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda
                        order by l.Emissao , Hora_venda

      END

ELSE

      BEGIN

           

            SELECT S.Documento,

                        Emissao = CONVERT(VARCHAR,L.Emissao,103),
					    pdv=convert(varchar,L.pdv) ,

                        S.PLU,

                        M.Descricao,

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null

                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                       


                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda

                        union all

                        select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0))))

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END

 
GO

GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Pedido_Venda_Analitico]    Script Date: 06/16/2015 16:07:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- sp_Rel_Pedido_Venda_Analitico 'MATRIZ', '20150529', '20150529', '', '', '', '', ''
CREATE PROCEDURE [dbo].[sp_Rel_Pedido_Venda_Analitico]
            @Filial					As Varchar(20),
            @DataDe                 As Varchar(8),
            @DataAte				As Varchar(8),
			@PLU					As Varchar(17) = '',
			@REF_Fornecedor			As Varchar(20) = '',
            @Descricao				As Varchar(60) = '',
			@Cliente				As Varchar(40) = '',
			@Simples    			As VarChar(3) = ''
AS
                
	Declare @String               As nVarchar(3000)
	Declare @String2			  As nVarchar(1024)
	Declare @Where                As nVarchar(1024)

	SET @Where = ' WHERE Pedido.Tipo = 1 AND PEDIDO.Status IN (1,2) '
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
	-- ** Filtro Pedido Simples
	IF @Simples <> '' 
		BEGIN
			IF @Simples = 'SIM'
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) = 1'
			ELSE
				SET @Where = @Where + ' AND ISNULL(Pedido.Pedido_Simples, 0) <> 1'
		END
	BEGIN
		SET @String = 'SELECT' 
		SET @String = @String + ' PedS = CASE WHEN Pedido.Pedido_Simples = 1 then ' + CHAR(39) + 'SIM' + CHAR(39) + ' ELSE ' + CHAR(39) + 'NAO' + CHAR(39) + ' END,' 
		SET @String = @String + ' Data = CONVERT(VARCHAR, Pedido.Data_cadastro, 102) , Pedido.Pedido,'
		SET @String = @String + ' Cliente.Nome_Cliente, ISNULL(Pedido.Funcionario, ' + CHAR(39) + CHAR(39) + ') AS Vendedor, MERCADORIA.PLU, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String = @String + ' Qtde= CONVERT(NUMERIC(12,2), ISNULL(SUM(PEDIDO_ITENS.QTDE * Pedido_Itens.Embalagem), 0)), '
		SET @String = @String + ' Vlr = CONVERT(DECIMAL(12,2), ISNULL(SUM(PEDIDO_ITENS.TOTAL),0))'
		SET @String = @String + ' FROM Pedido '
		SET @String2 = ' INNER JOIN Pedido_ITENS ON PEDIDO_ITENS.Pedido = PEDIDO.Pedido'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria ON Mercadoria.PLU = Pedido_itens.PLU'
		SET @String2 = @String2 + ' INNER JOIN Mercadoria_Loja ON Pedido_itens.PLU = MERCADORIA_LOJA.PLU '
		SET @String2 = @String2 + ' INNER JOIN Cliente ON cliente.Codigo_Cliente = pedido.Cliente_Fornec '
		SET @String2 = @String2 + @Where 
		SET @String2 = @String2 + ' GROUP BY MERCADORIA.PLU, '
		SET @String2 = @String2 + ' Pedido.Data_cadastro, Pedido.Pedido, REF_FORNECEDOR, MERCADORIA.DESCRICAO,'
		SET @String2 = @String2 + ' Pedido.Cliente_Fornec, Pedido.Funcionario, Pedido.pedido_simples, Cliente.Nome_Cliente '
	END
EXEC (@String + @String2)
--PRINT (@sTRING + @sTRING2)
 

 


 
