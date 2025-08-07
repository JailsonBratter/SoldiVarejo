IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_ProdutosAlterados]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_ProdutosAlterados]
end
GO
--PROCEDURES =======================================================================================
CREATE   Procedure [dbo].[sp_Rel_ProdutosAlterados]

                @Filial                 As Varchar(20),
                @plu					as Varchar(20),
                @ean					as Varchar(20),
                @ncm					as Varchar(20),
             	@Ref_Fornecedor         As Varchar(20),
                @Descricao              As Varchar(60) ,
                @DataDe                 As Varchar(8),
                @DataAte                As Varchar(8),
                @grupo					As varchar(20)
				,@subGrupo				As varchar(20)
				,@departamento			As varchar(20)
				,@familia				As varchar(40),
				@custoMargem			as varchar(40)


AS

Declare @String               As Varchar(max)
Declare @Where               As Varchar(max)

BEGIN
                --** Cria A String Com Os filtros selecionados

	
                               -- ** Começa a Criar query na Variavel String
		SET @String = 'SELECT '
		SET @String = @String + 'mercadoria.PLU,  isnull(e.EAN , '+ CHAR(39) + CHAR(39) +') as EAN, NCM = ISNULL(MERCADORIA.CF, '+ CHAR(39) + CHAR(39) +'),'
		SET @String = @String + 'mercadoria.ref_fornecedor AS REF_FORN, mercadoria.DESCRICAO , '
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin

		SET @String = @String + 'CONVERT(VARCHAR, mercadoria.Data_Alteracao, 103) As DATA_ALTERACAO,'
		end		
		if(@custoMargem='CUSTO-MARGEM-VENDA' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
			set @String = @String + ' mercadoria_loja.PRECO_CUSTO,MARGEM = convert(decimal(10,2), Case when mercadoria_loja.Preco_Custo > 0 and mercadoria_loja.Preco > 0 then'
			SET @String = @String + '((mercadoria_loja.Preco - mercadoria_loja.Preco_Custo ) / mercadoria_loja.Preco_Custo ) * 100 else 0 end),'
		end
	
	
		SET @String = @String + 'mercadoria_loja.preco as VENDA '
		if(@custoMargem='PROMOCAO' )
		begin
			SET @String = @String + ',Mercadoria_Loja.Preco_Promocao AS PROMOCAO, convert(varchar, mercadoria_loja.Data_Inicio,103) AS INICIO, CONVERT(varchar ,Mercadoria_Loja.Data_Fim,103) AS FIM '
		end
		
		
		SET @String = @String + ' from mercadoria inner join mercadoria_loja on mercadoria.plu = mercadoria_loja.plu'
        SET @String = @String + ' left join EAN e on mercadoria.plu=e.PLU
								 inner join W_BR_CADASTRO_DEPARTAMENTO c on (mercadoria.codigo_departamento= c.codigo_Departamento and mercadoria.filial=c.filial)'
                               --PRINT @STRING + @Where + ' GROUP BY Mercadoria.PLU, Mercadoria.Descricao ORDER BY vlr DESC'
		Set @Where = ' WHERE  (Mercadoria_Loja.Filial = ' + CHAR(39) + @Filial + CHAR(39)+')'
        
         IF (LEN(@DESCRICAO) > 0)
		BEGIN
            SET @Where = @Where + ' AND ( mercadoria.DESCRICAO LIKE '+ CHAR(39) +'%'+ @DESCRICAO + '%'+CHAR(39)+')'

        END      
		if(@custoMargem='PROMOCAO')
		begin
			SET @Where = @Where + ' AND ( mercadoria_loja.Promocao=1) '

		END        
        
		if(@custoMargem='ALTERADOS' or @custoMargem='ALTERADOS-CUST-MARG-VENDA' )
		begin
		
			Set @Where = @Where +' And (Mercadoria.Data_Alteracao BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)+')'
		end
	       
        
      if LEN(@plu)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.plu = '+ CHAR(39) + @plu + CHAR(39)+')'

      end	
      if LEN(@ean)>0
      begin
		    SET @Where = @Where + ' AND (e.ean = '+ CHAR(39) + @ean + CHAR(39)+')'

      end	
      if LEN(@ncm)>0
      begin
		    SET @Where = @Where + ' AND (mercadoria.CF = '+ CHAR(39) + @ncm + CHAR(39)+')'

      end	
      
      IF LEN(ISNULL(@Ref_Fornecedor,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (Mercadoria.Ref_Fornecedor LIKE '+ CHAR(39) + @Ref_Fornecedor + CHAR(39)+')'

        END     
	
	  IF LEN(ISNULL(@grupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_grupo = '+ CHAR(39) + @grupo + CHAR(39)+')'

        END     
	IF LEN(ISNULL(@subGrupo,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND (c.descricao_subgrupo = '+ CHAR(39) + @subGrupo + CHAR(39)+')'

        END     
        IF LEN(ISNULL(@departamento,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND c.descricao_departamento = '+ CHAR(39) + @departamento + CHAR(39)

        END    
	   IF LEN(ISNULL(@familia,'')) > 0
	   BEGIN

            SET @Where = @Where + ' AND mercadoria.descricao_familia = '+ CHAR(39) + @familia + CHAR(39)
		end
     	
	 

		--print(@Where)
	   EXECUTE(@String + @Where)

END

 

go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Produto_Fornecedor]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Produto_Fornecedor]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Rel_Produto_Fornecedor]

      @filial varchar(20),

      @Fornecedor varchar(40)

AS

 

SELECT DISTINCT

      m.PLU,

      m.Ref_fornecedor,

      EAN = ISNULL(e.ean,''),

      m.descricao,

      l.Preco_Custo,

      l.Preco

FROM

      Mercadoria m

      INNER JOIN Mercadoria_Loja l ON m.PLU = l.PLU

      LEFT OUTER JOIN EAN e ON m.PLU = e.PLU

      INNER JOIN NF_ITEM ni ON m.PLU = ni.PLU

      INNER JOIN Fornecedor f ON f.Fornecedor = ni.Cliente_Fornecedor

WHERE

      f.Fornecedor like '%'+@Fornecedor+'%'

      OR

      f.Razao_social like '%' + @Fornecedor + '%'

 

 

 
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_rel_cliente]
end
GO
--PROCEDURES =======================================================================================
CREATE procedure [dbo].[sp_rel_cliente]  
  @Filial                 As Varchar(20),
  @codigo as varchar(30),
  @nome as varchar(50),
  @cnpj as varchar(20),
  @tabela as varchar(10),
  @tipoRel as varchar(40)
  
  as
  
  Begin


		if(@tipoRel = 'NAO')
		BEGIN
		 select '|-CODIGO:-|'+a.codigo_cliente+ '|| |-NOME-|: '+a.nome_cliente +' || |-CNPJ-|: '+ isnull(a.cnpj,'_____________________')+ '  |-IE-|: '+ISNULL(a.Ie,'_____________________')+  '|| ||' AS 'DADOS GERAIS',
			'|-ENDERECO-|:'+isnull(a.endereco,'_______________________________') +','+isnull(a.endereco_nro,'___')+' |-BAIRRO-|:'+isnull(a.Bairro,'____________')+' |-CIDADE-|:'+ isnull(a.cidade,'_________')+' |-UF-|:'+isnull(a.Uf,'__') + '||' +
			'|-DATA CADASTRO-|:'+ convert(varchar,a.data_cadastro,103)+ ' || |-TELEFONE-| :' + ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%') ),'_____________') +
			' |-E-MAIL-|:'+ ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'),'_________________________')  +  '|| ||'
			as 'DETALHES'
			from cliente as  a
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)	
		END
		ELSE
		BEGIN

		  select 
				a.codigo_cliente As CODIGO, 
				a.nome_cliente as NOME,
				TELEFONE= 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS b where b.codigo_cliente = a.codigo_cliente and (b.meio_comunicacao like 'FONE%' OR b.meio_comunicacao like 'CELULAR%')), ''),
				EMAIL = 'SFT_'+ISNULL((Select  top 1 id_meio_comunicacao from cliente_contato AS c where c.codigo_cliente = a.codigo_cliente and c.meio_comunicacao like 'EMAIL%'), '')
			from cliente as  a
			WHERE (LEN(@codigo)=0 OR a.Codigo_Cliente = @codigo)
				  AND (LEN(@nome)=0 OR  a.Nome_Cliente like '%'+@nome+'%')
				  and (len(@cnpj)=0 or replace(replace(replace(a.CNPJ,'.',''),'-',''),'/','')= replace(replace(replace(@cnpj,'.',''),'-',''),'/',''))
				  and (@tabela='TODOS' OR A.Codigo_tabela = @tabela)	
		   END
END