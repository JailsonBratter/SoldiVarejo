
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('mercadoria') 
            AND  UPPER(COLUMN_NAME) = UPPER('Ativo_Ecommerce'))
begin
	alter table mercadoria alter column Ativo_Ecommerce tinyint

end 
else
begin
	alter table mercadoria add Ativo_Ecommerce tinyint
end

go 



IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.190.643', getdate();
GO