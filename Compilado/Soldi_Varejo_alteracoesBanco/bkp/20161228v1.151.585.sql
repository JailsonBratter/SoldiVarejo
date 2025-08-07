
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('cliente') 
            AND  UPPER(COLUMN_NAME) = UPPER('habilita_f9'))
begin
	alter table cliente alter column habilita_f9  tinyint
end
else
begin
		
		alter table cliente add habilita_f9  tinyint

end 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.151.585', getdate();
GO