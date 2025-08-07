--ALTERAÇÕES TABELA================================================================================= 

ALTER TABLE [dbo].[NF] DROP CONSTRAINT [DF__nf__Status__3F806842]
         Alter table nf alter column status varchar(20)



--PROCEDURES =======================================================================================
CREATE PROCEDURE SP_REL_HISTORIO_ENTRADA_SAIDA(
@FILIAL AS varchar(20) , 
@DataInicio as datetime,
@DataFim as datetime,
@plu as Varchar(20) )
As

--set @DataInicio ='20140401'
--set @DataFim ='20140910'
--Set @plu ='1'

select TIPO = CASE WHEN b.Tipo_NF=1 THEN 'SAIDA'ELSE'ENTRADA' END,CONVERT (VARCHAR ,b.data,102) data,
		a.qtde 
from NF_Item a inner join nf b on a.codigo = b.Codigo
WHERE A.PLU = @plu AND B.Data between @DataInicio AND @DataFim AND B.Tipo_NF = 2 and b.nf_Canc <>1
UNION ALL

Select Tipo= 'TOTAL' ,DATA= '',
	Entrada= (Select sum(NF_ITEM.qtde) from nf_item  INNER JOIN NF ON NF.Codigo=NF_Item.CODIGO where NF_ITEM.tipo_nf=2 and NF_Item.PLU = A.plu and nf.nf_Canc <>1 AND NF.Data between @DataInicio AND @DataFim) 
from Mercadoria A where a.PLU=@plu 


UNION ALL
select TIPO = CASE WHEN b.Tipo_NF=1 THEN 'SAIDA'ELSE'ENTRADA' END, CONVERT (VARCHAR ,b.data,102) data,
		a.qtde 	
from NF_Item a inner join nf b on a.codigo = b.Codigo
WHERE A.PLU = @plu AND B.Data between @DataInicio AND @DataFim AND B.Tipo_NF = 1 and  b.nf_Canc <>1
 UNION ALL
Select Tipo= 'TOTAL',DATA= '',
	saida = (Select sum(NF_ITEM.qtde) from nf_item  INNER JOIN NF ON NF.Codigo=NF_Item.CODIGO where NF_ITEM.tipo_nf=1 and NF_Item.PLU = A.plu AND nf.nf_Canc <>1 and  NF.Data between @DataInicio AND @DataFim)
from Mercadoria A where a.PLU=@plu  
UNION ALL
Select Tipo= 'TOTAL SALDO',DATA= '',
	Saldo = ((Select sum(NF_ITEM.qtde) from nf_item  INNER JOIN NF ON NF.Codigo=NF_Item.CODIGO where NF_ITEM.tipo_nf=2 and NF_Item.PLU = A.plu AND nf.nf_Canc <>1 AND NF.Data between @DataInicio AND @DataFim) - 
				(Select sum(NF_ITEM.qtde) from nf_item  INNER JOIN NF ON NF.Codigo=NF_Item.CODIGO where NF_ITEM.tipo_nf=1 and NF_Item.PLU = A.plu  AND nf.nf_Canc <>1 AND NF.Data between @DataInicio AND @DataFim)) 
from Mercadoria A where a.PLU=@plu 








go 







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
        + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS '
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
                + '    Mercadoria.Codigo_departamento, estado_mercadoria, Mercadoria.codigo_familia, Mercadoria.tipo, Mercadoria.validade, Mercadoria.Etiqueta, Tributacao.Nro_ECF, Tributacao.Saida_ICMS '
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





