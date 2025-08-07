-- ALTERACO TABELA =========================================================================================================================

ALTER TABLE FUNCIONARIO ADD AGENDA TINYINT
GO




--PROCEDURE ================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Cons_Cadastro_Mercadoria
end
GO

CREATE PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(3000)
    Declare @StringSQL2 As nVarChar(3000)
    Declare @Where        As nVarChar(1000)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
	DELETE FROM BUSCA_PRECO;
	insert into Busca_Preco 
	SELECT DISTINCT  PLU,Descricao_resumida,PRECO = CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END  
	FROM (

	SELECT [plu] = CONVERT(BIGINT, MERCADORIA.PLU), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim  
		FROM Mercadoria INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU  
		WHERE ISNULL(Inativo, 0) <= 0    AND MERCADORIA_LOJA.Filial = @Filial

	UNION ALL 


	SELECT [plu] = CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim FROM Mercadoria  INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU    INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU 
	 WHERE ISNULL(Inativo, 0) <= 0   AND MERCADORIA_LOJA.Filial = @Filial
	   
	 GROUP BY CONVERT(BIGINT, EAN.EAN),Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, MERCADORIA_LOJA.preco_promocao, MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim 
	 HAVING CONVERT(BIGINT, EAN.EAN) <=99999999999999 
	) A
	GROUP BY PLU,Descricao_resumida,CASE WHEN Data_Inicio>=CONVERT(VARCHAR,GETDATE(),102) AND Data_Fim <= CONVERT(VARCHAR,GETDATE(),102) THEN Preco_Promocao ELSE PRECO END ;


    
    
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    Mercadoria.descricao, Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' WHERE ISNULL(Inativo, 0) <= 0  '

    IF @TipoCadastro < 100
    BEGIN
            SET @Where = ' '
        END
    ELSE
        BEGIN
            SET @Where = ' AND Peso_Variavel.Codigo > 0 '
        END
    IF @Alterados =1
        begin
            SET @Where = @Where+ ' AND estado_mercadoria=1'
        end

    SET @Where = @Where+ ' AND MERCADORIA_LOJA.Filial = ' + CHAR(39) + @Filial + CHAR(39)

    -- Adicionar o EAN
    IF @TipoCadastro < 100
        BEGIN
            SET @StringSQL2 = ' UNION ALL SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, EAN.EAN), EAN.EAN, '
                + '    Mercadoria.descricao, Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
                + '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' WHERE ISNULL(Inativo, 0) <= 0 '

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 

GO 

--PROCEDURE ================================================================================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_simplificado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Venda_pedido_simplificado]
end
GO

Create PROCEDURE [dbo].[sp_Rel_Venda_pedido_simplificado] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5),
		@nome		as Varchar(30) = ''
		
    	as
			-- exec sp_rel_venda_pedido_simplificado 'MATRIZ','20150801','20150915','','',''
	
if len(@cliente)>0
begin

	SELECT  P.PLU ,M.Descricao, QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),2),UNITARIO = ROUND( UNITARIO ,2)
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')   )   
	And PD.Data_cadastro between @DataDe and @DataAte
	GROUP BY P.PLU,M.Descricao,P.unitario
	UNION ALL
	SELECT '','TOTAL',QDE=ROUND(SUM(P.Qtde*P.Embalagem),2),VALOR=ROUND(SUM(p.total),0),0
	FROM Pedido_itens P INNER JOIN Mercadoria M ON P.PLU = M.PLU
						INNER JOIN Pedido PD ON P.Pedido =PD.Pedido
						inner join cliente on PD.Cliente_Fornec= cliente.Codigo_Cliente 
	where CONVERT(VARCHAR, PD.pedido_simples) LIKE case when @simples='SIM' THEN  '1' ELSE case when @simples='NAO' THEN '0' ELSE '%' END END
	AND PD.Tipo =1 
	AND Pd.Status in (1,2)
	AND	 (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/','')    )
	And PD.Data_cadastro between @DataDe and @DataAte

	--GROUP BY P.PLU,M.Descricao,P.unitario	
	
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
			where  P.Pedido = Pedido.Pedido And P.Filial = Pedido.Filial And P.tipo = Pedido.Tipo),
			
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
			where   P.Filial = Pedido.Filial  AND PD.Data_cadastro between @DataDe and @DataAte) ,
			
			[Total Venda]=ROUND(SUM(Total),2), 

			[Total Pedido] = (SELECT Convert(Decimal(12,2),Isnull(SUM(P.Qtde*P.Embalagem*(P.unitario)),0))
			FROM Pedido_itens P INNER JOIN PEDIDO PD ON PD.Pedido = P.PEDIDO And P.tipo = PD.Tipo where  P.Filial = Pedido.Filial  AND PD.Data_cadastro between @DataDe and @DataAte ),
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


go

-- PROCEDURE =============================================================================

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Venda_pedido_cliente]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_Venda_pedido_cliente]
end
GO


CREATE PROCEDURE [dbo].[sp_Rel_Venda_pedido_cliente] 
		@FILIAL 	AS VARCHAR(17),
		@DataDe		As Varchar(8),
		@DataAte	As Varchar(8),
		@cliente   as varchar(40),
		@simples	as varchar(5)
		
    	as
			-- exec sp_Rel_Venda_pedido_cliente 'MATRIZ','20141201','20141220','',''
	
		Select 
			Simples = case when Pedido.pedido_simples=1 then 'SIM'ELSE 'NAO' END ,
			Pedido,
			convert(varchar,pedido.Data_cadastro,103) Data, 
			Cliente_Fornec Cod,
			cliente.Nome_Cliente,
			[Total Venda]=ROUND(Total,2), 
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
			(len(@cliente) =0 or  (Cliente.Codigo_Cliente = @cliente OR  replace(replace(replace(CLIENTE.CNPJ,'.',''),'-',''),'/','')  = replace(replace(replace(@cliente,'.',''),'-',''),'/',''))) 
		
				 





 go
 
 --PROCEDURE ================================================================================================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_NotasFiscais]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [sp_Rel_NotasFiscais]
end
GO

CREATE procedure [dbo].[sp_Rel_NotasFiscais]

     

      @Filial           varchar(20),                -- Loja

      @DataDe           varchar(8),                 -- Data Inicial

      @DataAte          varchar(8),                 -- Data Fim

      @Tipo             varchar(20),                -- Tipo = 1 - Saidas, Tipo = 2 - Entrada

      @Nota             varchar(20),                -- Nmero Nota Fiscal

      @Fornecedor       varchar(20),                -- Nome Fornecedor
      
      @NF_CANC          varchar(20),		    -- Normal = NAO, Cancelada = SIM
      @plu			   varchar(20),
      @ean			   varchar(20),
      @ref             varchar(20),
      @descricao		varchar(40),
      @NF_Devolucao    varchar(20),
      @Excluir_Natureza varchar(20),
      @Incluir_Natureza varchar(20)	

As

      DECLARE @NF varchar(5000)
      --Verifica se sera aplicado filtro por Nota

      IF LTRIM(RTRIM(@Nota)) <> ''
		begin
            SET @NF = 'Select isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor, Plu, Ref=CODIGO_REFERENCIA,nf_item.Descricao,nf_item.Qtde,nf_item.Embalagem,nf_item.Unitario,nf_item.Total '
            SET @NF = @nf +' from NF_Item INNER JOIN NF ON NF.CODIGO=NF_ITEM.CODIGO and nf.Tipo_NF = nf_item.Tipo_NF and nf.Cliente_Fornecedor= nf_item.Cliente_Fornecedor INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '
			SET @NF = @nf+'	where nf.codigo = '+@nota
			
			IF LTRIM(RTRIM(@Tipo)) = '1-Saida'
				SET @NF = @NF + 'AND NF.Tipo_NF = 1 '
			IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'
				SET @NF = @NF + 'AND NF.Tipo_NF = 2 '
			IF LTRIM(RTRIM(@Fornecedor)) <> ''
                SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

		end
	else
	begin
     

      SET @NF = 'SELECT NF.Filial,Tipo= case when nf.tipo_nf=1 then '+CHAR(39)+'SAIDA'+CHAR(39)+' else '+CHAR(39)+'ENTRADA'+CHAR(39)+' END, NF.Codigo ' + 'Nota' + ', isnull((Case When NF.Tipo_NF = 1 Then (Select Distinct Ltrim(Rtrim(Nome_Cliente)) From Cliente c where c.Codigo_Cliente = NF.Cliente_Fornecedor) Else  NF.Cliente_Fornecedor End),nf.cliente_fornecedor) Cliente_Fornecedor' + ', Convert(Varchar,NF.Emissao,103) ' + 'Emisso'

      SET @NF = @NF + ', Convert(Varchar,NF.Data,103) ' + 'Entrada' + ',Convert(decimal(15,2),Sum(NF_Item.Total+Isnull(NF.Frete,0))) ' + 'VlrProd' + ', NF.Total ' + 'VlrNota'

      SET @NF = @NF + ' From NF_Item Inner Join NF ON NF.Codigo = NF_Item.Codigo '

      SET @NF = @NF + 'AND NF.Cliente_Fornecedor = NF_Item.Cliente_Fornecedor AND NF.Filial = NF_Item.Filial INNER JOIN NATUREZA_OPERACAO NP ON NF.CODIGO_OPERACAO = NP.CODIGO_OPERACAO '

      SET @NF = @NF + 'WHERE NF.Data BETWEEN ' + CHAR(39) +  convert(varchar,@DataDe,112) + CHAR(39) + ' AND '

      SET @NF = @NF + CHAR(39) + convert(varchar,@DataAte,112)  + char(39) + ' '
      
      
      --Exibe todas as notas fiscais (canceladas)
      IF LTRIM(RTRIM(@NF_CANC)) = 'SIM'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 1 '
            

      --Exibe todas as notas fiscais (exceto canceladas)     
      IF LTRIM(RTRIM(@NF_CANC)) = 'NAO'

            SET @NF = @NF + 'AND Isnull(NF.NF_Canc,0) = 0 '
      

      --Verifica se sera aplicado filtro por Filial

      IF LTRIM(RTRIM(@Filial)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Filial) = ' + CHAR(39) + @Filial + CHAR(39) + ' '

     

      --Verifica se sera aplicado filtro por Fornecedor

      IF LTRIM(RTRIM(@Fornecedor)) <> ''

            SET @NF = @NF + 'AND LTRIM(NF.Cliente_Fornecedor) = ' + CHAR(39) + @Fornecedor + CHAR(39) + ' '

 


 

      ----Verifica se sera aplicado filtro por Tipo da Nota Fiscal

      IF LTRIM(RTRIM(@Tipo)) = '1-Saida'

            SET @NF = @NF + 'AND NF.Tipo_NF = 1 '

     

      IF LTRIM(RTRIM(@Tipo)) = '2-Entrada'

            SET @NF = @NF + 'AND NF.Tipo_NF = 2 '

 
	  begin
		if(LEN(@plu)>0)
		 SET @NF = @NF + ' and nf_item.plu='+CHAR(39)+@plu+CHAR(39)	
	  end
		
	
		if(@NF_Devolucao='SIM')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 1 '
		end
		if(@NF_Devolucao='NAO')
		begin
		 SET @NF = @NF + ' and NP.NF_devolucao = 0 '
		end
		
		if(LEN(@Excluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO NOT IN('+@Excluir_Natureza+') '
		end
		
		if(LEN(@Incluir_Natureza)>0)
		begin
		 SET @NF = @NF + ' and NP.CODIGO_OPERACAO IN('+@Incluir_Natureza+') '
		end
		


      SET @NF = @NF + ' Group by NF.Filial, NF.Codigo, NF.Cliente_Fornecedor, NF.Emissao, NF.data, NF.Tipo_NF, NF.Total '

      SET @NF = @NF + ' Order By NF.Filial, Convert(Varchar,NF.Data,103) '
	end
     
	
  execute(@NF)
--print @nf
