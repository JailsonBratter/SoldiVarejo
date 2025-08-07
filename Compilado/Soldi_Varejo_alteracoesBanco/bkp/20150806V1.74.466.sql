IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_venda_itens_agregado]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_rel_venda_itens_agregado
end
GO

CREATE procedure   [dbo].[sp_rel_venda_itens_agregado]   @Filial   As Varchar(20),
					@DataDe          As Varchar(8),
					@DataAte         As Varchar(8),
					@horaInicio      as time,
					@horafim	     as time,
					@plu			as varchar(20) 
	as 
/*
set @plu='4'
set @Filial='MATRIZ'
set @DataDe='20150514' 
set @DataAte = '20150514'
set @horaInicio='08:00'
set @horafim = '10:00'
*/
-- select top 100 * from saida_estoque 
SELECT s.PLU,m.Descricao, Qtde= SUM(s.qtde),Unitario = m.Preco,Total = sum(vlr) 
	FROM Saida_estoque s with (index(index_item_agregado))  inner join mercadoria m on s.PLU=m.PLU
where s.Filial=@Filial 
		and s.PLU= @plu 
		and s.Data_movimento between @DataDe and @DataAte 
		and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
group by s.PLU,m.descricao,m.Preco
union all
SELECT s.PLU,m.Descricao, Qtde= SUM(s.qtde),Unitario = m.Preco,Total = sum(vlr) 
	FROM Saida_estoque s with (index(index_item_agregado))  inner join mercadoria m on s.PLU=m.PLU
where s.Filial=@Filial and  Documento in (
		select Documento 
		from Saida_estoque 
		where Filial=@Filial and  PLU  = @plu and Data_movimento between @DataDe and @DataAte and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim
		) 
		and s.PLU<> @plu 
		and s.Data_movimento between @DataDe and @DataAte 
		and CONVERT(varchar , Hora_venda,108) between @horaInicio and @horafim 
group by s.PLU,m.descricao,m.Preco
order by SUM(qtde) desc				 





go




--[sp_Cons_Cadastro_Mercadoria] 'MATRIZ',0,0

ALTER PROCEDURE [dbo].[sp_Cons_Cadastro_Mercadoria] --'MATRIZ',0,0
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
        + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica, SUBSTRING(mercadoria.Codigo_departamento,1,3) as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
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
                + '    MERCADORIA_LOJA.data_inicio, MERCADORIA_LOJA.data_fim, MERCADORIA_LOJA.promocao, MERCADORIA_LOJA.promocao_automatica,SUBSTRING(mercadoria.Codigo_departamento,1,3) as codigo_grupo , ' + CHAR(39) + '001999' + CHAR(39) + ' as codigo_subgrupo, Imposto = ISNULL(Imposto_Nota.Aliquota_Imposto,0),'
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



