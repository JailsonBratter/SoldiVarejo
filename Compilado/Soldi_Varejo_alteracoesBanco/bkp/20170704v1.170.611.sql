
IF EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE UPPER(TABLE_NAME) = UPPER('Contrato_fornecedor_item') 
            AND  UPPER(COLUMN_NAME) = UPPER('vlr'))
begin
	alter table Contrato_fornecedor_item alter column vlr decimal(18,2)
end
else
begin
		
		alter table Contrato_fornecedor_item add vlr decimal(18,2)

end 
go 


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Versoes_Atualizadas]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Versoes_Atualizadas](
		[Versao] [varchar](20) NULL,
		[Data_Script] [datetime] NULL
	) ON [PRIMARY]
END



insert into Versoes_Atualizadas select 'Versão:1.170.611', getdate();
GO