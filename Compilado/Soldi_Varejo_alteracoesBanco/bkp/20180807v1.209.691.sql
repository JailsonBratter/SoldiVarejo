
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('filial') 
            AND  UPPER(COLUMN_NAME) = UPPER('cst_pis_cofins'))
begin
	alter table filial alter column cst_pis_cofins varchar(2)
end
else
begin
	alter table filial add cst_pis_cofins varchar(2)
end 
go 


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
GO

--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial       Varchar(20),
    @TipoCadastro Int = 0,
    @Alterados    int = 0
AS
    Declare @StringSQL  AS nVarChar(max)
    Declare @StringSQL2	As nVarChar(max)
    Declare @Where      As nVarChar(max)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    
    -- exec sp_Cons_Cadastro_Mercadoria  'MATRIZ',1,1
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

	    
	Declare @crt varchar(1), @cst_pisCofins varchar(2),@pis numeric(11,2),@cofins numeric(12,2)
	Select @crt= crt, 
		   @cst_pisCofins = cst_pis_cofins , 
		   @pis =pis, 
		   @cofins = cofins 
	from filial
	where filial = @filial;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    descricao= case when isnull(Mercadoria.descricao_Resumida,'+ char(39) + char(39) +')='+ char(39) + char(39) +' then mercadoria.descricao else Mercadoria.descricao_Resumida  end,'
		+ '    Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)' ;
    if(@crt =1)
	begin
		set @StringSQL = @StringSQL  + ',CST=ISNULL(tributacao.csosn,'+ char(39) + char(39) +')'
	end
	else
	begin   
		set @StringSQL = @StringSQL  + ',CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
	end

        set @StringSQL = @StringSQL  + ',und ';

	if(@crt =1)
	begin
		set @StringSQL = @StringSQL  + ',CST_PIS_COFINS ='+char(39)+@cst_pisCofins+char(39);
		set @StringSQL = @StringSQL  + ',pis_perc_saida='+convert(varchar,@pis);
		set @StringSQL = @StringSQL  + ',cofins_perc_saida='+convert(varchar,@cofins);
		
	end
	else
	begin 
	    set @StringSQL = @StringSQL  + ',CST_PIS_COFINS =Mercadoria.cst_saida';
		set @StringSQL = @StringSQL  + ',pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0)';
		set @StringSQL = @StringSQL  + ',cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) ';
	end

        set @StringSQL = @StringSQL  + ',Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
        + '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
        + '   ,mercadoria_loja.preco_atacado '
        + '   ,mercadoria_loja.qtde_atacado'
        + '   ,mercadoria.embalagem '
        + '   ,mercadoria.Porcao '+
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo'
        + ' WHERE ISNULL(Inativo, 0) <= 0  AND Tipo.Gera_Carga = 1 and Mercadoria_LOJA.Preco >0 and (ltrim(rtrim(isnull(mercadoria.descricao,'+ char(39) + char(39) +'))) <> '+ char(39) + char(39) +' or  ltrim(rtrim(isnull(mercadoria.Descricao_resumida,'+ char(39) + char(39) +'))) <>'+ char(39) + char(39) +')'

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
                + '   descricao= case when isnull(Mercadoria.descricao_Resumida,'+ char(39) + char(39) +')='+ char(39) + char(39) +' then mercadoria.descricao else Mercadoria.descricao_Resumida  end,'
				+ '    Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)' ;
    if(@crt =1)
	begin
		set @StringSQL2 = @StringSQL2  + ',CST=ISNULL(tributacao.csosn,'+ char(39) + char(39) +')'
	end
	else
	begin   
		set @StringSQL2 = @StringSQL2  + ',CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
	end

        set @StringSQL2 = @StringSQL2  + ',und ';

	if(@crt =1)
	begin
		set @StringSQL2 = @StringSQL2  + ',CST_PIS_COFINS ='+char(39)+@cst_pisCofins+char(39);
		set @StringSQL2 = @StringSQL2  + ',pis_perc_saida='+convert(varchar,@pis);
		set @StringSQL2 = @StringSQL2  + ',cofins_perc_saida='+convert(varchar,@cofins);
		
	end
	else
	begin 
	    set @StringSQL2 = @StringSQL2  + ',CST_PIS_COFINS =Mercadoria.cst_saida';
		set @StringSQL2 = @StringSQL2  + ',pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0)';
		set @StringSQL2 = @StringSQL2  + ',cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) ';
	end

        set @StringSQL2 = @StringSQL2  + ',Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
				+ '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
				+ '   ,mercadoria_loja.preco_atacado '
				+ '   ,mercadoria_loja.qtde_atacado'
				+ '   ,mercadoria.embalagem '
				+ '   ,mercadoria.Porcao '+
				+ '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --      + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo'
                + ' WHERE ISNULL(Inativo, 0) <= 0 AND Tipo.Gera_carga = 1 and Mercadoria_LOJA.Preco >0 and (ltrim(rtrim(isnull(mercadoria.descricao,'+ char(39) + char(39) +'))) <> '+ char(39) + char(39) +' or  ltrim(rtrim(isnull(mercadoria.Descricao_resumida,'+ char(39) + char(39) +'))) <>'+ char(39) + char(39) +')'

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 
GO 




insert into Versoes_Atualizadas select 'Versão:1.209.691', getdate();
GO