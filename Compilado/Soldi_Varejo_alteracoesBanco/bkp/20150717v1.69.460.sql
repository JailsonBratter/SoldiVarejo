if exists(select 1 from syscolumns where id =OBJECT_ID('fornecedor_meio_comunicacao') and name='id_comunicacao')
	alter table fornecedor_meio_comunicacao alter column id_comunicacao varchar(50)

else	
	
	alter table fornecedor_meio_comunicacao add id_comunicacao varchar(50)


go

IF NOT EXISTS( SELECT TABELA_COLUNA FROM SEQUENCIAIS WHERE TABELA_COLUNA ='TRIBUTACAO.CODIGO_TRIBUTACAO')
BEGIN

insert into SEQUENCIAIS (TABELA_COLUNA,DESCRICAO,SEQUENCIA,TAMANHO,PERMITE_POR_EMPRESA)
			VALUES('TRIBUTACAO.CODIGO_TRIBUTACAO','SEQUENCIA TRIBUTACAO',(SELECT MAX(CODIGO_TRIBUTACAO)FROM Tributacao),5,0)

END
GO



IF NOT EXISTS( SELECT TABELA_COLUNA FROM SEQUENCIAIS WHERE TABELA_COLUNA ='FUNCIONARIO.CODIGO')
BEGIN
INSERT INTO [SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('FUNCIONARIO.CODIGO'
           ,'CODIGO DE FUNCIONARIO'
           ,'000'
           ,3
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0
           )
           
   END
GO


IF NOT EXISTS( SELECT TABELA_COLUNA FROM SEQUENCIAIS WHERE TABELA_COLUNA ='OPERADORES.ID_OPERADOR')
BEGIN
INSERT INTO [SEQUENCIAIS]
           ([TABELA_COLUNA]
           ,[DESCRICAO]
           ,[SEQUENCIA]
           ,[TAMANHO]
           ,[OBS1]
           ,[OBS2]
           ,[OBS3]
           ,[OBS4]
           ,[OBS5]
           ,[OBS6]
           ,[OBS7]
           ,[OBS8]
           ,[DATA_PARA_TRANSFERENCIA]
           ,[PERMITE_POR_EMPRESA])
     VALUES
           ('OPERADORES.ID_OPERADOR'
           ,'CODIGO DE OPERADOR'
           ,'000'
           ,3
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,''
           ,GETDATE()
           ,0
           )
           
   END
GO





go

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


