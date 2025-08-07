
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('finalizadora') 
            AND  UPPER(COLUMN_NAME) = UPPER('naoComputa'))
begin
	alter table finalizadora alter column naoComputa tinyint 
end
else
begin
		
	alter table finalizadora add naoComputa tinyint default 0 

end 
go 



IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('saida_estoque') 
            AND  UPPER(COLUMN_NAME) = UPPER('naoComputa'))
begin
	alter table saida_estoque alter column naoComputa tinyint
end
else
begin
		
	alter table saida_estoque add naoComputa tinyint default 0

end 
go 



go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.184.633', getdate();
GO

