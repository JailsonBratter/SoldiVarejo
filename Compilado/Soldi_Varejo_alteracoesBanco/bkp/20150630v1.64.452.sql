
GO

/****** Object:  StoredProcedure [dbo].[sp_Rel_Outras_Movimentacoes]    Script Date: 06/30/2015 13:52:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- sp_Rel_Outras_Movimentacoes 'MATRIZ', '20150501', '20150630', '' 
CREATE PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@Movimentacao	As Varchar(30) = ''
AS
	Declare @String	As nVarchar(1024)
		
BEGIN
	SET @String = ' SELECT'
	SET @String = @String + ' i.Codigo_inventario As Codigo,'
	SET @String = @String + ' CONVERT(VARCHAR, i.Data, 102) AS Data,'
	SET @String = @String + ' i.tipoMovimentacao,'
	SET @String = @String + ' i.Usuario,'
	SET @String = @String + ' ii.plu,'
	SET @String = @String + ' m.descricao,'
	SET @String = @String + ' ii.Contada AS Qtde,'
	SET @String = @String + ' ii.custo as Vlr_Unit,'
	SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
	SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
	SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
	SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
	BEGIN
		IF LEN(ISNULL(@Movimentacao ,'')) > 0 
			SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
	END
	SET @String = @String + ' ORDER BY 1'

--PRINT @STRING
  EXECUTE(@String)

END
GO



Create procedure sp_rel_produtos_estoque
@filial varchar(20) ,
@plu varchar(20)
,@descricao varchar(40)
,@grupo varchar(20)
,@subGrupo varchar(20)
,@departamento varchar(20)
,@familia varchar(40)
as

    Select a.PLU,
    a.Descricao,
    b.descricao_grupo [GRUPO],
    b.descricao_subgrupo[SUBGRUPO],
    b.descricao_departamento [DEPARTAMENTO],
    isnull(c.Descricao_Familia,'')[FAMILIA],
    l.Preco_Custo AS [PRECO CUSTO],
    l.saldo_atual AS [SALDO ATUAL],
    convert(decimal(12,2),(l.Saldo_Atual*l.Preco_Custo))[VALOR ESTOQUE]
    from
    Mercadoria a left join W_BR_CADASTRO_DEPARTAMENTO b
    on (a.codigo_departamento= b.codigo_Departamento and a.filial=b.filial)
    inner join mercadoria_loja l on a.plu=l.PLU
    left join Familia c on  a.Codigo_familia =c.Codigo_familia
    where a.Inativo <>1 
    AND (a.PLU=@plu or len(@plu)=0)
		and (a.Descricao like '%'+@descricao+'%' or LEN(@descricao)=0)
		and (b.Descricao_grupo = @grupo or LEN(@grupo)=0)
		and (b.descricao_subgrupo = @subGrupo or LEN(@subGrupo)=0)
		and (b.descricao_departamento = @departamento or LEN(@departamento)=0)
		and (a.Descricao_familia = @familia or LEN(@familia)=0)										 
		and l.Filial = @filial
		
    

go
go
