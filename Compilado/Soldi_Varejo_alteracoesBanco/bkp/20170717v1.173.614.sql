
/****** Object:  StoredProcedure [dbo].[sp_Cons_Cadastro_Mercadoria]    Script Date: 07/17/2017 09:52:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Cons_Cadastro_Mercadoria]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
GO


--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
    @Filial            Varchar(20),
    @TipoCadastro    Int = 0,
    @Alterados        int = 0
AS
    Declare @StringSQL    AS nVarChar(max)
    Declare @StringSQL2 As nVarChar(max)
    Declare @Where        As nVarChar(max)
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
        + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
        + '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
        + '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
        + '   ,und '+
        + '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
        + '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
        + '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
        + '   ,mercadoria_loja.preco_atacado '
        + '   ,mercadoria_loja.qtde_atacado'
        + '   ,mercadoria.embalagem '
        + '   ,mercadoria.Porcao '+
        + '    FROM Mercadoria '
        + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
        --+ '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
        + ' LEFT OUTER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
        + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
        + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
        + ' INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo'
        + ' WHERE ISNULL(Inativo, 0) <= 0  AND Tipo.Gera_Carga = 1'

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
                + '    descricao=isnull(Mercadoria.descricao_Resumida,Mercadoria.descricao), Mercadoria.descricao_Resumida, Mercadoria_loja.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
                + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,  ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
				+ '    ,[Cod_plu] = CONVERT(FLOAT, MERCADORIA.PLU)'
				+ '	  ,CST=ISNULL(tributacao.indice_st,'+ char(39) + char(39) +')'
				+ '   ,und '+
				+ '   ,CST_PIS_COFINS =Mercadoria.cst_saida,pis_perc_saida=ISNULL(Mercadoria.pis_perc_saida,0),cofins_perc_saida=ISNULL(Mercadoria.cofins_perc_saida,0) '+
				+ '   ,Origem = ISNULL(mercadoria.Origem,'+ char(39) + char(39) +')'
				+ '   ,CEST =ISNULL(mercadoria.CEST,'+ char(39) + char(39) +')'
				+ '   ,mercadoria_loja.preco_atacado '
				+ '   ,mercadoria_loja.qtde_atacado'
				+ '   ,mercadoria.embalagem '
				+ '   ,mercadoria.Porcao '+
				+ '    FROM Mercadoria '
                + ' INNER JOIN MERCADORIA_LOJA ON MERCADORIA.PLU=MERCADORIA_LOJA.PLU '+
                + '    INNER JOIN Peso_variavel ON Mercadoria.Peso_Variavel = Peso_Variavel.Peso_Variavel'
        --        + '    INNER JOIN W_BR_CADASTRO_DEPARTAMENTO ON Mercadoria.Codigo_departamento = W_BR_CADASTRO_DEPARTAMENTO.codigo_departamento '
                + ' INNER JOIN EAN ON EAN.PLU = MERCADORIA.PLU'
                + ' LEFT OUTER JOIN Imposto_Nota ON CONVERT(FLOAT, Imposto_Nota.NCM) = CONVERT(FLOAT, REPLACE(ISNULL(Mercadoria.CF,0), ' + char(39) + '.' + char(39) + ', ' + CHAR(39) + CHAR(39) + '))'
                + '    INNER JOIN Tributacao ON Tributacao.Codigo_Tributacao = Mercadoria.Codigo_Tributacao '
                + ' INNER JOIN Tipo ON Tipo.Tipo = Mercadoria.Tipo'
                + ' WHERE ISNULL(Inativo, 0) <= 0 AND Tipo.Gera_carga = 1'

        END


    SET @StringSQL = @StringSQL + @Where + @StringSQL2 +@Where +' ORDER BY 1, 2' 
		
    --PRINT (@StringSQL)
       
    
    EXEC(@StringSQL)
END 





GO


IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('lista_finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_movimento'))
begin
	alter table lista_finalizadora alter column id_movimento bigInt
end
else
begin
		
	alter table lista_finalizadora add id_movimento bigInt

end 
go 



IF NOT  EXISTS(SELECT * FROM PARAMETROS WHERE PARAMETRO='PLU_DIGITO_VERIFICADOR')
BEGIN

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
           ('PLU_DIGITO_VERIFICADOR'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'COLOCAR DIGITO VERIFICADOR NO PLU'
           ,''
           ,''
           ,0
           ,''
           ,0
           ,0
           ,GETDATE()
           ,0
         )
END
GO



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.173.614', getdate();
GO