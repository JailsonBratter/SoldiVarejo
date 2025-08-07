


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_REL_VENDAS_POR_FINALIZADORA]') AND type in (N'P', N'PC'))
begin
	DROP PROCEDURE [dbo].[SP_REL_VENDAS_POR_FINALIZADORA]
end
go 
--PROCEDURES =======================================================================================
CREATE PROCEDURE [dbo].[SP_REL_VENDAS_POR_FINALIZADORA]
				@Filial      As Varchar(20),
                @DataDe      As Varchar(8),
                @DataAte     As Varchar(8)
AS
begin
-- exec SP_REL_VENDAS_POR_FINALIZADORA 'MATRIZ','20170501','20170520'
Declare crFinalizadora cursor
for  SELECT Finalizadora FROM Finalizadora GROUP BY Finalizadora, Nro_Finalizadora order by Nro_Finalizadora
declare @SqlString Nvarchar(max)
SET @SqlString = ''
DECLARE @FINALIZA VARCHAR(30)
open crFinalizadora
FETCH NEXT FROM crFinalizadora INTO @FINALIZA;
WHILE @@FETCH_STATUS = 0
   BEGIN
	
		SET  @SqlString = @SqlString + ',[' + @FINALIZA +'] =ISNULL((select SUM(ISNULL(total,0))from Lista_finalizadora where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and emissao =lf.emissao and id_finalizadora ='+CHAR(39) + @FINALIZA+ CHAR(39) + ' AND isnull(Cancelado,0)=0),0)'
	  --SET @SqlString = @SqlString + CONVERT(VARCHAR(10),@TRIB)
		 
      FETCH  NEXT FROM crFinalizadora INTO @FINALIZA;
      
   END;

CLOSE crFinalizadora;
DEALLOCATE crFinalizadora;
SET @SqlString = 'Select Data =convert(varchar,Emissao,103),SUM(ISNULL(total,0)) AS TOTAL '+@SqlString +'  from Lista_finalizadora lf  ' +
                    ' where Filial='+ CHAR(39) +@Filial+ CHAR(39) + '  and Emissao between ' + CHAR(39) + @DataDe +  CHAR(39) + ' and '+ CHAR(39) + @DataAte + CHAR(39) +
                    ' group by Emissao  order by convert(varchar,Emissao,102) ';


 -- print(@sqlString);
 EXECUTE (@SqlString);

end




GO


if not exists(select 1 from PARAMETROS where PARAMETRO='CARGA_OPERADORES')
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
           ('CARGA_OPERADORES'
           ,GETDATE()
           ,'FALSE'
           ,GETDATE()
           ,'FALSE'
           ,'HABILITA A CARGA DE OPERADORES'
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




go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.179.626', getdate();
GO