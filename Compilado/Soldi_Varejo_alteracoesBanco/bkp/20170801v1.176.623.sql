


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_rel_Resumo_Vendas_Cliente]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[sp_rel_Resumo_Vendas_Cliente]
GO

CREATE PROCEDURE sp_rel_Resumo_Vendas_Cliente (
@filial varchar(20),
@datade varchar(8),
@dataate varchar(8),
@Codigo_Cliente varchar(50))
AS
--SP_REL_RESUMO_VENDAS_CLIENTE 'MATRIZ', '20170701', '20170731', 1
SELECT 
Nome_Cliente = c.nome_cliente,
Emissao = CONVERT(VARCHAR, s.data_movimento, 103),
PDV = '.' + convert(varchar, s.Caixa_Saida),
'.' + s.Documento as Documento,
'.' + s.plu as PLU,
m.descricao,
Qtde = sum(s.qtde),
Vlr = sum(s.vlr)
FROM
Saida_estoque s 
INNER JOIN MERCADORIA m ON S.PLU = M.PLU 
INNER JOIN CLIENTE C on c.codigo_cliente = s.codigo_cliente 
WHERE
s.Filial = @Filial 
AND
s.Data_Movimento between @DataDe and @DataAte
AND 
((c.Codigo_Cliente = @Codigo_cliente OR replace(replace(replace(c.CNPJ,'.',''),'-',''),'/','') = replace(replace(replace(@Codigo_cliente,'.',''),'-',''),'/',''))) 
GROUP BY 
c.codigo_cliente, 
c.nome_cliente, 
s.data_movimento,
s.Caixa_Saida,
s.Documento,
s.plu,
m.descricao
ORDER BY
c.codigo_cliente,
s.data_movimento, 
s.caixa_saida, 
s.documento
go 

IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('conta_a_receber') 
            AND  UPPER(COLUMN_NAME) = UPPER('id_movimento'))
begin
	alter table conta_a_receber alter column id_movimento bigint
end
else
begin
		
	alter table conta_a_receber add id_movimento bigint

end 
go 



GO

/****** Object:  Table [dbo].[Promocao_Itens]    Script Date: 08/01/2017 14:28:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Promocao_Itens](
					[Filial] [varchar](20) NULL,
					[Tipo] [tinyint] NULL,
					[PLU] [varchar](17) NULL,
					[Validade] [datetime] NULL,
					[Conexao] [tinyint] NULL,
					[Qtde_Objetivo] [numeric](9, 0) NULL,
					[Qtde_Atingido] [numeric](9, 0) NULL,
					[Valor_Desconto] [numeric](12, 2) NULL
	) ON [PRIMARY]
END
GO

SET ANSI_PADDING OFF
GO


GO

/****** Object:  UserDefinedFunction [dbo].[F_Promocao_Item_TP1]    Script Date: 08/01/2017 14:27:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[F_Promocao_Item_TP1](@PLU VARCHAR(17))
                RETURNS Numeric(12,2)
AS
                BEGIN

                               DECLARE @RETORNO     Numeric(12,2)
                               DECLARE @QTDE                             NUMERIC(9)
                
                               SELECT 
                                               @QTDE =  (Qtde_Objetivo - Qtde_Atingido),
                                               @RETORNO = Valor_Desconto
                               FROM 
                                               Promocao_Itens 
                               WHERE
                                               Promocao_Itens.PLU = @PLU

                               IF @Qtde <= 0
                                               SET @RETORNO = 0
                                               

                               RETURN @RETORNO

                END

GO






go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.176.623', getdate();
GO

