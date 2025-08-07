CREATE NONCLUSTERED INDEX [IX_ID_Caderneta] ON [dbo].[Caderneta] 
(
	[Codigo_Cliente] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO



GO
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

                        Qtde=convert(varchar,S.Qtde),

                        Vlr=convert(varchar,S.vlr),

                        [-Desconto]=convert(varchar,isnull(s.desconto,0)),

                        [+Acrescimo]=convert(varchar,isnull(s.Acrescimo,0)),

                        Total=convert(varchar,(s.vlr-isnull(s.Desconto,0))+ISNULL(s.acrescimo,0)) 

            FROM Saida_estoque S INNER JOIN Lista_finalizadora L ON S.Documento=L.Documento and s.Caixa_Saida = l.pdv

                             INNER JOIN Mercadoria M ON S.PLU = M.PLU      

                        where s.Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                        and s.PLU like (case when @plu <>'' then @plu else '%' end )

                         and s.data_movimento BETWEEN @DataDe  AND  @DataAte
                         
                         and l.Emissao BETWEEN @DataDe  AND  @DataAte

                         And s.Data_Cancelamento is null
						and s.Data_movimento between @DataDe aND @DataAte 
                        and l.pdv like (case when @pdv <> '' then @pdv else '%' end)
                       


--                        Group by s.Documento,l.Emissao,l.pdv,s.plu,m.descricao,s.Qtde,s.vlr,s.Desconto,s.Acrescimo,Hora_venda

                        union all

                        select '','','','', id_finalizadora  ,'','','','', convert(varchar,(SUM(Lista_Finalizadora.Total)))

                             from Lista_finalizadora

                             where  Documento like (case when @cupom <>'' then @cupom  else '%' end  )

                             and Emissao BETWEEN @DataDe  AND  @DataAte

                             And Isnull(Cancelado,0) = 0

                             and pdv like (case when @pdv <> '' then @pdv else '%' end)

                        GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora

                       

      END

go

if not exists(select 1 from PARAMETROS where PARAMETRO='DIAS_CADERNETA')
begin	
INSERT INTO [PARAMETROS]
           ([PARAMETRO]
           ,[PENULT_ATUALIZACAO]
           ,[VALOR_DEFAULT]
           ,[ULT_ATUALIZACAO]
           ,[VALOR_ATUAL]
           ,[DESC_PARAMETRO]
           ,[TIPO_DADO]
           ,[RANGE_VALOR_ATUAL]
           ,[GLOBAL]
           ,[NOTA_PROGRAMADOR]
           ,[ESCOPO]
           ,[POR_USUARIO_OK]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('DIAS_CADERNETA'
           ,GETDATE()
           ,'120'
           ,GETDATE()
           ,'120'
           ,'QUANTIDADE DE DIAS DA PRIMEIRA CONSULTA DA CADERNETA'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO






--PROCEDURES =======================================================================================
ALTER PROCEDURE [dbo].[SP_REL_HISTORIO_ENTRADA_SAIDA](
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@plu as Varchar(20) )
As

--set @DataInicio ='20140401'
--set @DataFim ='20140910'
--Set @plu ='1'
Select Data = CONVERT(varchar,todos.data,103)
		,[Entrada]=Sum(todos.[NF Entrada])
		,[Entrada_Valor]=Sum(todos.[NF Compra Preco])
		,[Saida_Outros] = SUM(todos.[NF Saida])
		,[Saida_Valor] = Sum(todos.[NF Saida Preco])
		,[Cupom] = SUM(todos.[Cupom])
		,[Cupom_Valor] = Sum(todos.[Cupom Preco])
	 from (
select Data =b.data ,
	   [NF Entrada] =isnull((Select SUM (isnull(eitem.Qtde,0)*ISNULL(eitem.Embalagem,0)) 
							from NF_Item Eitem inner join nf Enf  on Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE Eitem.PLU = @plu AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1 ),0) 
	   ,[NF Compra Preco]	= isnull((Select Sum(isnull(eitem.Total,0)) 
							from NF_Item Eitem inner join nf Enf  on Eitem.codigo = Enf.Codigo 
							inner join Natureza_operacao Eop on Enf.Codigo_operacao = Eop.Codigo_operacao 	
							WHERE Eitem.PLU = @plu AND Enf.Data =b.data AND Enf.Tipo_NF = 2 and Enf.nf_Canc <>1 and Eop.NF_devolucao<>1 AND Eop.Baixa_estoque=1 group by eitem.Unitario  ),0) 				
	   ,[NF Saida] =isnull((Select SUM (isnull(sitem.Qtde,0)*ISNULL(sitem.Embalagem,0)) 
								from NF_Item Sitem inner join nf Snf  on sitem.codigo = Snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							
							WHERE Sitem.PLU = @plu AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 ),0) 
	   ,[NF Saida Preco]	= isnull((Select Sum(isnull(sitem.Total,0)) 
							from NF_Item sitem inner join nf snf  on sitem.codigo = snf.Codigo 
							inner join Natureza_operacao Sop on Snf.Codigo_operacao = Sop.Codigo_operacao 	
							WHERE Sitem.PLU = @plu AND Snf.Data =b.data AND Snf.Tipo_NF = 1 and Snf.nf_Canc <>1 and Sop.NF_devolucao<>1 AND Sop.Baixa_estoque=1 group by Sitem.Unitario  ),0) 				
	  ,[Cupom] = 0.00-- isnull((Select SUM(isnull(se.Qtde,0)) from Saida_estoque se with( index(IX_Saida_Estoque_01)) where se.plu =@plu and se.Data_movimento = b.data),0) 
	  ,[Cupom Preco]=0.00	
		
from NF_Item a inner join nf b on a.codigo = b.Codigo 
				inner join Natureza_operacao op on b.Codigo_operacao = op.Codigo_operacao 
		
WHERE A.PLU = @plu AND B.Data between @DataInicio AND @DataFim  and b.nf_Canc <>1 and op.NF_devolucao <>1
group by Data 

UNION ALL

select Data =se.Data_movimento  ,
	   [NF Entrada] =0
	   ,[NF Entrada Preco]	= 0
	   ,[NF Saida] =0
	   ,[NF Saida Preco]	= 0
	  ,[Cupom] = Convert(Decimal(18,2),SUM(isnull(se.Qtde,0)))  
	  ,[Cupom Preco]=Sum(se.vlr)	
from Saida_estoque se with( index(IX_Saida_Estoque_01)) 
where se.PLU=@plu and  se.Data_movimento between @DataInicio AND @DataFim
group by se.Data_movimento,se.vlr
) as todos
group by todos.Data
order by convert(varchar ,todos.Data,102) desc




go


GO


-- sp_Rel_Curva_ABC 'MATRIZ', '20120912', '20120912', NULL, NULL, NULL
-- sp_Rel_Curva_ABC 'MATRIZ', '20110719', '20110719', 10, NULL, NULL
-- sp_Rel_Curva_ABC 'MATRIZ', '20120719', '20120725', null, null, NULL,null,null,null,null
 

ALTER          Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60),
                @Ordem			as Varchar(40)
AS
                
                Declare @String               As nVarchar(2000)
		Declare @Where               As nVarchar(1024)
BEGIN
	
                --** Cria A String Com Os filtros selecionados
		Set @Where = ' WHERE Data_Movimento BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
		Set @Where = @Where + ' And Saida_Estoque.Filial = ' + CHAR(39) + @Filial + CHAR(39) + ' And data_cancelamento is null'
		--** Checa se o parametro @DESCRICAO contem alguma informação. Se tiver, 
                --** o sistema faz um LIKE no campo descrição recebido no parametro.
                BEGIN
                               IF LEN(ISNULL(@DESCRICAO,'')) > 0 
                                               SET @Where = @Where + ' AND Mercadoria.Descricao LIKE '+ CHAR(39) + @DESCRICAO + CHAR(39)
                END       
                --** Checa se o parametro @Grupo contem alguma informação. Se tiver, 
                
                BEGIN
                               IF LEN(ISNULL(@GRUPO,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_GRUPO='+ CHAR(39) + @GRUPO + CHAR(39) 
                               IF LEN(ISNULL(@subGrupo,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_SUBGRUPO='+ CHAR(39) + @subGrupo + CHAR(39) 
							   IF LEN(ISNULL(@Departamento,'')) > 0 
                                               SET @Where = @Where + ' AND w_br_cadastro_departamento.DESCRICAO_DEPARTAMENTO='+ CHAR(39) + @Departamento + CHAR(39) 
							   IF LEN(ISNULL(@Familia,'')) > 0 
                                               SET @Where = @Where + ' AND familia.DESCRICAO_familia='+ CHAR(39) + @Familia + CHAR(39) 
                
                END       
		
		
		
		-- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
                --** Checa se o parametro @nLinhas está com valor maior que 0. Se estiver, 
                --** o sistema retorna o numero de linhas recebido no parametro.
                BEGIN
                               if ISNULL(@nLinhas, 0) > 0 
                                               SET @String = @String + ' TOP ' + CONVERT(VARCHAR,@nLinhas)
                END
                SET @String = @String + ' Mercadoria.PLU, EAN = ISNULL((SELECT MAX(EAN.EAN) From EAN WHERE EAN.PLU = Mercadoria.PLU), ' + CHAR(39) + CHAR(39) + '), Mercadoria.Descricao,' 
                SET @String = @String + ' Qtde = Convert(Numeric(10,3), Sum(Qtde)), '
                SET @String = @String + ' [Total Custo] = Convert(Numeric(12,2), SUM(Qtde * Mercadoria.Preco_Custo)),'
                SET @String = @String + ' [Total Venda] = Convert(Numeric(12,2), Sum(VLR-desconto)), '
                set @String = @String + ' [Lucro Bruto] = Convert(Numeric(12,2),SUM((VLR-desconto) - (Qtde * Mercadoria.Preco_Custo))),'
		Set @String = @String + ' [%] = Convert(Varchar,convert(numeric(12,2), Convert(Numeric(12,2), Sum(VLR-desconto)) / (select sum(convert(numeric(12,2), (vlr-desconto))) from mercadoria inner join saida_estoque on Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia'
		--** Inserindo Clausula Subquery
		Set @String = @String + @Where + ' )* 100)) + '+ char(39)+ '%'+ char(39)
                SET @String = @String + ' From Mercadoria INNER JOIN Saida_Estoque ON Mercadoria.PLU = Saida_Estoque.PLU inner join w_br_cadastro_departamento on mercadoria.codigo_departamento = w_br_cadastro_departamento.codigo_departamento left join familia on mercadoria.codigo_familia=familia.codigo_familia '
 		--**Adciona aqui a Variavel @where da Query
		set @String = @String + @Where
		Begin
			if LEN(ISNULL(@Ordem,'')) = 0 
			Set @Ordem ='Qtde'
		end
		
                SET @String = @string + ' GROUP BY  Mercadoria.PLU, Mercadoria.Descricao  Order by ['+@Ordem+'] Desc'
 
 --PRINT @STRING
                EXECUTE(@String)
END


go




create procedure sp_Rel_Caderneta_Utilizado
@Filial as varchar(20)
,@codigo_cliente as varchar(8)
,@nome_cliente as varchar(50)
as 
Begin
	Select Codigo_Cliente,Nome_Cliente,Limite_Credito,Utilizado,Saldo=(Limite_Credito-Utilizado)  from Cliente
	Where (LEN(@codigo_cliente)=0 or Codigo_Cliente=@codigo_cliente)
	and (LEN(@nome_cliente)=0 or Nome_Cliente like '%'+@nome_cliente+'%' )
end

