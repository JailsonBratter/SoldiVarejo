
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Rel_Outras_Movimentacoes]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE sp_Rel_Outras_Movimentacoes
end
GO
--PROCEDURES =======================================================================================
CREATE   PROCEDURE [dbo].[sp_Rel_Outras_Movimentacoes]
	@Filial				As Varchar(20),
	@DataDe				As Varchar(8),
	@DataAte			As Varchar(8),
	@plu				AS Varchar(20),
	@ean				as varchar(20),
	@ref				as Varchar(20),
	@descricao			as Varchar(40),
	@Movimentacao	As Varchar(30) = ''
AS
	Declare @String	As nVarchar(1024)
		
BEGIN
	SET @String = ' SELECT'
	SET @String = @String + ' i.Codigo_inventario As Codigo,'
	SET @String = @String + ' CONVERT(VARCHAR, i.Data, 103) AS Data,'
	SET @String = @String + ' i.tipoMovimentacao,'
	SET @String = @String + ' i.Usuario,'
	SET @String = @String + ' ii.plu,'
	SET @String = @String + ' m.descricao,'
	SET @String = @String + ' ii.Contada AS Qtde,'
	SET @String = @String + ' ii.custo as Vlr_Unit,'
	SET @String = @String + ' Total_Item = CONVERT(DECIMAL(12,2), (ii.custo * ii.Contada ))'
	SET @String = @String + ' FROM inventario i inner join inventario_itens ii on i.Codigo_inventario = ii.Codigo_inventario '
	SET @String = @String + ' inner join mercadoria m on ii.PLU = m.PLU '
	SET @String = @String + ' left join ean  on m.PLU = ean.PLU '
	SET @String = @String + ' WHERE i.status = ' + CHAR(39) + 'ENCERRADO' + CHAR(39) + ' AND i.Data BETWEEN ' + CHAR(39) + @DataDe + CHAR(39) + ' AND ' + CHAR(39) + @DataAte + CHAR(39)
	BEGIN
		IF LEN(ISNULL(@Movimentacao ,'')) > 0 
			SET @String = @String + ' AND i.tipoMovimentacao = ' + CHAR(39) + @Movimentacao + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@plu ,'')) > 0 
			SET @String = @String + ' AND ii.plu = ' + CHAR(39) + @plu + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ean ,'')) > 0 
			SET @String = @String + ' AND ean = ' + CHAR(39) + @ean + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@ref ,'')) > 0 
			SET @String = @String + ' AND m.Ref_fornecedor = ' + CHAR(39) + @ref + CHAR(39)
	END
	BEGIN
		IF LEN(ISNULL(@descricao ,'')) > 0 
			SET @String = @String + ' AND m.descricao like ' + CHAR(39) + '%'+@descricao +'%'+ CHAR(39)
	END
	SET @String = @String + ' ORDER BY 1'

--PRINT @STRING
  EXECUTE(@String)

END
