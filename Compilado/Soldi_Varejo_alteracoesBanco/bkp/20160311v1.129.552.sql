IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_ALIQUOTA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [SP_REL_VENDAS_POR_ALIQUOTA]
end
GO
--PROCEDURES =======================================================================================
CREATE  PROCEDURE [dbo].[SP_REL_VENDAS_POR_ALIQUOTA]
				@Filial      As Varchar(20),
                @DataDe      As Varchar(8),
                @DataAte     As Varchar(8)
AS
begin
-- exec SP_REL_VENDAS_POR_ALIQUOTA 'MATRIZ','20151101','20151130'
Declare crTributacao cursor
for select Saida_ICMS from Tributacao group by Saida_ICMS order by Saida_ICMS
declare @SqlString Nvarchar(max)
SET @SqlString = ''
DECLARE @TRIB VARCHAR(10)
open crTributacao
FETCH NEXT FROM crTributacao INTO @TRIB;
WHILE @@FETCH_STATUS = 0
   BEGIN
	
		SET  @SqlString = @SqlString + ',[' + CASE WHEN @TRIB = '0.00' THEN 'ISENTO' ELSE @TRIB+'%' END + '] =ISNULL((select SUM(ISNULL(vlr,0)-isnull(desconto,0)) from saida_estoque with(index(ix_Rel_Venda_Aliquota)) where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and data_movimento =se.Data_movimento and Aliquota_ICMS ='+ @TRIB +' AND data_cancelamento IS NULL),0)'
	  --SET @SqlString = @SqlString + CONVERT(VARCHAR(10),@TRIB)
		 
      FETCH  NEXT FROM crTributacao INTO @TRIB;
      
   END;

CLOSE crTributacao;
DEALLOCATE crTributacao;
SET @SqlString = 'Select Data =convert(varchar,Data_movimento,103),SUM(ISNULL(vlr,0)-isnull(desconto,0)) AS TOTAL '+@SqlString +'  from saida_estoque se with(index(ix_Rel_Venda_Aliquota)) ' +
                    ' where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and Data_movimento between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) + ' AND  data_cancelamento IS NULL '+
                    ' group by Data_movimento  order by convert(varchar,data_movimento,102) ';


EXECUTE (@SqlString);

end


GO 
-- ATRIBUI CHAVE PRIMARIA PEDIDO 

 
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pedido]') AND name = N'PK_Pedido')
	ALTER TABLE [dbo].[Pedido] DROP CONSTRAINT [PK_Pedido]
GO

IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Pedido]') AND name = N'ix_pedido_fluxo_caixa')
	DROP INDEX [ix_pedido_fluxo_caixa] ON [dbo].[Pedido] WITH ( ONLINE = OFF )
GO


  ALTER TABLE PEDIDO ALTER COLUMN TIPO INT NOT NULL
  GO
  alter table pedido add constraint PK_Pedido PRIMARY KEY (FILIAL,PEDIDO,TIPO)
  GO
 
 CREATE NONCLUSTERED INDEX [ix_pedido_fluxo_caixa] ON [dbo].[Pedido] 
(
	[Tipo] ASC,
	[pedido_simples] ASC,
	[Data_cadastro] ASC,
	[Status] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

 

 
 GO
 
 
 if not exists(select 1 from PARAMETROS where PARAMETRO='OBS_NOTA_FANTASIA')
begin	
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
           ('OBS_NOTA_FANTASIA'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'INCLUIR O NOME FANTASIA DO DESTINATARIO NA OBS DA NOTA'
           ,''
           ,0
           ,1
           ,''
           ,0
           ,0
           ,NULL
           ,0)
 end
GO
