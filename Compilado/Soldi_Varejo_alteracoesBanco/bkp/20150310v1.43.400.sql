GO

CREATE TABLE PISCofins_CST_DePara

(CST_Saida Varchar(2) NOT NULL,

CST_Entrada Varchar(2) Not Null)

 

GO

INSERT INTO PISCofins_CST_DePara SELECT '01', '50'

INSERT INTO PISCofins_CST_DePara SELECT '02', '50'

INSERT INTO PISCofins_CST_DePara SELECT '03', '50'

INSERT INTO PISCofins_CST_DePara SELECT '04', '73'

INSERT INTO PISCofins_CST_DePara SELECT '05', '75'

INSERT INTO PISCofins_CST_DePara SELECT '06', '73'

INSERT INTO PISCofins_CST_DePara SELECT '07', '71'

INSERT INTO PISCofins_CST_DePara SELECT '08', '74'

INSERT INTO PISCofins_CST_DePara SELECT '09', '72'

INSERT INTO PISCofins_CST_DePara SELECT '49', '98'

 go


GO




ALTER PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria]
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
    SET NOCOUNT ON;

    SET @StringSQL = ''
    SET @StringSQL2 = ''



    SET @StringSQL = 'SELECT RTRIM(LTRIM(Mercadoria_loja.Filial)) AS Filial, [plu] = CONVERT(FLOAT, MERCADORIA.PLU), EAN.EAN , '
        + '    Mercadoria.descricao, Mercadoria.descricao_Resumida, Mercadoria_LOJA.Preco, PV = Case When Peso_Variavel.Codigo > 0 then 1 else 0 end,'
        + '    pv_balanca = Peso_Variavel.Codigo, MERCADORIA_LOJA.preco_promocao, NCM = Replace(isnull(CF,' + char(39) + char(39) + '),' + CHAR(39) + '.' + char(39) + ',' + Char(39)+ Char(39) + '), '
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, ' + CHAR(39) + '001' + CHAR(39) + ' as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS,mercadoria.alcoolico '
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

