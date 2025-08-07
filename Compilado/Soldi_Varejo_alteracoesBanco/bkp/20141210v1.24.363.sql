alter table lista_finalizadora add  Troco decimal

GO
/****** Object:  StoredProcedure [dbo].[sp_Movimento_Venda]    Script Date: 12/05/2014 09:30:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- SP_MOVIMENTO_VENDA 'MATRIZ', '20141202', '20141202', 'DINHEIRO'
CREATE Procedure [dbo].[sp_Movimento_Venda]
                @Filial          As Varchar(20),
                @DataDe          As Varchar(8),
                @DataAte         As Varchar(8),
                @finalizadora    As varchar(30)
AS
IF(@finalizadora ='')
	BEGIN
		SELECT 
			DATA = CONVERT(VARCHAR,EMISSAO,102), 
			PDV, 
			CUPOM = DOCUMENTO, 
			VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
			FINALIZADORA = id_finalizadora 
		FROM 
			Lista_finalizadora 
			INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
		WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
		GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
 
	END
ELSE
	BEGIN
		SELECT 
			DATA = CONVERT(VARCHAR,EMISSAO,102), 
			PDV, 
			CUPOM = DOCUMENTO, 
			VLR = SUM(Lista_Finalizadora.Total - ISNULL(Lista_Finalizadora.Troco, 0)), 
			FINALIZADORA = id_finalizadora 
		FROM 
			Lista_finalizadora 
			INNER JOIN Finalizadora ON Lista_finalizadora.finalizadora = finalizadora.Nro_Finalizadora  
		WHERE Lista_Finalizadora.Filial = @FILIAL AND (Emissao BETWEEN @DataDe  AND  @DataAte )
		AND finalizadora.Finalizadora  = @finalizadora  
		GROUP BY Emissao, PDV, DOCUMENTO ,id_finalizadora
 
	END



GO
/****** Object:  StoredProcedure [dbo].[sp_Rel_Curva_ABC]    Script Date: 12/10/2014 13:47:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- sp_Rel_Curva_ABC 'MATRIZ', '20120912', '20120912', NULL, NULL, NULL
-- sp_Rel_Curva_ABC 'MATRIZ', '20110719', '20110719', 10, NULL, NULL
-- sp_Rel_Curva_ABC 'MATRIZ', '20120719', '20120725', null, null, NULL,null,null,null
 

ALTER          Procedure [dbo].[sp_Rel_Curva_ABC] 
                @Filial                  As Varchar(20),
                @DataDe                            As Varchar(8),
                @DataAte          As Varchar(8),
                @nLinhas           As Integer,
                @Descricao        As Varchar(60),
                @Grupo            As Varchar(60),
                @subGrupo		as Varchar(60),
                @Departamento		as Varchar(60),
                @Familia		as Varchar(60)
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
                SET @String = @string + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao '
 

-- PRINT @STRING
                EXECUTE(@String)
END
 go







--sp_rel_conta_a_receber 'MATRIZ', '20141125', '20141125', 'emissao', '029'
ALTER  procedure [dbo].[sp_rel_conta_a_receber](
	@filial		varchar(20),
	@datade		varchar(8),
	@dataate	varchar(8),
	@tipo		varchar(50),
	@valor		VARCHAR(11) ,
	@cliente	varchar(250)
)As

Declare @String as nvarchar(2000)
Declare @Where as nvarchar(2000)


Begin
	--Monta Clausula Where da Procura
	set @where = 'Where Conta_a_receber.Filial = '+ char(39) + @filial + char(39) + ' and conta_a_receber.status <>3 and conta_a_receber.status <>4  and '
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
go 




alter table pedido add pedido_simples tinyint,centro_custo varchar(10)


GO

INSERT INTO PARAMETROS (PARAMETRO,VALOR_ATUAL,PERMITE_POR_EMPRESA,PENULT_ATUALIZACAO,ULT_ATUALIZACAO,GLOBAL,POR_USUARIO_OK ) 
		values ('PEDIDO_SIMPLES','FALSE',0,GETDATE(),GETDATE(),0,0)



