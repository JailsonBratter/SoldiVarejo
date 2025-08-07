
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('tabela_preco') 
            AND  UPPER(COLUMN_NAME) = UPPER('porc'))
begin
	alter table tabela_preco alter column porc numeric(18,2)
end
else
begin
		
		alter table tabela_preco add porc numeric(18,2)

end 
go 

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.165.605', getdate();
GO